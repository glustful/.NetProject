/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: productityListViewAdapter.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月9日 上午10:34:25 
 * @version: V1.0   
 */
package com.yoopoon.view.adapter;

import java.util.ArrayList;

import org.json.JSONObject;

import android.content.Context;
import android.content.Intent;
import android.graphics.Paint;
import android.os.Bundle;
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

import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;

/**
 * @ClassName: productityListViewAdapter
 * @Description: 首页下部商品展示Grid列表对应的Adapter
 * @author: 徐阳会
 * @date: 2015年9月9日 上午10:34:25
 */
public class ProductGridViewAdapter extends BaseAdapter {
	private Context mContext;
	private ArrayList<JSONObject> datas;

	public ProductGridViewAdapter(Context context, ArrayList<JSONObject> arrayList) {
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
	public View getView(final int position, View convertView, ViewGroup parent) {
		ProductViewHandler productityViewHandler = null;
		if (convertView != null) {
			productityViewHandler = (ProductViewHandler) convertView.getTag();
		} else {
			productityViewHandler = new ProductViewHandler();
			convertView = LayoutInflater.from(mContext).inflate(R.layout.gridview_product_item, null);
			convertView.setTag(productityViewHandler);
			productityViewHandler.initViewHandler(convertView);
		}
		//具体获取的key等API完成修改
		//int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
		//url等待后台API确定
		//String url = mContext.getString(R.string.url_image);
		//判断图片链接是否存在，不存在的时候使用默认图片
		String url;
		if (datas.get(position).optString("MainImg") == null || datas.get(position).optString("MainImg").equals("null")) {
			url = "http://iyookee.cn/modules/Index/static/image/index/activity4_c37e838.png";
		} else {
			url = datas.get(position).optString("MainImg");
		}
		//产品名称
		productityViewHandler.productityNameTextView.setText(datas.get(position).optString("Name", ""));
		//产品当前价格
		productityViewHandler.productityCurrentPriceTextView.setText("RMB "
				+ datas.get(position).optString("Price", "0.00"));
		//产品未打折前价格
		productityViewHandler.productityBeforePriceTextView.setText(" /折扣前"
				+ datas.get(position).optString("NewPrice", "0.00"));
		productityViewHandler.productityBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		//加载图片
		productityViewHandler.productityPhotoImageView.setLayoutParams(new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT));
		productityViewHandler.productityPhotoImageView.setTag(url);
		ImageLoader.getInstance().displayImage(url, productityViewHandler.productityPhotoImageView,
				MyApplication.getOptions(), MyApplication.getLoadingListener());
		//立即购买按钮点击事件
		productityViewHandler.purchaseButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "购买货物", Toast.LENGTH_SHORT).show();
			}
		});
		//点击购物车按钮，跳转到购物车
		productityViewHandler.cartImageView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "即将跳转到购物车", Toast.LENGTH_SHORT).show();
			}
		});
		//点击product GridView项跳转到产品详细信息Activity
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Bundle bundle = new Bundle();
				bundle.putString("productId", datas.get(position).optString("Id"));
				Intent intent = new Intent(mContext, ProductDetailActivity_.class);
				intent.putExtras(bundle);
				mContext.startActivity(intent);
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

	/**
	 * @ClassName: ProductViewHandler
	 * @Description: 产品对应的ViewHandler
	 * @author: 徐阳会
	 * @date: 2015年9月12日 下午1:38:58
	 */
	class ProductViewHandler {
		private ImageView productityPhotoImageView;
		private TextView productityCurrentPriceTextView;
		private TextView productityBeforePriceTextView;
		private TextView productityNameTextView;
		private Button purchaseButton;
		private Button hasPurchaseButton;
		private ImageView cartImageView;

		void initViewHandler(View view) {
			productityPhotoImageView = (ImageView) view.findViewById(R.id.img_product_photo);
			productityCurrentPriceTextView = (TextView) view.findViewById(R.id.tv_current_price);
			productityBeforePriceTextView = (TextView) view.findViewById(R.id.tv_before_price);
			productityNameTextView = (TextView) view.findViewById(R.id.tv_product_name);
			purchaseButton = (Button) view.findViewById(R.id.btn_purchase);
			hasPurchaseButton = (Button) view.findViewById(R.id.has_buy_button);
			cartImageView = (ImageView) view.findViewById(R.id.img_cart);
		}
	}
}
