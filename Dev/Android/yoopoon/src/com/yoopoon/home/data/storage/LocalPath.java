package com.yoopoon.home.data.storage;

import java.io.File;

import android.os.Environment;

/**
 * 缓存路径对象
 * 用于处理本地路径的名称及层级规则
 * 同时包含了部分配置文件的名称和路径
 * 
 */
public class LocalPath {
	/* 路径配置 */
	// 应用程序基础目录
	private static final String APP_BASE_DIR = "yoopoon";
	// 本地缓存基础目录
	private static final String CACHE_BASE_DIR = "cache";
	// 本地临时缓存基础目录
	private static final String TEMP_CACHE_BASE_DIR = "temp";
	// 本地设置基础目录
	private static final String CONFIG_BASE_DIR = "config";
	// 本地全局设置数据库文件名称
	private static final String CONFIG_DB_FILE_NAME = "config.db";
	// 用户缓存数据库基础目录
	private static final String USER_CACHE_DB_BASE_DIR = "database";
	// 用户缓存数据库文件名称
	public static final String USER_CACHE_DB_FILE_NAME = "cache.db";
	// 用户缓存文档基础目录
	private static final String USER_CACHE_DOC_BASE_DIR = "doc";
	// 应用程序基础路径，是所有本地数据的根目录，已添加末尾的'/'
	public String appBasePath = null;
	// 本地缓存基础路径，是所有缓存数据的根目录，已添加末尾的'/'
	public String cacheBasePath = null;
	// 本地临时缓存基础路径，是所有临时缓存数据的根目录，已添加末尾的'/'
	public String tempCacheBasePath = null;
	// 本地设置基础路径，是所有设置数据的根目录，已添加末尾的'/'
	public String configBasePath = null;
	// 本地全局设置数据库文件路径，是所有本地全局设置数据库文件的完整路径
	public String configDatabaseFilePath = null;
	// 用户缓存基础路径，是特定用户所有缓存数据的根目录，已添加末尾的'/'
	
	//   在初始化用户信息后有效
	public String userCacheBasePath = null;
	// 用户缓存数据库基础路径，是特定用户所有数据库缓存数据的根目录，已添加末尾的'/'
	// 注意：
	//   在初始化用户信息后有效
	public String userCacheDatabaseBasePath = null;
	// 用户缓存数据库文件路径，是特定用户数据库缓存文件的完整路径
	// 注意：
	//   在初始化用户信息后有效
	public String userCacheDatabaseFilePath = null;
	// 用户缓存文档基础路径，是特定用户所有文档缓存数据的根目录，已添加末尾的'/'
	// 注意：
	//   在初始化用户信息后有效
	public String userCacheDocBasePath = null;
	// 用户设置基础路径，是特定用户所有设置数据的根目录，已添加末尾的'/'
	
	//   在初始化用户信息后有效
	public String userConfigBasePath = null;

	// 确认目录存在
	public static void makePath(String path) {
		File file = new File(path);
		if (!file.exists()) {
			file.mkdirs();
		}
	}

	private static LocalPath _instance = null;

	public static LocalPath intance() {
		synchronized (LocalPath.class) {
			if (_instance == null) {
				_instance = new LocalPath();
				_instance.init();
			}
		}
		return _instance;
	}

	protected void init() {
		// 初始化应用程序基础路径
		appBasePath = String.format("%s/%s/", Environment.getExternalStorageDirectory().getPath(), APP_BASE_DIR);// /mnt/sdcard

		LocalPath.makePath(appBasePath);
		// 初始化本地缓存基础路径
		cacheBasePath = String.format("%s%s/", appBasePath, CACHE_BASE_DIR);
		LocalPath.makePath(cacheBasePath);
		// 初始化本地临时缓存基础路径
		tempCacheBasePath = String.format("%s%s/", cacheBasePath, TEMP_CACHE_BASE_DIR);
		LocalPath.makePath(tempCacheBasePath);
		// 初始化本地设置基础路径
		configBasePath = String.format("%s%s/", appBasePath, CONFIG_BASE_DIR);
		LocalPath.makePath(configBasePath);
		// 初始化本地全局设置数据库文件路径
		configDatabaseFilePath = String.format("%s%s", configBasePath, CONFIG_DB_FILE_NAME);
	}

	public Boolean setUser(String orgCode, String userCode) {
		String orgDir = orgCode;
		String userDir = userCode;
		if (orgDir != null && userDir != null) {
			// 初始化用户缓存基础路径
			userCacheBasePath = String.format("%s%s/%s/", cacheBasePath, orgDir, userDir);
			LocalPath.makePath(userCacheBasePath);
			// 初始化用户缓存数据库基础路径
			userCacheDatabaseBasePath = String.format("%s%s/", userCacheBasePath, USER_CACHE_DB_BASE_DIR);
			LocalPath.makePath(userCacheDatabaseBasePath);
			// 初始化用户缓存数据库文件路径
			userCacheDatabaseFilePath = String.format("%s%s", userCacheDatabaseBasePath, USER_CACHE_DB_FILE_NAME);
			// 初始化用户缓存文档基础路径
			userCacheDocBasePath = String.format("%s%s/", userCacheBasePath, USER_CACHE_DOC_BASE_DIR);
			LocalPath.makePath(userCacheDocBasePath);
			// 初始化用户设置基础路径
			userConfigBasePath = String.format("%s%s/%s/", configBasePath, orgDir, userDir);
			LocalPath.makePath(userConfigBasePath);

			return true;
		}
		return false;
	}
}
