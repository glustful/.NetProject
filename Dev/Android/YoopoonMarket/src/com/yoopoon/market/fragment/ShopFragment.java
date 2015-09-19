/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ShopFragment.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015-9-7 下午4:50:59 
 * @version: V1.0   
 */
package com.yoopoon.market.fragment;

import java.util.ArrayList;

import java.util.HashMap;

import javax.security.auth.PrivateCredentialPermission;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Canvas;
import android.graphics.ColorFilter;
import android.graphics.Paint;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.OnRefreshListener2;
import com.handmark.pulltorefresh.library.PullToRefreshExpandableListView;
import com.handmark.pulltorefresh.library.PullToRefreshGridView;
import com.handmark.pulltorefresh.library.PullToRefreshListView;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.component.YoopoonServiceController;
import com.yoopoon.market.CleanServeActivity_;
import com.yoopoon.market.DeliveryActivity_;
import com.yoopoon.market.MaternityMatronActivity;
import com.yoopoon.market.MaternityMatronActivity_;
import com.yoopoon.market.PoliticsActivity_;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.ResponseData.ResultState;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;

import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.ExpandableHeightGridView;
import com.yoopoon.market.view.NoScrollGridView;

import com.yoopoon.market.view.MyGridView;

import com.yoopoon.view.adapter.ProductGridViewAdapter;
import com.yoopoon.view.adapter.ProductListAdapter;

public class ShopFragment extends Fragment {
	private Context mContext;
	private ADController mADController;
	private View rootView;
	private ArrayList<String> imgs; // 存储顶端的广告图片地址
	private ProductListAdapter productListAdapter;
	private TextView burstPackageTextView;
	private static final String TAG = "ShopFragment";
	private ArrayList<JSONArray> servicesArrayList;
	//首页推荐商品控件
	private ImageView recommondProductImageView;//套餐图片
	private TextView recommondProductBeforePriceTextView; // 折扣前价格
	private TextView recommondProductCurrentPriceTextView;//当前价格
	private TextView recommondProductNameTextView; //套餐名称
	private Button recommondProductByButton;//立即购买
	private ImageView recommondProductCartImageView;//添加到购物车
	//搜索框对应的receiver
	//	private ProductRefreshByKeyWordReceiver byKeyWordReceiver;
	//首页展示商品的PTRGridView组件
	private PullToRefreshListView mPullToRefreshListView;
	private ListView productListView;
	//首页推荐商品视图
	View shopFragmentHeadView;
	// 首页推荐商品控件
	private ImageView burstPackageImageView;// 套餐图片
	private TextView beforePriceTextView; // 折扣前价格
	private TextView currentPriceTextView;// 当前价格
	private TextView burstPackageNameTextView; // 套餐名称
	private Button salesVolumeButton;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		// 如果获取的view存在父View则不解析直接去除原来的Fragment
		if (rootView != null) {
			ViewGroup parentViewGroup = (ViewGroup) rootView.getParent();
			if (parentViewGroup != null) {
				parentViewGroup.removeView(rootView);
			}
		} else {
			mContext = getActivity();
			mADController = new ADController(mContext);
			rootView = inflater.inflate(R.layout.fragment_shop, null);
			settingPullToRefreshListView();
			//获取商品
			//视图加载以及位置调整
			initUI();
		}
		return rootView;
	}
	/**
	 * @Title: initShopFragment
	 * @Description: 初始化和设置视图控件
	 */
	private void initUI() {
		productListView.addHeaderView(mADController.getRootView());
		//联网获取广告
		requestAdvertisements();
		shopFragmentHeadView = LayoutInflater.from(mContext).inflate(R.layout.fragment_shop_headview, null);
		//加载推荐套餐下的控件
		settingRecommondProduct();
		loadServiceEvent(shopFragmentHeadView);
		productListView.addHeaderView(shopFragmentHeadView);
		requestProduct();
	}
	/**
	 * @Title: settingPullToRefreshListView
	 * @Description: 设置PullToRefresh刷新配置参数
	 */
	private void settingPullToRefreshListView() {
		mPullToRefreshListView = (PullToRefreshListView) rootView.findViewById(R.id.ptr_listview_fragment_shop);
		mPullToRefreshListView.setMode(PullToRefreshBase.Mode.BOTH);
		mPullToRefreshListView.setOnRefreshListener(new RefreshListener());
		productListView = mPullToRefreshListView.getRefreshableView();
		productListView.setFadingEdgeLength(0);
		productListView.setFastScrollEnabled(false);
	}
	/**
	 * @Title: settingRecommondProduct
	 * @Description: 加载推荐套餐下的控件
	 */
	private void settingRecommondProduct() {
		recommondProductImageView = (ImageView) shopFragmentHeadView.findViewById(R.id.img_product);
		recommondProductBeforePriceTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_prime_price);
		recommondProductBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		recommondProductCurrentPriceTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_sales_price);
		recommondProductNameTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_product_name);
		recommondProductByButton = (Button) shopFragmentHeadView.findViewById(R.id.purchase_immediately_button);
		salesVolumeButton = (Button) shopFragmentHeadView.findViewById(R.id.has_buy_button);
		recommondProductCartImageView = (ImageView) shopFragmentHeadView.findViewById(R.id.img_cart);
		//点击图片跳转到商品详细信息
		recommondProductImageView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Bundle bundle = new Bundle();
				bundle.putString("productId", "1");
				Intent intent = new Intent(mContext, ProductDetailActivity_.class);
				intent.putExtras(bundle);
				mContext.startActivity(intent);
			}
		});
		//点击小购物车图片事件
		recommondProductCartImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				Toast.makeText(mContext, "添加到购物车", Toast.LENGTH_SHORT).show();
				return true;
			}
		});
		recommondProductByButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Toast.makeText(mContext, "立即购买", Toast.LENGTH_SHORT).show();
			}
		});
		requestProduct();
	}
	private void initRecommendProduct(JSONObject jsonObject) {
		recommondProductNameTextView.setText(jsonObject.optString("Name", ""));
		recommondProductCurrentPriceTextView.setText("RMB" + jsonObject.optString("Price", ""));
		if (jsonObject.optString("NewPrice", "").equals("null")) {
			recommondProductBeforePriceTextView.setText("折扣前0");
		} else {
			recommondProductBeforePriceTextView.setText("折扣前" + jsonObject.optString("NewPrice", ""));
		}
		if (jsonObject.optString("Owner", "0").equals("null")) {
			salesVolumeButton.setText("已有0人抢购");
		} else {
			salesVolumeButton.setText("已有" + jsonObject.optString("Owner", "0") + "人抢购");
		}
		String urlString = jsonObject.optString("MainImg", "");
		if (!urlString.equals("")) {
			ImageLoader.getInstance().displayImage(urlString, recommondProductImageView);
		}
	}
	/**
	 * @Title: requestAdvertisements
	 * @Description: 获取广告信息
	 */
	private void requestAdvertisements() {
		if (imgs == null)
			new RequestAdapter() {
				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						if (imgs == null) {
							imgs = new ArrayList<String>();
							JSONArray list = data.getJsonArray();
							if (list == null || list.length() < 1)
								return;
							for (int i = 0; i < list.length(); i++) {
								imgs.add(list.optJSONObject(i).optString("TitleImg"));
							}
							// 加载广告
							mADController.show(imgs);
						}
					}
				}
				@Override
				public void onProgress(ProgressMessage msg) {
				}
			}.setUrl("/api/Channel/GetTitleImg").setRequestMethod(RequestMethod.eGet).addParam("channelName", "banner")
					.notifyRequest();
	}
	/**
	 * @Title: requestProduct
	 * @Description: 获取商品列表，同时加载到Adapter中
	 */
	private void requestProduct() {
		HashMap<String, String> map = new HashMap<String, String>();
		map.put("Page", "1");
		map.put("PageCount", "10");
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						productListAdapter = new ProductListAdapter(mContext,
								JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
						productListView.setAdapter(productListAdapter);
						//加载推荐套餐内容
						initRecommendProduct(JSONArrayConvertToArrayList.convertToArrayList(jsonArray).get(0));
						mPullToRefreshListView.onRefreshComplete();
					} catch (JSONException e) {
						e.printStackTrace();
					}
				} else {
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_get_communityproduct)).addParam(map).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}
	/**
	 * @Title: requestProduct
	 * @Description: 传入参数，获取商品信息
	 * @param hashMap
	 */
	private void requestProduct(HashMap<String, String> hashMap) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						productListAdapter = new ProductListAdapter(mContext,
								JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
						productListView.setAdapter(productListAdapter);
						initRecommendProduct(JSONArrayConvertToArrayList.convertToArrayList(jsonArray).get(0));
						mPullToRefreshListView.onRefreshComplete();
					} catch (JSONException e) {
						e.printStackTrace();
					}
				} else {
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_get_communityproduct)).addParam(hashMap).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}
	/**
	 * @ClassName: ProductRefreshByAddressReceiver
	 * @Description: 创建监听地址改变的接收器
	 * @author: 徐阳会
	 * @date: 2015年9月17日 上午11:02:12
	 */
	/*private class ProductRefreshByAddressReceiver extends BroadcastReceiver {
		@Override
		public void onReceive(Context context, Intent intent) {
		}
	}*/
	/**
	 * @ClassName: ProductRefreshByKeyWordReceiver
	 * @Description: 创建顶端搜索框输入的字段进行搜索
	 * @author: 徐阳会
	 * @date: 2015年9月17日 下午5:22:43
	 */
	/*private class ProductRefreshByKeyWordReceiver extends BroadcastReceiver {
		@Override
		public void onReceive(Context context, Intent intent) {
			String keywordString = intent.getStringExtra("keyword");
			if (keywordString != null && (!keywordString.equals(""))) {
				HashMap<String, String> hashMap = new HashMap<String, String>();
				hashMap.put("Name", keywordString);
				requestProduct(hashMap);
			}
		}
	}*/
	/**
	 * @Title: loadRefreshByAddress
	 * @Description: 截获地址广播
	 */
	/*	private void loadRefreshByAddress() {
			IntentFilter intentFilter = new IntentFilter("com.yoopoon.market.productRefresh.Address");
			ProductRefreshByAddressReceiver addressReceiver = new ProductRefreshByAddressReceiver();
			mContext.registerReceiver(addressReceiver, intentFilter);
		}*/
	/*	private void loadRefreshByKeyword() {
			IntentFilter filter = new IntentFilter("com.yoopoon.market.search.byKeyword");
			byKeyWordReceiver = new ProductRefreshByKeyWordReceiver();
			mContext.registerReceiver(byKeyWordReceiver, filter);
		}*/
	private void loadServiceEvent(View view) {
		ImageView houseKeepingImageView = (ImageView) view.findViewById(R.id.img_house_keeping);
		ImageView washingImageView = (ImageView) view.findViewById(R.id.img_washing_clothes);
		ImageView takeInExpressImageView = (ImageView) view.findViewById(R.id.img_take_in_express);
		ImageView maternityMatronImageView = (ImageView) view.findViewById(R.id.img_maternity_matron);
		ImageView moreServiceImageView = (ImageView) view.findViewById(R.id.img_more_service);
		//家政服务
		houseKeepingImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				PoliticsActivity_.intent(getActivity()).start();
				return false;
			}
		});
		//清洗服务
		washingImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				CleanServeActivity_.intent(getActivity()).start();
				return false;
			}
		});
		//代收快递服务
		takeInExpressImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				DeliveryActivity_.intent(getActivity()).start();
				return false;
			}
		});
		//月嫂服务
		maternityMatronImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				MaternityMatronActivity_.intent(getActivity()).start();
				return false;
			}
		});
		//更多服务
		moreServiceImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				Intent intent = new Intent("com.yoopoon.market.service.moreservice");
				mContext.sendBroadcast(intent);
				return false;
			}
		});
	}
	@Override
	public void onStart() {
		super.onStart();
	}
	@Override
	public void onDestroy() {
		super.onDestroy();
		//mContext.unregisterReceiver(byKeyWordReceiver);
	}

	private class RefreshListener implements OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			requestProduct();
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			Toast.makeText(mContext, "上啦", Toast.LENGTH_SHORT).show();
		}
	}

	/*
	 * @Title: setUserVisibleHint
	 * @Description: 判断当Fragment对用户不可见的时候能调整Fragment的显示位置
	 * @param isVisibleToUser 
	 * @see android.support.v4.app.Fragment#setUserVisibleHint(boolean) 
	 */
	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		super.setUserVisibleHint(isVisibleToUser);
		if (rootView != null && !isVisibleToUser) {
			requestProduct();
		}
		/*private class ProductRefreshByAddressReceiver extends BroadcastReceiver {

			@Override
			public void onReceive(Context context, Intent intent) {
				Toast.makeText(mContext, "111111", Toast.LENGTH_SHORT).show();

			}

		}*/
		/*	private void loadRefreshByAddress() {
				IntentFilter intentFilter = new IntentFilter("com.yoopoon.market.productRefresh.Address");
				ProductRefreshByAddressReceiver addressReceiver = new ProductRefreshByAddressReceiver();
				mContext.registerReceiver(addressReceiver, intentFilter);

		 
			}*/
	}
}
