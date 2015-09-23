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
import java.util.List;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.content.Intent;
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
import android.widget.FrameLayout;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.OnRefreshListener2;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.market.BalanceActivity_;
import com.yoopoon.market.MaternityMatronActivity_;
import com.yoopoon.market.ProductDetailActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.ServeListActivity2_;
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.ResponseData.ResultState;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.market.utils.SplitStringWithDot;
import com.yoopoon.view.adapter.ProductListAdapter;

public class ShopFragment extends Fragment {
	private Context mContext;
	private ADController mADController;
	private View rootView;
	private ArrayList<String> imgs; // 存储顶端的广告图片地址
	private ProductListAdapter productListAdapter;
	private ProductListAdapter searchProductAdapter;
	private static final String TAG = "ShopFragment";
	// 首页推荐商品控件
	private ImageView recommondProductImageView;// 套餐图片
	private TextView recommondProductBeforePriceTextView; // 折扣前价格
	private TextView recommondProductCurrentPriceTextView;// 当前价格
	private TextView recommondProductNameTextView; // 套餐名称
	private Button recommondProductByButton;// 立即购买
	private ImageView recommondProductCartImageView;// 添加到购物车
	// 首页展示商品的PTRGridView组件
	private PullToRefreshListView mPullToRefreshListView;
	private PullToRefreshListView searchPullToRefreshListView;
	private ListView productListView;
	private ListView searchListView;
	// 首页推荐商品视图
	View shopFragmentHeadView;
	// 首页推荐商品控件
	private Button salesVolumeButton;
	// 分页获取商品状态码
	private int productPageCount = 1;
	// 搜索商品分页状态码
	private int searchPageCount = 1;
	private ArrayList<JSONObject> productJsonArrayList;
	// 搜索关键字
	private String keywordSearch = "";
	private FrameLayout searchProductFrameLayout;
	private FrameLayout productListFrameLayout;
	// 进度条对应的linearlayout
	private LinearLayout progressbarLinearlayout;
private int debugStatusCode=0;
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
			searchProductFrameLayout = (FrameLayout) rootView.findViewById(R.id.framelayout_search_product_list);
			productListFrameLayout = (FrameLayout) rootView.findViewById(R.id.framelayout_product_list);
			progressbarLinearlayout = (LinearLayout) rootView.findViewById(R.id.linearlayout_progressbar);
			settingPullToRefreshListView();
			productJsonArrayList = new ArrayList<JSONObject>();
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
		// 联网获取广告
		requestAdvertisements();
		shopFragmentHeadView = LayoutInflater.from(mContext).inflate(R.layout.fragment_shop_headview, null);
		// 加载推荐套餐下的控件
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
		// 搜索PTR
		searchPullToRefreshListView = (PullToRefreshListView) rootView.findViewById(R.id.ptr_search_product);
		searchPullToRefreshListView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		searchPullToRefreshListView.setOnRefreshListener(new searchRefreshListener());
		productListView = mPullToRefreshListView.getRefreshableView();
		productListView.setFadingEdgeLength(0);
		productListView.setFastScrollEnabled(false);
		// 搜索ListView
		searchListView = searchPullToRefreshListView.getRefreshableView();
		searchListView.setFadingEdgeLength(0);
		searchListView.setFastScrollEnabled(false);
	}
	/**
	 * @Title: settingRecommondProduct
	 * @Description: 加载推荐套餐下的控件
	 */
	private void settingRecommondProduct() {
		recommondProductImageView = (ImageView) shopFragmentHeadView.findViewById(R.id.img_product);
		recommondProductBeforePriceTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_prime_price);
		recommondProductCurrentPriceTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_sales_price);
		recommondProductNameTextView = (TextView) shopFragmentHeadView.findViewById(R.id.tv_product_name);
		recommondProductByButton = (Button) shopFragmentHeadView.findViewById(R.id.purchase_immediately_button);
		salesVolumeButton = (Button) shopFragmentHeadView.findViewById(R.id.has_buy_button);
		recommondProductCartImageView = (ImageView) shopFragmentHeadView.findViewById(R.id.img_cart);
		// 点击图片跳转到商品详细信息
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
		// 点击小购物车图片事件

		requestProduct();
	}
	/**
	 * @Title: initRecommendProduct
	 * @Description: 初始化推荐套餐商品信息
	 * @param jsonObject
	 */
	private void initRecommendProduct(JSONObject jsonObject) {
		final String name = jsonObject.optString("Name", "");
		recommondProductNameTextView.setText(name);
		final String price = SplitStringWithDot.split(jsonObject.optString("Price", "0"));
		recommondProductCurrentPriceTextView.setText("RMB"
				+ SplitStringWithDot.split(jsonObject.optString("Price", "0")));
		String old_price;
		if (jsonObject.optString("OldPrice", "").equals("null")) {
			old_price = "0";
			recommondProductBeforePriceTextView.setText("折扣前0");
			recommondProductBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		} else {
			old_price = SplitStringWithDot.split(jsonObject.optString("OldPrice", "0"));
			recommondProductBeforePriceTextView.setText("折扣前"
					+ SplitStringWithDot.split(jsonObject.optString("OldPrice", "0")));
			recommondProductBeforePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		}
		final String oldPrice = old_price;
		if (jsonObject.optString("Owner", "0").equals("null")) {
			salesVolumeButton.setText("已有0人抢购");
		} else {
			salesVolumeButton.setText("已有" + jsonObject.optString("Owner", "0") + "人抢购");
		}
		final String urlString = getString(R.string.url_image) + jsonObject.optString("MainImg", "");
		if (!urlString.equals("")) {
			ImageLoader.getInstance().displayImage(urlString, recommondProductImageView);
		}
		final String title = jsonObject.optString("Subtitte", "");
		final int id = jsonObject.optInt("Id", 0);
		recommondProductByButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				List<Staff> staffList = new ArrayList<Staff>();
				staffList.add(new Staff(title, name, urlString, 1, Float.parseFloat(price), Float.parseFloat(oldPrice)));
				BalanceActivity_.intent(getActivity()).staffList(staffList).start();
			}
		});
		recommondProductCartImageView.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(final View v) {
				new Thread() {

					public void run() {
						DBDao dao = new DBDao(mContext);
						if (dao.isExist(id)) {
							int count = dao.isExistCount(id);
							dao.updateCount(id, count + 1);
						} else {

							dao.add(new Staff(title, name, urlString, 1, Float.parseFloat(price), Float
									.parseFloat(oldPrice), id));
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
		progressbarLinearlayout.setVisibility(View.VISIBLE);
		HashMap<String, String> map = new HashMap<String, String>();
		map.put("Page", "1");
		map.put("PageCount", "10");
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				progressbarLinearlayout.setVisibility(View.GONE);
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						if (productJsonArrayList != null && productJsonArrayList.size() >= 1) {
							productListAdapter.refresh(productJsonArrayList);
						} else {
							productListAdapter = new ProductListAdapter(mContext,
									JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
							productListView.setAdapter(productListAdapter);
							productJsonArrayList = JSONArrayConvertToArrayList.convertToArrayList(jsonArray);
						}
						// 加载推荐套餐内容
						productListAdapter = new ProductListAdapter(mContext,
								JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
						productListView.setAdapter(productListAdapter);
						// 加载推荐套餐内容
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
	 * @Description: 传入参数，刷新商品信息
	 * @param hashMap
	 */
	private void requestProduct(HashMap<String, String> hashMap) {
		progressbarLinearlayout.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				progressbarLinearlayout.setVisibility(View.GONE);
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						if (searchProductFrameLayout.getVisibility() == View.VISIBLE) {
						} else {
							productListAdapter.refresh(JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
							initRecommendProduct(JSONArrayConvertToArrayList.convertToArrayList(jsonArray).get(0));
						}
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
	 * @Title: refreshProduct
	 * @Description: 传入参数，获取商品信息
	 * @param hashMap
	 */
	private void refreshProduct(HashMap<String, String> hashMap) {
		/*
		 * HashMap<String, String> map1 = new HashMap<String, String>(); map1.put("Page", 5 + "");
		 * map1.put("PageCount", "10");
		 */
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				mPullToRefreshListView.onRefreshComplete();
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						if (jsonArray.length() == 0) {
							Toast.makeText(mContext, "商品已经全部加载", Toast.LENGTH_SHORT).show();
							productPageCount = productPageCount - 1;
							return;
						}
						productListAdapter.addRefresh(JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
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
	private void loadServiceEvent(View view) {
		ImageView houseKeepingImageView = (ImageView) view.findViewById(R.id.img_house_keeping);
		ImageView washingImageView = (ImageView) view.findViewById(R.id.img_washing_clothes);
		ImageView takeInExpressImageView = (ImageView) view.findViewById(R.id.img_take_in_express);
		ImageView moreServiceImageView = (ImageView) view.findViewById(R.id.img_more_service);
		// 家政服务
		houseKeepingImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				ServeListActivity2_.intent(getActivity()).contents(new String[] { "家政", "家政" }).start();
				return false;
			}
		});
		// 清洗服务
		washingImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				ServeListActivity2_.intent(getActivity()).contents(new String[] { "清洗服务", "清洗" }).start();
				return false;
			}
		});
		// 代收快递服务
		takeInExpressImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				ServeListActivity2_.intent(getActivity()).contents(new String[] { "快递代收", "快递" }).start();
				return false;
			}
		});
		// 更多服务
		moreServiceImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				Intent intent = new Intent("com.yoopoon.market.service.moreservice");
				intent.addCategory(Intent.CATEGORY_DEFAULT);
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
	}

	private class RefreshListener implements OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			productJsonArrayList.clear();
			productPageCount = 1;
			requestProduct();
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			HashMap<String, String> map = new HashMap<String, String>();
			map.put("Page", ++productPageCount + "");
			map.put("PageCount", "10");
			refreshProduct(map);
		}
	}

	private class searchRefreshListener implements OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			HashMap<String, String> hashMap = new HashMap<String, String>();
			hashMap.put("Name", keywordSearch);
			hashMap.put("PageCount", "10");
			hashMap.put("Page", (++searchPageCount) + "");
			requestSearchProduct(hashMap);
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
		productPageCount = 1;
		searchPageCount = 1;
	}
	/**
	 * @Title: searchProduct
	 * @Description: 搜索商品
	 */
	public void searchProduct(String searchString) {
		++debugStatusCode;
		searchProductFrameLayout.setVisibility(View.VISIBLE);
		productListFrameLayout.setVisibility(View.GONE);
		keywordSearch = searchString;
		HashMap<String, String> hashMap = new HashMap<String, String>();
		hashMap.put("Name", searchString);
		hashMap.put("PageCount", "10");
		hashMap.put("Page", "1");
		requestSearchProduct(hashMap);
	}
	/**
	 * @Title: requestSearchProduct
	 * @Description: 获取搜索数据
	 * @param searchString
	 */
	private void requestSearchProduct(HashMap<String, String> hashMap) {
		progressbarLinearlayout.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if(debugStatusCode==2){
					Log.e("111111111111",debugStatusCode+"");
				}
				progressbarLinearlayout.setVisibility(View.GONE);
				searchPullToRefreshListView.onRefreshComplete();
				JSONArray jsonArray;
				if (data.getMRootData() != null) {
					try {
						jsonArray = data.getMRootData().getJSONArray("List");
						if (jsonArray.length() >= 1) {
							if (searchPageCount > 1) {
								searchProductAdapter.addRefresh(JSONArrayConvertToArrayList
										.convertToArrayList(jsonArray));
							} else if (searchPageCount == 1) {
								searchProductAdapter = new ProductListAdapter(mContext,
										JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
								searchListView.setAdapter(searchProductAdapter);
							}
						}else{
							searchProductAdapter.refresh(JSONArrayConvertToArrayList.convertToArrayList(jsonArray));
						}
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
	 * @Title: settingClearSearch
	 * @Description: 通过MainActivity中的调用设置搜索界面和商品列表界面的显示和隐藏
	 */
	public void settingClearSearch() {
		searchProductFrameLayout.setVisibility(View.GONE);
		productListFrameLayout.setVisibility(View.VISIBLE);
	}
}
