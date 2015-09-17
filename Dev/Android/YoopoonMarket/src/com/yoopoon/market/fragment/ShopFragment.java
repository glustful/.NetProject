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

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Paint;
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
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.component.YoopoonServiceController;
import com.yoopoon.market.CleanServeActivity_;
import com.yoopoon.market.DeliveryActivity_;
import com.yoopoon.market.MaternityMatronActivity;
import com.yoopoon.market.MaternityMatronActivity_;
import com.yoopoon.market.PoliticsActivity_;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.ProductList_;
import com.yoopoon.market.R;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.ResponseData.ResultState;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.market.view.ExpandableHeightGridView;
import com.yoopoon.market.view.MyGridView;
import com.yoopoon.market.view.NoScrollGridView;
import com.yoopoon.view.adapter.ProductGridViewAdapter;

public class ShopFragment extends Fragment {
	private Context mContext;
	private ADController mADController;
	private YoopoonServiceController serviceController;
	private View rootView;
	private ArrayList<String> imgs; // 存储顶端的广告图片地址
	private MyGridView commodityGridView;
	private ProductGridViewAdapter mProductGridViewAdapter;
	private TextView burstPackageTextView;
	private static final String TAG = "ShopFragment";
	private ArrayList<JSONArray> servicesArrayList;
	//首页推荐商品控件
	private ImageView burstPackageImageView;//套餐图片
	private TextView beforePriceTextView; // 折扣前价格
	private TextView currentPriceTextView;//当前价格
	private TextView burstPackageNameTextView; //套餐名称
	private Button salesVolumeButton;
	private ScrollView fragmentShopScrollView;
	//搜索框对应的receiver
	private ProductRefreshByKeyWordReceiver byKeyWordReceiver;

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
			// 解析布局
			rootView = inflater.inflate(R.layout.fragment_shop, null);
			fragmentShopScrollView = (ScrollView) rootView.findViewById(R.id.scrollview_fragment_shop);
			fragmentShopScrollView.scrollTo(0, 0);
			// 获取套餐折扣价后加上划线
			beforePriceTextView = (TextView) rootView.findViewById(R.id.tv_fragment_before_price);
			currentPriceTextView = (TextView) rootView.findViewById(R.id.current_price_textview);
			burstPackageNameTextView = (TextView) rootView.findViewById(R.id.package_name_textview);
			salesVolumeButton = (Button) rootView.findViewById(R.id.has_buy_button);
			beforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			// 爆款套餐和省到不行加粗样式设置
			burstPackageTextView = (TextView) rootView.findViewById(R.id.btn_burstpackage);
			burstPackageTextView.getPaint().setFakeBoldText(true);
			// 添加商品首页点击推荐商品后的事件
			burstPackageImageView = (ImageView) rootView.findViewById(R.id.burst_package_image);
			burstPackageImageView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Bundle bundle = new Bundle();
					bundle.putString("productId", "1");
					Intent intent = new Intent(mContext, ProductDetailActivity_.class);
					intent.putExtras(bundle);
					mContext.startActivity(intent);
				}
			});
			mADController = new ADController(mContext);
			//serviceController = new YoopoonServiceController(mContext);
			commodityGridView = (MyGridView) rootView.findViewById(R.id.gridview_commodity);
			initShopFragment();
			//地址广播暂时业务不要求
			//加载首页各个服务事件
			loadServiceEvent(rootView);
			loadRefreshByKeyword();
		}
		return rootView;
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
			//调整Fragment到顶端
			requestProduct();
			fragmentShopScrollView = (ScrollView) rootView.findViewById(R.id.scrollview_fragment_shop);
			fragmentShopScrollView.scrollTo(0, 0);
		}
	}
	/**
	 * @Title: initShopFragment
	 * @Description: 初始化和设置视图控件
	 */
	private void initShopFragment() {
		requestAdvertisements();
		//requestServices();
		requestProduct();
		LinearLayout linearLayout = (LinearLayout) rootView.findViewById(R.id.linearlayout_fragment_shop);
		//添加广告
		linearLayout.addView(mADController.getRootView(), 0);
	}
	private void initRecommendProduct(JSONObject jsonObject) {
		burstPackageNameTextView.setText(jsonObject.optString("Name", ""));
		currentPriceTextView.setText("RMB" + jsonObject.optString("Price", ""));
		beforePriceTextView.setText("折扣前" + jsonObject.optString("NewPrice", ""));
		salesVolumeButton.setText("已有" + jsonObject.optString("Owner", "0") + "人抢购");
		String urlString = jsonObject.optString("MainImg", "");
		if (!urlString.equals("")) {
			ImageLoader.getInstance().displayImage(urlString, burstPackageImageView);
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
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						mProductGridViewAdapter = new ProductGridViewAdapter(mContext,
								JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
						commodityGridView.setAdapter(mProductGridViewAdapter);
						//加载推荐套餐内容
						initRecommendProduct(JSONArrayConvertToArrayList.convertToArrayList(jsonArray).get(0));
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
		}.setUrl(getString(R.string.url_get_communityproduct)).setRequestMethod(RequestMethod.eGet).notifyRequest();
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
						mProductGridViewAdapter.refresh(JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
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
	private class ProductRefreshByKeyWordReceiver extends BroadcastReceiver {
		@Override
		public void onReceive(Context context, Intent intent) {
			String keywordString = intent.getStringExtra("keyword");
			if(keywordString!=null&&(!keywordString.equals(""))){
				HashMap<String, String> hashMap = new HashMap<String, String>();
				hashMap.put("Name", keywordString);
				requestProduct(hashMap);
			}
			
		}
	}

	/**
	 * @Title: loadRefreshByAddress
	 * @Description: 截获地址广播
	 */
	/*	private void loadRefreshByAddress() {
			IntentFilter intentFilter = new IntentFilter("com.yoopoon.market.productRefresh.Address");
			ProductRefreshByAddressReceiver addressReceiver = new ProductRefreshByAddressReceiver();
			mContext.registerReceiver(addressReceiver, intentFilter);
		}*/
	private void loadRefreshByKeyword() {
		IntentFilter filter = new IntentFilter("com.yoopoon.market.search.byKeyword");
		byKeyWordReceiver = new ProductRefreshByKeyWordReceiver();
		mContext.registerReceiver(byKeyWordReceiver, filter);
	}
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
		mContext.unregisterReceiver(byKeyWordReceiver);
	}
}
