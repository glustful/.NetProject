/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RequestProvinceArea.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: 链接网络，获取楼盘区域数据
 * @author: 徐阳会  
 * @updater: 徐阳会 
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
 * @Description:链接网络，获取楼盘区域信息类（异步线程）
 * @author: 徐阳会
 * @date: 2015年7月17日 下午5:18:08
 */
public class RequestHouseAreaCondition {
	public static final String LOGTAG = "RequestHouseAreaCondition:请求网络获取楼盘区域";
	private ArrayList<JSONObject> provinceJsonObjects = new ArrayList<JSONObject>();
	
	/** 
	 * @Title: requestHouseProvinceArea 
	 * @Description: 获取楼盘省份数据
	 * @param mCallback
	 */
	public static void requestHouseProvinceArea(final Callback mCallback) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("AreaList");
					mCallback.callback(list);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl("/api/Condition/GetCondition").setRequestMethod(RequestMethod.eGet).notifyRequest();
	}
	/** 
	 * @Title: requestHouseCityArea 
	 * @Description: 获取楼盘市所属城市数据
	 * @param parentIdValue 传入的省份ID
	 * @param callback 回调结果
	 */
	public static void requestHouseCityArea(String parentIdValue, final Callback callback) {
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
	/** 
	 * @Title: requestHouseDistrictArea 
	 * @Description: 获取楼盘所属县区数据
	 * @param parentIdValue 传入城市ID
	 * @param callback 回调参数
	 */
	public static void requestHouseDistrictArea(String parentIdValue, final Callback callback) {
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
