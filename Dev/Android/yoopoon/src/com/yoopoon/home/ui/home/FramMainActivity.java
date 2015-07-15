package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup.LayoutParams;
import android.widget.LinearLayout;
import android.widget.TabHost;
import android.widget.TabHost.OnTabChangeListener;
import android.widget.TabHost.TabSpec;
import android.widget.Toast;
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
	boolean isOpenAgent = false;
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
					mSearchFunction.clearSearch();
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

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if (keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN) {
			if ((System.currentTimeMillis() - exitTime) > 2000) {
				Toast.makeText(getApplicationContext(), "再按一次返回桌面", Toast.LENGTH_SHORT).show();
				exitTime = System.currentTimeMillis();
			} else {
				finish();
			}
			return true;
		}
		return super.onKeyDown(keyCode, event);
	}

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
		registerLogoutBroadcast();
	}

	private BroadcastReceiver logoutReceiver = new BroadcastReceiver() {

		public void onReceive(Context context, android.content.Intent intent) {
			Log.i(tag, "收到用户等出广播啦！");
			mainPager.setCurrentItem(0);

			// 用户登出后 清除所有用户数据
			SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(FramMainActivity.this);
			sp.edit().clear().commit();
		};
	};

	private void registerLogoutBroadcast() {
		IntentFilter filter = new IntentFilter("com.yoopoon.logout_action");
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(logoutReceiver, filter);
	}

	@Override
	protected void onRestart() {
		super.onRestart();
	}

	@Override
	protected void onDestroy() {
		this.unregisterReceiver(logoutReceiver);
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
		// TODO Auto-generated method stub
		return mainPager.getHeight();
	}

	@Override
	protected View getParentView() {
		// TODO Auto-generated method stub
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
