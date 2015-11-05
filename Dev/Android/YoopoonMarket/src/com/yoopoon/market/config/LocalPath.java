package com.yoopoon.market.config;

import java.io.File;
import android.os.Environment;

public class LocalPath {
	private static final String APP_BASE_DIR = "yoopoon";
	private static final String CACHE_BASE_DIR = "cache";
	private static final String TEMP_CACHE_BASE_DIR = "temp";
	private static final String CONFIG_BASE_DIR = "config";
	private static final String CONFIG_DB_FILE_NAME = "config.db";
	private static final String USER_CACHE_DB_BASE_DIR = "database";
	public static final String USER_CACHE_DB_FILE_NAME = "cache.db";
	private static final String USER_CACHE_DOC_BASE_DIR = "doc";
	public String appBasePath = null;
	public String cacheBasePath = null;
	public String tempCacheBasePath = null;
	public String configBasePath = null;
	public String configDatabaseFilePath = null;

	public String userCacheBasePath = null;
	public String userCacheDatabaseBasePath = null;
	public String userCacheDatabaseFilePath = null;
	public String userCacheDocBasePath = null;

	public String userConfigBasePath = null;

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
		appBasePath = String.format("%s/%s/", Environment.getExternalStorageDirectory().getPath(), APP_BASE_DIR);// /mnt/sdcard

		LocalPath.makePath(appBasePath);
		cacheBasePath = String.format("%s%s/", appBasePath, CACHE_BASE_DIR);
		LocalPath.makePath(cacheBasePath);
		tempCacheBasePath = String.format("%s%s/", cacheBasePath, TEMP_CACHE_BASE_DIR);
		LocalPath.makePath(tempCacheBasePath);
		configBasePath = String.format("%s%s/", appBasePath, CONFIG_BASE_DIR);
		LocalPath.makePath(configBasePath);
		configDatabaseFilePath = String.format("%s%s", configBasePath, CONFIG_DB_FILE_NAME);
	}

	public Boolean setUser(String orgCode, String userCode) {
		String orgDir = orgCode;
		String userDir = userCode;
		if (orgDir != null && userDir != null) {
			userCacheBasePath = String.format("%s%s/%s/", cacheBasePath, orgDir, userDir);
			LocalPath.makePath(userCacheBasePath);
			userCacheDatabaseBasePath = String.format("%s%s/", userCacheBasePath, USER_CACHE_DB_BASE_DIR);
			LocalPath.makePath(userCacheDatabaseBasePath);
			userCacheDatabaseFilePath = String.format("%s%s", userCacheDatabaseBasePath, USER_CACHE_DB_FILE_NAME);
			userCacheDocBasePath = String.format("%s%s/", userCacheBasePath, USER_CACHE_DOC_BASE_DIR);
			LocalPath.makePath(userCacheDocBasePath);
			userConfigBasePath = String.format("%s%s/%s/", configBasePath, orgDir, userDir);
			LocalPath.makePath(userConfigBasePath);

			return true;
		}
		return false;
	}
}
