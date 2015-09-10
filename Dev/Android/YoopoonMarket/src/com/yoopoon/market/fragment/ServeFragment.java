/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ShopFragment.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-7 下午4:50:59 
 * @version: V1.0   
 */
package com.yoopoon.market.fragment;

import java.util.Timer;
import java.util.TimerTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.yoopoon.market.AssuranceActivity_;
import com.yoopoon.market.CleanServeActivity_;
import com.yoopoon.market.R;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class ServeFragment extends Fragment {
	private static final String TAG = "ServeFragment";
	private static final int LOOPIMAGE = 0;
	View rootView;
	GridView gv;
	ViewPager vp;
	String[] functions = { "保险", "金融理财", "旅游", "汽车类服务", "家政", "清洗服务", "教育", "生活缴费", "快递代收" };
	int[] imgIdArray;
	ImageView[] tips;
	ImageView[] mImageViews;
	LinearLayout ll_points;
	boolean loopped = false;
	Handler handler = new Handler() {
		public void handleMessage(android.os.Message msg) {
			switch (msg.what) {
				case LOOPIMAGE:
					vp.setCurrentItem(vp.getCurrentItem() + 1);
					break;
			}
		};
	};

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_serve, null);
		init();
		return rootView;
	}

	private void init() {
		gv = (GridView) rootView.findViewById(R.id.gv);
		vp = (ViewPager) rootView.findViewById(R.id.vp);
		ll_points = (LinearLayout) rootView.findViewById(R.id.ll_points);

		gv.setAdapter(new MyGridViewAdapter());
		gv.setOnItemClickListener(new MyGridViewItemClickListener());

		initImages();
		vp.setAdapter(new MyViewPagerAdapter());
		vp.setCurrentItem((mImageViews.length) * 100);
		vp.addOnPageChangeListener(new MyPageChangeListener());

		if (!loopped) {
			loopped = true;
			loopImage();
		}

	}

	private class MyGridViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			switch (position) {
				case 0:
					AssuranceActivity_.intent(getActivity()).start();
					break;
				case 5:
					CleanServeActivity_.intent(getActivity()).start();
					break;

				default:
					break;
			}

		}

	}

	private void loopImage() {
		new Timer().schedule(new TimerTask() {

			@Override
			public void run() {
				Message msg = new Message();
				msg.what = LOOPIMAGE;
				handler.sendMessage(msg);

			}
		}, 1000, 2000);
	}

	private class MyPageChangeListener implements OnPageChangeListener {

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
			for (int i = 0; i < tips.length; i++)
				tips[i].setBackgroundResource(R.drawable.white_point);
			tips[arg0 % mImageViews.length].setBackgroundResource(R.drawable.black_point);
		}

	}

	void initImages() {
		imgIdArray = new int[] { R.drawable.logo_gray, R.drawable.logo_gray, R.drawable.logo_gray,
				R.drawable.logo_gray, R.drawable.logo_gray };

		// 将点点加入到ViewGroup中
		tips = new ImageView[imgIdArray.length];
		for (int i = 0; i < tips.length; i++) {
			ImageView imageView = new ImageView(getActivity());
			imageView.setLayoutParams(new LayoutParams(10, 10));
			tips[i] = imageView;
			if (i == 0) {
				tips[i].setBackgroundResource(R.drawable.black_point);
			} else {
				tips[i].setBackgroundResource(R.drawable.white_point);
			}

			LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(new ViewGroup.LayoutParams(
					LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
			layoutParams.leftMargin = 5;
			layoutParams.rightMargin = 5;
			ll_points.addView(imageView, layoutParams);
		}

		// 将图片装载到数组中
		mImageViews = new ImageView[imgIdArray.length];
		for (int i = 0; i < mImageViews.length; i++) {
			ImageView imageView = new ImageView(getActivity());
			mImageViews[i] = imageView;
			imageView.setBackgroundResource(imgIdArray[i]);
		}
	}

	static class ViewHolder {
		TextView tv_function;
		ImageView iv_function;
	}

	private class MyGridViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return 9;
		}

		@Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return null;
		}

		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder = null;

			if (convertView == null)
				convertView = View.inflate(getActivity(), R.layout.item_serve, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_function = (TextView) convertView.findViewById(R.id.tv);
				holder.iv_function = (ImageView) convertView.findViewById(R.id.iv);
				convertView.setTag(holder);
			}
			holder.tv_function.setText(functions[position]);
			return convertView;
		}
	}

	public class MyViewPagerAdapter extends PagerAdapter {

		@Override
		public int getCount() {
			return Integer.MAX_VALUE;
		}

		@Override
		public boolean isViewFromObject(View arg0, Object arg1) {
			return arg0 == arg1;
		}

		@Override
		public void destroyItem(View container, int position, Object object) {
			((ViewPager) container).removeView(mImageViews[position % mImageViews.length]);

		}

		/**
		 * 载入图片进去，用当前的position 除以 图片数组长度取余数是关键
		 */
		@Override
		public Object instantiateItem(View container, int position) {
			((ViewPager) container).addView(mImageViews[position % mImageViews.length], 0);
			return mImageViews[position % mImageViews.length];
		}

	}
}
