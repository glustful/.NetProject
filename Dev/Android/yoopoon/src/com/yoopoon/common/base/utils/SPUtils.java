/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SPUtils.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-26 上午11:27:31 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.preference.PreferenceManager;

public class SPUtils {

	public static void setIsAgentFromReceiver(Context context, boolean isFromReceiver) {
		SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(context);
		Editor editor = sp.edit();
		editor.putBoolean("isAgentFromReceiver", isFromReceiver);
		editor.commit();
	}

	public static boolean isBroker(Context context) {
		SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(context);
		return sp.getBoolean("isBroker", false);
	}

}
