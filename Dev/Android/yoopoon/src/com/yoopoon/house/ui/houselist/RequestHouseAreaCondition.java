/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RequestProvinceArea.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: TODO
 * @author: king  
 * @updater: king 
 * @date: 2015年7月17日 下午5:18:08 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.houselist;

import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;

import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: RequestProvinceArea
 * @Description:
 * @author: 徐阳会
 * @date: 2015年7月17日 下午5:18:08
 */
public class RequestHouseAreaCondition {
	private ArrayList<JSONObject> provinceJsonObjects = new ArrayList<JSONObject>();

	public static void requestHouseArea(final Callback callback) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("AreaList");
					callback.callback(list);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl("/api/Condition/GetCondition").setRequestMethod(RequestMethod.eGet).notifyRequest();
	}
	public static void requestHouseArea(String parentIdValue, final Callback callback) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("AreaList");
					callback.callback(list);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl("/api/Condition/GetCondition").setRequestMethod(RequestMethod.eGet)
				.addParam("parentId", parentIdValue).notifyRequest();
	}

	public interface Callback {
		void callback(JSONArray jsonArray);
	}
}
