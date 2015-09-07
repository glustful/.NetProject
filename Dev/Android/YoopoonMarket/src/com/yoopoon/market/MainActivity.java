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
import android.annotation.SuppressLint;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;
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
public class MainActivity extends MainActionBarActivity implements OnClickListener {
	private ViewPager vp;
	private List<Fragment> fragments = new ArrayList<Fragment>();
	private List<TextView> textviews = new ArrayList<TextView>();

	@Override
	@SuppressLint("InflateParams")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		init();
	}

	private void init() {
		vp = (ViewPager) findViewById(R.id.vp);
		fragments.add(new ShopFragment());
		fragments.add(new ServeFragment());
		fragments.add(new CartFragment());
		fragments.add(new MeFragment());
		vp.setAdapter(new MyPageAdapter(getSupportFragmentManager()));

		textviews.add((TextView) findViewById(R.id.tv1));
		textviews.add((TextView) findViewById(R.id.tv2));
		textviews.add((TextView) findViewById(R.id.tv3));
		textviews.add((TextView) findViewById(R.id.tv4));
		vp.setOnPageChangeListener(new MyPagerChangeListener());
		for (int i = 0; i < textviews.size(); i++)
			textviews.get(i).setOnClickListener(this);

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
			for (int i = 0; i < textviews.size(); i++)
				textviews.get(i).setBackgroundColor(Color.WHITE);
			textviews.get(arg0).setBackgroundColor(Color.BLACK);
		}

	}

	@Override
	public void onClick(View v) {
		for (int i = 0; i < textviews.size(); i++)
			textviews.get(i).setBackgroundColor(Color.WHITE);
		v.setBackgroundColor(Color.BLACK);
		switch (v.getId()) {
			case R.id.tv1:
				vp.setCurrentItem(0);
				break;

			case R.id.tv2:
				vp.setCurrentItem(1);
				break;

			case R.id.tv3:
				vp.setCurrentItem(2);
				break;

			case R.id.tv4:
				vp.setCurrentItem(3);
				break;

		}
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
		return null;
	}

}
