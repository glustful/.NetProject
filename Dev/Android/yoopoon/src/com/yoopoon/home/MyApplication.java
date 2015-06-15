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
import android.telephony.TelephonyManager;

import com.nostra13.universalimageloader.cache.disc.impl.UnlimitedDiskCache;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;
import com.nostra13.universalimageloader.utils.StorageUtils;

@EApplication
public class MyApplication extends Application {

	protected static final String TAG = "MyApplication";

	private static MyApplication mInstance;
	
	private String deviceId;
   
	
	/**
	 * 获取全局Application
	 * 
	 * @return
	 */
	public static MyApplication getInstance() {
		return mInstance;
	}

	@Override
	public void onCreate() {
		
		mInstance = this;
		
		TelephonyManager tm = (TelephonyManager)getSystemService(TELEPHONY_SERVICE);
		deviceId = tm.getDeviceId();
		
        initImageLoader();
		super.onCreate();
	}
	
	
	
	
	private void initImageLoader(){
		File meCacheDir = StorageUtils.getOwnCacheDirectory(this, "yoopoon/cache/imageloaderCache");
		ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(this)
		.threadPriority(Thread.NORM_PRIORITY - 2)
		.memoryCache(new WeakMemoryCache())
		.denyCacheImageMultipleSizesInMemory()
		.diskCacheFileNameGenerator(new Md5FileNameGenerator())
		.diskCacheSize(100 * 1024 * 1024) // 100 Mb
		.diskCacheFileCount(200)
		.diskCache(new UnlimitedDiskCache(meCacheDir))//自定义缓存路径
		.tasksProcessingOrder(QueueProcessingType.LIFO)
		.writeDebugLogs() // Remove for release app
  		.build();
       ImageLoader.getInstance().init(config);
	}

	
	
	
	public String getDeviceId(){
		return deviceId;
	}
	
	
	
	
	
	private List<Activity> mActivityList = new ArrayList<Activity>();
	
	public void addActivity(Activity activity) {
		mActivityList.add(activity);
	}
	
	public void removeActivity(Activity activity) {
		mActivityList.remove(activity);
	}
	
	public Boolean activityTaskStackContains(Activity activity){
		return mActivityList.contains(activity);
	}
	
	public Boolean isActivityStackTop(){
		return mActivityList.size() <= 1 ?true: false;
	}
	
	
	
	public String getVersionCode() throws NameNotFoundException {
		PackageManager manager = getPackageManager();
		PackageInfo info = manager.getPackageInfo(getPackageName(), 0);
		String versionName = info.versionName;
		return versionName;
	}
	
	

}
