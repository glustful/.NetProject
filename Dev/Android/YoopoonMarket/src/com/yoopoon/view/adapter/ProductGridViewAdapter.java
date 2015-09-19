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
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.Toast;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.Staff;

/**
 * @ClassName: productityListViewAdapter
 * @Description: 首页下部商品展示Grid列表对应的Adapter
 * @author: 徐阳会
 * @date: 2015年9月9日 上午10:34:25
 */
public class ProductGridViewAdapter extends BaseAdapter {
	private static final String TAG = "ProductGridViewAdapter";
	private Context mContext;
	private ArrayList<JSONObject> datas;

	public ProductGridViewAdapter(Context context, ArrayList<JSONObject> arrayList) {
		mContext = context;
		// 业务要求首页横向显示的商品为第一项
		arrayList.remove(0);
		if (datas != null)
			datas.removeAll(datas);
		datas = arrayList;
	}
	@Override
	public int getCount() {
		Log.e("Product", datas.size() + "");
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
			productityViewHandler.initViewHandler(convertView);
			convertView.setTag(productityViewHandler);
		}
		Log.i(TAG, datas.get(position).toString());
		// 判断图片链接是否存在，不存在的时候使用默认图片
		final String mainImgsString = datas.get(position).optString("MainImg");
		String url = "http://iyookee.cn/modules/Index/static/image/index/activity4_c37e838.png";
		if ((!mainImgsString.equals("")) && (!mainImgsString.equals("null"))) {
			url = mContext.getString(R.string.url_image) + datas.get(position).optString("MainImg");
		}
		// String url = "http://iyookee.cn/modules/Index/static/image/index/activity4_c37e838.png";
		// 产品名称
		final String name = datas.get(position).optString("Name", "");
		productityViewHandler.productityNameTextView.setText(name);
		// 产品当前价格
		final float price_previous = Float.parseFloat(datas.get(position).optString("Price", ""));
		productityViewHandler.productityCurrentPriceTextView.setText("RMB "
				+ datas.get(position).optString("Price", "0.00"));
		// 产品未打折前价格
		final float price_counted = Float.parseFloat(datas.get(position).optString("NewPrice", ""));
		productityViewHandler.productityBeforePriceTextView.setText(" /折扣前"
				+ datas.get(position).optString("NewPrice", "0.00"));
		productityViewHandler.productityBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		// 加载销量
		productityViewHandler.salesVolumeButton.setText(("已有" + datas.get(position).optString("Owner", "0") + "人抢购"));
		// 加载图片
		productityViewHandler.productityPhotoImageView.setLayoutParams(new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT));
		productityViewHandler.productityPhotoImageView.setTag(url);
		LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, 400);
		productityViewHandler.productityPhotoImageView.setLayoutParams(params);
		ImageLoader.getInstance().displayImage(url, productityViewHandler.productityPhotoImageView,
				MyApplication.getOptions(), MyApplication.getLoadingListener());
		// 立即购买按钮点击事件
		productityViewHandler.purchaseButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "购买货物", Toast.LENGTH_SHORT).show();
			}
		});
		// 点击购物车按钮，跳转到购物车
		/*final int id = datas.get(position).optInt("Id", 0);
		final String subtitle = datas.get(position).optString("Subtitte");
		productityViewHandler.cartImageView.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(final View v) {
				new Thread() {

					public void run() {
						DBDao dao = new DBDao(mContext);
						if (dao.isExist(id)) {
							int count = dao.isExistCount(id);
							dao.updateCount(id, count + 1);
						} else {
							String url = mContext.getString(R.string.url_image) + mainImgsString;
							dao.add(new Staff(subtitle, name, url, 1, price_counted, price_previous, id));
						}
						int[] start_location = new int[2];// 一个整型数组，用来存储按钮的在屏幕的X、Y坐标
						v.getLocationInWindow(start_location);// 这是获取购买按钮的在屏幕的X、Y坐标（这也是动画开始的坐标）
						Intent intent = new Intent("com.yoopoon.market.add_to_cart");
						intent.addCategory(Intent.CATEGORY_DEFAULT);
						intent.putExtra("start_location", start_location);
						mContext.sendBroadcast(intent);
					};
				}.start();
			}

		});*/
		// 点击product GridView项跳转到产品详细信息Activity
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
		// convertView = LayoutInflater.from(mContext).inflate(R.layout.gridview_product_item,
		// null);
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
		private Button salesVolumeButton;
		private ImageView cartImageView;

		void initViewHandler(View view) {
			productityPhotoImageView = (ImageView) view.findViewById(R.id.img_product_photo);
			productityCurrentPriceTextView = (TextView) view.findViewById(R.id.tv_current_price);
			productityBeforePriceTextView = (TextView) view.findViewById(R.id.tv_before_price);
			productityNameTextView = (TextView) view.findViewById(R.id.tv_product_name);
			purchaseButton = (Button) view.findViewById(R.id.btn_purchase);
			salesVolumeButton = (Button) view.findViewById(R.id.has_buy_button);
			cartImageView = (ImageView) view.findViewById(R.id.img_cart);
		}
	}
}
