/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FramHouseListViewAdapter.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: 楼盘ListView对应的Adapter
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015年7月14日 下午5:18:08 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.houselist;

import java.util.ArrayList;

import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.content.SharedPreferences;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.product.ProductDetailActivity_;
import com.yoopoon.house.ui.bonus.BrokerBonusActivity_;
import com.yoopoon.house.ui.broker.BrokerScoreActivity_;
import com.yoopoon.house.ui.broker.BrokerTakeGuestActivity_;

/**
 * @ClassName: FramHouseListViewAdapter
 * @Description: 房源中每个楼盘对应的详细信息Adapter
 * @author: 徐阳会
 * @date: 2015年7月14日 上午9:49:21
 */
public class FramHouseListViewAdapter extends BaseAdapter {
	Context mContext;
	ArrayList<JSONObject> datas;
	int height = 0;
	
	public FramHouseListViewAdapter(Context mContext) {
		this.mContext = mContext;
		datas = new ArrayList<JSONObject>();
		height = MyApplication.getInstance().getDeviceInfo((Activity) mContext).heightPixels / 6;
	}
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
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHandler viewHandler;
		SharedPreferences brokerSharedPreferences = mContext.getSharedPreferences("com.yoopoon.home_preferences", 0);
		if (convertView == null) {
			convertView = LayoutInflater.from(mContext).inflate(R.layout.home_fram_house_listview_item, null);
			LinearLayout listViewLinearLayout = (LinearLayout) convertView
					.findViewById(R.id.house_listview_item_linearlayout);
			LayoutParams layoutParams = (LayoutParams) listViewLinearLayout.getLayoutParams();
			// 获取屏幕的宽度的
			int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
			layoutParams.height = screenWidth / 3;
			listViewLinearLayout.setLayoutParams(layoutParams);
			viewHandler = new ViewHandler();
			viewHandler.initViewHandler(convertView);
			convertView.setTag(viewHandler);
		} else {
			viewHandler = (ViewHandler) convertView.getTag();
		}
		// 判断当前的用户是否是Broker,如果是则显示楼盘下方的经纪人专属推荐和带客功能,如果不是则不显示带客和推荐等功能
		boolean isBrokerStatus = brokerSharedPreferences.getBoolean("isBroker", false);
		if (!isBrokerStatus) {
			viewHandler.houseBrokerFunctionLinearLayout.setVisibility(View.GONE);
		} else {
			viewHandler.houseBrokerFunctionLinearLayout.setVisibility(View.VISIBLE);
		}
		final JSONObject item = datas.get(position);
		String url = mContext.getString(R.string.url_host_img) + item.optString("Productimg");
		viewHandler.houseImageView.setLayoutParams(new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.MATCH_PARENT, height));
		viewHandler.houseImageView.setTag(url);
		ImageLoader.getInstance().displayImage(url, viewHandler.houseImageView, MyApplication.getOptions(),
				MyApplication.getLoadingListener());
		// viewHandler.houseImageView.
		viewHandler.houseProductnameTextView.setText(item.optString("Productname"));
		viewHandler.housePriceTextView.setText(item.optString("Price") + "元/m²");
		viewHandler.houseTypeAcreaqeStockRuleTextView.setText(item.optString("Type") + "/" + item.optString("Acreage")
				+ "m²" + "/" + "在售" + item.optString("StockRule") + "套");
		viewHandler.houseAdvertisementTextView.setText(item.optString("Advertisement"));
		// 添加点击事件,点击图片跳转到楼盘详情
		// ##################### 徐阳会 2015年07月14日 新增 Start
		viewHandler.houseImageView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				ProductDetailActivity_.intent(mContext).productId(item.optString("Id")).start();
			}
		});
		// ##################### 徐阳会 2015年07月14日 新增 End
		// ##################### 郭俊军 被修改代码 Start
		/*
		 * convertView.setOnClickListener(new OnClickListener() {
		 * @Override public void onClick(View v) {
		 * ProductDetailActivity_.intent(mContext).productId(item.optString("Id")).start(); } });
		 */
		// ##################### 郭俊军 被修改代码 End
		// 携带楼盘和经纪人数据跳转到带客页面
		viewHandler.houseTakeGuestTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				BrokerTakeGuestActivity_.intent(mContext).intent_properString(item.optString("Productname"))
						.intent_propretyTypeString(item.optString("Type")).intent_propretyNumber(item.optString("Id"))
						.start();
			}
			// 携带楼盘和经纪人数据跳转到推荐页面
		});
		viewHandler.houseRecommendTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				BrokerTakeGuestActivity_.intent(mContext).intent_properString(item.optString("Productname"))
						.intent_propretyTypeString(item.optString("Type")).intent_propretyNumber(item.optString("Id"))
						.start();
			}
		});
		// 经纪人红包点击事件
		viewHandler.houseBonusTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				BrokerBonusActivity_.intent(mContext).start();
			}
		});
		// 经纪人积分点击事件
		viewHandler.houseScoreTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				BrokerScoreActivity_.intent(mContext).start();
			}
		});
		return convertView;
	}
	
	/**
	 * @ClassName: ViewHandler
	 * @Description: 创建ViewHandler来对房源页中的ListView进行视图的绑定和初始化
	 * @author: 徐阳会
	 * @date: 2015年7月14日 上午9:39:02
	 */
	private class ViewHandler {
		private ImageView houseImageView;
		private TextView houseProductnameTextView;
		private TextView housePriceTextView;
		private TextView houseTypeAcreaqeStockRuleTextView;
		private TextView houseAdvertisementTextView;
		private TextView houseTakeGuestTextView;
		private TextView houseRecommendTextView;
		private TextView houseBonusTextView;
		private TextView houseScoreTextView;
		private View houseBrokerFunctionLinearLayout;
		
		/**
		 * @Title: initViewHandler
		 * @Description: 初始化ViewHandler
		 * @param root
		 */
		void initViewHandler(View root) {
			houseImageView = (ImageView) root.findViewById(R.id.house_image);
			houseProductnameTextView = (TextView) root.findViewById(R.id.house_productname);
			housePriceTextView = (TextView) root.findViewById(R.id.house_price);
			houseTypeAcreaqeStockRuleTextView = (TextView) root.findViewById(R.id.tpye_area_stockRule);
			houseAdvertisementTextView = (TextView) root.findViewById(R.id.house_advertisement);
			houseTakeGuestTextView = (TextView) root.findViewById(R.id.house_takeguest);
			houseRecommendTextView = (TextView) root.findViewById(R.id.house_recommend);
			houseBonusTextView = (TextView) root.findViewById(R.id.house_bribe);
			houseScoreTextView = (TextView) root.findViewById(R.id.house_score);
			houseBrokerFunctionLinearLayout = root.findViewById(R.id.house_broker_function_linearlayout);
		}
	}
	
	/**
	 * @Title: refresh
	 * @Description: 获取数据刷新房源页对应的楼盘ListView
	 * @param mJsonObjects
	 */
	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		if (mJsonObjects != null) {
			datas.addAll(mJsonObjects);
		}
		// this.notifyDataSetInvalidated();
		this.notifyDataSetChanged();
	}
}
