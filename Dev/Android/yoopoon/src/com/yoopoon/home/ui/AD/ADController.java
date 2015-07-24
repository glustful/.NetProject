package com.yoopoon.home.ui.AD;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import android.app.Activity;
import android.content.Context;
import android.graphics.Bitmap;
import android.os.Handler;
import android.os.Message;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.DisplayMetrics;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.view.ViewGroup.MarginLayoutParams;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.listener.ImageLoadingListener;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

public class ADController {

	View rootView;

	Context mContext;

	LayoutInflater inflater;

	private ViewPager adViewPager;
	private LinearLayout pagerLayout;
	private List<View> pageViews;
	private ImageView[] imageViews;
	private ImageView imageView;
	private AdPageAdapter adapter;
	private AtomicInteger atomicInteger = new AtomicInteger(0);
	private boolean isContinue = true;
	private List<String> urls;

	public View getRootView() {
		if (rootView == null) {
			initViewPager();
		}
		return rootView;
	}

	public ADController(Context context) {
		mContext = context;
		inflater = LayoutInflater.from(mContext);
	}

	public void show(List<String> urls) {
		this.urls = urls;
		if (rootView == null)
			return;
		initPageAdapter();

		initCirclePoint();

		adViewPager.setAdapter(adapter);
		new Thread(new Runnable() {

			@Override
			public void run() {
				while (true) {
					if (isContinue) {
						viewHandler.sendEmptyMessage(atomicInteger.get());
						atomicOption();
					}
				}
			}
		}).start();
	}

	private void initViewPager() {

		rootView = inflater.inflate(R.layout.ad_page_view, null);

		// 从布局文件中获取ViewPager父容器
		pagerLayout = (LinearLayout) rootView.findViewById(R.id.view_pager_content);
		// 创建ViewPager
		adViewPager = new ViewPager(mContext);

		// 获取屏幕像素相关信息
		DisplayMetrics dm = new DisplayMetrics();
		((Activity) mContext).getWindowManager().getDefaultDisplay().getMetrics(dm);

		// 根据屏幕信息设置ViewPager广告容器的宽高
		adViewPager.setLayoutParams(new LayoutParams(dm.widthPixels, dm.heightPixels * 1 / 5));

		// 将ViewPager容器设置到布局文件父容器中
		pagerLayout.addView(adViewPager);

		adViewPager.setOnPageChangeListener(new AdPageChangeListener());

	}

	private void atomicOption() {
		atomicInteger.incrementAndGet();
		// if (atomicInteger.get() > imageViews.length - 1) {
		// atomicInteger.getAndAdd(-imageViews.length);
		// }
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {

		}
	}

	/*
	 * 每隔固定时间切换广告栏图片
	 */
	private final Handler viewHandler = new Handler() {

		@Override
		public void handleMessage(Message msg) {
			adViewPager.setCurrentItem(msg.what);
			super.handleMessage(msg);
		}

	};

	private void initPageAdapter() {
		pageViews = new ArrayList<View>();
		if (urls != null)
			for (int i = 0; i < urls.size(); i++) {
				ImageView img = new ImageView(mContext);

				img.setTag(mContext.getString(R.string.url_host_img) + urls.get(i));

				img.setScaleType(ScaleType.FIT_XY);
				pageViews.add(img);
			}
		adapter = new AdPageAdapter(pageViews);
	}

	private void initCirclePoint() {
		ViewGroup group = (ViewGroup) rootView.findViewById(R.id.viewGroup);
		group.removeAllViews();
		imageViews = new ImageView[pageViews.size()];
		// 广告栏的小圆点图标
		for (int i = 0; i < pageViews.size(); i++) {
			// 创建一个ImageView, 并设置宽高. 将该对象放入到数组中
			imageView = new ImageView(mContext);

			MarginLayoutParams lp = new MarginLayoutParams(20, 20);
			lp.rightMargin = 50;
			// imageView.setLayoutParams(lp);

			imageViews[i] = imageView;

			// 初始值, 默认第0个选中
			if (i == 0) {
				imageViews[i].setBackgroundResource(R.drawable.black_point);
			} else {
				imageViews[i].setBackgroundResource(R.drawable.white_point);
			}
			// 将小圆点放入到布局中
			group.addView(imageViews[i], lp);
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
			atomicInteger.getAndSet(arg0);
			// 重新设置原点布局集合
			for (int i = 0; i < imageViews.length; i++) {
				imageViews[arg0 % imageViews.length].setBackgroundResource(R.drawable.black_point);
				if (arg0 % imageViews.length != i) {
					imageViews[i % imageViews.length].setBackgroundResource(R.drawable.white_point);
				}
			}
		}
	}

	private final class AdPageAdapter extends PagerAdapter {
		private List<View> views = null;

		/**
		 * 初始化数据源, 即View数组
		 */
		public AdPageAdapter(List<View> views) {
			this.views = views;
		}

		/**
		 * 从ViewPager中删除集合中对应索引的View对象
		 */
		@Override
		public void destroyItem(View container, int position, Object object) {
			((ViewPager) container).removeView(views.get(position % views.size()));
		}

		/**
		 * 获取ViewPager的个数
		 */
		@Override
		public int getCount() {
			return Integer.MAX_VALUE;
		}

		/**
		 * 从View集合中获取对应索引的元素, 并添加到ViewPager中
		 */
		@Override
		public Object instantiateItem(View container, int position) {

			ImageView imageView = (ImageView) views.get(position % views.size());
			ImageLoader.getInstance().displayImage(imageView.getTag().toString(), imageView,
					MyApplication.getOptions(), new ImageLoadingListener() {

						@Override
						public void onLoadingStarted(String imageUri, View view) {
							// TODO Auto-generated method stub

						}

						@Override
						public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
							// TODO Auto-generated method stub

						}

						@Override
						public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
							if (imageUri.equals(view.getTag().toString())) {
								((ImageView) view).setImageBitmap(loadedImage);

							}

						}

						@Override
						public void onLoadingCancelled(String imageUri, View view) {
							// TODO Auto-generated method stub

						}
					});
			if (imageView.getParent() != null) {
				((ViewGroup) imageView.getParent()).removeView(imageView);
			}
			((ViewPager) container).addView(imageView, new LayoutParams(LayoutParams.MATCH_PARENT,
					LayoutParams.MATCH_PARENT));

			return imageView;
		}

		/**
		 * 是否将显示的ViewPager页面与instantiateItem返回的对象进行关联 这个方法是必须实现的
		 */
		@Override
		public boolean isViewFromObject(View view, Object object) {
			return view == object;
		}
	}
}
