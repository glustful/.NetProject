package com.yoopoon.home.ui.home;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;

import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.LinearLayout;
import android.widget.TabHost;
import android.widget.TabHost.OnTabChangeListener;
import android.widget.TabHost.TabSpec;
import android.widget.TextView;
import android.widget.Toast;

import com.yoopoon.common.base.utils.SPUtils;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.SearchActionBarActivity;

@EActivity(R.layout.home_main_activity)
public class FramMainActivity extends SearchActionBarActivity {
	static String tag = "FramMainActivity";
	@ViewById(android.R.id.tabhost)
	TabHost tabHost;
	@ViewById(R.id.home_main_pager)
	ViewPager mainPager;
	@ViewById(R.id.search_layout)
	LinearLayout searchLayout;
	@ViewById(R.id.tv_main_network)
	TextView tv_network;
	
	@Click(R.id.tv_main_network)
	void setNetwork() {
		Intent intent = new Intent("android.settings.SETTINGS");
		startActivity(intent);
	}
	
	boolean isOpenAgent = true;
	public static FramMainActivity instance;
	HomeMainAdapter pageAdapter;
	ArrayList<FragmentInfo> fInfo;
	
	@AfterInject
	void initData() {
		instance = this;
		fInfo = new ArrayList<FragmentInfo>();
	}
	@AfterViews
	void initUI() {
		checkNetworkState();
		MyApplication.getInstance().addActivity(this);
		searchLayout.addView(rootView, new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT));
		initMenu();
		initFragments();
		begin();
	}
	void initFragments() {
		Bundle argActive = new Bundle();
		fInfo.add(new FragmentInfo(FramActiveFragment_.class, argActive));
		Bundle argHouse = new Bundle();
		fInfo.add(new FragmentInfo(FramHouseFragment_.class, argHouse));
		if (isOpenAgent) {
			Bundle argAgent = new Bundle();
			fInfo.add(new FragmentInfo(FramAgentFragment_.class, argAgent));
		}
		Bundle argMe = new Bundle();
		fInfo.add(new FragmentInfo(FramMeFragment_.class, argMe));
	}
	@SuppressLint("NewApi")
	void initMenu() {
		tabHost.setup();
		tabHost.getTabWidget().setDividerDrawable(null);
		tabHost.addTab(getTabSpec("active", R.drawable.active_tab_selector, "活动"));
		tabHost.addTab(getTabSpec("house", R.drawable.house_tab_selector, "房源库"));
		if (isOpenAgent)
			tabHost.addTab(getTabSpec("agent", R.drawable.agent_tab_selector, "经纪人专区"));
		tabHost.addTab(getTabSpec("me", R.drawable.me_tab_selector, "个人中心"));
		tabHost.setOnTabChangedListener(mainTabChange);
		for (int i = 0; i < tabHost.getTabWidget().getChildCount(); i++) {
			final int j = i;
			tabHost.getTabWidget().getChildAt(i).setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					// Fragment切换的时候可以在这里进行控制
					mSearchFunction.clearSearch();
					FramSuper framSuper = (FramSuper) pageAdapter.getItem(j);
					// framSuper.setTitle();
					mainPager.setCurrentItem(j);
				}
			});
		}
	}
	private TabSpec getTabSpec(String content, int resId, String title) {
		MainTabView tab = MainTabView_.build(this);
		tab.setIndicator(resId);
		tab.setTitle(title);
		TabSpec tabSpec = tabHost.newTabSpec(content).setIndicator(tab)
				.setContent(new MainTabFactory(FramMainActivity.this));
		return tabSpec;
	}
	
	OnPageChangeListener pageListener = new OnPageChangeListener() {
		@Override
		public void onPageSelected(int position) {
			tabHost.setCurrentTab(position);
			// FramSuper framSuper = (FramSuper) pageAdapter.getItem(position);
			// framSuper.setTitle();
		}
		@Override
		public void onPageScrolled(int arg0, float arg1, int arg2) {
		}
		@Override
		public void onPageScrollStateChanged(int arg0) {
		}
	};
	OnTabChangeListener mainTabChange = new OnTabChangeListener() {
		@Override
		public void onTabChanged(String tabId) {
			int position = tabHost.getCurrentTab();
			mainPager.setCurrentItem(position);
		}
	};
	private long exitTime = 0;
	
	private void begin() {
		pageAdapter = new HomeMainAdapter(getSupportFragmentManager(), fInfo, FramMainActivity.this);
		mainPager.setOffscreenPageLimit(3);
		mainPager.setAdapter(pageAdapter);
		mainPager.setOnPageChangeListener(pageListener);
		mainPager.setCurrentItem(0);
	}
	/*
	 * (non Javadoc)
	 * @Title: onCreate
	 * @Description: TODO
	 * @param savedInstanceState
	 * @see com.yoopoon.home.SearchActionBarActivity#onCreate(android.os.Bundle)
	 */
	@Override
	@SuppressLint("InflateParams")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		registerReceivers();
	}
	private void registerReceivers() {
		// 网络状态广播监听
		IntentFilter networkFilter = new IntentFilter(ConnectivityManager.CONNECTIVITY_ACTION);
		this.registerReceiver(receiver, networkFilter);
		// 跳转至房源界面监听
		IntentFilter houseFilter = new IntentFilter("com.yoopoon.broker_takeguest");
		houseFilter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(brokerTakeGuestReceiver, houseFilter);
		// logout监听
		IntentFilter logoutFilter = new IntentFilter("com.yoopoon.logout_action");
		logoutFilter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, logoutFilter);
		// 跳转至经济人界面监听
		IntentFilter agentFilter = new IntentFilter("com.yoopoon.OPEN_AGENT_ACITON");
		agentFilter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, agentFilter);
		// 跳转至个人中心界面监听
		IntentFilter meFilter = new IntentFilter("com.yoopoon.OPEN_ME_ACTION");
		meFilter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, meFilter);
		// 跳转至活动界面监听
		IntentFilter activeFilter = new IntentFilter("com.yoopoon.OPEN_ACTIVE_ACTION");
		activeFilter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, activeFilter);
	}
	
	private BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();
			if ("com.yoopoon.logout_action".equals(action)) {
				mainPager.setCurrentItem(0);
				// 用户登出后 清除所有用户数据
				SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(FramMainActivity.this);
				sp.edit().clear().commit();
			} else if (ConnectivityManager.CONNECTIVITY_ACTION.equals(action)) {
				checkNetworkState();
			} else if ("com.yoopoon.OPEN_AGENT_ACITON".equals(action)) {
				mainPager.setCurrentItem(2);
			} else if ("com.yoopoon.OPEN_ME_ACTION".equals(action)) {
				mainPager.setCurrentItem(3);
			} else if ("com.yoopoon.OPEN_ACTIVE_ACTION".equals(action)) {
				mainPager.setCurrentItem(0);
			}
		}
	};
	private BroadcastReceiver brokerTakeGuestReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			SPUtils.setIsAgentFromReceiver(context, true);
			mainPager.setCurrentItem(1);
		}
	};
	
	private void checkNetworkState() {
		ConnectivityManager connectivityManager = (ConnectivityManager) getSystemService(Context.CONNECTIVITY_SERVICE);
		NetworkInfo info = connectivityManager.getActiveNetworkInfo();
		if (info != null && info.isAvailable() && info.isConnected()) {
			tv_network.setVisibility(View.GONE);
		} else {
			tv_network.setVisibility(View.VISIBLE);
		}
	}
	public void onBackPressed() {
		if ((System.currentTimeMillis() - exitTime) > 2000) {
			Toast.makeText(getApplicationContext(), "再按一次退出程序", Toast.LENGTH_SHORT).show();
			exitTime = System.currentTimeMillis();
		} else {
			android.os.Process.killProcess(android.os.Process.myPid());
		}
	}
	@Override
	protected void onRestart() {
		super.onRestart();
	}
	@Override
	protected void onDestroy() {
		this.unregisterReceiver(receiver);
		this.unregisterReceiver(brokerTakeGuestReceiver);
		super.onDestroy();
	}
	@Override
	protected void onResume() {
		super.onResume();
	}
	@Override
	protected void onStop() {
		super.onStop();
	}
	@Override
	protected void onStart() {
		super.onStart();
	}
	
	class MainTabFactory implements TabHost.TabContentFactory {
		private final Context mContext;
		
		public MainTabFactory(Context context) {
			mContext = context;
		}
		@Override
		public View createTabContent(String tag) {
			View v = new View(mContext);
			v.setMinimumWidth(0);
			v.setMinimumHeight(0);
			return v;
		}
	}
	
	@Override
	public void finish() {
		MyApplication.getInstance().removeActivity(this);
		super.finish();
	}
	@Override
	protected int getHeight() {
		return mainPager.getHeight();
	}
	@Override
	protected View getParentView() {
		return mainPager;
	}
	@Override
	protected void showResult(JSONArray optJSONArray) {
		searchLayout.setVisibility(View.VISIBLE);
	}
	@Override
	protected void cleanSearch() {
		searchLayout.setVisibility(View.GONE);
		System.out.println("cleansearch");
	}
}
