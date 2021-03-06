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

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.DisplayMetrics;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;
import com.yoopoon.market.ServeListActivity2_;
import com.yoopoon.market.ServeListActivity_;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class ServeFragment extends Fragment {
	private static final int LOOPIMAGE = 0;
	View rootView;
	GridView gv;
	ViewPager vp;
	private RelativeLayout adRelativeLayout;
	private Context mContext;
	// 9大功能
	String[] functions = { "保险", "金融理财", "旅游", "汽车类服务", "家政", "清洗服务", "教育", "生活缴费", "快递代收", "社区互动信息展示", "社区众筹信息展示" };
	int[] icons = { R.drawable.inssurance, R.drawable.financial, R.drawable.travel, R.drawable.uber,
			R.drawable.political, R.drawable.wash_machine, R.drawable.education, R.drawable.charge,
			R.drawable.delivery, R.drawable.interaction, R.drawable.allraise };
	int[] imgIdArray;
	ImageView[] tips;
	ImageView[] mImageViews;
	LinearLayout ll_points;
	boolean loopped = false;
	boolean isFirst = true;
	// hanlder用来处理ViewPager图片的轮播
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
		adRelativeLayout = (RelativeLayout) rootView.findViewById(R.id.rl_ad);
		mContext = getActivity();
		init();
		return rootView;
	}
	// 初始化
	private void init() {
		gv = (GridView) rootView.findViewById(R.id.gv);
		vp = (ViewPager) rootView.findViewById(R.id.vp);
		ll_points = (LinearLayout) rootView.findViewById(R.id.ll_points);
		//设置广告高度
		DisplayMetrics metrics = new DisplayMetrics();
		getActivity().getWindowManager().getDefaultDisplay().getMetrics(metrics);
		android.widget.RelativeLayout.LayoutParams adLayoutParams = new android.widget.RelativeLayout.LayoutParams(
				android.widget.RelativeLayout.LayoutParams.MATCH_PARENT, metrics.heightPixels * 1 / 5);
		adRelativeLayout.setLayoutParams(adLayoutParams);
		gv.setAdapter(new MyGridViewAdapter());
		gv.setOnItemClickListener(new MyGridViewItemClickListener());
		// initImages();
		if (mImageViews == null)
			requestData();
		else {
			addTips(mImageViews.length);
			vp.setAdapter(new MyViewPagerAdapter());
			vp.setCurrentItem((mImageViews.length) * 100);
			vp.addOnPageChangeListener(new MyPageChangeListener());
		}
	}
	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		super.setUserVisibleHint(isVisibleToUser);
		if (isVisibleToUser) {
			SharedPreferences sp = getActivity().getSharedPreferences(getString(R.string.share_preference),
					Context.MODE_PRIVATE);
			boolean isFirst = sp.getBoolean("isFirst", true);
			if (isFirst) {
				Intent intent = new Intent("com.yoopoon.market.show_shadow");
				intent.addCategory(Intent.CATEGORY_DEFAULT);
				getActivity().sendBroadcast(intent);
			}
		}
	}
	private void requestData() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						try {
							JSONArray array = object.getJSONArray("Object");
							String[] urls = new String[array.length()];
							for (int i = 0; i < array.length(); i++) {
								JSONObject jsonObject = array.getJSONObject(i);
								String url = jsonObject.optString("TitleImg", "");
								if((!url.equals("null"))&&(url!=null)&&(!url.equals("")))
								urls[i] = getString(R.string.url_image) + url;
							}
							initImages(urls);
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
					} else {
						Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
					}
				} else {
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_test)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	// GridView的每个Item都代表一个功能，点击后就触发相应的界面
	private class MyGridViewItemClickListener implements OnItemClickListener {
		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			switch (position) {
				case 0:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "保险", "保险" }).start();
					break;
				case 1:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "金融理财", "金融理财" }).start();
					break;
				case 2:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "旅游", "旅游" }).start();
					break;
				case 3:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "汽车类服务", "汽车" }).start();
					break;
				case 4:
					ServeListActivity2_.intent(getActivity()).contents(new String[] { "家政", "家政" }).start();
					break;
				case 5:
					ServeListActivity2_.intent(getActivity()).contents(new String[] { "清洗服务", "清洗" }).start();
					break;
				case 6:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "教育", "教育" }).start();
					break;
				case 7:
					ServeListActivity_.intent(getActivity()).contents(new String[] { "生活缴费", "费用" }).start();
					break;
				case 8:
					ServeListActivity2_.intent(getActivity()).contents(new String[] { "快递代收", "快递" }).start();
					break;
				case 9:
					ServeListActivity2_.intent(getActivity()).contents(new String[] { "社区互动信息展示", "互动" }).start();
					break;
				case 10:
					ServeListActivity2_.intent(getActivity()).contents(new String[] { "社区众筹信息展示", "众筹" }).start();
					break;
			}
		}
	}

	// 图片的轮播用Timer实现，但必须放在子线程中
	private void loopImage() {
		new Timer().schedule(new TimerTask() {
			@Override
			public void run() {
				Message msg = new Message();
				msg.what = LOOPIMAGE;
				handler.sendMessage(msg);
			}
		}, 4000, 4000);
	}

	// ViewPager的页面发生改变时，下面相应的点的颜色也要改变
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

	// 初始化图片
	void initImages(String[] urls) {
		// 将点点加入到ViewGroup中
		addTips(urls.length);
		// 将图片装载到数组中
		mImageViews = new ImageView[urls.length];
		for (int i = 0; i < urls.length; i++) {
			ImageView imageView = new ImageView(getActivity());
			imageView.setLayoutParams(new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.MATCH_PARENT));
			imageView.setScaleType(ScaleType.FIT_XY);
			imageView.setTag(urls[i]);
			if ((!urls[i].equals("null"))&&(!urls[i].equals(""))&&urls[i]!=null) {
				ImageLoader.getInstance().displayImage(urls[i], imageView, MyApplication.getOptions(),
						MyApplication.getLoadingListener());
			}
			mImageViews[i] = imageView;
		}
		vp.setAdapter(new MyViewPagerAdapter());
		vp.setCurrentItem((mImageViews.length) * 100);
		vp.addOnPageChangeListener(new MyPageChangeListener());
		if (!loopped) {
			loopped = true;
			loopImage();
		}
	}
	void addTips(int length) {
		tips = new ImageView[length];
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
	}

	static class ViewHolder {
		TextView tv_function;
		ImageView iv_function;
	}

	// GridView的Adapter
	private class MyGridViewAdapter extends BaseAdapter {
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return functions.length;
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
			holder.iv_function.setImageResource(icons[position]);
			return convertView;
		}
	}

	// ViewPager的Adapter
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
		@Override
		public Object instantiateItem(View container, int position) {
			((ViewPager) container).addView(mImageViews[position % mImageViews.length], 0);
			return mImageViews[position % mImageViews.length];
		}
	}
}
