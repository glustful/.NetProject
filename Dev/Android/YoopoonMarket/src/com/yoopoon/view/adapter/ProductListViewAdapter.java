/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ProductListViewAdapter.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: 产品列表中产品项listview对应的Adapter
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月10日 下午5:41:17 
 * @version: V1.0   
 */
package com.yoopoon.view.adapter;

import java.util.ArrayList;

import org.json.JSONObject;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.R.id;
import com.yoopoon.market.R.layout;

public class ProductListViewAdapter extends BaseAdapter {
	private Context mContext;
	private ArrayList<JSONObject> datas;

	public ProductListViewAdapter(Context context, ArrayList<JSONObject> arrayList) {
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
		ProductViewHandler productViewHandler;
		if (convertView != null) {
			productViewHandler = (ProductViewHandler) convertView.getTag();
		} else {
			productViewHandler = new ProductViewHandler();
			convertView = LayoutInflater.from(mContext).inflate(R.layout.item_product_list, null);
			productViewHandler.initViewHandler(convertView);
			convertView.setTag(productViewHandler);
		}
		String url = mContext.getString(R.string.url_image) + datas.get(position).optString("MainImg");
		productViewHandler.productPhotoImageView.setTag(url);
		if (!datas.get(position).optString("MainImg").equals("null")) {
			ImageLoader.getInstance().displayImage(url, productViewHandler.productPhotoImageView,
					MyApplication.getOptions(), MyApplication.getLoadingListener());
		}
		productViewHandler.productTitleTextView.setText(datas.get(position).optString("Name", ""));
		productViewHandler.productSubtitleTextView.setText(datas.get(position).optString("Subtitte", ""));
		productViewHandler.productAdvertisemenTextView.setText(datas.get(position).optString("Ad1", ""));
		productViewHandler.productPricTextView.setText(datas.get(position).optString("Price", ""));
		productViewHandler.productSalesValuemtTextView.setText(datas.get(position).optString("Owner", ""));
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
	/**
	 * @Title: refresh
	 * @Description: 传入数据刷新产品列表
	 * @param jsonObjects
	 */
	public void refresh(ArrayList<JSONObject> jsonObjects) {
		datas.clear();
		if (jsonObjects != null) {
			datas.addAll(jsonObjects);
		}
		this.notifyDataSetChanged();
	}

	class ProductViewHandler {
		private ImageView productPhotoImageView;
		private TextView productTitleTextView;
		private TextView productSubtitleTextView;
		private TextView productAdvertisemenTextView;
		private TextView productPricTextView;
		private TextView productSalesValuemtTextView;

		void initViewHandler(View view) {
			productPhotoImageView = (ImageView) view.findViewById(R.id.img_product_photo);
			productTitleTextView = (TextView) view.findViewById(R.id.tv_product_title);
			productSubtitleTextView = (TextView) view.findViewById(R.id.tv_product_subtitle);
			productAdvertisemenTextView = (TextView) view.findViewById(R.id.tv_product_advertisment);
			productPricTextView = (TextView) view.findViewById(R.id.tv_product_price);
			productSalesValuemtTextView = (TextView) view.findViewById(R.id.tv_product_sales_volume);
		}
	}
}
