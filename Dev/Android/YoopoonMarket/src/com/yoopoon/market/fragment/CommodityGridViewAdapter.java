/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: CommodityListViewAdapter.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月9日 上午10:34:25 
 * @version: V1.0   
 */
package com.yoopoon.market.fragment;

import java.util.ArrayList;

import org.json.JSONObject;

import android.content.Context;
import android.graphics.Paint;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;

/**
 * @ClassName: CommodityListViewAdapter
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月9日 上午10:34:25
 */
public class CommodityGridViewAdapter extends BaseAdapter {
	private Context					mContext;
	private ArrayList<JSONObject>	datas;
	
	public CommodityGridViewAdapter(Context context, ArrayList<JSONObject> arrayList) {
		mContext = context;
		datas = arrayList;
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
		CommodityViewHandler commodityViewHandler = null;
		if (convertView != null) {
			commodityViewHandler = (CommodityViewHandler) convertView.getTag();
		} else {
			commodityViewHandler = new CommodityViewHandler();
			convertView = LayoutInflater.from(mContext).inflate(R.layout.gridview_commodity_item, null);
			convertView.setTag(commodityViewHandler);
			commodityViewHandler.initViewHandler(convertView);
		}
		//具体获取的key等API完成修改
		//int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
		//url等待后台API确定
		//String url = mContext.getString(R.string.url_image_server);
		String url = "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg";
		commodityViewHandler.commodityNameTextView.setText(datas.get(position).optString("productName", ""));
		commodityViewHandler.commodityCurrentPriceTextView.setText("RMB "
				+ datas.get(position).optString("currentPrice", ""));
		commodityViewHandler.commodityBeforePriceTextView.setText(" /折扣前"
				+ datas.get(position).optString("beforePrice", ""));
		commodityViewHandler.commodityBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		commodityViewHandler.commodityPhotoImageView.setLayoutParams(new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT));
		commodityViewHandler.commodityPhotoImageView.setTag(url);
		ImageLoader.getInstance().displayImage(url, commodityViewHandler.commodityPhotoImageView,
				MyApplication.getOptions(), MyApplication.getLoadingListener());
		//立即购买按钮点击事件
		commodityViewHandler.purchaseButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "购买货物", Toast.LENGTH_SHORT).show();
			}
		});
		//点击获取跳转到货物详细信息
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "货物详细信息", Toast.LENGTH_SHORT).show();
			}
		});
		return convertView;
	}
	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		if (mJsonObjects != null) {
			datas.addAll(mJsonObjects);
		}
		// this.notifyDataSetInvalidated();
		this.notifyDataSetChanged();
	}
	
	class CommodityViewHandler {
		private ImageView	commodityPhotoImageView;
		private TextView	commodityCurrentPriceTextView;
		private TextView	commodityBeforePriceTextView;
		private TextView	commodityNameTextView;
		private Button		purchaseButton;
		private Button		hasPurchaseButton;
		
		void initViewHandler(View view) {
			commodityPhotoImageView = (ImageView) view.findViewById(R.id.img_commodity_photo);
			commodityCurrentPriceTextView = (TextView) view.findViewById(R.id.tv_current_price);
			commodityBeforePriceTextView = (TextView) view.findViewById(R.id.tv_before_price);
			commodityNameTextView = (TextView) view.findViewById(R.id.tv_commodity_name);
			purchaseButton = (Button) view.findViewById(R.id.btn_purchase);
			hasPurchaseButton = (Button) view.findViewById(R.id.has_buy_button);
		}
	}
}
