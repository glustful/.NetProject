package com.yoopoon.home.data.storage;

import java.util.ArrayList;

import com.yoopoon.common.base.DatabaseHelper;
import com.yoopoon.common.base.OnEachRow;
import com.yoopoon.common.base.OnTransaction;

import android.content.ContentValues;
import android.database.Cursor;
import android.util.Log;


/**
 * 本地数据库基类
 * 用于抽象本地数据库的通用操作
 * 
 */
public class LocalDatabase {
	private DatabaseHelper dbHelper = null;

	protected LocalDatabase() {
		dbHelper = new DatabaseHelper();
	}

	protected Boolean init() {
		return true;
	}

	public Boolean open(String path) {
		return dbHelper.openOrCreateDatabase(path);
	}

	public void close() {
		dbHelper.close();
	}

	public void onTransaction(OnTransaction onTransaction) {
		dbHelper.onTransaction(onTransaction);
	}

	public Boolean execSQL(String sql) {
		Log.d("SQL_COMMAND", sql);
		return dbHelper.execSQL(sql);
	}

	public Boolean execSQL(String sql, Object... bindArgs) {
		Log.d("SQL_COMMAND", sql);
		return dbHelper.execSQL(sql, bindArgs);
	}

	public ArrayList<ContentValues> query(OnEachRow onEachRow, Object cbData, String sql, String... bindArgs) {
        Log.d("SQL_COMMAND", sql);
        return dbHelper.query(onEachRow, cbData, sql, bindArgs);
    }
	
	public Cursor queryCursor(String sql,String... bindArgs){
		return dbHelper.queryCursor(sql, bindArgs);
	}

	public Boolean execCmd(SqlCmd cmd) {
		
		Boolean res = false;
		if (cmd.hasParams()) {
			res = execSQL(cmd.getSql(), cmd.getObjectParams());
		} else {
			res = execSQL(cmd.getSql());
		}
		
		return res;
	}

	public void execCmds(ArrayList<SqlCmd> cmds) {
		for (SqlCmd cmd : cmds) {
			execCmd(cmd);
		}
	}

	public ArrayList<ContentValues> queryCmd(SqlCmd cmd) {
		return queryCmd(cmd, null, null);//没有回调
	}

	public ArrayList<ContentValues> queryCmd(SqlCmd cmd, OnEachRow onEachRow, Object cbData) {
		
		ArrayList<ContentValues> res = query(onEachRow, cbData, cmd.getSql(), cmd.getStringParams());
		
		return res;
	}

	public ContentValues firstRow(SqlCmd cmd) {
		ArrayList<ContentValues> rows = queryCmd(cmd);
		if (rows.size() > 0) {
			return rows.get(0);
		}
		return null;
	}

	public Boolean hasRow(String tbName, String where, Object... params) {
		return hasRow(new SqlCmd(tbName).hasRow(where).ps(params));
	}

	public Boolean hasRow(SqlCmd cmd) {
		ContentValues row = firstRow(cmd);
		if (row != null) {
			return row.getAsInteger("count") > 0;
		}
		return false;
	}

	public Boolean isTableExsit(String tbName) {
		return hasRow(SqlCmd.isTableExist(tbName));
	}
}