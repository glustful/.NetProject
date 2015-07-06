package com.yoopoon.home.data.storage;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Set;

import com.yoopoon.common.base.Tools;

import android.content.ContentValues;
import android.util.Log;


/**
 * 本地数据库SQL辅助类
 * 用于辅助生成常用SQL命令
 *
 */
public class SqlCmd {
	// Sqlite字段数据类型
	public static final String COL_TYPE_NULL = "NULL";
	public static final String COL_TYPE_INTEGER = "INTEGER";
	public static final String COL_TYPE_REAL = "REAL";
	public static final String COL_TYPE_TEXT = "TEXT";
	public static final String COL_TYPE_BLOB = "BLOB";
	// Sqlite数据库主表
	private static final String SQLITE_TABLE_MASTER = "sqlite_master";
	// 表名
	private String tbName = "";
	// 列名
	private HashMap<String, Object> tbCols = new HashMap<String, Object>();
	// SQL语句
	private String sql = "";
	// SQL参数
	private ArrayList<String> ps = new ArrayList<String>();

	public static void showRows(List<ContentValues> rows) {
		Log.d("SQL_SHOW", String.format("%d rows:", rows.size()));
		for (ContentValues row : rows) {
			Log.d("SQL_SHOW", row.toString());
		}
	}

	public static SqlCmd isTableExist(String tbName) {
		return new SqlCmd(SQLITE_TABLE_MASTER).hasRow("TYPE = ? AND NAME = ?").ps("table", tbName);
	}

	public static boolean boolOfCol(String value) {
		return Boolean.valueOf(value);
	}

	public static Date dateOfCol(Long value) {
		if (value == null) {
			return null;
		} else {
			return new Date(value);
		}
	}

	public SqlCmd(String tbName) {
		this.tbName = tbName;
	}

	public SqlCmd col(String name, Object value) {
		tbCols.put(name, value);
		return this;
	}

	public SqlCmd sql(String sql) {
		this.sql = sql;
		return this;
	}

	public SqlCmd sql(String format, Object... args) {
		this.sql = String.format(format, args);
		return this;
	}

	public SqlCmd ps(Object... params) {
		for (Object pa : params) {
			ps.add(Tools.stringOfObject(pa));
		}
		return this;
	}

	public SqlCmd hasRow(String format, Object... args) {
		return select("COUNT(*) AS count").where(format, args);
	}

	public SqlCmd createTable() {
		String cs = "_id INTEGER PRIMARY KEY AUTOINCREMENT";
		Set<String> keys = tbCols.keySet();
		for (String key : keys) {
			cs += String.format(", %s %s", key, tbCols.get(key));
		}
		return sql("CREATE TABLE IF NOT EXISTS %s(%s)", tbName, cs);
	}

	private String appandNext(String val, String next) {
		if (val.length() > 0) {
			val += ", ";
		}
		val += next;
		return val;
	}

	public SqlCmd insertInto() {
		String cs = "";
		String vs = "";
		Set<String> keys = tbCols.keySet();
		for (String key : keys) {
			cs = appandNext(cs, key);
			vs = appandNext(vs, "?");
			ps(tbCols.get(key));
		}
		return sql("INSERT INTO %s(%s) VALUES(%s)", tbName, cs, vs);
	}
	
	
	

	public SqlCmd update() {
		String cs = "";
		Set<String> keys = tbCols.keySet();
		for (String key : keys) {
			cs = appandNext(cs, String.format("%s = ?", key));
			ps(tbCols.get(key));
		}
		return sql("UPDATE %s SET %s", tbName, cs);
	}

	public SqlCmd select(String format, Object... args) {
		return sql("SELECT %s FROM %s AS A", String.format(format, args), tbName);
	}

	public SqlCmd join(String format, Object... args) {
		return sql("%s %s", sql, String.format(format, args));
	}

	public SqlCmd delete() {
        return sql("DELETE FROM %s", tbName);
    }

    public SqlCmd replace(String colName, String oldName, String newName ) {
        return sql("DELETE FROM %s (%s) VALUES (%s,%s)", tbName,colName,oldName,newName);
    }
   



	public SqlCmd where(String format, Object... args) {
		return sql("%s WHERE %s", sql, String.format(format, args));
	}

	public SqlCmd orderBy(String format, Object... args) {
		return sql("%s ORDER BY %s", sql, String.format(format, args));
	}

	public SqlCmd limit(int l) {
		return sql("%s LIMIT %d", sql, l);
	}

	public Boolean hasParams() {
		return ps.size() > 0;
	}

	public String getSql() {
		return sql;
	}

	public Object[] getObjectParams() {
		return ps.toArray();
	}

	public String[] getStringParams() {
		String[] arr = new String[ps.size()];
		ps.toArray(arr);
		return arr;
	}
}
