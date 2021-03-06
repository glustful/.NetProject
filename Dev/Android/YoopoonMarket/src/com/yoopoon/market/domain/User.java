package com.yoopoon.market.domain;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.text.TextUtils;
import com.yoopoon.market.R;

public class User {

	public static boolean isLogin(Context context) {
		SharedPreferences sp = context.getSharedPreferences(context.getString(R.string.share_preference),
				Context.MODE_PRIVATE);
		return !(TextUtils.isEmpty(sp.getString("UserName", ""))) && !(TextUtils.isEmpty(sp.getString("Password", "")))
				&& sp.getInt("UserId", 0) != 0;
	}

	public static String getUserId(Context context) {
		SharedPreferences sp = context.getSharedPreferences(context.getString(R.string.share_preference),
				Context.MODE_PRIVATE);
		return String.valueOf(sp.getInt("UserId", 0));
	}

	public static String getUserName(Context context) {
		SharedPreferences sp = context.getSharedPreferences(context.getString(R.string.share_preference),
				Context.MODE_PRIVATE);
		return sp.getString("UserName", "");
	}

	public static void setProperty(Context context, String key, String value) {
		SharedPreferences sp = context.getSharedPreferences(context.getString(R.string.share_preference),
				Context.MODE_PRIVATE);
		Editor editor = sp.edit();
		editor.putString(key, value);
		editor.commit();
	}

	public static String getProperty(Context context, String key) {
		SharedPreferences sp = context.getSharedPreferences(context.getString(R.string.share_preference),
				Context.MODE_PRIVATE);
		return sp.getString(key, "");
	}
}
