package com.yoopoon.component;

import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;
import com.yoopoon.market.view.MyGridView;

public class YoopoonServiceController extends GridViewController {
	public YoopoonServiceController(Context context) {
		super(context);
	}
	
	private int	activeCount	= 0;
	
	@Override
	public void show(ArrayList<JSONArray> urls) {
		for (int i = 0; i < urls.size(); i++) {
			MyGridView mGridView = new MyGridView(mContext);
			mGridView.setLayoutParams(new GridView.LayoutParams(GridView.LayoutParams.MATCH_PARENT,
					GridView.LayoutParams.WRAP_CONTENT));
			mGridView.setNumColumns(3);
			mGridView.setStretchMode(GridView.STRETCH_COLUMN_WIDTH);
			mGridView.setHorizontalSpacing(1);
			mGridView.setVerticalSpacing(1);
			mGridView.setHorizontalScrollBarEnabled(false);
			mGridView.setVerticalScrollBarEnabled(false);
			mGridView.setAdapter(new GridAdapter(mContext, urls.get(i)));
			mGridView.setOnItemClickListener(new OnItemClickListener() {
				@Override
				public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
					onGridItemClick(parent, view, position, id);
					Toast.makeText(mContext, "测试用途" + position, Toast.LENGTH_SHORT).show();
				}
			});
			mViews.add(mGridView);
		}
		initCircle();
		mPagerAdapter.refresh(mViews);
	}
	public void show2(ArrayList<JSONArray> urls) {
		for (int i = 0; i < urls.size(); i++) {
			MyGridView mGridView = new MyGridView(mContext);
			mGridView.setLayoutParams(new GridView.LayoutParams(GridView.LayoutParams.MATCH_PARENT,
					GridView.LayoutParams.WRAP_CONTENT));
			mGridView.setNumColumns(3);
			mGridView.setStretchMode(GridView.STRETCH_COLUMN_WIDTH);
			mGridView.setHorizontalSpacing(1);
			mGridView.setVerticalSpacing(1);
			mGridView.setHorizontalScrollBarEnabled(false);
			mGridView.setVerticalScrollBarEnabled(false);
			// mGridView.setAdapter(new GridAdapter(mContext, urls.get(i)));
			mGridView.setAdapter(new GridAdapter2(new String[] { "霸王餐", "最热电影", "推拿按摩" }, new int[] {
					R.drawable.ic_launcher, R.drawable.ic_launcher, R.drawable.ic_launcher }));
			mGridView.setOnItemClickListener(new OnItemClickListener() {
				@Override
				public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
					onGridItemClick(parent, view, position, id);
				}
			});
			mViews.clear();
			mViews.add(mGridView);
		}
		// initCircle();
		mPagerAdapter.refresh(mViews);
	}
	public boolean isEmpty() {
		return !(activeCount > 0);
	}
	
	class GridAdapter2 extends BaseAdapter {
		String[]	titles;
		int[]		imgs;
		int			width;
		
		public GridAdapter2(String[] titles, int[] imgs) {
			this.titles = titles;
			this.imgs = imgs;
			width = ((int) MyApplication.getInstance().getDeviceInfo((Activity) mContext).widthPixels) / 6;
		}
		@Override
		public int getCount() {
			activeCount = titles.length;
			return titles.length;
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
			Holder holder;
			if (convertView == null) {
				convertView = LayoutInflater.from(mContext).inflate(R.layout.services_page_item_view, null);
				holder = new Holder();
				holder.init(convertView);
				convertView.setTag(holder);
			} else {
				holder = (Holder) convertView.getTag();
			}
			holder.img.setLayoutParams(new LinearLayout.LayoutParams(width, width));
			holder.img.setImageBitmap(null);
			holder.img.setImageResource(imgs[position]);
			holder.title.setText(titles[position]);
			return convertView;
		}
	}
	
	class GridAdapter extends BaseAdapter {
		Context		mContext;
		JSONArray	datas;
		int			width;
		
		public GridAdapter(Context context, JSONArray datas) {
			this.mContext = context;
			this.datas = datas;
			width = ((int) MyApplication.getInstance().getDeviceInfo((Activity) mContext).widthPixels) / 5;
		}
		@Override
		public int getCount() {
			activeCount = datas.length();
			return datas.length();
		}
		@Override
		public Object getItem(int position) {
			return datas.optJSONObject(position);
		}
		@Override
		public long getItemId(int position) {
			return position;
		}
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			Holder holder;
			JSONObject item = datas.optJSONObject(position);
			if (convertView == null) {
				convertView = LayoutInflater.from(mContext).inflate(R.layout.services_page_item_view, null);
				holder = new Holder();
				holder.init(convertView);
				convertView.setTag(holder);
			} else {
				holder = (Holder) convertView.getTag();
			}
			holder.img.setLayoutParams(new LinearLayout.LayoutParams(width, width));
			holder.img.setImageBitmap(null);
			holder.img.setTag(mContext.getString(R.string.url_image) + item.optString("TitleImg"));
			holder.title.setText(item.optString("Title"));
			holder.adTitle.setText(item.optString("AdSubTitle"));
			/*ImageLoader.getInstance().displayImage(mContext.getString(R.string.url_image) + item.optString("TitleImg"),
					holder.img, MyApplication.getOptions(), MyApplication.getLoadingListener());*/
			return convertView;
		}
	}
	
	class Holder {
		TextView	title;
		TextView	adTitle;
		ImageView	img;
		
		public void init(View root) {
			title = (TextView) root.findViewById(R.id.title);
			adTitle = (TextView) root.findViewById(R.id.adTitle);
			img = (ImageView) root.findViewById(R.id.imageView);
		}
	}
	
	@Override
	public void onGridItemClick(AdapterView<?> parent, View view, int position, long id) {
		//Toast.makeText(mContext, ""+position, Toast.LENGTH_SHORT).show();
	}
}
