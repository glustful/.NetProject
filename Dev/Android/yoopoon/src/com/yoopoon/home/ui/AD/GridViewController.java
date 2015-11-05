/*
 * created by guojunjun on 15-7-6
 */
package com.yoopoon.home.ui.AD;

import java.util.ArrayList;
import java.util.List;
import org.json.JSONArray;
import android.annotation.SuppressLint;
import android.content.Context;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.MarginLayoutParams;
import android.widget.AdapterView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import com.yoopoon.home.R;

/*
 * viewpage与Gridview组合，自定义头部
 */
public abstract class GridViewController {
	protected View rootView;

	protected Context mContext;

	protected LayoutInflater inflater;

	protected ViewPager mViewPager;
	protected MyPagerAdapter mPagerAdapter;
	protected List<View> mViews;
	private ImageView[] imageViews;
	protected ViewGroup circles;

	public View getRootView() {
		if (rootView == null) {
			initView();

		}
		return rootView;
	}

	public GridViewController(Context context) {
		mContext = context;
		inflater = LayoutInflater.from(mContext);
		mPagerAdapter = new MyPagerAdapter();
		mViews = new ArrayList<View>();
	}

	public abstract void show(ArrayList<JSONArray> urls);

	public void addHeadView(View headView) {
		circles = (ViewGroup) headView;
		((LinearLayout) rootView).addView(headView, 0);
	}

	@SuppressLint("InflateParams")
	private void initView() {

		rootView = inflater.inflate(R.layout.active_page_view, null);
		mViewPager = (ViewPager) rootView.findViewById(R.id.vPager);
		mViewPager.setAdapter(mPagerAdapter);
		mViewPager.setOnPageChangeListener(new AdPageChangeListener());

	}

	public void initCircle() {
		if (circles == null)
			return;
		ViewGroup group = (ViewGroup) circles.findViewById(R.id.circle);
		group.removeAllViews();
		imageViews = new ImageView[mViews.size()];
		// 广告栏的小圆点图标
		for (int i = 0; i < mViews.size(); i++) {
			// 创建一个ImageView, 并设置宽高. 将该对象放入到数组中
			ImageView imageView = new ImageView(mContext);

			MarginLayoutParams lp = new MarginLayoutParams(20, 20);
			lp.rightMargin = 50;
			// imageView.setLayoutParams(lp);

			imageViews[i] = imageView;

			// 初始值, 默认第0个选中
			if (i == 0) {
				imageViews[i].setBackgroundResource(R.drawable.red_point_icon);
			} else {
				imageViews[i].setBackgroundResource(R.drawable.white_point);
			}
			// 将小圆点放入到布局中
			group.addView(imageViews[i], lp);
			System.out.println("addcircle");
		}
	}

	/**
	 * ViewPager 页面改变监听器
	 */
	private final class AdPageChangeListener implements OnPageChangeListener {

		/**
		 * 页面滚动状态发生改变的时候触发
		 */
		@Override
		public void onPageScrollStateChanged(int arg0) {
		}

		/**
		 * 页面滚动的时候触发
		 */
		@Override
		public void onPageScrolled(int arg0, float arg1, int arg2) {
		}

		/**
		 * 页面选中的时候触发
		 */
		@Override
		public void onPageSelected(int arg0) {
			// 获取当前显示的页面是哪个页面

			// 重新设置原点布局集合
			for (int i = 0; i < imageViews.length; i++) {
				imageViews[arg0].setBackgroundResource(R.drawable.red_point_icon);
				if (arg0 != i) {
					imageViews[i].setBackgroundResource(R.drawable.white_point);
				}
			}
		}
	}

	protected class MyPagerAdapter extends PagerAdapter {

		public List<View> mListViews;

		public MyPagerAdapter() {
			this.mListViews = new ArrayList<View>();
		}

		@Override
		public void destroyItem(View arg0, int arg1, Object arg2) {
			((ViewPager) arg0).removeView(mListViews.get(arg1));
		}

		@Override
		public void finishUpdate(View arg0) {
		}

		@Override
		public int getCount() {
			return mListViews.size();
		}

		@Override
		public Object instantiateItem(View arg0, int arg1) {
			((ViewPager) arg0).addView(mListViews.get(arg1), 0);
			return mListViews.get(arg1);
		}

		@Override
		public boolean isViewFromObject(View arg0, Object arg1) {
			return arg0 == (arg1);
		}

		public void refresh(List<View> mViews) {
			this.mListViews.clear();
			this.mListViews.addAll(mViews);
			this.notifyDataSetChanged();
		}

	}

	public abstract void onGridItemClick(AdapterView<?> parent, View view, int position, long id);

}
