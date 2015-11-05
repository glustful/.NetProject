/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SmsService.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.service 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-31 上午9:35:22 
 * @version: V1.0   
 */
package com.yoopoon.home.service;

import java.util.regex.Matcher;
import java.util.regex.Pattern;
import android.app.Service;
import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.database.ContentObserver;
import android.database.Cursor;
import android.net.Uri;
import android.os.Handler;
import android.os.IBinder;
import com.yoopoon.common.base.utils.Utils;

/**
 * @ClassName: SmsService
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-31 上午9:35:22
 */
public class SmsService extends Service {
	private static final String TAG = "SmsService";

	@Override
	public IBinder onBind(Intent intent) {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public void onCreate() {
		super.onCreate();
		// registerReceiver();
		smsObserver = new SmsObserver(this, smsHandler);
		getContentResolver().registerContentObserver(SMS_INBOX, true, smsObserver);
	}

	private SmsObserver smsObserver;

	public Handler smsHandler = new Handler() {
		// 这里可以进行回调的操作
		// TODO

	};

	class SmsObserver extends ContentObserver {

		public SmsObserver(Context context, Handler handler) {
			super(handler);
		}

		@Override
		public void onChange(boolean selfChange) {
			super.onChange(selfChange);
			// 每当有新短信到来时，使用我们获取短消息的方法
			getSmsFromPhone();
		}
	}

	private Uri SMS_INBOX = Uri.parse("content://sms/");

	public void getSmsFromPhone() {
		ContentResolver cr = getContentResolver();
		// String[] projection = new String[] { "body" };// "_id", "address", "person",,
		// "date","type"
		// String where = " address = '1069046042316128' AND date >  " + (System.currentTimeMillis()
		// - 10 * 60 * 1000);
		Cursor cur = cr.query(SMS_INBOX, null, null, null, "date desc"); //
		if (null == cur)
			return;
		if (cur.moveToNext()) {
			String number = cur.getString(cur.getColumnIndex("address"));// 手机号
			String name = cur.getString(cur.getColumnIndex("person"));// 联系人姓名列表
			String body = cur.getString(cur.getColumnIndex("body"));
			String date = cur.getString(cur.getColumnIndex("date"));
			// Log.i(TAG, number + ":" + body + "---" + name + "....." + date);
			// 这里我是要获取自己短信服务号码中的验证码~~
			if ("1069046042316128".equals(number)) {
				Pattern pattern = Pattern.compile("[a-zA-Z0-9]{6}");
				Matcher matcher = pattern.matcher(body);
				if (matcher.find()) {
					String res = matcher.group();
					// Log.i(TAG, res);
					if (System.currentTimeMillis() - Long.parseLong(date) < 60000)
						Utils.sendBroadcastWithStringExtra(this, Utils.GET_CODE_ACTION, "Code", res);
				}
			}
		}
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
	}

}
