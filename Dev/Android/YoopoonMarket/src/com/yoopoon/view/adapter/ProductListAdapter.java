package com.yoopoon.view.adapter;

import java.util.ArrayList;

import org.androidannotations.annotations.EService;
import org.json.JSONObject;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.utils.SplitStringWithDot;
import com.yoopoon.view.adapter.ProductGridViewAdapter.ProductViewHandler;

import android.content.Context;
import android.content.Intent;
import android.graphics.Paint;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.LinearLayout.LayoutParams;

public class ProductListAdapter extends BaseAdapter {
	private Context mContext;
	private ArrayList<JSONObject> datas;

	public ProductListAdapter(Context context, ArrayList<JSONObject> arrayList) {
		mContext = context;
		arrayList.remove(0);
		datas = arrayList;
	}
	@Override
	public int getCount() {
		if (datas.size() % 2 == 0) {
			return (datas.size() / 2);
		} else {
			return ((datas.size() + 1) / 2);
		}
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
		 
		if ((datas.size() - position * 2) >= 1) {
			ProductViewHandler productityViewHandler = null;
			if (convertView != null) {
				productityViewHandler = (ProductViewHandler) convertView.getTag();
			} else {
				productityViewHandler = new ProductViewHandler();
				convertView = LayoutInflater.from(mContext).inflate(R.layout.item_product_info, null);
				productityViewHandler.initViewHandler(convertView);
				convertView.setTag(productityViewHandler);
			}
			//判断两张图片链接是否存在，不存在的时候使用默认图片
			String mainImgsString1 = datas.get(position).optString("MainImg");
			String url1 = "http://iyookee.cn/modules/Index/static/image/index/activity4_c37e838.png";
			if ((!mainImgsString1.equals("")) && (!mainImgsString1.equals("null"))) {
				url1 = mContext.getString(R.string.url_image) + datas.get(position * 2).optString("MainImg");
			}
			String mainImgsString2 = datas.get(position).optString("MainImg");
			//##############################################################################################
			//                   第一件商品数据绑定
			//##############################################################################################
			//产品名称
			productityViewHandler.productityNameTextView1.setText(datas.get(position * 2).optString("Name", ""));
			//产品当前价格
			productityViewHandler.productityCurrentPriceTextView1.setText("RMB "
					+ SplitStringWithDot.split(datas.get(position * 2).optString("Price", "0")));
			//产品未打折前价格
			productityViewHandler.productityBeforePriceTextView1.setText(" /折扣前"
					+ SplitStringWithDot.split(datas.get(position * 2).optString("NewPrice", "0")));
			productityViewHandler.productityBeforePriceTextView1.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			//加载销量
			if (datas.get(position * 2).optString("Owner", "0").equals("null")) {
				productityViewHandler.salesVolumeButton1.setText(("已有0人抢购"));
			} else {
				productityViewHandler.salesVolumeButton1.setText(("已有"
						+ datas.get(position * 2).optString("Owner", "0") + "人抢购"));
			}
			//加载图片
			productityViewHandler.productityPhotoImageView1.setLayoutParams(new LinearLayout.LayoutParams(
					LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT));
			productityViewHandler.productityPhotoImageView1.setTag(url1);
			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, 400);
			productityViewHandler.productityPhotoImageView1.setLayoutParams(params);
			ImageLoader.getInstance().displayImage(url1, productityViewHandler.productityPhotoImageView1,
					MyApplication.getOptions(), MyApplication.getLoadingListener());
			//立即购买按钮点击事件
			productityViewHandler.purchaseButton1.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Toast.makeText(mContext, "购买货物", Toast.LENGTH_SHORT).show();
				}
			});
			productityViewHandler.productityPhotoImageView1.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Bundle bundle = new Bundle();
					bundle.putString("productId", datas.get(position*2).optString("Id"));
					Intent intent = new Intent(mContext, ProductDetailActivity_.class);
					intent.putExtras(bundle);
					mContext.startActivity(intent);
				}
			});
			//购物车动画效果
			final int id1 = datas.get(position*2).optInt("Id", 0);
			final String subtitle1 = datas.get(position*2).optString("Subtitte");
			productityViewHandler.cartImageView1.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(final View v) {
					new Thread() {
						public void run() {
							DBDao dao = new DBDao(mContext);
							if (dao.isExist(id1)) {
								int count = dao.isExistCount(id1);
								dao.updateCount(id1, count + 1);
							} else {
								String mainImageString = datas.get(position * 2).optString("MainImg", "");
								String url = mContext.getString(R.string.url_image) + mainImageString;
								String name = datas.get(position * 2).optString("Name", "");
								float price_counted = Float.parseFloat(datas.get(position * 2).optString("Price", ""));
								float price_previous = Float.parseFloat(datas.get(position * 2).optString("NewPrice",
										""));
								dao.add(new Staff(subtitle1, name, url, 1, price_counted, price_previous, id1));
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
			});
			//##############################################################################################
			//                   第一件商品数据绑定
			//##############################################################################################
			//
			//
			//##############################################################################################
			//                   第二件商品数据绑定
			//##############################################################################################
			if (datas.size() % 2 != 0 && ((datas.size() - 1) / 2 == position)) {
				//如果是奇数则隐藏控件
				/*productityViewHandler.productityPhotoImageView2.setVisibility(View.GONE);
				productityViewHandler.productityNameTextView2.setVisibility(View.GONE);
				productityViewHandler.productityCurrentPriceTextView2.setVisibility(View.GONE);
				productityViewHandler.productityBeforePriceTextView2.setVisibility(View.GONE);
				productityViewHandler.salesVolumeButton2.setVisibility(View.GONE);
				productityViewHandler.purchaseButton2.setVisibility(View.GONE);
				productityViewHandler.cartImageView2.setVisibility(View.GONE);*/
			/*	productityViewHandler.rightProductRelativeLayout.setVisibility(View.GONE);*/
			} else {
				{
					String url2 = "http://iyookee.cn/modules/Index/static/image/index/activity4_c37e838.png";
					if ((!mainImgsString2.equals("")) && (!mainImgsString2.equals("null"))) {
						url2 = mContext.getString(R.string.url_image)
								+ datas.get(position * 2 + 1).optString("MainImg");
					}
					//产品名称
					productityViewHandler.productityNameTextView2.setText(datas.get(position * 2 + 1).optString("Name",
							""));
					//产品当前价格
					productityViewHandler.productityCurrentPriceTextView2.setText("RMB "
							+ SplitStringWithDot.split(datas.get(position * 2 + 1).optString("Price", "0")));
					//产品未打折前价格
					productityViewHandler.productityBeforePriceTextView2.setText(" /折扣前"
							+ SplitStringWithDot.split(datas.get(position * 2 + 1).optString("NewPrice", "0")));
					productityViewHandler.productityBeforePriceTextView2.getPaint().setFlags(
							Paint.STRIKE_THRU_TEXT_FLAG);
					//加载销量
					if (datas.get(position * 2 + 1).optString("Owner", "0").equals("null")) {
						productityViewHandler.salesVolumeButton2.setText(("已有0人抢购"));
					} else {
						productityViewHandler.salesVolumeButton2.setText(("已有"
								+ datas.get(position * 2 + 1).optString("Owner", "0") + "人抢购"));
					}
					//加载图片
					productityViewHandler.productityPhotoImageView2.setLayoutParams(new LinearLayout.LayoutParams(
							LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT));
					productityViewHandler.productityPhotoImageView2.setTag(url2);
					LayoutParams params2 = new LayoutParams(LayoutParams.MATCH_PARENT, 400);
					productityViewHandler.productityPhotoImageView2.setLayoutParams(params);
					ImageLoader.getInstance().displayImage(url2, productityViewHandler.productityPhotoImageView2,
							MyApplication.getOptions(), MyApplication.getLoadingListener());
					//立即购买按钮点击事件
					productityViewHandler.purchaseButton2.setOnClickListener(new OnClickListener() {
						@Override
						public void onClick(View v) {
							Toast.makeText(mContext, "购买货物", Toast.LENGTH_SHORT).show();
						}
					});
					productityViewHandler.productityPhotoImageView2.setOnClickListener(new OnClickListener() {
						@Override
						public void onClick(View v) {
							Bundle bundle = new Bundle();
							bundle.putString("productId", datas.get(position*2+1).optString("Id"));
							Intent intent = new Intent(mContext, ProductDetailActivity_.class);
							intent.putExtras(bundle);
							mContext.startActivity(intent);
						}
					});
					final int id2 = datas.get(position*2+1).optInt("Id", 0);
					final String subtitle2 = datas.get(position*2+1).optString("Subtitte");
					productityViewHandler.cartImageView2.setOnClickListener(new OnClickListener() {
						@Override
						public void onClick(final View v) {
							new Thread() {
								public void run() {
									DBDao dao = new DBDao(mContext);
									if (dao.isExist(id2)) {
										int count = dao.isExistCount(id2);
										dao.updateCount(id2, count + 1);
									} else {
										String mainImageString = datas.get(position * 2+1).optString("MainImg", "");
										String url = mContext.getString(R.string.url_image) + mainImageString;
										String name = datas.get(position * 2+1).optString("Name", "");
										float price_counted = Float.parseFloat(datas.get(position * 2+1).optString(
												"Price", ""));
										float price_previous = Float.parseFloat(datas.get(position * 2+1).optString(
												"NewPrice", ""));
										dao.add(new Staff(subtitle2, name, url, 1, price_counted, price_previous, id2));
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
					});
				}
			}
			//##############################################################################################
			//                   第二件商品数据绑定
			//##############################################################################################
			return convertView;
		} else {
			return null;
		}
	}
	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		if (mJsonObjects != null) {
			datas.addAll(mJsonObjects);
		}
		this.notifyDataSetChanged();
	}
	public void addRefresh(ArrayList<JSONObject> mJsonObjects) {
		if (mJsonObjects != null) {
			datas.addAll(mJsonObjects);
		}
		this.notifyDataSetChanged();
	}

	class ProductViewHandler {
		private ImageView productityPhotoImageView1;
		private TextView productityCurrentPriceTextView1;
		private TextView productityBeforePriceTextView1;
		private TextView productityNameTextView1;
		private Button purchaseButton1;
		private Button salesVolumeButton1;
		private ImageView cartImageView1;
		private ImageView productityPhotoImageView2;
		private TextView productityCurrentPriceTextView2;
		private TextView productityBeforePriceTextView2;
		private TextView productityNameTextView2;
		private Button purchaseButton2;
		private Button salesVolumeButton2;
		private ImageView cartImageView2;
		private RelativeLayout rightProductRelativeLayout;

		void initViewHandler(View view) {
			//第一件商品对应的控件
			productityPhotoImageView1 = (ImageView) view.findViewById(R.id.img_product_photo1);
			productityCurrentPriceTextView1 = (TextView) view.findViewById(R.id.tv_current_price1);
			productityBeforePriceTextView1 = (TextView) view.findViewById(R.id.tv_before_price1);
			productityNameTextView1 = (TextView) view.findViewById(R.id.tv_product_name1);
			purchaseButton1 = (Button) view.findViewById(R.id.btn_purchase1);
			salesVolumeButton1 = (Button) view.findViewById(R.id.has_buy_button1);
			cartImageView1 = (ImageView) view.findViewById(R.id.img_cart1);
			//第二件商品对应的控件
			productityPhotoImageView2 = (ImageView) view.findViewById(R.id.img_product_photo2);
			productityCurrentPriceTextView2 = (TextView) view.findViewById(R.id.tv_current_price2);
			productityBeforePriceTextView2 = (TextView) view.findViewById(R.id.tv_before_price2);
			productityNameTextView2 = (TextView) view.findViewById(R.id.tv_product_name2);
			purchaseButton2 = (Button) view.findViewById(R.id.btn_purchase2);
			salesVolumeButton2 = (Button) view.findViewById(R.id.has_buy_button2);
			cartImageView2 = (ImageView) view.findViewById(R.id.img_cart2);
			rightProductRelativeLayout = (RelativeLayout) view.findViewById(R.id.relativelayout_right_product);
		}
	}
}
