/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FramHouseFragment.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: 房源库对应的Fragment
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015年7月17日 上午11:50:16 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.UiThread;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Color;
import android.graphics.drawable.BitmapDrawable;
import android.net.ConnectivityManager;
import android.os.Bundle;
import android.preference.PreferenceManager;
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
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.common.base.utils.NetworkUtils;
import com.yoopoon.common.base.utils.SPUtils;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;
import com.yoopoon.house.ui.houselist.HouseListViewAdapter;
import com.yoopoon.house.ui.houselist.RequestHouseAreaCondition;
import com.yoopoon.house.ui.houselist.RequestHouseAreaCondition.Callback;

@SuppressLint({ "ShowToast", "InflateParams" })
@EFragment()
public class FramHouseFragment extends FramSuper implements OnClickListener {
	// ##############################################################################################
	// 所有变量和属性声明如下
	// ###############################################################################################
	public static final String TAG = "FramHouseFragment";
	// 当前Fragment绑定的View
	View rootView;
	// 房源库ListView对应的Layout
	View houseListViewlayout;
	// 房源库ListView对应的Layout中的经纪人推荐和经纪人带客TextView
	TextView brokerTakeTextView, brokerrecommendTextView;
	// 存储方法传送到服务器的参数
	HashMap<String, String> parameter;
	// 存储联网获取的楼盘列表Json对象数据
	ArrayList<JSONObject> houseListJsonObjects;
	// 负责ListView上下滑动加载楼盘的PullToRefreshListView
	PullToRefreshListView pullToRefreshListView;
	public static FramHouseFragment framHouseFragmentInstance;

	public static FramHouseFragment getInstance() {
		return framHouseFragmentInstance;
	}

	// 当前上下文
	Context mContext;
	// 承载楼盘的ListView
	ListView houseListView;
	// 楼盘ListView绑定的Adapter
	HouseListViewAdapter houseListViewAdapter;
	// 首页广告
	ADController mAdController;
	// 首页楼盘数量显示
	TextView houseTotalCountTextView;
	// 首页楼盘总共数量
	string houseTotaoCount = null;
	// 首页顶端楼盘类型ID
	String houseTypeIdNumber = null;
	// 房源页顶端楼盘检索条件对应的显示TextView，分别是楼盘区域，楼盘类型，楼盘价格和重置
	private TextView area_name_textview;
	private TextView type_textview;
	private TextView price_textview;
	private TextView reset_textview;
	// 传入参数（传入到服务器的参数）
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
	// 房源顶端楼盘类型Json数组
	ArrayList<JSONObject> houseTypeJsonObjects = new ArrayList<JSONObject>();
	// 房源页顶端楼盘对应省市区LinearLayout（使用LinearLayout动态加载省市区数据）
	ArrayList<JSONObject> houseProvinceJsonObjects = new ArrayList<JSONObject>();
	ArrayList<JSONObject> houseCityJsonObjects = new ArrayList<JSONObject>();
	ArrayList<JSONObject> houseDistrictJsonObjects = new ArrayList<JSONObject>();
	private LinearLayout houseProvinceLinearlayout, houseCityLinearLayout, houseDistrictlinearLayout,
			houseAreaLinearLayout;
	// 楼盘区域位置对应的PopuWindow
	private PopupWindow houseAreaConditionPopupWindow;
	// 设置是否是从经纪人页面或者个人信息页面跳转到房源库
	private boolean setBrokerBackground = false;

	// ##############################################################################################
	// 所有变量和属性声明如上
	// ###############################################################################################
	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		/**
		 * 如果rootView存在则不在重新创建
		 */
		if (rootView != null) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			framHouseFragmentInstance = this;
			mContext = getActivity();
			// 获取Fragment对应的视图
			rootView = LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_house_fragment, null);
			pullToRefreshListView = (PullToRefreshListView) rootView.findViewById(R.id.matter_list_view);
			// 房源库listView对应的layout
			houseListViewlayout = LayoutInflater.from(mContext).inflate(R.layout.home_fram_house_listview_item, null);
			// 首页楼盘数量
			mAdController = new ADController(mContext);
			houseListJsonObjects = new ArrayList<JSONObject>();
			// #####################################楼盘区域省市区UI设置######################################
			LayoutParams layoutParams = new LayoutParams(mContext.getResources().getDisplayMetrics().widthPixels / 4,
					LayoutParams.WRAP_CONTENT);
			houseProvinceLinearlayout = new LinearLayout(mContext);
			houseCityLinearLayout = new LinearLayout(mContext);
			houseDistrictlinearLayout = new LinearLayout(mContext);
			houseAreaLinearLayout = new LinearLayout(mContext);
			houseAreaLinearLayout.setOrientation(LinearLayout.HORIZONTAL);
			houseAreaLinearLayout.addView(houseProvinceLinearlayout, layoutParams);
			houseAreaLinearLayout.addView(houseCityLinearLayout, layoutParams);
			houseAreaLinearLayout.addView(houseDistrictlinearLayout, layoutParams);
			// 初始化显示楼盘区域的Popuwindows
			houseAreaConditionPopupWindow = new PopupWindow(houseAreaLinearLayout, mContext.getResources()
					.getDisplayMetrics().widthPixels * 3 / 4, LayoutParams.WRAP_CONTENT, true);
			// #####################################楼盘区域省市区UI设置######################################
			// 房源首页顶端楼盘检索TextView（条件）
			area_name_textview = (TextView) rootView.findViewById(R.id.area_name_textview);
			type_textview = (TextView) rootView.findViewById(R.id.type_textview);
			price_textview = (TextView) rootView.findViewById(R.id.price_textview);
			reset_textview = (TextView) rootView.findViewById(R.id.reset_textview);
			// 楼盘检索条件点击的事件绑定
			area_name_textview.setOnClickListener(this);
			type_textview.setOnClickListener(this);
			price_textview.setOnClickListener(this);
			reset_textview.setOnClickListener(this);
			// 初始化传输到服务器的参数
			initParameter();
			// 初始化Fragment对应的视图，加载控件
			initFramHouseFragmentView();
			registerReceiver();
		}
		return rootView;
	}

	private void registerReceiver() {
		IntentFilter filter = new IntentFilter(ConnectivityManager.CONNECTIVITY_ACTION);
		getActivity().registerReceiver(receiver, filter);
	}

	private BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			if (NetworkUtils.isNetworkConnected(context)) {
				// requestAdvertisements();
				// requestHouseList();
			}
		}
	};

	/*
	 * @Title: onClick
	 * @Description: 房源首页顶端楼盘检索条件点击事件
	 * @param v 传入的View
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.area_name_textview:
				houseCityLinearLayout.removeAllViews();
				houseDistrictlinearLayout.removeAllViews();
				requestHouseProvince();
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

	/*
	 * @Title: setUserVisibleHint
	 * @Description: 判断如果用户是从经纪人个人信息页面过来的,则清除房源页顶端的检索条件
	 * @param isVisibleToUser
	 * @see android.support.v4.app.Fragment#setUserVisibleHint(boolean)
	 */
	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		super.setUserVisibleHint(isVisibleToUser);
		if (!isVisibleToUser && rootView != null) {
			area_name_textview.setText("区域");
			type_textview.setText("类型");
			price_textview.setText("价格");
			houseConditionReset();
			setBrokerBackground = false;
		}
		if (isVisibleToUser) {
			boolean setBackground = PreferenceManager.getDefaultSharedPreferences(getActivity()).getBoolean(
					"isAgentFromReceiver", false);
			houseListViewAdapter.setSetBrokerBackground(setBackground);
			SPUtils.setIsAgentFromReceiver(getActivity(), false);
			houseListViewAdapter.refresh(houseListJsonObjects);
		}
	}

	// ##############################################################################################
	// 所有与广播有关的逻辑代码如下
	// ###############################################################################################
	// 创建经纪人注销登录时候接收到的广播
	private BroadcastReceiver houseFramRefreshReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			houseListViewAdapter.refresh(houseListJsonObjects);
		}
	};

	/**
	 * @Title: registhouseFramRefreshBroadcast
	 * @Description: 注册经纪人注销登录时候接收到的广播到FramHouseFragment
	 */
	private void registhouseFramRefreshBroadcast() {
		IntentFilter intentFilter = new IntentFilter("com.yoopoon.logout_action");
		intentFilter.addCategory(Intent.CATEGORY_DEFAULT);
		mContext.registerReceiver(houseFramRefreshReceiver, intentFilter);
	}

	private BroadcastReceiver houseBrokerRefreshReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			setBrokerBackground = intent.getBooleanExtra("comeFromBroker", false);
			houseListViewAdapter.refresh(houseListJsonObjects);
		}
	};

	/**
	 * @Title: registBrokerHouseBroadcast
	 * @Description: 注册从经纪人页面或者个人信息页面过来后经纪人推荐和经纪人带客背景效果
	 */
	private void registBrokerHouseBroadcast() {
		IntentFilter filter = new IntentFilter("com.yoopoon.broker_takeguest");
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		mContext.registerReceiver(houseBrokerRefreshReceiver, filter);
	}

	// ##############################################################################################
	// 所有与广播有关的逻辑代码如上
	// ###############################################################################################
	//
	//
	// ##############################################################################################
	// 所有开启异步线程联网获取数据的逻辑代码如下
	// ###############################################################################################
	/* 与获取楼盘区域位置有关的联网控制代码如下 */
	/**
	 * @Title: requestHouseProvince
	 * @Description: 开启异步线程获取楼盘省份信息
	 */
	private void requestHouseProvince() {
		if (houseProvinceJsonObjects.size() > 0) {
			initHouseProvince(houseProvinceJsonObjects);
			return;
		}
		RequestHouseAreaCondition.requestHouseProvinceArea(new Callback() {
			@Override
			public void callback(JSONArray jsonArray) {
				for (int i = 0; i < jsonArray.length(); i++) {
					try {
						JSONObject jsonObject = jsonArray.getJSONObject(i);
						houseProvinceJsonObjects.add(jsonObject);
					} catch (JSONException e) {
						e.printStackTrace();
					}
				}
				initHouseProvince(houseProvinceJsonObjects);
			}
		});
	}

	/**
	 * @Title: requestHouseCity
	 * @Description: 开启异步线程，获取楼盘城市区域信息
	 * @param id 传入省份ID
	 */
	private void requestHouseCity(String id) {
		RequestHouseAreaCondition.requestHouseCityArea(id, new Callback() {
			@Override
			public void callback(JSONArray jsonArray) {
				if (jsonArray != null) {
					houseCityJsonObjects.clear();
					for (int i = 0; i < jsonArray.length(); i++) {
						try {
							JSONObject jsonObject = jsonArray.getJSONObject(i);
							houseCityJsonObjects.add(jsonObject);
						} catch (JSONException e) {
							e.printStackTrace();
						}
					}
					initHouseCity(houseCityJsonObjects);
				}
			}
		});
	}

	/**
	 * @Title: requestHouseDistrict
	 * @Description: 开启异步线程，获取楼盘县区位置信息
	 * @param id
	 */
	private void requestHouseDistrict(String id) {
		RequestHouseAreaCondition.requestHouseDistrictArea(id, new Callback() {
			@Override
			public void callback(JSONArray jsonArray) {
				houseDistrictJsonObjects.clear();
				for (int i = 0; i < jsonArray.length(); i++) {
					try {
						JSONObject jsonObject = jsonArray.getJSONObject(i);
						houseDistrictJsonObjects.add(jsonObject);
					} catch (JSONException e) {
						e.printStackTrace();
					}
				}
				initHouseDistrict(houseDistrictJsonObjects);
			}
		});
	}

	/* 与获取楼盘区域位置有关的联网控制代码如上 */
	/**
	 * @Title: requestHouseTypeList
	 * @Description: 开启一个线程，获取楼盘类型，如三室一厅或者五室两厅等，调用initHouseTypeList()初始化Popuwindow
	 */
	private void requestHouseTypeList() {
		if (type_textview.getTag() == null) {
			type_textview.setTag("succeed");
			new RequestAdapter() {

				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						JSONArray list = data.getMRootData().optJSONArray("TypeList");
						if (list == null || list.length() < 1) {
							return;
						}
						int size = list.length();
						for (int i = 0; i < size; i++) {
							JSONObject jsonObject = list.optJSONObject(i);
							houseTypeJsonObjects.add(jsonObject);
						}
						// 传入参数初始化Populwindow
						initHouseType(houseTypeJsonObjects, area_name_textview, type_textview, price_textview);
					}
				}

				@Override
				public void onProgress(ProgressMessage msg) {
				}
			}.setUrl(getString(R.string.url_house_condition)).setRequestMethod(RequestMethod.eGet).notifyRequest();
		}
		initHouseType(houseTypeJsonObjects, area_name_textview, type_textview, price_textview);
	}

	/**
	 * @Title: requestAdvertisements
	 * @Description: 开启异步线程，获取房源页顶端广告信息
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
					// 添加广告
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
	 * @Title: requestHouseTotalCount
	 * @Description: 开启异步线程，获取当前显示的楼盘数量
	 */
	private int totalCount = 0;

	private void requestHouseTotalCount() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					String houseTotalCountJSon = data.getMRootData().optString("TotalCount");
					totalCount = Integer.parseInt(houseTotalCountJSon);
					// 初始化楼盘总数量
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
	 * @Title: requestHouseList
	 * @Description: 开启异步线程，获取所有的楼盘，以ListView的形式展示出来
	 */
	private void requestHouseList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				pullToRefreshListView.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					String houseCount = data.getMRootData().optString("TotalCount");
					if (list == null || list.length() < 1) {
						houseListViewAdapter.refresh(houseListJsonObjects);
						// 如果没有返回值，则设置楼盘数量为0

						initHouseTotalCountTextView(houseListJsonObjects.size() + "");
						// initHouseTotalCountTextView("0");

						return;
					}
					for (int i = 0; i < list.length(); i++) {
						houseListJsonObjects.add(list.optJSONObject(i));
					}
					houseListViewAdapter.refresh(houseListJsonObjects);

					initHouseTotalCountTextView(houseCount);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_product_search)).setRequestMethod(RequestMethod.eGet).addParam(parameter)
				.notifyRequest();
	}

	// ##############################################################################################
	// 所有开启异步线程联网获取数据的逻辑代码如上
	// ###############################################################################################
	//
	//
	// ##############################################################################################
	// 所有从异步线程中获取数据后初始化和设置UI和的逻辑代码如下：初始化楼盘省市区，初始化楼盘类型，
	// 初始化楼盘价格（不需要联网），初始化楼盘ListView列表，初始化楼盘数量
	// ###############################################################################################
	/**
	 * @Title: initFramHouseFragmentView
	 * @Description: 初始化FramHouseFragment对应的UI和其他设置信息
	 */
	public void initFramHouseFragmentView() {
		// 初始化房源页顶端楼盘区域的显示控件和楼盘区域数据
		pullToRefreshListView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		houseListView = pullToRefreshListView.getRefreshableView();
		pullToRefreshListView.setOnRefreshListener(new HowWillIrefresh());
		// 添加房源首页广告
		houseListView.addHeaderView(mAdController.getRootView());
		// 添加房源首页房屋数量
		requestHouseTotalCount();
		houseListView.setFastScrollEnabled(false);
		houseListView.setFadingEdgeLength(0);
		houseListViewAdapter = new HouseListViewAdapter(mContext);
		houseListView.setAdapter(houseListViewAdapter);
		// 开启一个异步线程，获取广告数据，同时加载广告数据
		requestAdvertisements();
		// 开启一个异步线程，获取广告数据，同事加载楼盘列表
		requestHouseList();
		// 接受广播
		registhouseFramRefreshBroadcast();
		registBrokerHouseBroadcast();
	}

	/**
	 * @Title: initHouseProvince
	 * @Description: 初始化和设置楼盘对应的省份UI
	 * @param jsonArray 从异步线程获取的楼盘所属省份Json数组
	 */
	private void initHouseProvince(final ArrayList<JSONObject> jsonArray) {
		houseProvinceLinearlayout.setOrientation(LinearLayout.VERTICAL);
		houseProvinceLinearlayout.setBackgroundColor(Color.WHITE);
		houseAreaConditionPopupWindow.setTouchable(true);
		houseAreaConditionPopupWindow.setBackgroundDrawable(new BitmapDrawable());
		houseAreaConditionPopupWindow.setOutsideTouchable(true);
		houseAreaConditionPopupWindow.setFocusable(true);
		// houseAreaConditionPopuWindow = HousePopuwindow.getHousePopupwindowInstance();
		houseProvinceLinearlayout.removeAllViews();
		for (int i = 0; i < jsonArray.size(); i++) {
			final TextView textView = new TextView(mContext);
			textView.setGravity(Gravity.CENTER);
			textView.setText(jsonArray.get(i).optString("AreaName"));
			textView.setTextSize(18);
			textView.setPadding(10, 10, 10, 10);
			houseProvinceLinearlayout.addView(textView);
			final String parentIdValue = jsonArray.get(i).optString("Id");
			textView.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					requestHouseCity(parentIdValue);
					houseDistrictlinearLayout.removeAllViews();
				}
			});
		}
		houseAreaConditionPopupWindow.showAsDropDown(area_name_textview);
	}

	/**
	 * @Title: initHouseCity
	 * @Description: 初始化和设置楼盘对应的城市UI
	 * @param arrayList 从异步线程获取的楼盘所属城市Json数组
	 */
	private void initHouseCity(ArrayList<JSONObject> arrayList) {
		houseCityLinearLayout.setOrientation(LinearLayout.VERTICAL);
		houseCityLinearLayout.setBackgroundColor(Color.WHITE);
		houseCityLinearLayout.removeAllViews();
		for (int i = 0; i < arrayList.size(); i++) {
			final TextView textView = new TextView(mContext);
			textView.setGravity(Gravity.CENTER);
			textView.setText(arrayList.get(i).optString("AreaName"));
			textView.setTextSize(18);
			textView.setPadding(10, 10, 10, 10);
			houseCityLinearLayout.addView(textView);
			final String parentIdValue = arrayList.get(i).optString("Id");
			textView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					requestHouseDistrict(parentIdValue);
				}
			});
		}
		houseAreaConditionPopupWindow.update();
	}

	/**
	 * @Title: initHouseDistrict
	 * @Description: 初始化和设置楼盘对应的区县UI
	 * @param arrayList 从异步线程获取的楼盘所属区县Json数组
	 */
	private void initHouseDistrict(ArrayList<JSONObject> arrayList) {
		houseDistrictlinearLayout.setOrientation(LinearLayout.VERTICAL);
		houseDistrictlinearLayout.setBackgroundColor(Color.WHITE);
		houseDistrictlinearLayout.removeAllViews();
		for (int i = 0; i < arrayList.size(); i++) {
			final TextView textView = new TextView(mContext);
			final String parentIdValue = arrayList.get(i).optString("Id");
			final String parentName = arrayList.get(i).optString("AreaName");
			textView.setGravity(Gravity.CENTER);
			textView.setText(parentName);
			textView.setTextSize(18);
			textView.setPadding(10, 10, 10, 10);
			houseDistrictlinearLayout.addView(textView);
			textView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					area_name_textview.setText(parentName);
					AreaNameValue = parentName;
					houseListJsonObjects.clear();
					initParameter();
					requestHouseList();
					houseAreaConditionPopupWindow.dismiss();
				}
			});
		}
		houseAreaConditionPopupWindow.update();
	}

	/**
	 * @Title: initHouseType
	 * @Description: 初始化楼盘类型ListView 对应的UI
	 * @param houseTypeJsonArray 传入的楼盘类型对应的Json数组
	 * @param area_name_textview 传入的楼盘区域TextView
	 * @param type_textview 传入的楼盘类型TextView
	 * @param price_textview 传入的楼盘价格TextView
	 */
	private void initHouseType(ArrayList<JSONObject> houseTypeJsonArray, View area_name_textview, View type_textview,
			View price_textview) {
		// 创建LinearLayout承载PopuWindows
		LinearLayout linearLayout = new LinearLayout(mContext);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		linearLayout.setBackgroundColor(Color.WHITE);
		ScrollView scrollView = new ScrollView(mContext);
		scrollView.setFillViewport(true);
		// scrollView.setScrollbarFadingEnabled(false);
		scrollView.addView(linearLayout, LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
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
		// 创建Popuwindow同时初始化PopuWindows的属性
		houseTypeWindow = new PopupWindow(scrollView, mContext.getResources().getDisplayMetrics().widthPixels / 2,
				LayoutParams.WRAP_CONTENT, true);
		houseTypeWindow.setTouchable(true);
		houseTypeWindow.setBackgroundDrawable(new BitmapDrawable());
		houseTypeWindow.setOutsideTouchable(true);
		houseTypeWindow.setFocusable(true);
		// 判断是否接收到异步线程过来的数据
		if (houseTypeJsonArray == null || houseTypeJsonArray.size() < 1) {
			// Toast.makeText(mContext, "正在获取数据,请稍候", Toast.LENGTH_SHORT).show();
			return;
		} else {
			// 向承载LinearLayout中动态添加TextView以承载楼盘类型
			for (int i = 0; i < houseTypeJsonArray.size(); i++) {
				TextView textView = new TextView(mContext);
				textView.setText(houseTypeJsonArray.get(i).optString("TypeName"));
				// textView.setWidth(screenWidth / 3);
				textView.setGravity(Gravity.CENTER);
				textView.setPadding(10, 10, 10, 10);
				textView.setTextSize(18);
				final String megStringString = houseTypeJsonArray.get(i).optString("TypeName").toString();
				final String houseTypeIdString = houseTypeJsonArray.get(i).optString("TypeId").toString();
				// 设置TextView中字体的行间距
				linearLayout.addView(textView);
				textView.setOnClickListener(new OnClickListener() {
					@Override
					public void onClick(View v) {
						houseType_textview.setText(megStringString);
						houseTypeIdNumber = houseTypeIdString;
						TypeIdValue = houseTypeIdString;
						// 更新参数
						houseListJsonObjects.clear();
						initParameter();
						requestHouseList();
						houseTypeWindow.dismiss();
					}
				});
			}
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
	 * @Title: initHousePriceList
	 * @Description: 根据选择的价格，设置需要传入到服务器的PriceBegin和PriceEnd
	 */
	private void initHousePriceList() {
		// 创建承载楼盘价格的LinearLayout，同时设置LinearLayout的相应属性,创建Popuwindow
		LinearLayout linearLayout = new LinearLayout(mContext);
		linearLayout.setOrientation(LinearLayout.VERTICAL);
		linearLayout.setBackgroundColor(Color.WHITE);
		housePriceWindow = new PopupWindow(linearLayout, mContext.getResources().getDisplayMetrics().widthPixels / 3,
				LayoutParams.WRAP_CONTENT, true);
		housePriceWindow.setTouchable(true);
		housePriceWindow.setBackgroundDrawable(new BitmapDrawable());
		housePriceWindow.setOutsideTouchable(true);
		housePriceWindow.setFocusable(true);
		List<String> housePriceArrayList = new ArrayList<String>();
		{
			// 获取传入到服务器的参数
			if (area_name_textview.getText().toString().equals("区域")) {
				AreaNameValue = "";
			}
			PageValue = "1";
			if (houseTypeIdNumber == null) {
				TypeIdValue = "";
			} else {
				TypeIdValue = houseTypeIdNumber;
			}
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
			textView.setGravity(Gravity.CENTER);
			textView.setPadding(10, 10, 10, 10);
			textView.setTextSize(18);
			linearLayout.addView(textView);
			textView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					price_textview.setText(msgString);
					initPrice(msgString);
					houseListJsonObjects.clear();
					initParameter();
					requestHouseList();
					housePriceWindow.dismiss();
				}
			});
			housePriceWindow.showAsDropDown(price_textview, -50, 0);
		}
	}

	/**
	 * 传入从异步线程中获得的房源页面中楼盘数量，设置总的楼盘数量UI
	 * @param houseTotaoCount 楼盘数量
	 */
	@UiThread
	public void initHouseTotalCountTextView(String houseTotaoCount) {
		houseListView.removeHeaderView(houseTotalCountTextView);
		houseTotalCountTextView = new TextView(mContext);
		AbsListView.LayoutParams houseTotalCountParams = new AbsListView.LayoutParams(LayoutParams.MATCH_PARENT, 150);
		houseTotalCountTextView.setLayoutParams(houseTotalCountParams);
		houseTotalCountTextView.setText("共" + houseTotaoCount + "个楼盘");
		houseTotalCountTextView.setBackgroundColor(getResources().getColor(R.color.hosue_total_color));
		houseTotalCountTextView.setGravity(Gravity.CENTER_VERTICAL);
		int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
		houseTotalCountTextView.setPadding(screenWidth / 10, 0, 0, 0);
		houseListView.addHeaderView(houseTotalCountTextView);
	}

	private String areaName = "";

	/**
	 * @Title: initParameter
	 * @Description: 对每次开启异步线程联网的数据进行参数初始化
	 */
	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}
		if (!areaName.equals(AreaNameValue)) {
			areaName = AreaNameValue;
			PageValue = "1";
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

		Log.i(TAG, parameter.toString());

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
			initHouseTotalCountTextView(String.valueOf(totalCount));
		}
	}

	/**
	 * @Title: houseConditionReset
	 * @Description: 点击房源页重置按钮触发的重置楼盘条件操作
	 */
	private void houseConditionReset() {
		houseProvinceLinearlayout.removeAllViews();
		houseCityLinearLayout.removeAllViews();
		houseDistrictlinearLayout.removeAllViews();
		AreaNameValue = "";
		PriceBeginValue = "";
		PriceEndValue = "";
		TypeIdValue = "";
		PageValue = "1";
		area_name_textview.setText("区域");
		type_textview.setText("类型");
		price_textview.setText("价格");
		houseListJsonObjects.clear();
		initParameter();
		requestHouseList();
	}

	// ##############################################################################################
	// 所有从异步线程中获取数据后初始化和设置UI和的逻辑代码如上：初始化楼盘省市区，初始化楼盘类型，
	// 初始化楼盘价格（不需要联网），初始化楼盘ListView列表，初始化楼盘数量
	// ###############################################################################################
	@AfterInject
	void afterInject() {
	}
	/*
	 * @Override public void setTitle() { if (houseListViewAdapter != null) {
	 * houseListViewAdapter.setSetBrokerBackground(false);
	 * houseListViewAdapter.notifyDataSetInvalidated(); } }
	 */
}
