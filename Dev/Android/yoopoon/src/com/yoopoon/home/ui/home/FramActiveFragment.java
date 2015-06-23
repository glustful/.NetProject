package com.yoopoon.home.ui.home;

import java.util.ArrayList;

import org.androidannotations.annotations.EFragment;
import org.json.JSONArray;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;
import com.yoopoon.home.ui.AD.ActiveController;

@EFragment()
public class FramActiveFragment extends FramSuper {

	View rootView;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater,
			@Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (null != rootView) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(
					R.layout.home_fram_active_fragment, null);

			listView = (PullToRefreshListView) rootView
					.findViewById(R.id.matter_list_view);
			instance = this;
			mContext = getActivity();
			mAdController = new ADController(mContext);
			mActiveController = new ActiveController(mContext);
			initViews();
		}

		return rootView;
	}

	static String TAG = "FramActivityFragment";

	ListView refreshView;
	Context mContext;
	public static FramActiveFragment instance;
	ADController mAdController;
	ActiveController mActiveController;
	PullToRefreshListView listView;

	public static FramActiveFragment getInstance() {
		return instance;
	}

	void initViews() {

		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.DISABLED);
		refreshView = listView.getRefreshableView();

		refreshView.addHeaderView(mAdController.getRootView());
		refreshView.addHeaderView(mActiveController.getRootView());

		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		ArrayAdapter<String> adapter = new ArrayAdapter<String>(mContext,
				android.R.layout.simple_list_item_1, new String[] { "房源列表" });
		refreshView.setAdapter(adapter);
		requestList();
		requestActiveList();
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

	@Override
	public String getTitle() {
		// TODO Auto-generated method stub
		return "活动";
	}

	void requestList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {
					ArrayList<String> imgs = new ArrayList<String>();
					JSONArray list = data.getJsonArray();
					if (list == null || list.length() < 1)
						return;
					for (int i = 0; i < list.length(); i++) {
						imgs.add(list.optJSONObject(i).optString("TitleImg"));
					}
					mAdController.show(imgs);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_channel_titleimg))
				.setRequestMethod(RequestMethod.eGet)

				.addParam("channelName", "banner").notifyRequest();
	}

	void requestActiveList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {
					
					JSONArray list = data.getJsonArray();
					if (list == null || list.length() < 1)
						return;
					
					mActiveController.show(list);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_channel_active_titleimg))
				.setRequestMethod(RequestMethod.eGet)

				.addParam("channelName", "活动").notifyRequest();
	}

}
