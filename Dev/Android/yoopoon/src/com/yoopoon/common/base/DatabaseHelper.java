package com.yoopoon.common.base;

import java.util.ArrayList;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.DatabaseUtils;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

/**
 * 数据库处理对象
 * 用于封装对SQLite数据库的相关操作
 
 */
public class DatabaseHelper {
	private SQLiteDatabase db = null;

	private Boolean isOpen() {
		return (db != null) && (db.isOpen());
	}

	private Boolean checkSql(String sql) {
		return (sql != null) && (sql.length() > 0);
	}

	private Boolean checkArgs(Object[] bindArgs) {
		return (bindArgs != null) && (bindArgs.length > 0);
	}

	public Boolean openOrCreateDatabase(String dbPath) {
		try {
			db = SQLiteDatabase.openOrCreateDatabase(dbPath, null);
			return true;
		} catch (SQLException e) {
			Log.d("DatabaseHelper", "DatabaseHelper openOrCreateDatabase failed: " + e.toString());
		}
		return false;
	}

	public void close() {
		if (isOpen()) {
			db.close();
		}
	}

	// 注意：
	//    由于Sqlite单挑数据库操作都会起动一次事务，导致非事务中的数据库操作性能非常慢
	//    请将大量数据库操作放在一次事务中集中处理
	public void onTransaction(OnTransaction onTransaction) {
		if (isOpen() && onTransaction != null) {
			// 手动开始事务
			db.beginTransaction();
			try {
				onTransaction.transaction();

				// 设置事务处理成功，不设置会自动回滚不提交
				db.setTransactionSuccessful();
			} catch (Exception e) {
				e.printStackTrace();
			} finally {
				// 提交事务
				db.endTransaction();
			}
		}
	}

	public Boolean execSQL(String sql) {
		if (isOpen() && checkSql(sql)) {
			try {
				db.execSQL(sql);
				return true;
			} catch (SQLException e) {
				Log.d("DatabaseHelper", "DatabaseHelper execSQL failed: " + e.toString());
			}
		}
		return false;
	}

	public Boolean execSQL(String sql, Object... bindArgs) {
		if (isOpen() && checkSql(sql) && checkArgs(bindArgs)) {
			try {
				db.execSQL(sql, bindArgs);
				return true;
			} catch (SQLException e) {
				Log.d("DatabaseHelper", "DatabaseHelper execSQL failed: " + e.toString());
			}
		}
		return false;
	}
	
	public Cursor queryCursor(String sql,String... bindArgs){
		if(isOpen() && checkSql(sql)){
			return db.rawQuery(sql, bindArgs);
		}
		return null;
	}

	public ArrayList<ContentValues> query(OnEachRow onEachRow, Object cbData, String sql, String... bindArgs) {
		ArrayList<ContentValues> rs = new ArrayList<ContentValues>();
		if (isOpen() && checkSql(sql)) {
			try {
				Cursor c = db.rawQuery(sql, bindArgs);
				c.moveToFirst();
				while (!c.isAfterLast()) {
					ContentValues row = new ContentValues();
					DatabaseUtils.cursorRowToContentValues(c, row);
					rs.add(row);

					// 回调外部处理每一行数据
					if (onEachRow != null) {
						onEachRow.eachRow(row, cbData);
					}

					c.moveToNext();
				}
				c.close();
			} catch (SQLException e) {
				Log.d("DatabaseHelper", "DatabaseHelper rawQuery failed: " + e.toString());
			}
		}
		return rs;
	}
}
