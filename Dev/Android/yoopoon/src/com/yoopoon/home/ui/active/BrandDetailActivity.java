package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Bean;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.graphics.Color;
import android.util.Log;
import android.view.View;
import android.widget.LinearLayout;

import com.etsy.android.grid.StaggeredGridView;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

@EActivity(R.layout.brand_detail_view)
public class BrandDetailActivity extends MainActionBarActivity {
	private static final String TAG = "BrandDetailActivity";
	@Extra
	String mJson;
	@ViewById(R.id.grid_view)
	StaggeredGridView mListView;
	@ViewById(R.id.ll_progress)
	LinearLayout ll_progress;
	JSONObject jsonObject;
	@Bean
	BrandDetailAdapter mAdapter;
	BrandDetailHeaderView mBrandDetailHeaderView;
	int divider = 5;
	
	@AfterViews
	void initView() {
		backButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("品牌详情");
		mBrandDetailHeaderView = BrandDetailHeaderView_.build(this);
		try {
			jsonObject = new JSONObject(mJson);
			Log.i(TAG, jsonObject.toString());
			mBrandDetailHeaderView.init(jsonObject);
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		initRecyclerView();
		requestProductByBrandId();
	}
	private void requestProductByBrandId() {
		if (jsonObject == null)
			return;
		ll_progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData() != null) {
						JSONObject contentObj = data.getMRootData().optJSONObject("content");
						if (contentObj != null) {
							String content = contentObj.optString("Content", "");
							if (!StringUtils.isEmpty(content))
								mBrandDetailHeaderView.setContent(content);
						}
						JSONArray array = data.getMRootData().optJSONArray("productList");
						if (array != null) {
							mAdapter.setData(array);
						}
						ll_progress.setVisibility(View.GONE);
					}
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_product_GetProductsByBrand)).setRequestMethod(RequestMethod.eGet)
				.addParam("BrandId", jsonObject.optString("Id")).notifyRequest();
	}
	private void initRecyclerView() {
		mListView.addHeaderView(mBrandDetailHeaderView);
		mListView.setAdapter(mAdapter);
	}
	@Override
	public void backButtonClick(View v) {
		this.finish();
	}
	@Override
	public void titleButtonClick(View v) {
		// TODO Auto-generated method stub
	}
	@Override
	public void rightButtonClick(View v) {
		// TODO Auto-generated method stub
	}
	@Override
	public Boolean showHeadView() {
		// TODO Auto-generated method stub
		return true;
	}
}
