package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.json.JSONArray;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.BitmapDrawable;
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
import android.widget.LinearLayout;
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
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
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
	// 首页顶端楼盘类型ID
	String houseTypeIdNumber = null;
	// 首页楼盘数量
	string houseTotaoCount = null;
	// 首页顶端搜索条件栏
	private TextView area_name_textview;
	private TextView type_textview;
	private TextView price_textview;
	private TextView reset_textview;
	// 全局传入参数（传入到服务器的参数）
	/*
	 * private static String AreaNameValue = ""; private static String IsDescendingValue = "true";
	 * private static String OrderByValue = "OrderByAddtime"; private static String PageValue = "1";
	 * private static String PageCountValue = "10"; private static String PriceBeginValue = "";
	 * private static String PriceEndValue = ""; private static String TypeIdValue = "";
	 */
	private String AreaNameValue = "";
	private String IsDescendingValue = "true";
	private String OrderByValue = "OrderByAddtime";
	private String PageValue = "1";
	private String PageCountValue = "10";
	private String PriceBeginValue = "";
	private String PriceEndValue = "";
	private String TypeIdValue = "";
	// 房源页顶端楼盘类型PopupWindow
	private PopupWindow houseTypeWindow;
	// 房源页顶端楼盘价格PopupWindow
	private PopupWindow housePriceWindow;
	// 房源也顶端楼盘类型
	ArrayList<JSONObject> houseTypeJsonObjects = new ArrayList<JSONObject>();

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
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
	 * @param houseTotaoCount 楼盘数量
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
	 * @Title: requestHouseList
	 * @Description: 开启一个异步的线程，获取房源中楼盘列表
	 */
	private void requestHouseList() {
		Log.e(LOGTAG, parameter.toString());
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				listView.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1) {
						mHouseInfoAdapter.refresh(mJsonObjects);
						return;
					}
					for (int i = 0; i < list.length(); i++) {
						mJsonObjects.add(list.optJSONObject(i));
					}
					mHouseInfoAdapter.refresh(mJsonObjects);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_product_search)).setRequestMethod(RequestMethod.eGet).addParam(parameter)
				.notifyRequest();
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
		}.setUrl(getString(R.string.url_product_search)).setRequestMethod(RequestMethod.eGet).addParam(parameter)
				.notifyRequest();
	}
	/**
	 * @Title: requestAdvertisements
	 * @Description: 获取房源首页顶端的广告列表
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
	 * @Title: requestHouseTypeList
	 * @Description: 开启一个线程，获取楼盘类型，如三室一厅或者五室两厅等，调用initHouseTypeList()初始化Popuwindow
	 */
	private void requestHouseTypeList() {
		if (houseTypeJsonObjects.size() == 0) {
			new RequestAdapter() {
				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						JSONArray list = data.getMRootData().optJSONArray("TypeList");
						if (list == null || list.length() < 1) {
							return;
						}
						int size = list.length();
						// ArrayList<JSONObject> houseTypeJsonObjects = new ArrayList<JSONObject>();
						for (int i = 0; i < size; i++) {
							JSONObject jsonObject = list.optJSONObject(i);
							houseTypeJsonObjects.add(jsonObject);
						}
						// 传入参数初始化Populwindow
						initHouseTypeList(houseTypeJsonObjects, area_name_textview, type_textview, price_textview);
					}
				}
				@Override
				public void onProgress(ProgressMessage msg) {
				}
			}.setUrl(getString(R.string.url_house_condition)).setRequestMethod(RequestMethod.eGet).notifyRequest();
		} else {
			initHouseTypeList(houseTypeJsonObjects, area_name_textview, type_textview, price_textview);
		}
	}
	/**
	 * 初始化首页顶端楼盘类型
	 * @param houseTypeStrings
	 */
	private void initHouseTypeList(ArrayList<JSONObject> houseTypeJsonArray, View area_name_textview,
			View type_textview, View price_textview) {
		// 创建LinearLayout承载PopuWindows
		LinearLayout linearLayout = new LinearLayout(mContext);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		linearLayout.setBackgroundColor(Color.WHITE);
		// 获取房源页顶端的区域类型，楼盘类型，和楼盘价格TextView
		final TextView houseType_textview = (TextView) type_textview;
		final TextView houseArea_textview = (TextView) area_name_textview;
		final TextView housePrice_textview = (TextView) price_textview;
		// 初始化页码
		PageValue = "1";
		// 初始化联网获取数据的参数
		if (houseArea_textview.getText().toString().equals("区域")) {
			AreaNameValue = "";
		} else {
			AreaNameValue = houseArea_textview.getText().toString();
		}
		// 获取价格TextView中的数据，初始化价格参数
		initPrice(housePrice_textview.getText().toString());
		// 创建Popuwindows同时初始化PopuWindows的属性
		houseTypeWindow = new PopupWindow(linearLayout, LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT, true);
		houseTypeWindow.setTouchable(true);
		houseTypeWindow.setBackgroundDrawable(new BitmapDrawable());
		houseTypeWindow.setOutsideTouchable(true);
		houseTypeWindow.setFocusable(true);
		// 判断是否接收到异步线程过来的数据
		if (houseTypeJsonArray == null && houseTypeJsonArray.size() < 1) {
			Toast.makeText(mContext, "获取数据失败，请重刷新", Toast.LENGTH_SHORT).show();
			return;
		} else {
			// 向承载LinearLayout中动态添加TextView以承载楼盘类型
			for (int i = 0; i < houseTypeJsonArray.size(); i++) {
				TextView textView = new TextView(mContext);
				textView.setText(houseTypeJsonArray.get(i).optString("TypeName"));
				int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
				textView.setWidth(screenWidth / 3);
				textView.setGravity(Gravity.CENTER);
				textView.setTextSize(17);
				final String mesString = houseTypeJsonArray.get(i).optString("TypeName").toString();
				final String houseTypeIdString = houseTypeJsonArray.get(i).optString("TypeId").toString();
				linearLayout.addView(textView);
				textView.setOnClickListener(new OnClickListener() {
					@Override
					public void onClick(View v) {
						houseType_textview.setText(mesString);
						houseTypeIdNumber = houseTypeIdString;
						Log.e(LOGTAG, houseTypeIdNumber);
						TypeIdValue = houseTypeIdString;
						// 更新参数
						mJsonObjects.clear();
						initParameter();
						requestHouseList();
						houseTypeWindow.dismiss();
					}
				});
			}
			// houseTypeWindow.showAsDropDown(houseType_textview);
			houseTypeWindow.showAsDropDown(houseType_textview, -50, 0);
		}
	}
	/**
	 * @Title: initPrice
	 * @Description: 传入price_textview中的文本值，对传入的数据进行判断，根据判断的值对参数中的priceBegin和priceEnd进行赋值。
	 * @param tempPrice 传入的price_textview，类型是String
	 */
	private void initPrice(String tempPrice) {
		if (tempPrice.equals("4000以下")) {
			PriceBeginValue = "0";
			PriceEndValue = "4000";
			return;
		} else if (tempPrice.equals("4000-5000")) {
			PriceBeginValue = "4000";
			PriceEndValue = "5000";
			return;
		} else if (tempPrice.equals("5000-6000")) {
			PriceBeginValue = "5000";
			PriceEndValue = "6000";
			return;
		} else if (tempPrice.equals("6000-7000")) {
			PriceBeginValue = "6000";
			PriceEndValue = "7000";
			return;
		} else if (tempPrice.equals("7000-8000")) {
			PriceBeginValue = "7000";
			PriceEndValue = "8000";
			return;
		} else if (tempPrice.equals("8000-9000")) {
			PriceBeginValue = "8000";
			PriceEndValue = "9000";
			return;
		} else if (tempPrice.equals("9000-10000")) {
			PriceBeginValue = "5000";
			PriceEndValue = "6000";
			return;
		} else if (tempPrice.equals("10000以上")) {
			PriceBeginValue = "10000";
			PriceEndValue = "50000";
			return;
		} else {
			PriceBeginValue = "";
			PriceEndValue = "";
			return;
		}
	}

	/**
	 * @ClassName: HowWillIrefresh
	 * @Description: 设置以什么样式来呈现房源首页的楼盘
	 * @author: 徐阳会
	 * @date: 2015年7月8日 下午3:44:26
	 */
	private class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {
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
			int tempCount = Integer.parseInt(PageValue);
			tempCount++;
			PageValue = tempCount + "";
			initParameter();
			requestHouseList();
		}
	}

	/**
	 * @Title: initHousePriceList
	 * @Description: 初始化房源首页楼盘价格列表
	 */
	private void initHousePriceList() {
		// 创建承载楼盘价格的LinearLayout，同时设置LinearLayout的相应属性,创建Popuwindow
		LinearLayout linearLayout = new LinearLayout(mContext);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		linearLayout.setBackgroundColor(Color.WHITE);
		housePriceWindow = new PopupWindow(linearLayout, LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT, true);
		housePriceWindow.setTouchable(true);
		housePriceWindow.setBackgroundDrawable(new BitmapDrawable());
		housePriceWindow.setOutsideTouchable(true);
		housePriceWindow.setFocusable(true);
		List<String> housePriceArrayList = new ArrayList<String>();
		{
			// 获取传入到服务器的参数
			if (area_name_textview.getText().toString().equals("区域")) {
				AreaNameValue = "";
			} else {
				AreaNameValue = area_name_textview.getText().toString();
			}
			PageValue = "1";
			if (houseTypeIdNumber == null) {
				TypeIdValue = "";
			} else {
				TypeIdValue = houseTypeIdNumber;
			}
			Log.e(LOGTAG, TypeIdValue);
		}
		// 初始化ArrayList中保存的数据
		// 配成value.xml中的数组
		housePriceArrayList.add("4000以下");
		housePriceArrayList.add("4000-5000");
		housePriceArrayList.add("5000-6000");
		housePriceArrayList.add("6000-7000");
		housePriceArrayList.add("7000-8000");
		housePriceArrayList.add("8000-9000");
		housePriceArrayList.add("9000-10000");
		housePriceArrayList.add("10000以上");
		// 循环添加TextView到价格列表中
		for (int i = 0; i < housePriceArrayList.size(); i++) {
			TextView textView = new TextView(mContext);
			final String msgString = housePriceArrayList.get(i).toString();
			textView.setText(msgString);
			int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
			textView.setWidth(screenWidth / 3);
			textView.setGravity(Gravity.CENTER);
			textView.setTextSize(17);
			linearLayout.addView(textView);
			textView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					price_textview.setText(msgString);
					initPrice(msgString);
					Log.e(LOGTAG, AreaNameValue);
					Log.e(LOGTAG, PriceBeginValue);
					Log.e(LOGTAG, PriceEndValue);
					mJsonObjects.clear();
					initParameter();
					requestHouseList();
					housePriceWindow.dismiss();
				}
			});
			housePriceWindow.showAsDropDown(price_textview, -50, 0);
		}
	}
	private void houseConditionReset() {
		AreaNameValue = "";
		PriceBeginValue = "";
		PriceEndValue = "";
		TypeIdValue = "";
		PageValue = "1";
		area_name_textview.setText("区域");
		type_textview.setText("类型");
		price_textview.setText("价格");
		mJsonObjects.clear();
		initParameter();
		requestHouseList();
	}
	/*
	 * @Title: onClick
	 * @Description: 房源首页顶端楼盘条件点击事件
	 * @param v 传入的View
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
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
				initHousePriceList();
				break;
			case R.id.reset_textview:
				houseConditionReset();
				break;
			default:
				break;
		}
	}
}
