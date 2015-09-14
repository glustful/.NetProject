/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MainActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-7 下午4:51:39 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;

import com.yoopoon.market.fragment.CartFragment;
import com.yoopoon.market.fragment.MeFragment;
import com.yoopoon.market.fragment.ServeFragment;
import com.yoopoon.market.fragment.ShopFragment;

/**
 * @ClassName: MainActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:51:39
 */
@EActivity(R.layout.activity_main)
public class MainActivity extends FragmentActivity implements OnClickListener {
	private static final String TAG = "MainActivity";
	private Context mContext;
	@ViewById(R.id.vp)
	ViewPager vp;
	@ViewById(R.id.rg)
	RadioGroup rg;
	@ViewById(R.id.rightBtn)
	Button rightBtn;
	@ViewById(R.id.search_layout)
	View searchLayout;
	@ViewById(R.id.btn_select)
	Button btn_select;
	@ViewById(R.id.tv_firstserve)
	TextView tv_first;
	@ViewById(R.id.rightBtn)
	Button btn_category;
	List<Fragment> fragments = new ArrayList<Fragment>();
	List<LinearLayout> lls = new ArrayList<LinearLayout>();
	String[] areas = { "北京", "大理", "香格里拉", "西双版纳" };
	int checkedItem = 0;

	@AfterViews
	void initUI() {
		mContext = MainActivity.this;
		fragments.add(new ShopFragment());
		fragments.add(new ServeFragment());
		fragments.add(new CartFragment());
		fragments.add(new MeFragment());
		vp.setAdapter(new MyPageAdapter(getSupportFragmentManager()));
		lls.add((LinearLayout) findViewById(R.id.ll1));
		lls.add((LinearLayout) findViewById(R.id.ll2));
		lls.add((LinearLayout) findViewById(R.id.ll3));
		lls.add((LinearLayout) findViewById(R.id.ll4));
		for (LinearLayout ll : lls)
			ll.setOnClickListener(this);
		btn_select.setOnClickListener(new SearchViewClickListener());
		btn_category.setOnClickListener(new SearchViewClickListener());
		vp.setOnPageChangeListener(new MyPagerChangeListener());
		rightBtn.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(mContext, ProductClassifyActivity_.class);
				startActivity(intent);
			}
		});
	}

	private class SearchViewClickListener implements OnClickListener {
		@Override
		public void onClick(View v) {
			switch (v.getId()) {
				case R.id.btn_select:
					AlertDialog.Builder builder = new Builder(MainActivity.this);
					builder.setTitle("请选择地区");
					builder.setPositiveButton("确定", new DialogInterface.OnClickListener() {
						@Override
						public void onClick(DialogInterface dialog, int which) {
							btn_select.setText(areas[checkedItem]);
							dialog.dismiss();
						}
					});
					builder.setNegativeButton("取消", new DialogInterface.OnClickListener() {
						@Override
						public void onClick(DialogInterface dialog, int which) {
							dialog.dismiss();
						}
					});
					builder.setSingleChoiceItems(areas, checkedItem, new DialogInterface.OnClickListener() {
						@Override
						public void onClick(DialogInterface dialog, int which) {
							checkedItem = which;
							btn_select.setText(areas[which]);
							dialog.dismiss();
						}
					});
					builder.show();
					break;
				case R.id.rightBtn:
					ProductClassifyActivity_.intent(MainActivity.this).start();
					break;
				default:
					break;
			}
		}
	}

	private class MyPageAdapter extends FragmentPagerAdapter {
		public MyPageAdapter(FragmentManager fm) {
			super(fm);
		}
		@Override
		public Fragment getItem(int arg0) {
			return fragments.get(arg0);
		}
		@Override
		public int getCount() {
			return fragments.size();
		}
	}

	private class MyPagerChangeListener implements OnPageChangeListener {
		@Override
		public void onPageScrollStateChanged(int arg0) {
			// TODO Auto-generated method stub
		}
		@Override
		public void onPageScrolled(int arg0, float arg1, int arg2) {
			// TODO Auto-generated method stub
		}
		@Override
		public void onPageSelected(int arg0) {
			onClick(lls.get(arg0));
			searchLayout.setVisibility((arg0 > 1) ? View.GONE : View.VISIBLE);
			tv_first.setVisibility((arg0 == 0) ? View.GONE : View.VISIBLE);
		}
	}

	@Override
	public void onBackPressed() {
		// super.onBackPressed();
		exit();
	}

	long exitTime;

	public void exit() {
		if ((System.currentTimeMillis() - exitTime) > 2000) {
			Toast.makeText(getApplicationContext(), "再按一次退出程序", Toast.LENGTH_SHORT).show();
			exitTime = System.currentTimeMillis();
		} else {
			finish();
			System.exit(0);
		}
	}
	public void toServe(View v) {
		vp.setCurrentItem(1);
		showFirstTv(false);
	}
	public void showFirstTv(boolean shown) {
		tv_first.setVisibility(shown ? View.VISIBLE : View.GONE);
	}
	@Override
	public void onClick(View v) {
		for (LinearLayout ll : lls) {
			RadioButton radioButton = (RadioButton) ll.getChildAt(0);
			TextView textView = (TextView) ll.getChildAt(1);
			radioButton.setChecked(false);
			textView.setTextColor(Color.GRAY);
		}
		LinearLayout ll = (LinearLayout) v;
		RadioButton radioButton = (RadioButton) ll.getChildAt(0);
		TextView textView = (TextView) ll.getChildAt(1);
		radioButton.setChecked(true);
		textView.setTextColor(Color.RED);
		switch (v.getId()) {
			case R.id.ll1:
				vp.setCurrentItem(0);
				break;
			case R.id.ll2:
				vp.setCurrentItem(1);
				break;
			case R.id.ll3:
				vp.setCurrentItem(2);
				break;
			case R.id.ll4:
				vp.setCurrentItem(3);
				break;
		}
	}
}
