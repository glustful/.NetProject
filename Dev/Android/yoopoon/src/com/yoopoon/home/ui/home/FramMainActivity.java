package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;

import android.annotation.SuppressLint;
import android.content.Context;
import android.os.Bundle;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TabHost;
import android.widget.TabHost.OnTabChangeListener;
import android.widget.TabHost.TabSpec;
import android.widget.Toast;

import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.SearchActionBarActivity;
import com.yoopoon.home.SearchFunction.OnSearchCallBack;
import com.yoopoon.home.data.net.ResponseData;

@EActivity(R.layout.home_main_activity)
public class FramMainActivity extends SearchActionBarActivity {
	
	static String tag = "FramMainActivity";
	@ViewById(android.R.id.tabhost)
	TabHost tabHost;
	@ViewById(R.id.home_main_pager)
	ViewPager mainPager;
	@ViewById(R.id.home_main_loading_layout)
	RelativeLayout loadingLayout;
	boolean isOpenAgent = false;
	public static FramMainActivity instance;
	HomeMainAdapter pageAdapter;
	ArrayList<FragmentInfo> fInfo;

	@AfterInject
	void initData(){
		instance = this;
		fInfo = new ArrayList<FragmentInfo>();
	}
	@AfterViews
	void initUI() {
		MyApplication.getInstance().addActivity(this);

		initMenu();
		initFragments();
		begin();
	}

	
	void initFragments() {

		Bundle argActive = new Bundle();
		fInfo.add(new FragmentInfo(FramActiveFragment_.class, argActive));

		Bundle argHouse = new Bundle();

		fInfo.add(new FragmentInfo(FramHouseFragment_.class, argHouse));
		if(isOpenAgent){
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
		tabHost.addTab(getTabSpec("active", R.drawable.active_tab_selector,"活动"));
		tabHost.addTab(getTabSpec("house", R.drawable.house_tab_selector,"房源库"));
		if(isOpenAgent)
		tabHost.addTab(getTabSpec("agent", R.drawable.agent_tab_selector,"经纪人专区"));
		tabHost.addTab(getTabSpec("me", R.drawable.me_tab_selector,"个人中心"));
		tabHost.setOnTabChangedListener(mainTabChange);

	}

	private TabSpec getTabSpec(String content, int resId,String title) {
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

	void setIamgeListener(ImageView v, final int i) {
		v.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				mainPager.setCurrentItem(i);
			}
		});
	}

	private long exitTime = 0;

	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event) {
		if (keyCode == KeyEvent.KEYCODE_BACK
				&& event.getAction() == KeyEvent.ACTION_DOWN) {
			if ((System.currentTimeMillis() - exitTime) > 2000) {
				Toast.makeText(getApplicationContext(), "再按一次返回桌面",
						Toast.LENGTH_SHORT).show();
				exitTime = System.currentTimeMillis();
			} else {
				finish();
			}
			return true;
		}
		return super.onKeyDown(keyCode, event);
	}

	private void begin() {
		pageAdapter = new HomeMainAdapter(getSupportFragmentManager(), fInfo,
				FramMainActivity.this);
		mainPager.setOffscreenPageLimit(3);
		mainPager.setAdapter(pageAdapter);
		mainPager.setOnPageChangeListener(pageListener);
		mainPager.setCurrentItem(0);
		
	}

	@Override
	protected void onRestart() {
		super.onRestart();
	}

	@Override
	protected void onDestroy() {

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
	public void initSearchParam() {
		if(this.SearchParameter==null){
			this.SearchParameter = new HashMap<String, String>();
		}
		this.SearchParameter.clear();
		this.SearchParameter.put("page", "1");
		this.SearchParameter.put("pageSize", "10");
		this.SearchParameter.put("condition", "");
	}
	@Override
	public OnSearchCallBack setSearchCallBack() {
		
		return new OnSearchCallBack() {
			
			@Override
			public void textChange(Boolean isText) {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void search(ResponseData data) {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void deltext() {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void clearRefresh() {
				// TODO Auto-generated method stub
				
			}
			
			@Override
			public void addMore(ResponseData data) {
				// TODO Auto-generated method stub
				
			}
		};
	}
	
}