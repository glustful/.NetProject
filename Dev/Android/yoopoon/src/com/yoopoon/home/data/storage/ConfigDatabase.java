package com.yoopoon.home.data.storage;

/**
 * 全局配置数据库对象
 * 用于处理应用程序全局配置数据库相关的操作
 * 
 */
public class ConfigDatabase extends LocalDatabase {
	private static ConfigDatabase _instance = null;

	public static ConfigDatabase instance() {
		synchronized (ConfigDatabase.class) {
			if (_instance == null) {
				_instance = new ConfigDatabase();
				_instance.init();
			}
		}
		return _instance;
	}

	@Override
	protected Boolean init() {
		// 打开配置数据库
		if (super.open(LocalPath.intance().configDatabaseFilePath)) {
			
			return true;
		}
		return false;
	}
}