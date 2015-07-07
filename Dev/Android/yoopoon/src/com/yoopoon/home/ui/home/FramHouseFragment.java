package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.json.JSONArray;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.content.Context;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v7.appcompat.R.string;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AbsListView;

import android.widget.LinearLayout.LayoutParams;
import android.widget.ListView;
import android.widget.PopupWindow;
import android.widget.TextView;
import android.widget.Toast;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;

@SuppressLint("ShowToast")
@EFragment()
public class FramHouseFragment extends FramSuper implements OnClickListener {
	public static final String LOGTAG = "FramHouseFragment";
	// 当前Fragment绑定的View
	View rootView;
	// 存储Get方法传入参数
	HashMap<String, String> parameter;
	// 获取的Json对象
	ArrayList<JSONObject> mJsonObjects;
	// 刷新ListView
	PullToRefreshListView listView;
	public static FramHouseFragment instance;

	public static FramHouseFragment getInstance() {
		return instance;
	}

	// 当前上下文
	Context mContext;
	// 承载楼盘的ListView
	ListView refreshView;
	// 楼盘ListView绑定的Adapter
	FramHouseListViewAdapter mHouseInfoAdapter;
	// 首页广告
	ADController mAdController;
	// 首页楼盘数量显示
	TextView houseTotalCountTextView;
	// 首页楼盘数量
	string houseTotaoCount = null;

	// 首页顶端搜索条件栏
	private TextView area_name_textview;
	private TextView type_textview;
	private TextView price_textview;
	private TextView reset_textview;
	// 全局传入参数
	private static String AreaNameValue = "";
	private static String IsDescendingValue = "true";
	private static String OrderByValue = "OrderByAddtime";
	private static String PageValue = "1";
	private static String PageCountValue = "10";
	private static String PriceBeginValue = "";
	private static String PriceEndValue = "";
	private static String TypeIdValue = "";

	// 房源页顶端条件ListView
	private ListView houseTypeListView;
	// 房源页顶端条件View（Java代码创建）

	// 房源页顶端PopupWindow
	private PopupWindow houseTypeWindow;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container,
			@Nullable Bundle savedInstanceState) {
		Log.e(LOGTAG, "Fragment oncreateView");
		/**
		 * 如果rootView存在则不在重新创建
		 */
		if (rootView != null) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_house_fragment, null);
			// PullToRefreshListView
			listView = (PullToRefreshListView) rootView.findViewById(R.id.matter_list_view);

			mContext = getActivity();
			// 首页楼盘数量
			houseTotalCountTextView = new TextView(mContext);
			instance = this;
			mAdController = new ADController(mContext);
			mJsonObjects = new ArrayList<JSONObject>();

			// 房源首页顶端TextView（条件）
			area_name_textview = (TextView) rootView.findViewById(R.id.area_name_textview);
			type_textview = (TextView) rootView.findViewById(R.id.type_textview);
			price_textview = (TextView) rootView.findViewById(R.id.price_textview);
			reset_textview = (TextView) rootView.findViewById(R.id.reset_textview);
			Log.e(LOGTAG, area_name_textview.toString());
			// 条件点击的事件绑定
			area_name_textview.setOnClickListener(this);
			type_textview.setOnClickListener(this);
			price_textview.setOnClickListener(this);
			reset_textview.setOnClickListener(this);

			// 初始化传入参数
			initParameter();

			initViews();
		}

		return rootView;
	}

	/**
	 * 初始化界面Fragment
	 */
	void initViews() {
		listView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		refreshView = listView.getRefreshableView();
		listView.setOnRefreshListener(new HowWillIrefresh());

		// 添加房源首页广告

		refreshView.addHeaderView(mAdController.getRootView());
		// 添加房源首页房屋数量
		requestHouseTotalCount();

		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);

		mHouseInfoAdapter = new FramHouseListViewAdapter(mContext);
		refreshView.setAdapter(mHouseInfoAdapter);

		requestAdvertisements();

		requestHouseList();
	}

	/**
	 * 获取ListView Items传入的参数
	 */
	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}

		parameter.clear();
		parameter.put("AreaName", AreaNameValue);
		parameter.put("IsDescending", IsDescendingValue);
		parameter.put("OrderBy", OrderByValue);
		parameter.put("Page", PageValue);
		parameter.put("PageCount", PageCountValue);
		parameter.put("PriceBegin", PriceBeginValue);
		parameter.put("PriceEnd", PriceEndValue);
		parameter.put("TypeId", TypeIdValue);

	}

	/**
	 * 初始化房源页面中楼盘数量
	 * 
	 * @param houseTotaoCount
	 *            楼盘数量
	 */
	@UiThread
	public void initHouseTotalCountTextView(String houseTotaoCount) {
		AbsListView.LayoutParams houseTotalCountParams = new AbsListView.LayoutParams(LayoutParams.MATCH_PARENT, 50);
		houseTotalCountTextView.setLayoutParams(houseTotalCountParams);

		houseTotalCountTextView.setText("共" + houseTotaoCount + "个楼盘");

		houseTotalCountTextView.setBackgroundColor(getResources().getColor(R.color.hosue_total_color));
		houseTotalCountTextView.setGravity(Gravity.CENTER_VERTICAL);

		refreshView.addHeaderView(houseTotalCountTextView);
	}

	@AfterInject
	void afterInject() {

	}

	/**
	 * 开启一个线程，连接网络，获取楼盘列表
	 */
	private void requestHouseList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				listView.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {

					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1)
						return;
					for (int i = 0; i < list.length(); i++) {
						mJsonObjects.add(list.optJSONObject(i));
					}

					Log.e("requestHouseList", "Succeed");
					mHouseInfoAdapter.refresh(mJsonObjects);

				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_product_search)).setRequestMethod(RequestMethod.eGet)

		.addParam(parameter).notifyRequest();
	}

	/**
	 * 开启一个线程，获取楼盘数量
	 */
	private void requestHouseTotalCount() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {

					String houseTotalCountJSon = data.getMRootData().optString("TotalCount");
					initHouseTotalCountTextView(houseTotalCountJSon);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_product_search)).setRequestMethod(RequestMethod.eGet)

		.addParam(parameter).notifyRequest();
	}

	/**
	 * 开启一个线程，获取首页广告
	 */
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
		}.setUrl(getString(R.string.url_channel_titleimg)).setRequestMethod(RequestMethod.eGet)
				.addParam("channelName", "banner").notifyRequest();
	}

	/**
	 * 开启一个线程，获取楼盘类型
	 */
	private void requestHouseTypeList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("TypeList");
					if (list == null || list.length() < 1) {

						return;
					}

					int size = list.length();
					String[] tempHouseList = new String[size];
					for (int i = 0; i < list.length(); i++) {
						JSONObject jsonObject = list.optJSONObject(i);
						tempHouseList[i] = jsonObject.optString("TypeName").toString();
						Log.e("requestHouseTypeList", tempHouseList[i].toString());
					}
					initHouseTypeList(tempHouseList);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_house_condition)).setRequestMethod(RequestMethod.eGet)

		.addParam(parameter).notifyRequest();
	}

	/**
	 * 初始化首页顶端楼盘类型
	 * 
	 * @param houseTypeStrings
	 */
	private void initHouseTypeList(String[] tempHouseList) {
		// houseTypeListView = new ListView(mContext);
		if (tempHouseList == null && tempHouseList.length < 1) {
			Toast.makeText(mContext, "获取数据失败，请重刷新", Toast.LENGTH_SHORT).show();
			return;
		} else {

			for (int i = 0; i < tempHouseList.length; i++) {
				TextView textView = new TextView(mContext);
				textView.setText(tempHouseList[i]);

			}
			// 初始化popupwindow
		}
	}

	/**
	 * 控制以什么样的方式显示ListView,覆写pullToRefresh
	 * 
	 * @author king
	 *
	 */
	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {

			String label = DateUtils.formatDateTime(getActivity(), System.currentTimeMillis(),
					DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
			requestHouseList();
		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			Toast.makeText(mContext, "Testing", Toast.LENGTH_SHORT);
			int tempCount=Integer.parseInt(PageValue);
			tempCount++;
			PageValue=tempCount+"";
			initParameter();
			requestHouseList();
		}

	}

	/**
	 * 首页顶端点击事件
	 */
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.area_name_textview:

			break;
		case R.id.type_textview:

			requestHouseTypeList();
			break;
		case R.id.price_textview:

			break;
		case R.id.reset_textview:

		default:
			break;
		}
	}
}