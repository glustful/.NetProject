package com.yoopoon.market.db.dao;

import android.content.ContentValues;
import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import com.yoopoon.market.db.DBOpenHelper;

public class DBDao {
	private static final String TABLE_NAME = "cart";
	private DBOpenHelper dbOpenHelper;

	public DBDao(Context context) {
		dbOpenHelper = new DBOpenHelper(context);
	}

	public long add(String title) {
		SQLiteDatabase db = dbOpenHelper.getWritableDatabase();
		ContentValues values = new ContentValues();
		values.put("title", title);
		long result = db.insert(TABLE_NAME, null, values);
		return result;
	}

}
