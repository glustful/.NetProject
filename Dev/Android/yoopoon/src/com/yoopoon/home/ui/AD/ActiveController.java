package com.yoopoon.home.ui.AD;

import org.json.JSONArray;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.LinearLayout;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

public class ActiveController {

	View rootView;

	Context mContext;

	LayoutInflater inflater;
	GridView mGridView;
	GridAdapter adapter;
	

	public View getRootView() {
		if (rootView == null) {
			initView();
			
		}
		return rootView;
	}

	public ActiveController(Context context) {
		mContext = context;
		inflater = LayoutInflater.from(mContext);
	}

	public void show(JSONArray urls) {
		
		adapter = new GridAdapter(mContext,urls);
		
		mGridView.setAdapter(adapter);
	}

	private void initView() {

		rootView = inflater.inflate(R.layout.active_page_view, null);

		mGridView = (GridView) (rootView);

		mGridView.setOnItemClickListener(null);

	}

	class GridAdapter extends BaseAdapter{
		Context mContext;
		JSONArray datas;
		int width;
		public GridAdapter(Context context,JSONArray datas){
			this.mContext = context;
			
			this.datas = datas;
			width = ((int)MyApplication.getInstance().getDeviceInfo((Activity)mContext).widthPixels)/4;
		}
		@Override
		public int getCount() {
			
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
			if(convertView == null){
				
				convertView = LayoutInflater.from(mContext).inflate(R.layout.active_page_item_view, null);
				holder = new Holder();
				holder.init(convertView);
				convertView.setTag(holder);
			}else{
				holder = (Holder) convertView.getTag();
			}
			
			holder.img.setLayoutParams(new LinearLayout.LayoutParams(width, width));
			holder.img.setImageBitmap(null);
			holder.img.setTag(mContext.getString(R.string.url_host_img)+item.optString("TitleImg"));
			holder.title.setText(item.optString("Title"));
			holder.adTitle.setText(item.optString("AdSubTitle"));
			ImageLoader.getInstance().displayImage(mContext.getString(R.string.url_host_img)+item.optString("TitleImg"), holder.img, MyApplication.getOptions(), MyApplication.getLoadingListener());
			return convertView;
		}
	}
	
	class Holder{
		TextView title;
		TextView adTitle;
		ImageView img;
		
		public void init(View root){
			title = (TextView) root.findViewById(R.id.title);
			adTitle = (TextView) root.findViewById(R.id.adTitle);
			img = (ImageView) root.findViewById(R.id.imageView);
		}
	}
}
