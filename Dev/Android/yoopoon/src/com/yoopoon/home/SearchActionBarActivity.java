package com.yoopoon.home;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.EActivity;
import org.json.JSONArray;
import org.json.JSONObject;

import android.annotation.SuppressLint;
import android.content.Context;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.text.format.DateUtils;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.ListView;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.SearchFunction.OnSearchCallBack;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.active.ActiveBrandAdapter;

@EActivity()
public abstract class SearchActionBarActivity extends ActionBarActivity {
	View headView;
	ActionBar actionBar;
	protected HashMap<String, String> SearchParameter;
	protected SearchFunction mSearchFunction;
	Context mContext;
	PullToRefreshListView listView;
	ListView refreshView;
	ActiveBrandAdapter mActiveBrandAdapter;
	protected FrameLayout rootView;
	ArrayList<JSONObject> mJsonObjects;

	@SuppressLint("InflateParams")
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mContext = this;
		actionBar = getSupportActionBar();
		initSearchParam();
		mSearchFunction = new SearchFunction(this, getString(R.string.url_brand_searchBrand));
		mSearchFunction.setParam(SearchParameter);
		mSearchFunction.setSearchCallBack(setSearchCallBack());
		headView = mSearchFunction.getSearchView();
		mJsonObjects = new ArrayList<JSONObject>();
		backShow();
		initViews();
	}
	private void backShow() {
		headView.setVisibility(View.VISIBLE);
		actionBar.setDisplayShowHomeEnabled(false);
		actionBar.setDisplayShowCustomEnabled(true);
		actionBar.setDisplayShowTitleEnabled(false);
		actionBar.setDisplayHomeAsUpEnabled(false);
		ActionBar.LayoutParams lp = new ActionBar.LayoutParams(android.view.ViewGroup.LayoutParams.MATCH_PARENT,
				android.view.ViewGroup.LayoutParams.MATCH_PARENT);
		lp.gravity = lp.gravity & ~Gravity.HORIZONTAL_GRAVITY_MASK | Gravity.LEFT;
		actionBar.setCustomView(headView, lp);
	}
	public ActionBar getMainActionBar() {
		return actionBar;
	}
	@Override
	public boolean dispatchTouchEvent(MotionEvent ev) {
		if (ev.getAction() == MotionEvent.ACTION_MOVE) {
			activityYMove();
		}
		return super.dispatchTouchEvent(ev);
	}
	@Override
	public boolean onTouchEvent(MotionEvent event) {
		return true;
	}
	protected void activityYMove() {
	}
	public void initSearchParam() {
		if (this.SearchParameter == null) {
			this.SearchParameter = new HashMap<String, String>();
		}
		this.SearchParameter.clear();
		this.SearchParameter.put("page", "1");
		this.SearchParameter.put("pageCount", "10");
		this.SearchParameter.put("condition", "");
	}
	public OnSearchCallBack setSearchCallBack() {
		return new OnSearchCallBack() {
			@Override
			public void textChange(Boolean isText) {
				if (!isText) {
					cleanSearch();
				}
			}
			@Override
			public void search(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list != null || list.length() > 0)
						for (int i = 0; i < list.length(); i++) {
							mJsonObjects.add(list.optJSONObject(i));
						}
					mActiveBrandAdapter.refresh(mJsonObjects);
					showResult(data.getMRootData().optJSONArray("List"));
				} else {
					ToastUtils.showToast(mContext, data.getMsg(), 3000);
				}
			}
			@Override
			public void deltext() {
				cleanSearch();
			}
			@Override
			public void clearRefresh() {
				cleanSearch();
			}
			@Override
			public void addMore(ResponseData data) {
				listView.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list != null || list.length() > 0) {
						for (int i = 0; i < list.length(); i++) {
							mJsonObjects.add(list.optJSONObject(i));
						}
						mActiveBrandAdapter.refresh(mJsonObjects);
						showResult(data.getMRootData().optJSONArray("List"));
					} else {
						ToastUtils.showToast(mContext, "数据已经加载完成", 3000);
						mSearchFunction.descCount();
					}
				} else {
					ToastUtils.showToast(mContext, data.getMsg(), 3000);
					mSearchFunction.descCount();
				}
			}
		};
	}
	void initViews() {
		rootView = (FrameLayout) LayoutInflater.from(mContext).inflate(R.layout.home_fram_active_fragment, null);
		listView = (PullToRefreshListView) rootView.findViewById(R.id.matter_list_view);
		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		refreshView = listView.getRefreshableView();
		// refreshView.setDividerHeight(5);
		refreshView.setEmptyView(rootView.findViewById(android.R.id.empty));
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mActiveBrandAdapter = new ActiveBrandAdapter(mContext);
		refreshView.setAdapter(mActiveBrandAdapter);
	}
	protected abstract void showResult(JSONArray optJSONArray);
	protected abstract int getHeight();
	protected abstract View getParentView();
	protected abstract void cleanSearch();

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(mContext, System.currentTimeMillis(), DateUtils.FORMAT_SHOW_TIME
					| DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
		}
		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			// mSearchFunction.pageCount++;
			mSearchFunction.search();
		}
	}

	public enum CurrentState {
		eManual, eSearch
	}
}
