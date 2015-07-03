package com.yoopoon.home.ui.active;

import java.util.ArrayList;

import org.json.JSONObject;

import com.nostra13.universalimageloader.cache.disc.naming.Md5FileNameGenerator;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

import android.app.Activity;
import android.content.Context;
import android.graphics.Color;
import android.text.Html;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;
import android.widget.TextView;

public class ActiveBrandAdapter extends BaseAdapter {
	Context mContext;
	ArrayList<JSONObject> datas;
	int height = 0;
	public ActiveBrandAdapter(Context context){
		this.mContext = context;
		datas = new ArrayList<JSONObject>();
		height = MyApplication.getInstance().getDeviceInfo((Activity)mContext).heightPixels/6;
	}
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return datas.size();
	}

	@Override
	public Object getItem(int position) {
		// TODO Auto-generated method stub
		return datas.get(position);
	}

	@Override
	public long getItemId(int position) {
		// TODO Auto-generated method stub
		return position;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		Holder mHolder;
		if(convertView == null){
			convertView = LayoutInflater.from(mContext).inflate(R.layout.active_page_brand_item, null);
			mHolder = new Holder();
			mHolder.init(convertView);
			convertView.setTag(mHolder);
		}else{
			mHolder = (Holder) convertView.getTag();
		}
		final JSONObject item = datas.get(position);
		String url = mContext.getString(R.string.url_host_img)+item.optString("Bimg");
		mHolder.img.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, height));
		mHolder.img.setTag(url);
		ImageLoader.getInstance().displayImage(url, mHolder.img, MyApplication.getOptions(),MyApplication.getLoadingListener());
		mHolder.title.setText(""+item.optString("Bname")+"");
		JSONObject parameter = item.optJSONObject("ProductParamater");
		mHolder.city.setText("["+parameter.optString("所属城市")+"]");
		mHolder.area.setText(parameter.optString("占地面积"));
		mHolder.rightAdTitle.setText(Html.fromHtml(item.optString("AdTitle")));
		mHolder.adTitle.setText(item.optString("SubTitle"));
		
		mHolder.callPhone.setText("来电咨询："+parameter.optString("来电咨询"));
		mHolder.callPhone.setTag(Tools.optString(parameter, "来电咨询","10086"));
		mHolder.preferential.setText("最高享用优惠："+parameter.optString("最高优惠")+"元/套");
		mHolder.callPhone.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				Tools.callPhone(mContext, v.getTag().toString());
				
			}
		});
		convertView.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				BrandDetailActivity_.intent(mContext)
				.mJson(item.toString())
				.start();
				
			}
		});
		return convertView;
	}
	
	class Holder{
		ImageView img;
		TextView title;
		TextView adTitle;
		TextView callPhone;
		TextView preferential;
		TextView rightAdTitle;
		TextView city;
		TextView area;
		
		
		void init(View root){
			img = (ImageView) root.findViewById(R.id.img);
			title = (TextView) root.findViewById(R.id.title);
			adTitle = (TextView) root.findViewById(R.id.adTitle);
			callPhone = (TextView) root.findViewById(R.id.callPhone);
			preferential = (TextView) root.findViewById(R.id.preferential);
			city = (TextView) root.findViewById(R.id.city);
			rightAdTitle = (TextView) root.findViewById(R.id.rightAdTitle);
						
			area = (TextView) root.findViewById(R.id.area);
		}
	}

	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		datas.addAll(mJsonObjects);
		this.notifyDataSetChanged();
	}

}
