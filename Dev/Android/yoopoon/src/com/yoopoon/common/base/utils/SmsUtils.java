/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SmsUtils.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 上午10:51:53 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

import android.content.Context;
import android.util.Log;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: SmsUtils
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 上午10:51:53
 */
public class SmsUtils {

	// 注册=0,修改密码=1,找回密码=2,添加银行卡=3,佣金提现=4,添加合伙人=5,推荐经纪人=6
	public static final int REGISTER_IDENTIFY_CODE = 0;
	public static final int CHANGEPSW_IDENTIFY_CODE = 1;
	public static final int FINDPSW_IDENTIFY_CODE = 2;
	public static final int ADD_BANKCARD_IDENTIFY_CODE = 3;
	public static final int TAKECASH_IDENTIFY_CODE = 4;
	public static final int ADD_PARTNER_IDENTIFY_CODE = 5;
	public static final int RECOMMED_BROKER_IDENTIFY_CODE = 6;
	private static final String TAG = "SmsUtils";

	public static void requestIdentifyCode(Context context, final String json, final RequestSMSListener lis) {

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData().optString("Message", "").equals("1")) {
						lis.succeed(data.getMRootData().optString("Desstr"));
						return;
					}
				}
				if (data.getMsg().contains("Value 1 of type java.lang.Integer")) {
					lis.succeed("邀请已发送");
					return;
				}
				lis.fail(data.getMsg());

			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(context.getString(R.string.url_sms_sendSMS)).SetJSON(json).notifyRequest();
	}

	public interface RequestSMSListener {

		void fail(String msg);

		void succeed(String code);
	}

	public static void getCodeForBroker(Context context, String json, final RequestSMSListener lis) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					Log.i(TAG, data.toString());
					if (data.getMRootData().optString("Message", "").equals("1")) {
						lis.succeed(data.getMRootData().optString("Desstr"));
						return;
					}
				}
				lis.fail(data.getMsg());
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(context.getString(R.string.url_getcode_forbroker)).SetJSON(json).notifyRequest();
	}

}
