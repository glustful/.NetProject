package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.json.JSONArray;
import org.json.JSONObject;

import android.R.integer;
import android.content.Context;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.appcompat.R.string;
import android.text.format.DateUtils;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.ListView;
import android.widget.TextView;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;

@EFragment()
/* @EFragment() */
public class FramHouseFragment extends FramSuper {
	public static final String LOGTAG = "FramHouseFragment";
	View rootView;
	HashMap<String, String> parameter;
	ArrayList<JSONObject> mJsonObjects;
	PullToRefreshListView listView;
	public static FramHouseFragment instance;

	public static FramHouseFragment getInstance() {
		return instance;
	}

	Context mContext;
	ListView refreshView;
	FramHouseListViewAdapter mHouseInfoAdapter;
	ADController mAdController;
	TextView houseTotalCountTextView;
	string houseTotaoCount = null;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater,
			@Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		Log.e(LOGTAG, "Fragment oncreateView");
		
		if (rootView != null) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(
					R.layout.home_fram_house_fragment, null);
			listView = (PullToRefreshListView) rootView
					.findViewById(R.id.matter_list_view);
			mContext = getActivity();
			houseTotalCountTextView = new TextView(mContext);
			instance = this;
			mAdController = new ADController(mContext);
			mJsonObjects = new ArrayList<JSONObject>();
			initParameter();

			initViews();
		}

		return rootView;
	}

	/**
	 * 初始化界面Fragment
	 */
	void initViews() {

		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.DISABLED);
		refreshView = listView.getRefreshableView();

		refreshView.addHeaderView(mAdController.getRootView());
		requestHouseTotalCount();

		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mHouseInfoAdapter = new FramHouseListViewAdapter(mContext);
		refreshView.setAdapter(mHouseInfoAdapter);
		requestAdvertisements();
		requestHouseList();
	}

	/**
	 * 检索所有房子Get传入参数
	 */
	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}

		parameter.clear();
		parameter.put("AreaName", "");
		parameter.put("OrderBy", "OrderByAddtime");
		parameter.put("IsDescending", "true");
		parameter.put("Page", "1");
		parameter.put("PageCount", "10");
		parameter.put("PriceBegin", "");
		parameter.put("PriceEnd", "");
		parameter.put("TypeId", "");
	}

	@UiThread
	public void initHouseTotalCountTextView(String houseTotaoCount) {
		LinearLayout.LayoutParams houseTotalCountParams = new LinearLayout.LayoutParams(
				LayoutParams.MATCH_PARENT, 100);
		houseTotalCountParams.gravity=Gravity.CENTER_HORIZONTAL;
		houseTotalCountTextView.setLayoutParams(houseTotalCountParams);
		houseTotalCountTextView.setText("总共" + houseTotaoCount + "个楼盘");
		houseTotalCountTextView.setBackgroundColor(0xBEBEBE);
		//houseTotalCountTextView.
		//refreshView.addHeaderView(houseTotalCountTextView);
	}

	@AfterInject
	void afterInject() {

	}

	/**
	 * 开启一个线程，连接网络，获取房屋列表
	 */
	private void requestHouseList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {

					JSONArray list = data.getMRootData().optJSONArray("List");
					String houseTotalCountJSon = data.getMRootData().optString(
							"TotalCount");
					Log.e(LOGTAG, houseTotalCountJSon);
					if (list == null || list.length() < 1)
						return;
					for (int i = 0; i < list.length(); i++) {
						mJsonObjects.add(list.optJSONObject(i));
					}
					mHouseInfoAdapter.refresh(mJsonObjects);

				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_product_search))
				.setRequestMethod(RequestMethod.eGet)

				.addParam(parameter).notifyRequest();
	}

	private void requestHouseTotalCount() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {

					String houseTotalCountJSon = data.getMRootData().optString(
							"TotalCount");
					initHouseTotalCountTextView(houseTotalCountJSon);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_product_search))
				.setRequestMethod(RequestMethod.eGet)

				.addParam(parameter).notifyRequest();
	}

	private void requestAdvertisements() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {
					ArrayList<String> imgs = new ArrayList<String>();
					JSONArray list = data.getJsonArray();
					if (list == null || list.length() < 1)
						return;
					for (int i = 0; i < list.length(); i++) {
						imgs.add(list.optJSONObject(i).optString("TitleImg"));
					}
					mAdController.show(imgs);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_channel_titleimg))
				.setRequestMethod(RequestMethod.eGet)

				.addParam("channelName", "banner").notifyRequest();
	}

	/**
	 * 控制以什么样的方式显示ListView
	 * 
	 * @author king
	 *
	 */
	class HowWillIrefresh implements
			PullToRefreshBase.OnRefreshListener2<ListView> {

		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(getActivity(),
					System.currentTimeMillis(), DateUtils.FORMAT_SHOW_TIME
							| DateUtils.FORMAT_SHOW_DATE
							| DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);

		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {

		}

	}

}