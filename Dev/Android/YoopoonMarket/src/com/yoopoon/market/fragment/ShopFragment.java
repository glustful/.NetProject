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
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.component.YoopoonServiceController;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.ResponseData.ResultState;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.market.view.MyGridView;
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
			// 解析布局
			rootView = inflater.inflate(R.layout.fragment_shop, null);
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
			// serviceController = new YoopoonServiceController(mContext);
			commodityGridView = (MyGridView) rootView.findViewById(R.id.gridview_commodity);
			initShopFragment();
			loadRefreshByAddress();
		}
		return rootView;
	}

	/**
	 * @Title: initShopFragment
	 * @Description: 初始化和设置视图控件
	 */
	private void initShopFragment() {
		requestAdvertisements();
		// requestServices();
		requestProduct();
		LinearLayout linearLayout = (LinearLayout) rootView.findViewById(R.id.linearlayout_fragment_shop);
		// 添加广告
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
						// 加载推荐套餐内容
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
	 * @ClassName: ProductRefreshByAddressReceiver
	 * @Description: 创建监听地址改变的接收器
	 * @author: 徐阳会
	 * @date: 2015年9月17日 上午11:02:12
	 */
	private class ProductRefreshByAddressReceiver extends BroadcastReceiver {

		@Override
		public void onReceive(Context context, Intent intent) {
			Toast.makeText(mContext, "111111", Toast.LENGTH_SHORT).show();

		}

	}

	private void loadRefreshByAddress() {
		IntentFilter intentFilter = new IntentFilter("com.yoopoon.market.productRefresh.Address");
		ProductRefreshByAddressReceiver addressReceiver = new ProductRefreshByAddressReceiver();
		mContext.registerReceiver(addressReceiver, intentFilter);

	}
}
