package com.yoopoon.market;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONObject;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.handmark.pulltorefresh.library.PullToRefreshBase.OnRefreshListener2;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.utils.JSONArrayConvertToArrayList;
import com.yoopoon.view.adapter.ProductCommentAdapter;

import android.content.Context;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.view.View;
import android.widget.ListView;
import android.widget.Toast;

@EActivity(R.layout.activity_product_comment)
public class ProductCommentActivity extends MainActionBarActivity {
	private String productId;
	private String commentAmount;
	private ProductCommentAdapter mProductCommentAdapter;
	private Context mContext;
	private PullToRefreshListView ptrListView;
	private ListView commentListView;
	@ViewById(R.id.ptr_listview_comment)
	PullToRefreshListView mPullToRefreshListView;
	private int commentPage = 1;

	@AfterViews
	void initProductComment() {
		productId = getIntent().getExtras().getString("productId");
		commentAmount = getIntent().getExtras().getString("commentAmount");
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backWhiteButton.setText("后退");
		backWhiteButton.setTextColor(Color.WHITE);
		headView.setBackgroundColor(Color.RED);
		titleButton.setText("评价" + "(" + commentAmount + ")");
		titleButton.setTextColor(Color.WHITE);
		rightButton.setVisibility(View.GONE);
		mContext = ProductCommentActivity.this;
		//获取控件
		mPullToRefreshListView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		mPullToRefreshListView.setOnRefreshListener(new RefreshListener());
		commentListView = mPullToRefreshListView.getRefreshableView();
		//加载评论
		requestComment();
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
	/**
	 * @Title: requestComment
	 * @Description: 获取评论
	 */
	private void requestComment() {
		HashMap<String, String> hashMap = new HashMap<String, String>();
		hashMap.put("ProductId", productId);
		hashMap.put("Page", "1");
		hashMap.put("PageCount", "10");
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONArray jsonArray = data.getMRootData().optJSONArray("Model");
					ArrayList<JSONObject> arrayList = JSONArrayConvertToArrayList.convertToArrayList(jsonArray);
					mProductCommentAdapter = new ProductCommentAdapter(mContext, arrayList);
					commentListView.setAdapter(mProductCommentAdapter);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_comment)).addParam(hashMap).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}
	private void requestComment(HashMap<String, String> hashMap) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				mPullToRefreshListView.onRefreshComplete();
				if (data.getMRootData() != null) {
					JSONArray jsonArray = data.getMRootData().optJSONArray("Model");
					ArrayList<JSONObject> arrayList = JSONArrayConvertToArrayList.convertToArrayList(jsonArray);
					if (arrayList.size() >= 1) {
						mProductCommentAdapter.refresh(arrayList);
					}
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_comment)).addParam(hashMap).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	private class RefreshListener implements OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			Toast.makeText(mContext, "pull down to refresh", Toast.LENGTH_SHORT).show();
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			HashMap<String, String> hashMap = new HashMap<String, String>();
			hashMap.put("ProductId", productId);
			hashMap.put("Page", (++commentPage) + "");
			hashMap.put("PageCount", "10");
			requestComment(hashMap);
		}
	}
}
