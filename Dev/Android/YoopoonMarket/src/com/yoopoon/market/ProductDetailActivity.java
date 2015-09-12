package com.yoopoon.market;

import java.util.ArrayList;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.advertisement.ADController;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData.ResultState;
import android.app.ActionBar.LayoutParams;
import android.content.Context;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.util.Log;
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
	private TextView productPrimePriceTextView;
	//private ListView commentListView;
	//private PullToRefreshListView ptrListView;
	//评论对应的布局
	private LinearLayout commentLinearlayout;
	//添加更多评论对应的布局
	private static int count = 2;
	private ArrayList<JSONObject> jsonObjects;

	/*private LinearLayout commentItemLinearLayout;*/
	/*	private ImageView userPhotoImageView;
		private TextView userNickNameTextView;
		private TextView commentContentTextView;*/
	@AfterViews
	void initProductComment() {
		linearLayout = (LinearLayout) findViewById(R.id.linearlayout_product_detail);
		mADController = new ADController(mContext);
		//获取广告信息
		requestAdvertisements();
		//添加广告
		linearLayout.addView(mADController.getRootView(), 0);
		//设置原价特殊删除线
		productPrimePriceTextView = (TextView) findViewById(R.id.tv_product_prime_price);
		productPrimePriceTextView.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
		//货物评论对应LinearLayout
		commentLinearlayout = (LinearLayout) findViewById(R.id.linearlayout_product_comment);
		//addMoreCommentLinearLayout = (LinearLayout) findViewById(R.id.linearlayout_add_more_comment);
		/*commentItemLinearLayout = (LinearLayout) LayoutInflater.from(mContext).inflate(R.layout.item_product_comment,
				null);*/
		//###############################################################################
		//                      如下的代码只做API出来前的测试用途
		//###############################################################################
		jsonObjects = new ArrayList<JSONObject>();
		for (int i = 0; i < 20; i++) {
			JSONObject jsonObject = new JSONObject();
			try {
				jsonObject.put("nickName", "xuyanghui" + i);
				jsonObject.put("comment", "味道非常美味" + i);
			} catch (JSONException e) {
				e.printStackTrace();
			}
			jsonObjects.add(jsonObject);
		}
		loadComment(jsonObjects);
		//原使用pulltorefresh方法
		/*
		ptrListView = (PullToRefreshListView) findViewById(R.id.ptr_listview_product_comment);
		ptrListView.setMode(Mode.PULL_FROM_END);
		ptrListView.setOnRefreshListener(new RefreshListener());
		commentListView = ptrListView.getRefreshableView();
		Log.e("1111111111", jsonObjects.toString());
		commentAdapter = new ProductCommentListViewAdapter(ProductDetailActivity.this, jsonObjects);
		commentListView.setAdapter(commentAdapter);*/
		//###############################################################################
		//                      如上的代码只做API出来前的测试用途
		//###############################################################################
	}
	/**
	 * @Title: loadComment
	 * @Description: 循环加载产品评论
	 * @param jsonObjects
	 */
	private void loadComment(ArrayList<JSONObject> jsonObjects) {
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
		addMoreCommentButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				commentLinearlayout.removeAllViews();
				loadMoreComment();
			}
		});
		commentLinearlayout.addView(addMoreCommentButton, count);
	}
	/**
	 * @Title: loadMoreComment
	 * @Description: 每次增加2，控制需要显示的广告数量
	 */
	public void loadMoreComment() {
		Log.e("product", count + "");
		count += 2;
		loadComment(jsonObjects);
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
	/*	private class RefreshListener implements OnRefreshListener2<ListView> {
			@Override
			public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
				Toast.makeText(mContext, "pull down to refresh", Toast.LENGTH_SHORT).show();
			}
			@Override
			public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
				Toast.makeText(mContext, "pull up to refresh", Toast.LENGTH_SHORT).show();
			}
		}*/
}
