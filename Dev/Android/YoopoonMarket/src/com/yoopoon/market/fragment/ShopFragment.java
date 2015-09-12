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
import android.content.Context;
import android.content.Intent;
import android.graphics.Paint;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.GridView;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.component.YoopoonServiceController;
import com.yoopoon.market.ProductClassifyActivity_;
import com.yoopoon.market.ProductList_;
import com.yoopoon.market.R;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.ResponseData.ResultState;

public class ShopFragment extends Fragment {
	private Context mContext;
	private ADController mADController;
	private YoopoonServiceController serviceController;
	private View rootView;
	private ArrayList<String> imgs; // 存储顶端的广告图片地址
	private GridView commodityGridView;
	private CommodityGridViewAdapter mCommodityGridViewAdapter;
	private TextView beforePriceTextView; // 折扣前价格
	private TextView burstPackageTextView;

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
			beforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			// 爆款套餐和省到不行加粗样式设置
			burstPackageTextView = (TextView) rootView.findViewById(R.id.btn_burstpackage);
			burstPackageTextView.getPaint().setFakeBoldText(true);
			// ###############################################################################
			// 如下的代码只做API出来前的测试用途
			// ###############################################################################
			burstPackageTextView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Intent intent = new Intent(mContext, ProductClassifyActivity_.class);
					startActivity(intent);
				}
			});
			TextView saveMoneyTextView = (TextView) rootView.findViewById(R.id.btn_save_money);
			saveMoneyTextView.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					Intent intent = new Intent(mContext, ProductList_.class);
					Bundle bundle = new Bundle();
					bundle.putString("productClassification", "云南特产");
					intent.putExtras(bundle);
					startActivity(intent);
				}
			});
			// ###############################################################################
			// 如上的代码只做API出来前的测试用途
			// ###############################################################################
			mADController = new ADController(mContext);
			serviceController = new YoopoonServiceController(mContext);
			commodityGridView = (GridView) rootView.findViewById(R.id.gridview_commodity);
			// 测试用数据
			ArrayList<JSONObject> arrayList = new ArrayList<JSONObject>();
			for (int i = 0; i < 10; i++) {
				JSONObject jsonObject = new JSONObject();
				try {
					jsonObject.put("productName", "方便面" + i);
					jsonObject.put("currentPrice", "80" + i);
					jsonObject.put("beforePrice", "180" + i);
					arrayList.add(jsonObject);
				} catch (JSONException e) {
					e.printStackTrace();
				}
			}
			mCommodityGridViewAdapter = new CommodityGridViewAdapter(mContext, arrayList);
			commodityGridView.setAdapter(mCommodityGridViewAdapter);
			// 对Fragment_shop中的视图控件初始化和设置
			initShopFragment();
		}
		return rootView;
	}

	/**
	 * @Title: initShopFragment
	 * @Description: 初始化和设置视图控件
	 */
	private void initShopFragment() {
		requestAdvertisements();
		requestServices();
		LinearLayout linearLayout = (LinearLayout) rootView.findViewById(R.id.linearlayout_fragment_shop);
		linearLayout.addView(mADController.getRootView(), 0);
		linearLayout.addView(serviceController.getRootView(), 1);
	}

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
							// 添加广告
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

	private ArrayList<JSONArray> servicesArrayList;

	/**
	 * @Title: requestServices
	 * @Description: 获取首页优服务
	 */
	private void requestServices() {
		if (servicesArrayList == null) {
			new RequestAdapter() {
				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						if (servicesArrayList == null) {
							servicesArrayList = new ArrayList<JSONArray>();
							JSONArray list = data.getJsonArray();
							if (list == null || list.length() < 1)
								return;
							servicesArrayList.add(list);
							serviceController.show(servicesArrayList);
						}
					}
				}

				@Override
				public void onProgress(ProgressMessage msg) {
				}
			}.setUrl("/api/channel/GetActiveTitleImg").setRequestMethod(RequestMethod.eGet)
					.addParam("ChannelName", "活动").notifyRequest();
		}
	}

}
