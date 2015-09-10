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
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RadioGroup.OnCheckedChangeListener;
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
public class MainActivity extends MainActionBarActivity {
	@ViewById(R.id.vp)
	ViewPager vp;
	@ViewById(R.id.rg)
	RadioGroup rg;
	@ViewById(R.id.search_layout)
	View searchLayout;
	List<Fragment> fragments = new ArrayList<Fragment>();
	List<RadioButton> radioButtons = new ArrayList<RadioButton>();

	@AfterViews
	void initUI() {
		fragments.add(new ShopFragment());
		fragments.add(new ServeFragment());
		fragments.add(new CartFragment());
		fragments.add(new MeFragment());
		vp.setAdapter(new MyPageAdapter(getSupportFragmentManager()));

		radioButtons.add((RadioButton) findViewById(R.id.rb1));
		radioButtons.add((RadioButton) findViewById(R.id.rb2));
		radioButtons.add((RadioButton) findViewById(R.id.rb3));
		radioButtons.add((RadioButton) findViewById(R.id.rb4));
		vp.setOnPageChangeListener(new MyPagerChangeListener());

		for (final RadioButton radioButton : radioButtons) {
			radioButton.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					radioButton.setChecked(true);

				}
			});
		}

		rg.setOnCheckedChangeListener(new OnCheckedChangeListener() {

			@Override
			public void onCheckedChanged(RadioGroup group, int checkedId) {
				for (RadioButton radioButton : radioButtons)
					radioButton.setTextColor(Color.GRAY);
				switch (checkedId) {
					case R.id.rb1:
						vp.setCurrentItem(0);
						radioButtons.get(0).setTextColor(Color.RED);
						break;
					case R.id.rb2:
						vp.setCurrentItem(1);
						radioButtons.get(1).setTextColor(Color.RED);
						break;
					case R.id.rb3:
						vp.setCurrentItem(2);
						radioButtons.get(2).setTextColor(Color.RED);
						break;
					case R.id.rb4:
						vp.setCurrentItem(3);
						radioButtons.get(3).setTextColor(Color.RED);
						break;
				}

			}
		});

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
			radioButtons.get(arg0).setChecked(true);
			searchLayout.setVisibility((arg0 > 1) ? View.GONE : View.VISIBLE);
		}

	}

	public void toServe(View v) {
		vp.setCurrentItem(1);
	}

	@Override
	public void backButtonClick(View v) {

	}

	@Override
	public void titleButtonClick(View v) {

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {
		return false;
	}

}
