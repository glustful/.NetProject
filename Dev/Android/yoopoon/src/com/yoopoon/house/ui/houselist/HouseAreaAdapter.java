/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FramHouseAreaAdapter.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: 房源页顶端对应的楼盘区域Adapter（暂时没有启用）
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015年7月17日 上午11:50:16 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.houselist;

import java.util.ArrayList;

import org.json.JSONObject;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.yoopoon.home.R;

/**
 * @ClassName: FramHouseAreaAdapter
 * @Description:
 * @author: 徐阳会
 * @date: 2015年7月17日 上午11:50:16
 */
public class HouseAreaAdapter extends BaseAdapter {
	// ########################## 如下是对变量和属性的初始化###################
	private static final String LOGTAG = "FramHouseAreaAdapter";
	Context mContext;
	ArrayList<JSONObject> datas;
	private LayoutInflater mLayoutInflater;
	private int selectedPosition = -1;
	
	public HouseAreaAdapter(Context context, ArrayList<JSONObject> arrayList, int selected) {
		super();
		mContext = context;
		datas = arrayList;
		mLayoutInflater = LayoutInflater.from(this.mContext);
		selectedPosition = selected;
	}
	// ########################## 如上是对变量和属性的初始化###################
	@Override
	public int getCount() {
		return datas.size();
	}
	@Override
	public Object getItem(int position) {
		return datas.get(position);
	}
	@Override
	public long getItemId(int position) {
		return position;
	}
	public void setSelectedPosition(int position) {
		selectedPosition = position;
	}
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHandler viewHandler = null;
		if (convertView == null) {
			viewHandler = new ViewHandler();
			convertView = mLayoutInflater.inflate(R.layout.item_house_area, null);
			convertView.setTag(viewHandler);
		} else {
			convertView.getTag();
		}
		final JSONObject itemJsonObject = datas.get(position);
		// 初始化Viewhandler
		viewHandler.initViewHandler(convertView);
		viewHandler.areaNameTextView.setText(itemJsonObject.optString("AreaName"));
		if (selectedPosition == position) {
			viewHandler.mLinearLayout.setSelected(true);
			viewHandler.mLinearLayout.setPressed(true);
			viewHandler.mLinearLayout.setBackgroundColor(Color.RED);
		} else {
			viewHandler.mLinearLayout.setSelected(false);
			viewHandler.mLinearLayout.setPressed(false);
			viewHandler.mLinearLayout.setBackgroundColor(Color.TRANSPARENT);
		}
		return convertView;
	}
	
	/** 
	 * @ClassName: ViewHandler 
	 * @Description: 楼盘区域对应的ViewHandler
	 * @author: 徐阳会
	 * @date: 2015年7月22日 上午9:13:28  
	 */
	class ViewHandler {
		private TextView areaNameTextView;
		private ImageView arrowImageView;
		private LinearLayout mLinearLayout;
		
		/** 
		 * @Title: initViewHandler 
		 * @Description: 初始化ViewHandler
		 * @param rootView 当前Activity对应的视图
		 */
		public void initViewHandler(View rootView) {
			areaNameTextView = (TextView) rootView.findViewById(R.id.area_name_textview);
			arrowImageView = (ImageView) rootView.findViewById(R.id.area_arrow_imageView);
			mLinearLayout = (LinearLayout) rootView.findViewById(R.id.house_area_linearLayout);
		}
	}
}
