package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;
import org.androidannotations.annotations.EFragment;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.common.base.utils.NetworkUtils;
import com.yoopoon.common.base.utils.SPUtils;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.AD.ADController;
import com.yoopoon.home.ui.active.ActiveController;
import com.yoopoon.home.ui.agent.AgentBrandAdapter;
import com.yoopoon.home.ui.agent.CommentFunction;
import com.yoopoon.home.ui.agent.HeroController;
import com.yoopoon.home.ui.agent.HeroView;
import com.yoopoon.home.ui.agent.HeroView_;
import com.yoopoon.home.ui.agent.RichesView;
import com.yoopoon.home.ui.agent.RichesView_;

@EFragment()
public class FramAgentFragment extends FramSuper implements OnClickListener {
	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		super.setUserVisibleHint(isVisibleToUser);

		if (isVisibleToUser) {
			final User user = User.lastLoginUser(getActivity());
			if (user == null || (!user.isBroker() && !SPUtils.isBroker(getActivity()))) {
				isFirst = true;
				final String text = (user == null) ? "亲，你还没登录呢" : "亲，你还不是经纪人哦";
				handler.postDelayed(new Runnable() {

					@Override
					public void run() {
						Intent intent = new Intent("com.yoopoon.OPEN_ME_ACTION");
						intent.addCategory(Intent.CATEGORY_DEFAULT);
						getActivity().sendBroadcast(intent);
						Toast.makeText(getActivity(), text, Toast.LENGTH_SHORT).show();
					}
				}, 100);
				// FramMainActivity_.intent(getActivity()).start();
			} else if (isFirst) {
				isFirst = false;
				requestList();
				requestActiveList();
				requestBrandList();
				requestHeroList();
			}
		}

	}

	View rootView;
	HashMap<String, String> parameter;
	ArrayList<JSONObject> mJsonObjects;
	boolean isFirst = true;
	Handler handler = new Handler();

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (null != rootView) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_agent_fragment, null);
			listView = (PullToRefreshListView) rootView.findViewById(R.id.matter_list_view);
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
			registerReceiver();
		}
		return rootView;
	}

	private void registerReceiver() {
		IntentFilter filter = new IntentFilter(ConnectivityManager.CONNECTIVITY_ACTION);
		getActivity().registerReceiver(receiver, filter);
	}

	private BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			if (NetworkUtils.isNetworkConnected(context)) {
				requestList();
				requestActiveList();
				requestBrandList();
				requestHeroList();
			}
		}
	};

	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}
		parameter.clear();
		parameter.put("page", "1");
		parameter.put("pageSize", "6");
		parameter.put("type", "all");
	}

	static String TAG = "FramAgentFragment";
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
		refreshView.addHeaderView(HeroView_.build(mContext).setText("推荐楼盘")
				.setTextColor(getResources().getColor(R.color.yellow)));
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mAgentBrandAdapter = new AgentBrandAdapter(mContext);
		refreshView.setAdapter(mAgentBrandAdapter);
		initMCommonFunctions();
	}

	private void initMCommonFunctions() {
	}

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(getActivity(), System.currentTimeMillis(),
					DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
		}
	}

	private ArrayList<String> imgs;

	void requestList() {
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
							mAdController.show(imgs);
						}
					}
				}

				@Override
				public void onProgress(ProgressMessage msg) {
					// TODO Auto-generated method stub
				}
			}.setUrl(getString(R.string.url_channel_titleimg)).setRequestMethod(RequestMethod.eGet)
					.addParam("channelName", "banner").notifyRequest();
	}

	private ArrayList<JSONArray> activeDataSource;

	void requestActiveList() {
		if (activeDataSource == null)
			new RequestAdapter() {

				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						if (activeDataSource == null) {
							activeDataSource = new ArrayList<JSONArray>();
							JSONArray list = data.getJsonArray();
							if (list == null || list.length() < 1)
								return;
							if (list.length() > 3) {
								JSONArray tmp = new JSONArray();
								tmp.put(list.optJSONObject(0));
								tmp.put(list.optJSONObject(1));
								tmp.put(list.optJSONObject(2));
								activeDataSource.add(tmp);
								mActiveController.show2(activeDataSource);
							} else {
								activeDataSource.add(list);
								mActiveController.show2(activeDataSource);
							}
						}
					}
				}

				@Override
				public void onProgress(ProgressMessage msg) {
					// TODO Auto-generated method stub
				}
			}.setUrl(getString(R.string.url_channel_active_titleimg)).setRequestMethod(RequestMethod.eGet)
					.addParam("channelName", "活动").notifyRequest();
	}

	private void requestBrandList() {
		if (mJsonObjects.size() == 0)
			initParameter();
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1)
						return;
					mJsonObjects.clear();
					for (int i = 0; i < list.length(); i++) {
						Log.i(TAG, list.optJSONObject(i).toString());
						mJsonObjects.add(list.optJSONObject(i));
					}
					mAgentBrandAdapter.refresh(mJsonObjects);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_brand_getOneBrand)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	private ArrayList<JSONArray> heroArrays;

	private void requestHeroList() {
		if (heroArrays == null)
			new RequestAdapter() {

				@Override
				public void onReponse(ResponseData data) {
					if (data.getResultState() == ResultState.eSuccess) {
						if (data.getMRootData() != null) {
							if (heroArrays == null) {
								JSONObject obj = data.getMRootData();
								try {
									JSONArray array = obj.getJSONArray("List");
									heroArrays = new ArrayList<JSONArray>();
									heroArrays.add(array);
									mHeroController.show(heroArrays);
								} catch (JSONException e) {
									// TODO Auto-generated catch block
									e.printStackTrace();
								}
							}

						}
					}

				}

				@Override
				public void onProgress(ProgressMessage msg) {
					// TODO Auto-generated method stub

				}
			}.setUrl(getString(R.string.url_fortune_hero)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	/*
	 * (non Javadoc)
	 * @Title: onResume
	 * @Description: TODO
	 * @see android.support.v4.app.Fragment#onResume()
	 */
	@Override
	public void onResume() {

		super.onResume();
	}

	/*
	 * (non Javadoc)
	 * @Title: onStart
	 * @Description: TODO
	 * @see android.support.v4.app.Fragment#onStart()
	 */
	@Override
	public void onStart() {
		super.onStart();
	}

	/*
	 * (non Javadoc)
	 * @Title: onClick
	 * @Description: TODO
	 * @param v
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
	@Override
	public void onClick(View v) {
		// TODO Auto-generated method stub
	}
}
