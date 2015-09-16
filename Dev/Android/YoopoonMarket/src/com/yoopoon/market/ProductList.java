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
import java.util.HashMap;
import java.util.Map;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.OnRefreshListener2;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.view.adapter.ProductListViewAdapter;

/**
 * @ClassName: ProductList
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月10日 下午2:16:15
 */
@EActivity(R.layout.acitvity_product_list)
public class ProductList extends MainActionBarActivity implements OnClickListener {
	private Dialog screenPriceDialog, sortDialog;
	private Context mContext;
	private ListView productListView;
	private ProductListViewAdapter mProductListViewAdapter;
	private Button confirmButton, cancelButton, resetPriceButton;
	private EditText productBeginPriceEditText, productEndPriceEditText;
	private Button resetSortButton, cancelSortButton;
	private static int priceBegain = 0, priceEnd = 0;
	private String classificationId; //从其他activity过来的分类id
	private String titleString;
	//排序状态码 0对应空，1对应低价到高，2对应高价到低价，3对应低销量到高销量，4对应高销量到低销量
	private static int sortStatusCode = 0;
	//存储传送到服务器的参数
	//配置排序的四个按钮
	private Button changedCodlorButton;
	private Button sortByPriceFromLowerButton, sortByPriceFromHigherButton, sortBySalesVolumeFromLowerButton,
			sortBySalesVolumeFromHigherButton;
	//存储分类状态信息
	private String classificationStatusCode="";
	@ViewById
	LinearLayout linearLayout_product_list;
	@ViewById
	PullToRefreshListView ptr_listview_product_list;
	@ViewById(R.id.btn_screen_product_list)
	Button screenProductButton;
	@ViewById(R.id.btn_setting_sort_method)
	Button settingSortMethodButton;

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
		titleString = bundle.getString("classificationName");
		classificationId = bundle.getString("classificationId");
		titleButton.setText(titleString);
		backButton.setText("后退");
		//房屋列表
		ptr_listview_product_list.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		ptr_listview_product_list.setOnRefreshListener(new RefreshListener());
		productListView = ptr_listview_product_list.getRefreshableView();
		//筛选按钮和事件设置
		screenProductButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				AlertDialog.Builder builder = new AlertDialog.Builder(mContext);
				View screenPriceView = View.inflate(mContext, R.layout.dialog_set_price, null);
				confirmButton = (Button) screenPriceView.findViewById(R.id.btn_confirm);
				cancelButton = (Button) screenPriceView.findViewById(R.id.btn_cancel);
				resetPriceButton = (Button) screenPriceView.findViewById(R.id.btn_reset_price_setting);
				productBeginPriceEditText = (EditText) screenPriceView.findViewById(R.id.et_price_begin);
				productEndPriceEditText = (EditText) screenPriceView.findViewById(R.id.et_price_end);
				confirmButton.setOnClickListener(ProductList.this);
				cancelButton.setOnClickListener(ProductList.this);
				resetPriceButton.setOnClickListener(ProductList.this);
				builder.setView(screenPriceView);
				screenPriceDialog = builder.show();
			}
		});
		//设置综合排序方式和重置排序
		settingSortMethodButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				AlertDialog.Builder builder = new AlertDialog.Builder(mContext);
				View sortProductView = View.inflate(mContext, R.layout.dialog_set_sort_method, null);
				//视图控件获取和初始化
				resetSortButton = (Button) sortProductView.findViewById(R.id.btn_reset_sort_method);
				cancelSortButton = (Button) sortProductView.findViewById(R.id.btn_cancel_sort_method);
				//初始化排序控件
				sortByPriceFromLowerButton = (Button) sortProductView.findViewById(R.id.btn_sort_by_price_form_lower);
				sortByPriceFromHigherButton = (Button) sortProductView.findViewById(R.id.btn_sort_by_price_from_higher);
				sortBySalesVolumeFromLowerButton = (Button) sortProductView
						.findViewById(R.id.btn_sort_by_sales_volume_from_lower);
				sortBySalesVolumeFromHigherButton = (Button) sortProductView
						.findViewById(R.id.btn_sort_by_sales_volume_from_higher);
				//添加点击事件
				resetSortButton.setOnClickListener(ProductList.this);
				cancelSortButton.setOnClickListener(ProductList.this);
				//设置排序四种方法事件
				sortByPriceFromLowerButton.setOnClickListener(ProductList.this);
				sortByPriceFromHigherButton.setOnClickListener(ProductList.this);
				sortBySalesVolumeFromLowerButton.setOnClickListener(ProductList.this);
				sortBySalesVolumeFromHigherButton.setOnClickListener(ProductList.this);
				builder.setView(sortProductView);
				sortDialog = builder.show();
			}
		});
		requestClassification();
		requsetProductList();
	}
	//######################################################################################################
	//                               此处的id=1只做测试使用
	//######################################################################################################
	/**
	 * @Title: requestClassification
	 * @Description: 根据父类参数获取3级分类信息
	 */
	private void requestClassification() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONArray jsonArray = data.getMRootData().optJSONArray("Object");
					addClassificationButton(JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_category_get)).addParam("id", "2").setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}
	private void addClassificationButton(ArrayList<JSONObject> arrayList) {
		changedCodlorButton = new Button(mContext);
		for (final JSONObject jsonObject : arrayList) {
			Button button = new Button(mContext);
			button.setText(jsonObject.optString("Name"));
			button.setBackgroundColor(Color.TRANSPARENT);
			button.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					//点击效果背景色设置
					changedCodlorButton.setTextColor(Color.BLACK);
					Toast.makeText(mContext, jsonObject.optString("Name").toString(), Toast.LENGTH_SHORT).show();
					Button button1 = (Button) v;
					//点击效果背景色设置
					button1.setTextColor(Color.rgb(255, 34, 30));
					changedCodlorButton = button1;
					classificationStatusCode=jsonObject.optString("Id");
					screenPrice();
				}
			});
			linearLayout_product_list.addView(button);
		}
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

	/**
	 * @Title: requsetProductList
	 * @Description: 请求网络数据，当Activity启动的时候加载产品
	 */
	private void requsetProductList() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONArray array = data.getMRootData().optJSONArray("List");
					mProductListViewAdapter = new ProductListViewAdapter(mContext,
							JSONArrayConvertToArrayList.convertToArrayList(array));
					productListView.setAdapter(mProductListViewAdapter);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_get_communityproduct)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}
	/**
	 * @Title: requsetProductList
	 * @Description:设置参数，链接网络，货物商品列表
	 * @param hashMap
	 */
	private void requsetProductList(HashMap<String, String> hashMap) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONArray array = data.getMRootData().optJSONArray("List");
					mProductListViewAdapter.refresh(JSONArrayConvertToArrayList.convertToArrayList(array));
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_get_communityproduct)).setRequestMethod(RequestMethod.eGet).addParam(hashMap)
				.notifyRequest();
	}
	/*
	 * @Title: onClick
	 * @Description: Activity中条件筛选的点击事件
	 * @param v 
	 * @see android.view.View.OnClickListener#onClick(android.view.View) 
	 */
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.btn_confirm:
				if ((!TextUtils.isEmpty(productBeginPriceEditText.getText()))
						&& (!TextUtils.isEmpty(productEndPriceEditText.getText()))) {
					screenProductButton.setText(productBeginPriceEditText.getText() + "元 - "
							+ productEndPriceEditText.getText() + "元");
				}
				//判断输入的价格为空时整理逻辑
				if (TextUtils.isEmpty(productBeginPriceEditText.getText())) {
				} else {
					priceBegain = Integer.parseInt(productBeginPriceEditText.getText().toString());
				}
				if (TextUtils.isEmpty(productEndPriceEditText.getText())) {
				} else {
					priceEnd = Integer.parseInt(productEndPriceEditText.getText().toString());
				}
				screenPrice();
				screenPriceDialog.dismiss();
				break;
			case R.id.btn_cancel:
				screenPriceDialog.dismiss();
				break;
			case R.id.btn_reset_price_setting:
				screenProductButton.setText("筛选");
				priceBegain = 0;
				priceEnd = 0;
				screenPriceDialog.dismiss();
				break;
			case R.id.btn_reset_sort_method:
				sortStatusCode = 0;
				settingSortMethodButton.setText("综合排序");
				sortDialog.dismiss();
				break;
			case R.id.btn_cancel_sort_method:
				sortDialog.dismiss();
				break;
			case R.id.btn_sort_by_price_form_lower://低价到高价
				//设置排序状态码
				sortStatusCode = 1;
				settingSortMethodButton.setText("低价到高价");
				screenPrice();
				sortDialog.dismiss();
				break;
			case R.id.btn_sort_by_price_from_higher://高价到低价
				//设置排序状态码
				sortStatusCode = 2;
				settingSortMethodButton.setText("高价到低价");
				screenPrice();
				sortDialog.dismiss();
				break;
			case R.id.btn_sort_by_sales_volume_from_lower://低销量到高销量
				//设置排序状态码
				sortStatusCode = 3;
				settingSortMethodButton.setText("低销量到高销量");
				screenPrice();
				sortDialog.dismiss();
				break;
			case R.id.btn_sort_by_sales_volume_from_higher://高销量到低销量
				//设置排序状态码
				sortStatusCode = 4;
				settingSortMethodButton.setText("高销量到低销量");
				screenPrice();
				sortDialog.dismiss();
				break;
			default:
				break;
		}
	}
	//###################################################################################################
	//                                     注意：API只提供了价格排序，销量排序为提供
	//###################################################################################################
	private void screenPrice() {
		HashMap<String, String> hashMap = new HashMap<String, String>();
		//获取排序参数,同时设置参数到Map中
		if (sortStatusCode == 0) {
			hashMap.put("IsDescending", "");
			hashMap.put("OrderBy", "");
		} else if (sortStatusCode == 1) {
			hashMap.put("IsDescending", "false");
			hashMap.put("OrderBy", "OrderByPrice");
		} else if (sortStatusCode == 2) {
			hashMap.put("IsDescending", "true");
			hashMap.put("OrderBy", "OrderByPrice");
		} else if (sortStatusCode == 3) {
			hashMap.put("IsDescending", "false");
			hashMap.put("OrderBy", "OrderByPrice");
		} else if (sortStatusCode == 4) {
			hashMap.put("IsDescending", "true");
			hashMap.put("OrderBy", "OrderByPrice");
		}
		//获取分类参数（等待API完成）
		if(!classificationStatusCode.equals("")){
			hashMap.put("CategoryId", classificationStatusCode);
		}
		//获取价格参数
		if (priceBegain != 0) {
			hashMap.put("PriceBegin", priceBegain + "");
		}
		if (priceEnd != 0) {
			hashMap.put("PriceEnd", priceEnd + "");
		}
		requsetProductList(hashMap);
	}
	//###################################################################################################
	//                                     注意：API只提供了价格排序，销量排序为提供
	//###################################################################################################
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
