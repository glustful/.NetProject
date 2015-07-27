package com.yoopoon.home;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.EApplication;
import android.app.Activity;
import android.app.Application;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.graphics.Bitmap;
import android.telephony.TelephonyManager;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.ImageView;
import com.nostra13.universalimageloader.cache.disc.impl.UnlimitedDiskCache;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;
import com.nostra13.universalimageloader.core.listener.ImageLoadingListener;
import com.nostra13.universalimageloader.utils.StorageUtils;
import com.yoopoon.common.base.utils.CrashHandler;

@EApplication
public class MyApplication extends Application {

	protected static final String TAG = "MyApplication";

	private static MyApplication mInstance;
	private static DisplayImageOptions mOptions;
	private static ImageLoadingListener mLoadingListener;

	private String deviceId;

	/**
	 * 获取全局Application
	 * @return
	 */
	public static MyApplication getInstance() {
		return mInstance;
	}

	@Override
	public void onCreate() {

		mInstance = this;

		TelephonyManager tm = (TelephonyManager) getSystemService(TELEPHONY_SERVICE);
		deviceId = tm.getDeviceId();

		initImageLoader();

		// 通过单利模式，获得异常处理类
		CrashHandler handler = CrashHandler.getInstance();
		// 初始化
		handler.init(getApplicationContext());
		super.onCreate();
	}

	private void initImageLoader() {
		File meCacheDir = StorageUtils.getOwnCacheDirectory(this, "yoopoon/cache/imageloaderCache");
		ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(this)
				.threadPriority(Thread.NORM_PRIORITY - 2).memoryCache(new WeakMemoryCache())
				.denyCacheImageMultipleSizesInMemory().diskCacheFileNameGenerator(new Md5FileNameGenerator())
				.diskCacheSize(100 * 1024 * 1024)
				// 100 Mb
				.diskCacheFileCount(200).diskCache(new UnlimitedDiskCache(meCacheDir))// 自定义缓存路径
				.tasksProcessingOrder(QueueProcessingType.LIFO)

				.writeDebugLogs() // Remove for release app
				.build();

		ImageLoader.getInstance().init(config);

	}

	public String getDeviceId() {
		return deviceId;
	}

	private List<Activity> mActivityList = new ArrayList<Activity>();

	public void addActivity(Activity activity) {
		mActivityList.add(activity);
	}

	public void removeActivity(Activity activity) {
		mActivityList.remove(activity);
	}

	public Boolean activityTaskStackContains(Activity activity) {
		return mActivityList.contains(activity);
	}

	public Boolean isActivityStackTop() {
		return mActivityList.size() <= 1 ? true : false;
	}

	public String getVersionCode() throws NameNotFoundException {
		PackageManager manager = getPackageManager();
		PackageInfo info = manager.getPackageInfo(getPackageName(), 0);
		String versionName = info.versionName;
		return versionName;
	}

	public static DisplayImageOptions getOptions() {
		if (mOptions == null) {
			mOptions = new DisplayImageOptions.Builder().showImageOnLoading(R.drawable.logo_gray)
					.showImageForEmptyUri(R.drawable.logo_gray).showImageOnFail(R.drawable.logo_gray)
					.cacheInMemory(true).cacheOnDisk(true).build();
		}
		return mOptions;
	}

	public static ImageLoadingListener getLoadingListener() {
		if (mLoadingListener == null) {
			mLoadingListener = new ImageLoadingListener() {

				@Override
				public void onLoadingStarted(String imageUri, View view) {
					// TODO Auto-generated method stub

				}

				@Override
				public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
					// TODO Auto-generated method stub

				}

				@Override
				public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {

					if (imageUri.equals(view.getTag().toString())) {
						if (view.getWidth() > 0) {
							// loadedImage
						}
						((ImageView) view).setImageBitmap(loadedImage);
						/*
						 * ((ImageView) view).setBackgroundDrawable(new
						 * BitmapDrawable(getInstance().getResources(), loadedImage));
						 */
					}

				}

				@Override
				public void onLoadingCancelled(String imageUri, View view) {
					// TODO Auto-generated method stub

				}
			};
		}
		return mLoadingListener;
	}

	public DisplayMetrics getDeviceInfo(Activity activity) {
		// 获取屏幕像素相关信息
		DisplayMetrics dm = new DisplayMetrics();
		activity.getWindowManager().getDefaultDisplay().getMetrics(dm);
		return dm;
	}
}
