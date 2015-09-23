package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;
import com.yoopoon.market.domain.ServiceOrderEntity;
import com.yoopoon.market.fragment.ServiceOrder1;
import com.yoopoon.market.fragment.ServiceOrder2;
import com.yoopoon.market.fragment.ServiceOrder3;

@EActivity(R.layout.activity_me_service)
public class MeServiceActivity extends MainActionBarActivity implements OnClickListener {
	private static final String TAG = "MeServiceActivity";
	@ViewById(R.id.vp)
	ViewPager vp;
	@Extra
	int item;
	@Extra
	List<ServiceOrderEntity> services;
	List<ServiceOrderEntity> createdOrders = new ArrayList<ServiceOrderEntity>(); // 待付款
	List<ServiceOrderEntity> payedOrders = new ArrayList<ServiceOrderEntity>();// 待发货
	List<ServiceOrderEntity> deliveringOrders = new ArrayList<ServiceOrderEntity>();// 待收货
	List<Fragment> fragments = new ArrayList<Fragment>();
	List<TextView> textViews = new ArrayList<TextView>();

	@AfterViews
	void initUI() {
		Log.i(TAG, "item = " + item);
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("我的服务");

		fragments.add(new ServiceOrder1());
		fragments.add(new ServiceOrder2());
		fragments.add(new ServiceOrder3());

		textViews.add((TextView) findViewById(R.id.tv1));
		textViews.add((TextView) findViewById(R.id.tv2));
		textViews.add((TextView) findViewById(R.id.tv3));

		vp.setAdapter(new MyViewPagerAdapter(getSupportFragmentManager()));
		vp.setOnPageChangeListener(new MyPageChangeListener());

		for (TextView tv : textViews)
			tv.setOnClickListener(this);
		vp.setCurrentItem(item);

		initList();
	}

	void initList() {
		for (ServiceOrderEntity service : services) {
			switch (service.Status) {
				case 1:
					createdOrders.add(service);
					break;
				case 2:
					payedOrders.add(service);
					break;
				case 3:
					deliveringOrders.add(service);
					break;
				default:
					break;
			}
		}
	}

	public List<ServiceOrderEntity> getServiceList(int item) {
		switch (item) {
			case 0:
				return createdOrders;
			case 1:
				return payedOrders;
			case 2:
				return deliveringOrders;

			default:
				break;
		}
		return null;
	}

	class MyPageChangeListener implements OnPageChangeListener {

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
			for (TextView tv : textViews)
				tv.setBackgroundResource(R.drawable.white_tv_bg);
			textViews.get(arg0).setBackgroundResource(R.drawable.red_line_bg);
		}

	}

	class MyViewPagerAdapter extends FragmentPagerAdapter {

		public MyViewPagerAdapter(FragmentManager fm) {
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

	@Override
	public void backButtonClick(View v) {
		finish();

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
		return true;
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.tv1:
			case R.id.tv2:
			case R.id.tv3:
			case R.id.tv4:

				for (TextView tv : textViews)
					tv.setBackgroundResource(R.drawable.white_tv_bg);
				v.setBackgroundResource(R.drawable.red_line_bg);
				int item = Integer.parseInt((String) v.getTag());
				vp.setCurrentItem(item);
				break;
		}
	}

}
