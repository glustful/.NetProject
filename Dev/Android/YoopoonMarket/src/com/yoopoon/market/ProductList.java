/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ProductList.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: 该类负责的是从产品分类选择界面过来后产品列表展示的控制，也就是控制产品条件检索等
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月10日 下午2:16:15 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.Toast;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.OnRefreshListener2;
import com.handmark.pulltorefresh.library.PullToRefreshListView;

/**
 * @ClassName: ProductList
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月10日 下午2:16:15
 */
@EActivity(R.layout.acitvity_product_list)
public class ProductList extends MainActionBarActivity {
	private Context mContext;
	private ListView productListView;
	private ProductListViewAdapter mProductListViewAdapter;
	private ArrayList<JSONObject> jsonArrayList;
	@ViewById
	LinearLayout linearLayout_product_list;
	@ViewById
	PullToRefreshListView ptr_listview_product_list;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mContext = ProductList.this;
	}
	@AfterViews
	void initProductList() {
		//设置标题栏
		titleButton.setVisibility(View.VISIBLE);
		backButton.setVisibility(View.VISIBLE);
		Bundle bundle = getIntent().getExtras();
		String titleString = bundle.getString("productClassification");
		titleButton.setText(titleString);
		backButton.setText("后退");
		//测试使用
		for (int i = 0; i < 10; i++) {
			Button button = new Button(mContext);
			button.setText("button" + i);
			button.setBackgroundColor(Color.TRANSPARENT);
			linearLayout_product_list.addView(button);
		}
		//房屋列表
		ptr_listview_product_list.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		ptr_listview_product_list.setOnRefreshListener(new RefreshListener());
		productListView = ptr_listview_product_list.getRefreshableView();
		jsonArrayList = new ArrayList<JSONObject>();
		for (int i = 0; i < 20; i++) {
			JSONObject jsonObject = new JSONObject();
			try {
				jsonObject.put("productTitle", "米糕" + i);
				jsonObject.put("productSubtitle", "云南雪糕" + i);
				jsonObject.put("productAdvertisement", "30免运费" + i);
				jsonObject.put("productPrict", "￥80.0" + i);
				jsonObject.put("productSalesValuem", "100" + i);
			} catch (JSONException e) {
				e.printStackTrace();
			}
			jsonArrayList.add(jsonObject);
		}
		mProductListViewAdapter = new ProductListViewAdapter(mContext, jsonArrayList);
		
		productListView.setAdapter(mProductListViewAdapter);
	}

	/**
	 * @ClassName: RefreshListenerSetting
	 * @Description: 设置PTR刷新的方式
	 * @author: 徐阳会
	 * @date: 2015年9月10日 下午3:44:03
	 */
	private class RefreshListener implements OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			Toast.makeText(mContext, "pull down to refresh", Toast.LENGTH_SHORT).show();
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			Toast.makeText(mContext, "pull up to refresh", Toast.LENGTH_SHORT).show();
		}
	}

	@Override
	public void backButtonClick(View v) {
		finish();
	}
	@Override
	public void titleButtonClick(View v) {
	}
	@Override
	public void rightButtonClick(View v) {
	}
	@Override
	public Boolean showHeadView() {
		return true;
	}
}
