package com.yoopoon.home.ui.home;

import java.util.ArrayList;
import java.util.HashMap;
import org.androidannotations.annotations.EFragment;
import org.json.JSONArray;
import org.json.JSONObject;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.ConnectivityManager;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.TextView;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.common.base.utils.NetworkUtils;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.AD.ADController;
import com.yoopoon.home.ui.active.ActiveBrandAdapter;
import com.yoopoon.home.ui.active.ActiveController;

@EFragment()
public class FramActiveFragment extends FramSuper {
	View rootView;
	HashMap<String, String> parameter;
	ArrayList<JSONObject> mJsonObjects;
	String str = "<p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">景观资源配套——</span><span style=\";font-family:微软雅黑;font-size:27px\">五甲塘生态公园、西亮塘湿地公园、世纪金源中心公园三大城市中心生态（湿地）公园，草海长堤、海埂公园、民族村等城市人文地标环伺周边</span></p><p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">体育休闲配套——</span><span style=\";font-family:微软雅黑;font-size:27px\">红塔体育中心、海埂训练基地、滇池高尔夫、强林高尔夫练习场、张松涛体育中心、滇池春天温泉会馆、袁晓岑博物馆等休闲体育资源让您随时强身健体；</span></p><p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">医疗资源配套——</span><span style=\";font-family:微软雅黑;font-size:27px\">项目周边的昆明同仁医院、儿童医院、在建中的省中医院，昆明广福路医院，昆明圣安妇产医院等大中型医院都将为您及家人的健康保驾护航</span></p><p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">生活配套——</span><span style=\";font-family:微软雅黑;font-size:27px\">项目周边聚集的众多成熟生活社区及大型专业卖场，为您提供了各种便利。步行10分钟内就可到达沃尔玛、广福路商业美食街，还有家乐福、邦盛酒店市场、大商汇建材市场等大型百货市场；</span></p><p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">金融配套——</span><span style=\";font-family:微软雅黑;font-size:27px\">项目半径一公里范围内有工商银行、富滇银行、农业银行、农村信用合用社、招商银行、建设银行、中国银行、浦发银行、国家开发银行、交通银行、邮政储蓄银行；</span></p><p style=\"margin-right:0;margin-left:0;text-indent:53px;text-autospace:ideograph-numeric;text-align:justify;text-justify:inter-ideograph;line-height:150%\"><span style=\";font-family:微软雅黑;font-weight:bold;font-size:27px\">交通配套</span><span style=\";font-family:微软雅黑;font-size:27px\">——前兴路公交枢纽站与项目比邻，本项目周边内拥有多条直达城市东西南本的公交线路</span></p><p><br/></p>";
	TextView tv_network;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (null != rootView) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_active_fragment, null);
			listView = (PullToRefreshListView) rootView.findViewById(R.id.matter_list_view);
			tv_network = (TextView) rootView.findViewById(R.id.tv_active_network);
			instance = this;
			mJsonObjects = new ArrayList<JSONObject>();
			initParameter();
			mContext = getActivity();
			mAdController = new ADController(mContext);
			mActiveController = new ActiveController(mContext);

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

	private void autoIncreatePage() {
		parameter.put("page", (Integer.parseInt(parameter.get("page")) + 1) + "");
	}

	static String TAG = "FramActivityFragment";
	ListView refreshView;
	Context mContext;
	public static FramActiveFragment instance;
	ADController mAdController;
	ActiveController mActiveController;
	PullToRefreshListView listView;
	ActiveBrandAdapter mActiveBrandAdapter;

	public static FramActiveFragment getInstance() {
		return instance;
	}

	void initViews() {
		listView.setOnRefreshListener(new HowWillIrefresh());
		listView.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		refreshView = listView.getRefreshableView();
		// refreshView.setDividerHeight(5);
		refreshView.addHeaderView(mAdController.getRootView());
		refreshView.addHeaderView(mActiveController.getRootView());
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		mActiveBrandAdapter = new ActiveBrandAdapter(mContext);
		refreshView.setAdapter(mActiveBrandAdapter);
		requestList();
		requestActiveList();
		requestBrandList();
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
			autoIncreatePage();
			requestBrandList();
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
		}.setUrl(getString(R.string.url_channel_titleimg)).setRequestMethod(RequestMethod.eGet)
				.addParam("channelName", "banner").notifyRequest();
	}

	void requestActiveList() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					ArrayList<JSONArray> dataSource = new ArrayList<JSONArray>();
					JSONArray list = data.getJsonArray();
					if (list == null || list.length() < 1)
						return;
					dataSource.add(list);
					mActiveController.show(dataSource);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_channel_active_titleimg)).setRequestMethod(RequestMethod.eGet)
				.addParam("ChannelName", "活动").notifyRequest();
	}

	private void requestBrandList() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				listView.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1) {
						ToastUtils.showToast(mContext, "亲，已经没有更多的品牌啦！", 3000);
						descCount();
						return;
					}
					for (int i = 0; i < list.length(); i++) {
						mJsonObjects.add(list.optJSONObject(i));
					}
					mActiveBrandAdapter.refresh(mJsonObjects);
				} else {
					descCount();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_brand_GetAllBrand)).setRequestMethod(RequestMethod.eGet).addParam(parameter)
				.notifyRequest();
	}

	protected void descCount() {
		int page = Integer.parseInt(parameter.get("page"));
		page = page > 1 ? page - 1 : 1;
		parameter.put("page", page + "");
	}
}
