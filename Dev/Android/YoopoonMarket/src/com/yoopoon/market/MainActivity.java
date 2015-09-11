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
import android.graphics.Color;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.View;
import android.view.View.OnClickListener;
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
	@ViewById(R.id.vp)
	ViewPager vp;
	@ViewById(R.id.rg)
	RadioGroup rg;
	@ViewById(R.id.search_layout)
	View searchLayout;
	List<Fragment> fragments = new ArrayList<Fragment>();
	List<LinearLayout> lls = new ArrayList<LinearLayout>();

	@AfterViews
	void initUI() {
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

		vp.setOnPageChangeListener(new MyPagerChangeListener());

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
