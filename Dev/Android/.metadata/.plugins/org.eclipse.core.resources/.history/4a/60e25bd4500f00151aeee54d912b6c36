package com.yxst.epic.yixin;

import java.io.File;
import java.lang.ref.Reference;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.afinal.simplecache.ACache;
import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.EApplication;
import org.androidannotations.annotations.sharedpreferences.Pref;

import android.app.Activity;
import android.app.Application;
import android.app.NotificationManager;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.telephony.TelephonyManager;

import com.avos.avoscloud.AVACL;
import com.avos.avoscloud.AVAnalytics;
import com.avos.avoscloud.AVOSCloud;
import com.fasterxml.jackson.core.Version;
import com.miicaa.common.base.Debugger;
import com.miicaa.home.cast.PushMessage;
import com.miicaa.home.data.DataCenter;
import com.miicaa.home.data.business.account.AccountInfo;
import com.miicaa.home.data.old.UserAccount;
import com.miicaa.home.data.service.CacheCtrlSrv;
import com.miicaa.home.ui.home.FramMainActivity;
import com.miicaa.home.ui.login.HomeLoginActivity;
import com.miicaa.home.ui.login.HomeLoginActivity_;
import com.miicaa.perferences.MiicaaAuthorityPerf_;
import com.miicaa.service.ContactRefreshService;
import com.miicaa.service.EnterpriseService;
import com.miicaa.service.ProgressNotifyService;
import com.miicaa.service.VersionPanduanService;
import com.miicaa.utils.AllUtils;
import com.miicaa.utils.AuthorityUtils.AuthorityState;
import com.miicaa.utils.NetUtils.OnReloginListener;
import com.nostra13.universalimageloader.cache.disc.impl.UnlimitedDiskCache;
import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.cache.memory.BaseMemoryCache;
import com.nostra13.universalimageloader.cache.memory.impl.WeakMemoryCache;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.ImageLoaderConfiguration;
import com.nostra13.universalimageloader.core.assist.QueueProcessingType;
import com.nostra13.universalimageloader.utils.StorageUtils;
import com.yxst.epic.yixin.data.dto.model.Member;
import com.yxst.epic.yixin.data.dto.response.LoginResponse;
import com.yxst.epic.yixin.data.rest.IMInterface;
import com.yxst.epic.yixin.data.rest.YixinHost;
import com.yxst.epic.yixin.db.DaoMaster;
import com.yxst.epic.yixin.db.DaoSession;
import com.yxst.epic.yixin.download.DownloadManager;
import com.yxst.epic.yixin.download.DownloadService;
import com.yxst.epic.yixin.preference.CachePrefs_;
import com.yxst.epic.yixin.push.service.PushCliService;
import com.yxst.epic.yixin.rest.IMInterfaceProxy;
import com.yxst.epic.yixin.service.MsgService;
import com.yxst.epic.yixin.upload.UploadManager;
import com.yxst.epic.yixin.upload.UploadService;
import com.yxst.epic.yixin.utils.LockPatternUtils;
import com.yxst.epic.yixin.utils.SmileyParser;
@EApplication
public class MyApplication extends Application {

	protected static final String TAG = "MyApplication";

	private static MyApplication mInstance;
	private LockPatternUtils mLockPatternUtils;

	private static DaoMaster daoMaster;
	private static DaoSession daoSession;
	private String deviceId;
	//设置是付费用户还是非付费用户
	private String mUid;
	
	@Pref
	CachePrefs_ mCachePrefs;
	
	/**
	 * 全局控制权限
	 */
	@Pref
	MiicaaAuthorityPerf_ mMiicaaAuthorityPerfs;
	
	UploadManager mUploadManager;
	
	DownloadManager mDownloadManager;
	/**
	 * 获取全局Application
	 * 
	 * @return
	 */
	public static MyApplication getInstance() {
		return mInstance;
	}

	  private AccountInfo accountInfo = AccountInfo.instance();
	@Override
	public void onCreate() {
		AVOSCloud.initialize(this, 
				"aa4sxa2cczll15k1t3847guedto7d9vii96a408ikxlyew7j",
				"1xahe2e2di5ki4xlc28tesaa7xgbzzt7dimt3tg9uc60c509");
		AVAnalytics.enableCrashReport(this, true);

		mInstance = this;
		PushCliService.startService(this); 
		EnterpriseService.start(this);
		SmileyParser.init(this);
		mLockPatternUtils = new LockPatternUtils(this);
		TelephonyManager tm = (TelephonyManager)getSystemService(TELEPHONY_SERVICE);
		deviceId = tm.getDeviceId();
		mCachePrefs.getSharedPreferences()
				.registerOnSharedPreferenceChangeListener(
						mOnSharedPreferenceChangeListener);

		MsgService.getMsgWriter(this);
		mUploadManager = UploadService.getUploadManager(this);
			mDownloadManager = DownloadService.getDownloadManager(this);
		Debugger.init(this);
        DataCenter.intance();
        CacheCtrlSrv.init(this);
        initImageLoader();
		super.onCreate();
	}
	
	public LockPatternUtils getLockPatternUtils() {
		return mLockPatternUtils;
	}

	@Override
	public void onTerminate() {
		mCachePrefs.getSharedPreferences()
				.unregisterOnSharedPreferenceChangeListener(
						mOnSharedPreferenceChangeListener);

		super.onTerminate();
	}

	/**
	 * 取得DaoMaster
	 * 
	 * @param context
	 * @return
	 */
	public static DaoMaster getDaoMaster(Context context) {
		if (daoMaster == null) {
			DaoMaster.OpenHelper helper = new DaoMaster.DevOpenHelper(context,
					"message-db", null);
			daoMaster = new DaoMaster(helper.getWritableDatabase());
		}
		return daoMaster;
	}

	/**
	 * 取得DaoSession
	 * 
	 * @param context
	 * @return
	 */
	public static DaoSession getDaoSession(Context context) {
		if (daoSession == null) {
			if (daoMaster == null) {
				daoMaster = getDaoMaster(context);
			}
			daoSession = daoMaster.newSession();
		}
		return daoSession;
	}
	
	
	
	private void initImageLoader(){
		File meCacheDir = StorageUtils.getOwnCacheDirectory(this, "miicaa/cache/imageloaderCache");
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

	private OnSharedPreferenceChangeListener mOnSharedPreferenceChangeListener = new OnSharedPreferenceChangeListener() {
		@Override
		public void onSharedPreferenceChanged(
				SharedPreferences sharedPreferences, String key) {
			String uidOld = mUid;
			String uidNew = mCachePrefs.uid().get();
			mUid = uidNew;
			if (uidNew == null || (uidNew != null && !uidNew.equals(uidOld))) {
				PushCliService.startService(getApplicationContext());
			}
		}
	};

	public void putLoginResponse(LoginResponse response) {
		ACache.get(this).put("LoginResponse", response);

		if (response != null && response.Member != null) {
			Member member = response.Member;
			mCachePrefs.uid().put(member.Uid);
			mCachePrefs.userName().put(member.UserName);
			mCachePrefs.token().put(response.Token);
			
		}
	}
	
	public LoginResponse getLoginResponse() {
		return (LoginResponse) ACache.get(this).getAsObject("LoginResponse");
	}

	public String getUid() {
		return mCachePrefs.uid().get();
	}

	public String getLocalUserName() {
		return mCachePrefs.userName().get();
	}
	
	public String getLocalUserMiLiao(){
		String uid =  AccountInfo.instance().getLastUserInfo().getId()+""
				;
		return uid;
	}

	public Member getLocalMember() {
		LoginResponse response = (LoginResponse) ACache.get(this).getAsObject("LoginResponse");
		if (response != null) {
			return response.Member;
		}
		return null;
	}
	
	public String getToken() {
		return mCachePrefs.token().get();
	}

	
	public void onReset(){
		mMiicaaAuthorityPerfs.photoAuthority().put(AllUtils.NORMAL_User);
		mMiicaaAuthorityPerfs.subTaskAuth().put(AllUtils.NORMAL_User);
		mMiicaaAuthorityPerfs.approveProcessAuth().put(AllUtils.NORMAL_User);
		mCachePrefs.uid().put(null);
		mCachePrefs.userName().put(null);
		mCachePrefs.token().put(null);
//		Log.d("sharedpference_1", "uid:"+mCachePrefs.uid().get());
//		Log.d("sharedpference_1", "username:"+mCachePrefs.userName().get());
//		Log.d("sharedpference_1", "token:"+mCachePrefs.token().get());
	}
	
	public String getDeviceId(){
		return deviceId;
	}
	
	public void onReLogin() {
		 NotificationManager nm = (NotificationManager)getSystemService(NOTIFICATION_SERVICE);
		 nm.cancelAll();
		 PushMessage.stopPushMessage(this);//停止消息推送服务
		  ContactRefreshService.stop(this);//停止刷新通讯录
		onReset();
		Intent i = HomeLoginActivity_.intent(this).get();
		 Bundle bundle = new Bundle();
         bundle.putString("exit", "exit");
         i.putExtra("bundle", bundle);
         i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
         startActivity(i);
		/*
		 * 如果session超时，那么就重新登录
		 */
		for (Iterator<Activity> it = mActivityList.iterator(); it.hasNext();) {
			
			try {
				Activity activity = it.next();
				activity.finish();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}
	
	
	@Background
	public void relogin(OnReloginListener listener){
		onReset();
		IMInterface im = IMInterfaceProxy.create(20*1000);
		
		HomeLoginActivity.LoginYouxin(im, this,listener);
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
	
	public AccountInfo getAccountInfo() {
        return accountInfo;
    }
	
	
	public String getVersionCode() throws NameNotFoundException {
		PackageManager manager = getPackageManager();
		PackageInfo info = manager.getPackageInfo(getPackageName(), 0);
		String versionName = info.versionName;
		return versionName;
	}
	
	public UploadManager getUploadManager() {
		return mUploadManager;
	}
public DownloadManager getDownloadManager() {
		return mDownloadManager;
	}


    public  void setAuthority(AuthorityState state,int auhority){
    	if(state == AuthorityState.ePhoto){
    		mMiicaaAuthorityPerfs.photoAuthority().put(auhority);
    	}else if(state == AuthorityState.eSubTask){
    		mMiicaaAuthorityPerfs.subTaskAuth().put(auhority);
    	}else if(state == AuthorityState.eApproveProcess){
    		mMiicaaAuthorityPerfs.approveProcessAuth().put(auhority);
    	}
    }
    
    
    public int getAuthority(AuthorityState state){
    	if(state == AuthorityState.ePhoto){
    	 return	mMiicaaAuthorityPerfs.photoAuthority().get();
    	}else if(state == AuthorityState.eSubTask){
    	 return mMiicaaAuthorityPerfs.subTaskAuth().get();
    	}else if(state == AuthorityState.eApproveProcess){
    	 return	mMiicaaAuthorityPerfs.approveProcessAuth().get();
    	}
    	return AllUtils.NORMAL_User;
    }
}
