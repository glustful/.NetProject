package com.yoopoon.market;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.market.R.string;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData.ResultState;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;

import android.app.ActionBar.LayoutParams;
import android.content.Context;
import android.content.Intent;
import android.database.DataSetObservable;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

@EActivity(R.layout.activity_product_detail)
public class ProductDetailActivity extends MainActionBarActivity {
	private ADController mADController;
	private ArrayList<String> imgs;
	private LinearLayout linearLayout;
	private Context mContext;
	private String productId;
	//评论对应的布局
	private LinearLayout commentLinearlayout;
	//添加更多评论对应的布局
	private static int count = 2;
	private ArrayList<JSONObject> jsonObjects;
	//商品详细信息对应的布局视图
	private TextView productTitleTextView;
	private TextView productSubtitleTextView;
	private TextView productPrictTextView;
	private TextView productPrimePriceTextView;
	private TextView productSalesVolumeTextView;

	/**
	 * @Title: initProductComment
	 * @Description: 初始化商品详细信息界面
	 */
	@AfterViews
	void initProductDetail() {
		//获取从首页过来的id
		productId = getIntent().getExtras().getString("productId");
		linearLayout = (LinearLayout) findViewById(R.id.linearlayout_product_detail);
		mADController = new ADController(mContext);
		//获取广告信息
		//添加广告
		linearLayout.addView(mADController.getRootView(), 0);
		//初始化商品详细信息视图
		productTitleTextView = (TextView) findViewById(R.id.tv_product_title);
		productSubtitleTextView = (TextView) findViewById(R.id.tv_product_subtitle);
		productPrictTextView = (TextView) findViewById(R.id.tv_product_selling_price);
		productPrimePriceTextView = (TextView) findViewById(R.id.tv_product_prime_price);
		productSalesVolumeTextView = (TextView) findViewById(R.id.tv_product_sales_volume);
		//设置删除线
		productPrimePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		//货物评论对应LinearLayout
		commentLinearlayout = (LinearLayout) findViewById(R.id.linearlayout_product_comment);
		requestProductDetail();
	}
	/**
	 * @Title: loadComment
	 * @Description: 循环加载产品评论
	 * @param jsonObjects
	 */
	private void loadComment(ArrayList<JSONObject> jsonObjects, Boolean commentNullStatus) {
		//判断商品是否有评论，如果没有commentNullStatus为true
		if (commentNullStatus) {
			count = 1;
		}
		for (int i = 0; i < count; i++) {
			JSONObject jsonObject = jsonObjects.get(i);
			LinearLayout commentItemLinearLayout = (LinearLayout) LayoutInflater.from(mContext).inflate(
					R.layout.item_product_comment, null);
			ImageView userPhotoImageView = (ImageView) commentItemLinearLayout
					.findViewById(R.id.img_comment_user_photo);
			TextView userNickNameTextView = (TextView) commentItemLinearLayout
					.findViewById(R.id.tv_comment_user_nickname);
			TextView commentContentTextView = (TextView) commentItemLinearLayout.findViewById(R.id.tv_comment_content);
			String url = "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg";
			userPhotoImageView.setTag(url);
			ImageLoader.getInstance().displayImage(url, userPhotoImageView, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			userNickNameTextView.setText(jsonObject.optString("nickName"));
			commentContentTextView.setText(jsonObject.optString("comment"));
			commentLinearlayout.addView(commentItemLinearLayout, i);
		}
		Button addMoreCommentButton = new Button(mContext);
		android.view.ViewGroup.LayoutParams params = new LayoutParams(android.view.ViewGroup.LayoutParams.MATCH_PARENT,
				LayoutParams.WRAP_CONTENT);
		addMoreCommentButton.setLayoutParams(params);
		addMoreCommentButton.setText("点击加载更多评论");
		addMoreCommentButton.setBackgroundColor(Color.TRANSPARENT);
		//如果只有默认评论的话隐藏加载更多
		if (commentNullStatus) {
			addMoreCommentButton.setVisibility(View.GONE);
		}
		if (commentNullStatus) {
			addMoreCommentButton.setVisibility(View.VISIBLE);
		}
		addMoreCommentButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				commentLinearlayout.removeAllViews();
				loadMoreComment();
			}
		});
		//添加滑动获取更多图文详情
		int sceenWidth = this.getWindowManager().getDefaultDisplay().getWidth();
		LinearLayout linearLayout = new LinearLayout(mContext);
		android.view.ViewGroup.LayoutParams linearLayoutLayoutParams = new android.view.ViewGroup.LayoutParams(
				LinearLayout.LayoutParams.MATCH_PARENT, 150);
		linearLayout.setLayoutParams(linearLayoutLayoutParams);
		linearLayout.setOrientation(LinearLayout.HORIZONTAL);
		//
		View view = new View(mContext);
		android.view.ViewGroup.LayoutParams viewLayoutParams = new android.view.ViewGroup.LayoutParams(
				Integer.parseInt(sceenWidth / 4 + ""), 2);
		view.setLayoutParams(viewLayoutParams);
		view.setBackgroundColor(Color.BLACK);
		linearLayout.addView(view, 0);
		//
		TextView addMoreTextView = new TextView(mContext);
		addMoreTextView.setBackgroundColor(Color.TRANSPARENT);
		addMoreTextView.setText("继续拖动，查看图文详情");
		android.view.ViewGroup.LayoutParams buttonLayoutParams = new android.view.ViewGroup.LayoutParams(
				android.view.ViewGroup.LayoutParams.WRAP_CONTENT, android.view.ViewGroup.LayoutParams.MATCH_PARENT);
		addMoreTextView.setLayoutParams(buttonLayoutParams);
		addMoreTextView.setGravity(Gravity.CENTER_VERTICAL);
		linearLayout.addView(addMoreTextView, 1);
		//
		View view2 = new View(mContext);
		android.view.ViewGroup.LayoutParams view2Params = new android.view.ViewGroup.LayoutParams(
				Integer.parseInt(sceenWidth / 4 + ""), 2);
		view2.setLayoutParams(view2Params);
		view2.setBackgroundColor(Color.BLACK);
		linearLayout.addView(view2);
		linearLayout.setGravity(Gravity.CENTER);
		linearLayout.setBackgroundColor(Color.rgb(228, 228, 228));
		commentLinearlayout.addView(addMoreCommentButton, count);
		commentLinearlayout.addView(linearLayout, count + 1);
	}
	/**
	 * @Title: loadMoreComment
	 * @Description: 每次增加2，控制需要显示的广告数量
	 */
	public void loadMoreComment() {
		Log.e("product", count + "");
		count += 2;
		loadComment(jsonObjects, true);
	}
	@Override
	protected void onPause() {
		super.onPause();
		count = 2;
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
				JSONArray array = data.getMRootData().optJSONArray("Comments");
				JSONObject productJsonObject = data.getMRootData().optJSONObject("ProductModel");
				if (array != null && array.length() >= 1) {
					loadComment(JSONArrayConvertToArrayList.convertToArrayList(array), true);
				}
				//设置商品详细信息
				initProductDetailInfo(productJsonObject);
				//设置广告,同事加载广告
				loadProductAdvertisements(productJsonObject);
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
		productTitleTextView.setText(jsonObject.optString("Name", ""));
		productSubtitleTextView.setText(jsonObject.optString("Subtitte", ""));
		productPrictTextView.setText("￥" + jsonObject.optString("Price", "0.00"));
		productPrimePriceTextView.setText("原价：" + jsonObject.optString("NewPrice", "0.00"));
		productSalesVolumeTextView.setText("已有" + jsonObject.optString("Owner", "0") + "人抢购");
	}
	private void loadProductAdvertisements(JSONObject jsonObject) {
		ArrayList<String> arrayList = new ArrayList<String>();
		String image0 = jsonObject.optString("Img", "");
		String image1 = jsonObject.optString("Img1", "");
		String image2 = jsonObject.optString("Img2", "");
		String image3 = jsonObject.optString("Img3", "");
		String image4 = jsonObject.optString("Img4", "");
		/*if (!image0.equals("null")) {
			arrayList.add("http://img.iyookee.cn/20150629/20150629_224642_944_279.jpg");
		}
		if (!image1.equals("null")) {
			arrayList.add("http://img.iyookee.cn/20150629/20150629_224642_944_279.jpg");
		}
		if (!image2.equals("null")) {
			arrayList.add("http://img.iyookee.cn/20150629/20150629_224642_944_279.jpg");
		}
		if (!image3.equals("null")) {
			arrayList.add(image3);
		}
		if (!image4.equals("null")) {
			arrayList.add(image4);
		}*/
		//如下广告做测试用途
		for (int i = 0; i < 3; i++) {
			arrayList.add("20150629/20150629_224642_944_279.jpg");
		}
		mADController.show(arrayList);
	}
}
