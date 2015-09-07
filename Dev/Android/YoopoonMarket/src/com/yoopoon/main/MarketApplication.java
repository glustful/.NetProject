/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MyApplication.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.main 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月7日 下午2:27:10 
 * @version: V1.0   
 */
package com.yoopoon.main;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.app.Application;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.graphics.Bitmap;
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
import com.yoopoon.yoopoonmarket.R;

/** 
 * @ClassName: MyApplication 
 * @Description: 
 * @author: 徐阳会
 * @date: 2015年9月7日 下午2:27:10  
 */
public class MarketApplication extends Application {
	private static MarketApplication mMarketApplication;
	private static DisplayImageOptions mDisplayImageOptions;
	private static ImageLoadingListener mLoadingListener;
	private List<Activity> activityList = new ArrayList<Activity>();
	
	public MarketApplication getInstense() {
		return mMarketApplication;
	}
	/** 
	 * @Title: addActivity 
	 * @Description: 添加Activity到Activity栈中
	 * @param activity
	 */
	public void addActivity(Activity activity) {
		activityList.add(activity);
	}
	/** 
	 * @Title: removeActivity 
	 * @Description: 从Activity栈中移除Activity
	 * @param activity
	 */
	public void removeActivity(Activity activity) {
		activityList.remove(activity);
	}
	/** 
	 * @Title: isActivityInTaskStack 
	 * @Description: 判断Activity是否在任务栈中
	 * @param activity
	 * @return
	 */
	public boolean isActivityInTaskStack(Activity activity) {
		return activityList.contains(activity);
	}
	/** 
	 * @Title: isActivityInTaskStackTop 
	 * @Description: 判断Activity任务栈中是否只有一个任务或者没有任务
	 * @return
	 */
	public boolean isActivityInTaskStackTop() {
		return activityList.size() <= 1 ? true : false;
	}
	@Override
	public void onCreate() {
		super.onCreate();
		mMarketApplication = this;
		initImageLoader();
	}
	/** 
	 * @Title: getAppVersionCode 
	 * @Description: 获取应用程序版本编码
	 * @return
	 * @throws NameNotFoundException
	 */
	public String getAppVersionCode() throws NameNotFoundException {
		PackageManager manager = getPackageManager();
		PackageInfo info = manager.getPackageInfo(getPackageName(), 0);
		return info.versionCode + "";
	}
	/** 
	 * @Title: getAppVersionName 
	 * @Description: 获取应用程序版本名称
	 * @return
	 * @throws NameNotFoundException
	 */
	public String getAppVersionName() throws NameNotFoundException {
		PackageManager manager = getPackageManager();
		PackageInfo info = manager.getPackageInfo(getPackageName(), 0);
		return info.versionName + "";
	}
	/** 
	 * @Title: initImageLoader 
	 * @Description: 初始化应用程序级别的ImageLoader组件，设置相应的加载参数
	 */
	private void initImageLoader() {
		File meCacheDir = StorageUtils.getOwnCacheDirectory(this, "yoopoon/cache/imageloaderCache");
		ImageLoaderConfiguration config = new ImageLoaderConfiguration.Builder(this)
				.threadPriority(Thread.NORM_PRIORITY - 2).memoryCache(new WeakMemoryCache())
				.denyCacheImageMultipleSizesInMemory().diskCacheFileNameGenerator(new Md5FileNameGenerator())
				.diskCacheSize(100 * 1024 * 1024).diskCacheFileCount(200).diskCache(new UnlimitedDiskCache(meCacheDir))
				.tasksProcessingOrder(QueueProcessingType.LIFO).writeDebugLogs().build();
		ImageLoader.getInstance().init(config);
	}
	/** 
	 * @Title: getOptions 
	 * @Description: Imageloader默认加载参数设置
	 * @return
	 */
	public static DisplayImageOptions getOptions() {
		if (mDisplayImageOptions == null) {
			mDisplayImageOptions = new DisplayImageOptions.Builder().showImageOnLoading(R.drawable.logo_gray)
					.showImageForEmptyUri(R.drawable.logo_gray).showImageOnFail(R.drawable.logo_gray)
					.cacheInMemory(true).cacheOnDisk(true).build();
		}
		return mDisplayImageOptions;
	}
	/** 
	 * @Title: getImageLoadingListener 
	 * @Description: TODO
	 * @return
	 */
	public static ImageLoadingListener getImageLoadingListener() {
		if (mLoadingListener == null) {
			mLoadingListener = new ImageLoadingListener() {
				@Override
				public void onLoadingStarted(String imageUri, View view) {
				}
				@Override
				public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
				}
				@Override
				public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
					if (imageUri.equals(view.getTag().toString())) {
						if (view.getWidth() > 0) {
							// loadedImage
						}
						// ((ImageView) view).setScaleType(ScaleType.FIT_XY);
						((ImageView) view).setImageBitmap(loadedImage);
					}
				}
				@Override
				public void onLoadingCancelled(String imageUri, View view) {
				}
			};
		}
		return mLoadingListener;
	};
}
