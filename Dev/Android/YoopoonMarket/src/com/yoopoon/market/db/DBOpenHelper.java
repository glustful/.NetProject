package com.yoopoon.market.db;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class DBOpenHelper extends SQLiteOpenHelper {

	public DBOpenHelper(Context context) {
		super(context, "yoopoonmarket.db", null, 1);
	}

	@Override
	public void onCreate(SQLiteDatabase db) {
		String sql = "create table cart ("
				+ "id integer primary key autoincrement ,title varchar(50), imgurl varchar(50), category varchar(50), "
				+ "price_counted float, price_previous float, amount integer, productid integer)";
		db.execSQL(sql);
	}

	@Override
	public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {

	}

}
