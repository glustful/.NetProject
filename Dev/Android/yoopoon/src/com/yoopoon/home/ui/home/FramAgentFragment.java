package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.EFragment;
import org.json.JSONArray;
import org.json.JSONObject;

import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;
import com.yoopoon.home.ui.AD.ActiveController;
import com.yoopoon.home.ui.active.ActiveBrandAdapter;
import com.yoopoon.home.ui.agent.AgentBrandAdapter;
import com.yoopoon.home.ui.agent.CommentFunction;
import com.yoopoon.home.ui.agent.HeroController;
import com.yoopoon.home.ui.agent.HeroView;
import com.yoopoon.home.ui.agent.HeroView_;
import com.yoopoon.home.ui.agent.RichesView;
import com.yoopoon.home.ui.agent.RichesView_;
import com.yoopoon.home.ui.home.FramActiveFragment.HowWillIrefresh;

import android.content.Context;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
@EFragment()
public class FramAgentFragment extends FramSuper{

	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		// TODO Auto-generated method stub
		super.setUserVisibleHint(isVisibleToUser);
		if(isVisibleToUser && isFirst){
			isFirst = false;
			requestList();
			requestActiveList();
			requestBrandList();
		}
	}

	View rootView;
	HashMap<String,String> parameter;
	ArrayList<JSONObject> mJsonObjects;
	boolean isFirst = true;
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
					R.layout.home_fram_agent_fragment, null);

			listView = (PullToRefreshListView) rootView
					.findViewById(R.id.matter_list_view);
			instance = this;
			mJsonObjects = new ArrayList<JSONObject>();
			initParameter();
			mContext = getActivity();
			mAdController = new ADController(mContext);
			mActiveController = new ActiveController(mContext);
			mCommentFunction = new CommentFunction(mContext);
			mHeroController = new HeroController(mContext);
			mRichesView = RichesView_.build(mContext);
			mHeroView = HeroView_.build(mContext);
			initViews();
		}

		return rootView;
	}

	private void initParameter() {
		if(parameter == null){
			parameter = new HashMap<String, String>();
		}
		parameter.clear();
		parameter.put("page", "1");
		parameter.put("pageSize", "6");
		parameter.put("type", "all");
		
	}

	static String TAG = "FramActivityFragment";

	ListView refreshView;
	Context mContext;
	public static FramAgentFragment instance;
	ADController mAdController;
	ActiveController mActiveController;
	PullToRefreshListView listView;
	AgentBrandAdapter mAgentBrandAdapter;
	CommentFunction mCommentFunction;
	HeroController mHeroController;
	RichesView mRichesView;
	HeroView mHeroView;

	public static FramAgentFragment getInstance() {
		return instance;
	}

	void initViews() {

		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.DISABLED);
		refreshView = listView.getRefreshableView();
				
		refreshView.addHeaderView(mAdController.getRootView());
		refreshView.addHeaderView(mCommentFunction.getRootView());
		refreshView.addHeaderView(mActiveController.getRootView());
		mActiveController.addHeadView(mRichesView);
		refreshView.addHeaderView(mHeroController.getRootView());
		mHeroController.addHeadView(mHeroView);
		refreshView.addHeaderView(HeroView_.build(mContext).setText("推荐楼盘").setTextColor(getResources().getColor(R.color.yellow)));
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mAgentBrandAdapter = new AgentBrandAdapter(mContext);
		refreshView.setAdapter(mAgentBrandAdapter);
		
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
					if(list.length()>3){
					JSONArray tmp = new JSONArray();
					tmp.put(list.optJSONObject(0));
					tmp.put(list.optJSONObject(1));
					tmp.put(list.optJSONObject(2));
					mActiveController.show(tmp);
					}else{
						mActiveController.show(list);
					}
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
	
	private void requestBrandList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {

				if (data.getResultState() == ResultState.eSuccess) {
					
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1)
						return;
					for(int i=0;i<list.length();i++){
						mJsonObjects.add(list.optJSONObject(i));
					}
					mAgentBrandAdapter.refresh(mJsonObjects);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_brand_GetAllBrand))
				.setRequestMethod(RequestMethod.eGet)

				.addParam(parameter)
				.notifyRequest();
		
	}

	
}