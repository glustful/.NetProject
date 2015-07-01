package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.active.ActiveBrandAdapter;
import com.yoopoon.home.ui.home.FramActiveFragment.HowWillIrefresh;

@EFragment(R.layout.home_fram_house_fragment)
/*@EFragment()*/
public class FramHouseFragment extends FramSuper {
	public static final String LOGTAG = "FramHouseFragment";
	View rootView;
	HashMap<String, String> parameter;
	ArrayList<JSONObject> mJsonObjects;
	PullToRefreshListView listView;
	public static FramHouseFragment instance;

	public static FramHouseFragment getInstance() {
		return instance;
	}

	Context mContext;
	ListView refreshView;
	FramHouseListViewAdapter mHouseInfoAdapter;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater,
			@Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		Log.e(LOGTAG, "Fragment oncreateView");

		if (rootView != null) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(
					R.layout.home_fram_house_fragment, null);
			listView = (PullToRefreshListView) rootView
					.findViewById(R.id.matter_list_view);
			instance = this;
			mJsonObjects = new ArrayList<JSONObject>();
			initParameter();
			mContext = getActivity();
			initViews();
		}

		return rootView;
	}

	void initViews() {

		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.DISABLED);
		refreshView = listView.getRefreshableView();

		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mHouseInfoAdapter = new FramHouseListViewAdapter(mContext);
		refreshView.setAdapter(mHouseInfoAdapter);

		requestHouseList();
	}

	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}

		parameter.clear();
		parameter.put("AreaName", "");
		parameter.put("OrderBy", "OrderByAddtime");
		parameter.put("IsDescending", "true");
		parameter.put("Page", "1");
		parameter.put("PageCount", "10");
		parameter.put("PriceBegin", "");
		parameter.put("PriceEnd", "");
		parameter.put("TypeId", "");
	}

	@AfterInject
	void afterInject() {

	}

	private void requestHouseList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {

					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1)
						return;
					for (int i = 0; i < list.length(); i++) {
						mJsonObjects.add(list.optJSONObject(i));
					}
					mHouseInfoAdapter.refresh(mJsonObjects);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_product_search))
				.setRequestMethod(RequestMethod.eGet)

				.addParam(parameter).notifyRequest();

	}

	class HowWillIrefresh implements
			PullToRefreshBase.OnRefreshListener2<ListView> {

		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(getActivity(),
					System.currentTimeMillis(), DateUtils.FORMAT_SHOW_TIME
							| DateUtils.FORMAT_SHOW_DATE
							| DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);

		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {

		}

	}

}