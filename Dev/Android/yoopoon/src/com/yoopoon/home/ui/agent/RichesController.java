package com.yoopoon.home.ui.agent;

import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.LinearLayout;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.AD.GridViewController;
import com.yoopoon.home.ui.agent.HeroController.GridAdapter;
import com.yoopoon.home.ui.view.MyGridView;

public class RichesController extends GridViewController {

	public RichesController(Context context) {
		super(context);

	}

	@Override
	public void show(ArrayList<JSONArray> urls) {

		for(int i=0;i<urls.size();i++){
			MyGridView mGridView = new MyGridView(mContext);
			mGridView.setLayoutParams(new GridView.LayoutParams(GridView.LayoutParams.MATCH_PARENT, GridView.LayoutParams.WRAP_CONTENT));
			mGridView.setNumColumns(3);
			mGridView.setStretchMode(GridView.STRETCH_COLUMN_WIDTH);
			mGridView.setHorizontalSpacing(1);
			mGridView.setVerticalSpacing(1);
			mGridView.setHorizontalScrollBarEnabled(false);
			mGridView.setVerticalScrollBarEnabled(false);
			mGridView.setAdapter(new GridAdapter(mContext, urls.get(i)));    
			mViews.add(mGridView);
		}
		mPagerAdapter.refresh(mViews);

		
	}

	class GridAdapter extends BaseAdapter {
		Context mContext;
		JSONArray datas;
		int width;

		public GridAdapter(Context context, JSONArray datas) {
			this.mContext = context;

			this.datas = datas;
			width = ((int) MyApplication.getInstance().getDeviceInfo(
					(Activity) mContext).widthPixels) / 4;
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
			if (convertView == null) {

				convertView = LayoutInflater.from(mContext).inflate(
						R.layout.active_page_item_view, null);
				holder = new Holder();
				holder.init(convertView);
				convertView.setTag(holder);
			} else {
				holder = (Holder) convertView.getTag();
			}

			holder.img.setLayoutParams(new LinearLayout.LayoutParams(width,
					width));
			holder.img.setImageBitmap(null);
			holder.img.setTag(mContext.getString(R.string.url_host_img)
					+ item.optString("TitleImg"));
			holder.title.setText(item.optString("Title"));
			holder.adTitle.setText(item.optString("AdSubTitle"));
			ImageLoader.getInstance().displayImage(
					mContext.getString(R.string.url_host_img)
							+ item.optString("TitleImg"), holder.img,
					MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			return convertView;
		}
	}

	class Holder {
		TextView title;
		TextView adTitle;
		ImageView img;

		public void init(View root) {
			title = (TextView) root.findViewById(R.id.title);
			adTitle = (TextView) root.findViewById(R.id.adTitle);
			img = (ImageView) root.findViewById(R.id.imageView);
		}
	}

	@Override
	public void onGridItemClick(AdapterView<?> parent, View view, int position,
			long id) {
		// TODO Auto-generated method stub
		
	}
}
