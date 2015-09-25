package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONObject;

import android.content.Context;
import android.content.Intent;
import android.graphics.Paint;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.ViewGroup;
import android.view.WindowManager;
import android.view.animation.AccelerateInterpolator;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.AnimationSet;
import android.view.animation.LinearInterpolator;
import android.view.animation.RotateAnimation;
import android.view.animation.ScaleAnimation;
import android.view.animation.TranslateAnimation;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.PopupWindow;
import android.widget.RelativeLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ProductAdvertisement;
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.market.utils.SplitStringWithDot;
import com.yoopoon.market.view.HtmlWebView;

@EActivity(R.layout.activity_product_detail)
public class ProductDetailActivity extends MainActionBarActivity {
	private static final String TAG = "ProductDetailActivity";
	private ProductAdvertisement productAdvertisement;
	private ArrayList<String> imgs;
	private LinearLayout linearLayout;
	private Context mContext;
	private String productId;
	// 评论对应的布局
	private LinearLayout commentLinearlayout;
	// 添加更多评论对应的布局
	private static int count = 2;
	private ArrayList<JSONObject> commentJsonObjects;
	// 商品详细信息对应的布局视图
	private TextView productTitleTextView;
	private TextView productSubtitleTextView;
	private TextView productPrictTextView;
	private TextView productPrimePriceTextView;
	private TextView productSalesVolumeTextView;
	@ViewById(R.id.scrollview_product_detail)
	ScrollView productDetailScrollView;
	@ViewById(R.id.img_product_detail_return)
	ImageView returnToShopImageView;// 返回按钮
	@ViewById(R.id.img_cart)
	ImageView cartImageView;// 购物车
	@ViewById(R.id.btn_add_more_comment)
	Button addMoreCommentButton;
	@ViewById(R.id.tv_product_comment_amount)
	TextView productCommentAmount;
	@ViewById(R.id.linearlayout_slider_add_more)
	LinearLayout sliderAddMoreLinearLayout;
	@ViewById(R.id.linearlayout_shopping)
	LinearLayout shoppingLinearLayout;
	@ViewById(R.id.webview_product_description)
	HtmlWebView productDescriptionWebView;
	String header = "<!doctype html><html><head><meta name = \"viewport\" content = \"width = device-width\"/></head><body>";
	String tail = "</body></html>";
	@ViewById(R.id.btn_add_cart)
	Button addToCartButton;
	// 动画对应的商品图片
	@ViewById(R.id.img_animation_cart)
	ImageView animationCartImageView;
	// 获取购物车和加入购物车按钮位置
	private int cartXPosition;
	private int cartYPosition;
	private int pictureXPosition;
	private int pictureYPosition;
	// 商品图片位置
	private String productImageURL;
	Staff staff;
	//设置商品数量对应的Popupwindow
	private LinearLayout productAmountLinearLayout;
	private Button reduceButton, addButton, confirmButton;
	private EditText productAmountEditText;
	private ImageView cancelImageView;
	private PopupWindow mPopupwindow;
	private RelativeLayout productDetailRelativeLayout;

	@Click(R.id.btn_purchase)
	void purchase() {
		initPopupWindow();
	}
	private void initPopupWindow() {
		productAmountLinearLayout = (LinearLayout) LayoutInflater.from(mContext).inflate(R.layout.pupupwindow_purchase,
				null);
		reduceButton = (Button) productAmountLinearLayout.findViewById(R.id.btn_reduce_amount);
		addButton = (Button) productAmountLinearLayout.findViewById(R.id.btn_add_amount);
		productAmountEditText = (EditText) productAmountLinearLayout.findViewById(R.id.et_parduct_amount);
		confirmButton = (Button) productAmountLinearLayout.findViewById(R.id.btn_confirm_amount);
		mPopupwindow = new PopupWindow(productAmountLinearLayout, ViewGroup.LayoutParams.MATCH_PARENT,
				ViewGroup.LayoutParams.WRAP_CONTENT);
		cancelImageView = (ImageView) productAmountLinearLayout.findViewById(R.id.img_cancel_setting_amount);
		mPopupwindow.setBackgroundDrawable(new BitmapDrawable());
		mPopupwindow.setOutsideTouchable(true);
		mPopupwindow.setFocusable(true);
		mPopupwindow.showAtLocation(shoppingLinearLayout, Gravity.BOTTOM, 0, 0);
		mPopupwindow.setInputMethodMode(PopupWindow.INPUT_METHOD_NEEDED);
		mPopupwindow.setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_ADJUST_PAN);
		mPopupwindow.setOnDismissListener(new popDismissListener());
		popBackgroundAlpha(0.5f);
		//减少数量
		reduceButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				int productAmount = Integer.parseInt(productAmountEditText.getText().toString().trim());
				if (productAmount > 1) {
					productAmount = productAmount - 1;
				} else if (productAmount <= 1) {
					productAmount = 1;
				}
				productAmountEditText.setText(productAmount + "");
			}
		});
		addButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				int productAmount = Integer.parseInt(productAmountEditText.getText().toString().trim());
				productAmount = productAmount + 1;
				productAmountEditText.setText(productAmount + "");
			}
		});
		confirmButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				if (staff == null)
					return;
				List<Staff> staffList = new ArrayList<Staff>();
				staffList.add(staff);
				staff.count = Integer.parseInt(productAmountEditText.getText().toString().trim());
				BalanceActivity_.intent(ProductDetailActivity.this).staffList(staffList).start();
			}
		});
		cancelImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				mPopupwindow.dismiss();
				return true;
			}
		});
	}

	// 设置返回状态码，如果是首页跳转返回首页，如果是商品列表页回商品列表页
	private String comeFromstatusCode;

	/**
	 * @Title: initProductComment
	 * @Description: 初始化商品详细信息界面
	 */
	@AfterViews
	void initProductDetail() {
		// 获取从首页过来的id
		comeFromstatusCode = getIntent().getExtras().getString("comeFromstatusCode");
		productId = getIntent().getExtras().getString("productId");
		linearLayout = (LinearLayout) findViewById(R.id.linearlayout_product_detail);
		productDetailRelativeLayout = (RelativeLayout) LayoutInflater.from(mContext).inflate(
				R.layout.activity_product_detail, null);
		productAdvertisement = new ProductAdvertisement(mContext);
		// 获取广告信息
		// 添加广告
		linearLayout.addView(productAdvertisement.getRootView(), 0);
		// 初始化商品详细信息视图
		productTitleTextView = (TextView) findViewById(R.id.tv_product_title);
		productSubtitleTextView = (TextView) findViewById(R.id.tv_product_subtitle);
		productPrictTextView = (TextView) findViewById(R.id.tv_product_selling_price);
		productPrimePriceTextView = (TextView) findViewById(R.id.tv_product_prime_price);
		productSalesVolumeTextView = (TextView) findViewById(R.id.tv_product_sales_volume);
		// 设置删除线
		productPrimePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		// 货物评论对应LinearLayout
		commentLinearlayout = (LinearLayout) findViewById(R.id.linearlayout_product_comment);
		requestProductDetail();
		requestComment();
		returnToShopImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if (comeFromstatusCode.equals("shopFragment")) {
					Intent intent = new Intent(ProductDetailActivity.this, MainActivity_.class);
					startActivity(intent);
				} else if (comeFromstatusCode.equals("ProductClassificationList")) {
					Intent intent = new Intent(ProductDetailActivity.this, ProductClassificationList_.class);
					startActivity(intent);
				} else if (comeFromstatusCode.equals("productKeywordList")) {
					Intent intent = new Intent(ProductDetailActivity.this, ProductKeywordList_.class);
					startActivity(intent);
				}
				return true;
			}
		});
		addMoreCommentButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent(ProductDetailActivity.this, ProductCommentActivity_.class);
				Bundle bundle = new Bundle();
				bundle.putString("productId", productId);
				bundle.putString("commentAmount", commentJsonObjects.size() + "");
				intent.putExtras(bundle);
				startActivity(intent);
			}
		});
		// productDescriptionWebView.loadData(header + "HTML代码" + tail, "text/html;charset=utf-8",
		// null);
		// productDescriptionWebView.loadUrl("http://www.baidu.com");
		addToCartButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				loadProductAnimationPicture();
				new Thread() {
					public void run() {
						if (staff != null) {
							Log.i(TAG, staff.toString());
							int productId = staff.productId;
							DBDao dao = new DBDao(ProductDetailActivity.this);
							if (dao.isExist(productId)) {
								dao.updateCount(productId, dao.isExistCount(productId) + 1);
							} else
								dao.add(staff);
							Intent intent = new Intent("com.yoopoon.market.daocount");
							intent.putExtra("Count", dao.getAllCounts());
							intent.addCategory(Intent.CATEGORY_DEFAULT);
							ProductDetailActivity.this.sendBroadcast(intent);
						}
					};
				}.start();
			}
		});
		cartImageView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent("com.yoopoon.market.showcart");
				intent.addCategory(Intent.CATEGORY_DEFAULT);
				/*
				 * Intent intent = new Intent(ProductDetailActivity.this, MainActivity_.class);
				 * Bundle bundle = new Bundle(); bundle.putBoolean("productDetailCartClick", true);
				 * intent.putExtra("productDetailBundle", bundle); mContext.startActivity(intent);
				 */
				sendBroadcast(intent);
				MainActivity_.intent(mContext).start();
			}
		});
	}
	/**
	 * @Title: loadComment
	 * @Description: 循环加载产品评论
	 * @param jsonObjects
	 */
	private void loadComment(ArrayList<JSONObject> jsonObjects, int number) {
		int childCount = commentLinearlayout.getChildCount();
		if (jsonObjects.size() == childCount) {
			return;
		} else {
			for (int i = 0; i < number; i++) {
				JSONObject jsonObject = jsonObjects.get(childCount + i);
				LinearLayout commentItemLinearLayout = (LinearLayout) LayoutInflater.from(mContext).inflate(
						R.layout.item_product_comment, null);
				ImageView userPhotoImageView = (ImageView) commentItemLinearLayout
						.findViewById(R.id.img_comment_user_photo);
				TextView userNickNameTextView = (TextView) commentItemLinearLayout
						.findViewById(R.id.tv_comment_user_nickname);
				TextView commentContentTextView = (TextView) commentItemLinearLayout
						.findViewById(R.id.tv_comment_content);
				/*
				 * String url = ""; if (jsonObject.optString("UserImg").equals("null") ||
				 * jsonObject.optString("UserName").equals("")) { url =
				 * "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg"; } else { url =
				 * mContext.getString(R.string.url_image) + jsonObject.optString("UserImg"); }
				 */
				String url = "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg";
				userPhotoImageView.setTag(url);
				ImageLoader.getInstance().displayImage(url, userPhotoImageView, MyApplication.getOptions(),
						MyApplication.getLoadingListener());
				userNickNameTextView.setText(jsonObject.optString("UserName"));
				commentContentTextView.setText(jsonObject.optString("Content"));
				if (childCount == 0) {
					commentLinearlayout.addView(commentItemLinearLayout, i);
				} else {
					commentLinearlayout.addView(commentItemLinearLayout, i + childCount);
				}
			}
			if (childCount >= 2) {
				new Thread() {
					public void run() {
						runOnUiThread(new Runnable() {
							@Override
							public void run() {
								productDetailScrollView.fullScroll(ScrollView.FOCUS_DOWN);
							}
						});
					};
				}.start();
			}
		}
	}
	/**
	 * @Title: loadMoreComment
	 * @Description: 每次增加2，控制需要显示的广告数量
	 */
	public void loadMoreComment() {
		count += 2;
		requestComment();
	}
	@Override
	protected void onPause() {
		super.onPause();
		count = 2;
		if (mPopupwindow != null) {
			mPopupwindow.dismiss();
		}
		popBackgroundAlpha(1.0f);
	}
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mContext = this;
	}
	@Override
	public void backButtonClick(View v) {
	}
	@Override
	public void titleButtonClick(View v) {
	}
	@Override
	public void rightButtonClick(View v) {
	}
	@Override
	public Boolean showHeadView() {
		return null;
	}
	/**
	 * @Title: requestProductDetail
	 * @Description: 获取商品详细信息，包括商品评价,同时刷新界面
	 */
	private void requestProductDetail() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONObject productJsonObject = data.getMRootData().optJSONObject("ProductModel");
					//设置商品详细信息
					initProductDetailInfo(productJsonObject);
					//设置广告,同事加载广告
					loadProductAdvertisements(productJsonObject);
					if ((!productJsonObject.optString("MainImg").equals(""))
							&& (!productJsonObject.optString("MainImg").equals("null"))) {
						productImageURL = getString(R.string.url_image) + productJsonObject.optString("MainImg");
					}
					//JSONObject productJsonObject = data.getMRootData().optJSONObject("ProductModel");
					// 设置商品详细信息
					//initProductDetailInfo(productJsonObject);
					// 设置广告,同事加载广告
					//loadProductAdvertisements(productJsonObject);
					/*if ((!productJsonObject.optString("MainImg").equals("null"))
							&& (!productJsonObject.optString("MainImg").equals("null"))) {
						productImageURL = getString(R.string.url_image) + productJsonObject.optString("MainImg");

					}*/
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_get_communityproduct)).setRequestMethod(RequestMethod.eGet)
				.addParam("id", productId).notifyRequest();
	}
	/**
	 * @Title: initProductDetailInfo
	 * @Description: 根据传入的参数对商品详细信息进行设置
	 * @param jsonObject
	 */
	private void initProductDetailInfo(JSONObject jsonObject) {
		String name = jsonObject.optString("Name", "");
		String subtitte = jsonObject.optString("Subtitte", "");
		String price = SplitStringWithDot.split(jsonObject.optString("Price", "0"));
		String oldPrice;
		String imageUrl = getString(R.string.url_image) + jsonObject.optString("MainImg");
		productTitleTextView.setText(jsonObject.optString("Name", ""));
		productSubtitleTextView.setText(jsonObject.optString("Subtitte", ""));
		productPrictTextView.setText("￥" + SplitStringWithDot.split(jsonObject.optString("Price", "0")));
		if (!jsonObject.optString("OldPrice", "0").equals("null")) {
			oldPrice = SplitStringWithDot.split(jsonObject.optString("OldPrice", "0"));
			productPrimePriceTextView.setText("原价：" + SplitStringWithDot.split(jsonObject.optString("OldPrice", "0")));
		} else {
			oldPrice = "0";
			productPrimePriceTextView.setText("原价0");
		}
		if (jsonObject.optString("Owner", "0").equals("null")) {
			productSalesVolumeTextView.setText("已有0人抢购");
		} else {
			productSalesVolumeTextView.setText("已有" + jsonObject.optString("Owner", "0") + "人抢购");
		}
		float price_counted = Float.parseFloat(price);
		float price_previous = Float.parseFloat(oldPrice);
		int id = Integer.parseInt(productId);
		staff = new Staff(subtitte, name, imageUrl, 1, price_counted, price_previous, id);
	}
	private void loadProductAdvertisements(JSONObject jsonObject) {
		ArrayList<String> arrayList = new ArrayList<String>();
		String image0 = jsonObject.optString("Img", "");
		String image1 = jsonObject.optString("Img1", "");
		String image2 = jsonObject.optString("Img2", "");
		String image3 = jsonObject.optString("Img3", "");
		String image4 = jsonObject.optString("Img4", "");
		if (!image0.equals("null")) {
			arrayList.add(image0);
		} else {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		if (!image1.equals("null")) {
			arrayList.add(image1);
		} else {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		if (!image2.equals("null")) {
			arrayList.add(image2);
		} else {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		if (!image3.equals("null")) {
			arrayList.add(image3);
		} else {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		if (!image4.equals("null")) {
			arrayList.add(image4);
		} else {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		/*
		 * //如下广告做测试用途 for (int i = 0; i < 5; i++) {
		 * arrayList.add("20150629/20150629_224642_944_279.jpg"); }
		 */
		productAdvertisement.show(arrayList);
	}
	/**
	 * @Title: requestComment
	 * @Description: 获取评论
	 */
	private void requestComment() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONArray jsonArray = data.getMRootData().optJSONArray("Model");
					ArrayList<JSONObject> arrayList = JSONArrayConvertToArrayList.convertToArrayList(jsonArray);
					commentJsonObjects = arrayList;
					productCommentAmount.setText(commentJsonObjects.size() + "");
					/*
					 * if (jsonArray.length() >= 2) {
					 * addMoreCommentButton.setVisibility(View.VISIBLE); loadComment(arrayList, 2);
					 * } else if (jsonArray.length() == 1) {
					 * addMoreCommentButton.setVisibility(View.VISIBLE); loadComment(arrayList, 1);
					 * } else { return; }
					 */
					if (jsonArray.length() >= 1) {
						addMoreCommentButton.setVisibility(View.VISIBLE);
						loadComment(arrayList, 1);
					} else {
						return;
					}
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_comment)).addParam("ProductId", productId).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}
	private void loadProductAnimationPicture() {
		if (productImageURL != null && animationCartImageView != null) {
			if ((!productImageURL.equals("")) && (!productImageURL.equals("null"))) {
				/*ImageLoader.getInstance().displayImage(productImageURL, animationCartImageView,
						MyApplication.getOptions(), MyApplication.getLoadingListener());*/
				ImageLoader.getInstance().displayImage(productImageURL, animationCartImageView);
				animationCartImageView.setVisibility(View.VISIBLE);
				int[] cartLocation = new int[2];
				int[] pictureLocation = new int[2];
				cartImageView.getLocationInWindow(cartLocation);
				animationCartImageView.getLocationInWindow(pictureLocation);
				cartXPosition = cartLocation[0];
				cartYPosition = cartLocation[1];
				pictureXPosition = pictureLocation[0];
				pictureYPosition = pictureLocation[1];
				//animationCartImageView.setTag(productImageURL);
				AnimationSet animationSet = new AnimationSet(true);
				TranslateAnimation translateAnimationX = new TranslateAnimation(0,
						(cartXPosition - pictureXPosition - 70), 0, 0);
				TranslateAnimation translateAnimationY = new TranslateAnimation(0, 0, 0,
						(cartYPosition - pictureYPosition) - 50);
				translateAnimationX.setInterpolator(new LinearInterpolator());
				translateAnimationX.setRepeatCount(0);// 动画重复执行的次数
				translateAnimationX.setFillAfter(true);
				translateAnimationY.setInterpolator(new AccelerateInterpolator());
				translateAnimationY.setRepeatCount(0);// 动画重复执行的次数
				translateAnimationX.setFillAfter(true);
				RotateAnimation rotateAnimation = new RotateAnimation(0, 1080, Animation.RELATIVE_TO_SELF, 0.5f,
						Animation.RELATIVE_TO_SELF, 0.5f);
				Animation scaleAnimation = new ScaleAnimation(1, 0, 1, 0, Animation.RELATIVE_TO_SELF, 0.5f,
						Animation.RELATIVE_TO_SELF, 0.5f);
				translateAnimationX.setDuration(1500);
				translateAnimationY.setDuration(1000);
				scaleAnimation.setDuration(1500);
				rotateAnimation.setDuration(1500);
				// 添加动画
				// animationSet.setDuration(1500);
				animationSet.addAnimation(rotateAnimation);
				animationSet.addAnimation(scaleAnimation);
				animationSet.addAnimation(translateAnimationX);
				animationSet.addAnimation(translateAnimationY);
				/* animationSet.addAnimation(translateAnimation); */
				animationCartImageView.startAnimation(animationSet);
				animationSet.setAnimationListener(new AnimationListener() {
					@Override
					public void onAnimationStart(Animation animation) {
					}
					@Override
					public void onAnimationRepeat(Animation animation) {
					}
					@Override
					public void onAnimationEnd(Animation animation) {
						animationCartImageView.setVisibility(View.GONE);
					}
				});
			}
		}
	}
	public void popBackgroundAlpha(float bgAlpha) {
		WindowManager.LayoutParams layoutParams = getWindow().getAttributes();
		layoutParams.alpha = bgAlpha; //0.0-1.0
		getWindow().setAttributes(layoutParams);
	}

	class popDismissListener implements PopupWindow.OnDismissListener {
		@Override
		public void onDismiss() {
			popBackgroundAlpha(1.0f);
		}
	}
}
