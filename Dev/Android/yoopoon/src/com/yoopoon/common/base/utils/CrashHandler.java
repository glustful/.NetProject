package com.yoopoon.common.base.utils;

import java.io.File;
import java.io.FileOutputStream;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.io.Writer;
import java.lang.Thread.UncaughtExceptionHandler;
import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.Map;
import android.content.Context;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Build;
import android.os.Environment;
import android.os.Looper;
import android.widget.Toast;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.data.user.User;

public class CrashHandler implements UncaughtExceptionHandler {
	// 系统默认的UncaughtException处理类
	Thread.UncaughtExceptionHandler exceptionHandler;

	// 上下文环境
	Context context;

	// 存放消息
	HashMap<String, String> infos = new HashMap<String, String>();

	// 单列模式
	private static CrashHandler handler = new CrashHandler();

	private CrashHandler() {
	}

	public static CrashHandler getInstance() {
		return handler;
	}

	// 初始化操作
	public void init(Context context) {
		this.context = context;
		// 获得系统默认的异常处理方法
		exceptionHandler = Thread.getDefaultUncaughtExceptionHandler();
		// 设置系统自带异常处理方法为本类
		Thread.setDefaultUncaughtExceptionHandler(this);
	}

	/*
	 * Throwable的说明：简单的认知就是Throwable就是exception的父类 The superclass of all classes which can be thrown
	 * by the virtual machine. The two direct subclasses are recoverable exceptions (Exception) and
	 * unrecoverable errors (Error). This class provides common methods for accessing a string
	 * message which provides extra information about the circumstances in which the Throwable was
	 * created (basically an error message in most cases), and for saving a stack trace (that is, a
	 * record of the call stack at a particular point in time) which can be printed later. A
	 * Throwable can also include a cause, which is a nested Throwable that represents the original
	 * problem that led to this Throwable. It is often used for wrapping various types oferrors into
	 * a common Throwable without losing the detailed original error information. When printing the
	 * stack trace, the trace of the cause is included.
	 */
	@Override
	public void uncaughtException(Thread thread, Throwable ex) {
		if (!handleException(ex) && exceptionHandler != null) {
			// 当ex为空的时候，调用此方法（如果用户自己处理try-catch操作，也不会运行到此界面）
			// 调用系统默认的处理方法
			exceptionHandler.uncaughtException(thread, ex);
		}// if
		else {
			try {
				Thread.sleep(3000);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
			// 退出程序

			android.os.Process.killProcess(android.os.Process.myPid());
			System.exit(1);
		}// else
	}

	// 当有异常的时候，ex不等于null，返回false
	private boolean handleException(Throwable ex) {
		if (ex == null) {
			return false;
		}
		// 有异常
		new Thread() {
			@Override
			public void run() {
				// Looper用于封装了android线程中的消息循环，默认情况下一个线程是不存在消息循环
				// （message loop）的，需要调用Looper.prepare()来给线程创建一个消息循环，
				// 调用Looper.loop()来使消息循环起作用，从消息队列里取消息，处理消息。
				Looper.prepare();
				if (!User.lastLoginUser(MyApplication.getInstance()).remember)
					SPUtils.clearAllInfo(MyApplication.getInstance());
				Toast.makeText(context, "很抱歉,程序出现异常,即将退出.", Toast.LENGTH_LONG).show();
				Looper.loop();
			}
		}.start();
		// 手机设备信息
		collectDeviceInfo(context);
		// 保存到文件sdcard中区
		wirteToFile(ex);
		return true;
	}

	private void collectDeviceInfo(Context context) {
		/*
		 * PackageManager相关和ActivitManager相关。 PackageManager相关 本类API是对所有基于加载信息的数据结构的封装，包括以下功能：
		 * 安装，卸载应用 查询permission相关信息
		 * 查询Application相关信息(application，activity，receiver，service，provider及相应属性等） 查询已安装应用
		 * 增加，删除permission 清除用户数据、缓存，代码段等 非查询相关的API需要特定的权限，具体的API请参考SDK文档。 ActivityManager相关
		 * 本类API是对运行时管理功能和运行时数据结构的封装，包括以下功能 激活／去激活activity 注册／取消注册动态接受intent 发送／取消发送intent
		 * activity生命周期管理（暂停，恢复，停止，销毁等） activity task管理（前台－>后台，后台－>前台，最近task查询，运行时task查询）
		 * 激活／去激活service 激活／去激活provider等
		 */
		// 获得包管理器
		PackageManager pi = context.getPackageManager();
		try {
			// 获得这个应用的包的信息
			PackageInfo info = pi.getPackageInfo(context.getPackageName(), PackageManager.GET_ACTIVITIES);
			if (pi != null) {
				// 版本和名字
				String vName = info.versionName == null ? "null" : info.versionName;
				String vCode = info.versionCode + "";
				// 写入
				infos.put("versionName", vName);
				infos.put("versionCode", vCode);
			}
		} catch (NameNotFoundException e) {
			e.printStackTrace();
		}
		/*
		 * Field: This class represents a field字段. Information about the field can be accessed, and
		 * the field's value can be accessed dynamically.
		 */
		// Build:
		// Information about the current build, extracted from system properties.
		Field[] fields = Build.class.getDeclaredFields();
		for (Field field : fields) {
			try {
				field.setAccessible(true);
				System.out.println("field.getName()=" + field.get(null).toString());
				infos.put(field.getName(), field.get(null).toString());
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	// 写入到文件中区
	private void wirteToFile(Throwable ex) {
		StringBuffer sb = new StringBuffer();
		for (Map.Entry<String, String> map : infos.entrySet()) {
			String key = map.getKey();
			String value = map.getValue();
			sb.append(key + "=" + value + "\n");
		}
		Writer writer = new StringWriter();
		PrintWriter printWriter = new PrintWriter(writer);
		ex.printStackTrace(printWriter);
		Throwable cause = ex.getCause();
		while (cause != null) {
			cause.printStackTrace(printWriter);
			cause = cause.getCause();
		}
		printWriter.close();
		String reString = writer.toString();
		sb.append(reString);
		sendMail(sb.toString());
		try {
			long time = System.currentTimeMillis();
			String fileName = "crash-" + time + ".log";
			if (Environment.getExternalStorageState().equals(Environment.MEDIA_MOUNTED)) {
				String path = "/mnt/sdcard/crash";
				File dir = new File(path);
				if (!dir.exists()) {
					dir.mkdirs();
				}
				FileOutputStream stream = new FileOutputStream(path + fileName);
				stream.write(sb.toString().getBytes());
				stream.close();
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	private void sendMail(String content) {
		MailUtils mailUitls = new MailUtils();
		mailUitls.setHost("smtp.qq.com"); // 指定要使用的邮件服务器
		mailUitls.setAccount("286617690@qq.com", "liangying12138"); // 指定帐号和密码

		mailUitls.send("286617690@qq.com", "286617690@qq.com", "yoopoon异常", content);
	}

}
