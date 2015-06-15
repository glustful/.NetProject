package com.yoopoon.home.ui.home;

import java.util.ArrayList;
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

import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

@EActivity(R.layout.home_main_activity)
public class FramMainActivity extends MainActionBarActivity {
	
	static String tag = "FramMainActivity";
	@ViewById(android.R.id.tabhost)
	TabHost tabHost;
	@ViewById(R.id.home_main_pager)
	ViewPager mainPager;
	@ViewById(R.id.home_main_loading_layout)
	RelativeLayout loadingLayout;

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

		Bundle argMatter = new Bundle();
		fInfo.add(new FragmentInfo(FramActiveFragment_.class, argMatter));

		Bundle argTalk = new Bundle();

		fInfo.add(new FragmentInfo(FramHouseFragment_.class, argTalk));
		Bundle argContact = new Bundle();
		fInfo.add(new FragmentInfo(FramAgentFragment_.class, argContact));

		Bundle argMe = new Bundle();
		fInfo.add(new FragmentInfo(FramMeFragment_.class, argMe));
	}

	@SuppressLint("NewApi")
	void initMenu() {

		tabHost.setup();
		tabHost.getTabWidget().setDividerDrawable(null);
		tabHost.addTab(getTabSpec("active", R.drawable.me_tab_selector));
		tabHost.addTab(getTabSpec("house", R.drawable.me_tab_selector));
		tabHost.addTab(getTabSpec("agent", R.drawable.me_tab_selector));
		tabHost.addTab(getTabSpec("me", R.drawable.me_tab_selector));
		tabHost.setOnTabChangedListener(mainTabChange);

	}

	private TabSpec getTabSpec(String content, int resId) {
		MainTabView tab = MainTabView_.build(this);

		tab.setIndicator(resId);
		TabSpec tabSpec = tabHost.newTabSpec(content).setIndicator(tab)
				.setContent(new MainTabFactory(FramMainActivity.this));
		return tabSpec;
	}

	OnPageChangeListener pageListener = new OnPageChangeListener() {

		@Override
		public void onPageSelected(int position) {
			FramSuper fs = (FramSuper) pageAdapter.getItem(position);
			getCenterButton().setText(fs.getTitle());
			getCenterButton().setVisibility(View.VISIBLE);
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
		mainPager.setAdapter(pageAdapter);
		mainPager.setOnPageChangeListener(pageListener);
		mainPager.setCurrentItem(0);
		getCenterButton().setText("活动");
		getCenterButton().setVisibility(View.VISIBLE);
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
	public void backButtonClick(View v) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void titleButtonClick(View v) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void rightButtonClick(View v) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public Boolean showHeadView() {
		// TODO Auto-generated method stub
		return true;
	}

}
