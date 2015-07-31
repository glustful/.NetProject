package com.yoopoon.common.base.utils;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.view.inputmethod.InputMethodManager;

public class Utils {
	private static final String TAG = "Utils";

	public static void hiddenSoftBorad(Context context) {
		try {
			((InputMethodManager) context.getSystemService(Context.INPUT_METHOD_SERVICE)).hideSoftInputFromWindow(
					((Activity) context).getCurrentFocus().getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
		} catch (Exception e) {

		}
	}

	public static final String GET_CODE_ACTION = "com.yoopoon.home.GET_CODE_ACTION";

	public static void sendBroadcastWithStringExtra(Context context, String action, String extraName, String extraValue) {
		Intent intent = new Intent(action);
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		intent.putExtra(extraName, extraValue);
		// Log.i(TAG, extraName + " : " + extraValue);
		context.sendBroadcast(intent);
	}

}
