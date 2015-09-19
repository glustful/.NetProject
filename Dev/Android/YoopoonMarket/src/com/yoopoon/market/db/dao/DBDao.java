package com.yoopoon.market.db.dao;

import java.util.ArrayList;
import java.util.List;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import com.yoopoon.market.db.DBOpenHelper;
import com.yoopoon.market.domain.Staff;

public class DBDao {
	private static final String TABLE_NAME = "cart";
	private DBOpenHelper dbOpenHelper;

	public DBDao(Context context) {
		dbOpenHelper = new DBOpenHelper(context);
	}

	public long add(Staff entity) {
		SQLiteDatabase db = dbOpenHelper.getWritableDatabase();
		ContentValues values = new ContentValues();
		values.put("title", entity.title);
		values.put("category", entity.category);
		values.put("imgurl", entity.image);
		values.put("price_counted", entity.price_counted);
		values.put("price_previous", entity.price_previous);
		values.put("amount", entity.count);
		values.put("productid", entity.productId);
		long result = db.insert(TABLE_NAME, null, values);
		db.close();
		return result;
	}

	public List<Staff> findAll() {
		List<Staff> list = new ArrayList<Staff>();
		SQLiteDatabase db = dbOpenHelper.getReadableDatabase();
		Cursor cursor = db.query(TABLE_NAME, null, null, null, null, null, "id desc");
		while (cursor.moveToNext()) {
			String title = cursor.getString(cursor.getColumnIndex("title"));
			String imgUrl = cursor.getString(cursor.getColumnIndex("imgurl"));
			String category = cursor.getString(cursor.getColumnIndex("category"));
			float price_counted = cursor.getFloat(cursor.getColumnIndex("price_counted"));
			float price_previous = cursor.getFloat(cursor.getColumnIndex("price_previous"));
			int amount = cursor.getInt(cursor.getColumnIndex("amount"));
			int id = cursor.getInt(cursor.getColumnIndex("id"));
			int productid = cursor.getInt(cursor.getColumnIndex("productid"));
			Staff entity = new Staff(title, category, imgUrl, amount, price_counted, price_previous, productid);
			entity.id = id;
			list.add(entity);
		}
		cursor.close();
		db.close();
		return list;
	}

	public boolean isExist(int productId) {
		SQLiteDatabase db = dbOpenHelper.getReadableDatabase();
		Cursor cursor = db.query(TABLE_NAME, null, "productid=?", new String[] { productId + "" }, null, null, null);
		boolean result = cursor.moveToFirst();
		cursor.close();
		db.close();
		return result;
	}

	public int isExistCount(int productId) {

		SQLiteDatabase db = dbOpenHelper.getReadableDatabase();
		Cursor cursor = db.query(TABLE_NAME, null, "productid=?", new String[] { productId + "" }, null, null, null);
		if (cursor.moveToFirst()) {
			int count = cursor.getInt(cursor.getColumnIndex("amount"));
			return count;
		}
		cursor.close();
		db.close();
		return 0;
	}

	public boolean delete(int id) {
		SQLiteDatabase db = dbOpenHelper.getWritableDatabase();
		int result = db.delete(TABLE_NAME, "id=?", new String[] { id + "" });
		db.close();
		return result == -1 ? false : true;
	}

	public int modify(Staff entity) {
		SQLiteDatabase db = dbOpenHelper.getWritableDatabase();

		ContentValues values = new ContentValues();
		values.put("title", entity.title);
		values.put("category", entity.category);
		values.put("imgurl", entity.image);
		values.put("price_counted", entity.price_counted);
		values.put("price_previous", entity.price_previous);
		values.put("amount", entity.count);
		int result = db.update(TABLE_NAME, values, "id=?", new String[] { entity.id + "" });
		db.close();
		return result;
	}

	public int updateCount(int productid, int count) {
		SQLiteDatabase db = dbOpenHelper.getWritableDatabase();
		ContentValues values = new ContentValues();
		values.put("amount", count);
		int result = db.update(TABLE_NAME, values, "productid=?", new String[] { productid + "" });
		db.close();
		return result;
	}

	public List<Staff> findPart(int offset, int limit) {
		List<Staff> staffList = new ArrayList<Staff>();
		SQLiteDatabase db = dbOpenHelper.getReadableDatabase();
		Cursor cursor = db.rawQuery("select * from cart order by id desc limit ?,?", new String[] { offset + "",
				limit + "" });
		while (cursor.moveToNext()) {
			String title = cursor.getString(cursor.getColumnIndex("title"));
			String imgUrl = cursor.getString(cursor.getColumnIndex("imgurl"));
			String category = cursor.getString(cursor.getColumnIndex("category"));
			float price_counted = cursor.getFloat(cursor.getColumnIndex("price_counted"));
			float price_previous = cursor.getFloat(cursor.getColumnIndex("price_previous"));
			int amount = cursor.getInt(cursor.getColumnIndex("amount"));
			int id = cursor.getInt(cursor.getColumnIndex("id"));
			int productid = cursor.getInt(cursor.getColumnIndex("productid"));
			Staff entity = new Staff(title, category, imgUrl, amount, price_counted, price_previous, productid);
			entity.id = id;
			staffList.add(entity);
		}
		cursor.close();
		db.close();
		return staffList;
	}

	public int getAllCounts() {
		int sum = 0;
		SQLiteDatabase db = dbOpenHelper.getReadableDatabase();
		Cursor cursor = db.query(TABLE_NAME, null, null, null, null, null, "id desc");
		while (cursor.moveToNext()) {
			int amount = cursor.getInt(cursor.getColumnIndex("amount"));
			sum += amount;
		}
		cursor.close();
		db.close();
		return sum;
	}
}
