/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MeFooterView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午2:14:07 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.home;

import org.androidannotations.annotations.EFragment;
import org.json.JSONException;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.TextView;
import android.widget.Toast;
import com.round.progressbar.CircleProgressDialog;
import com.yoopoon.home.MeTaskActivity_;
import com.yoopoon.home.R;
import com.yoopoon.home.RecBuildActivity_;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.login.HomeLoginActivity_;
import com.yoopoon.home.ui.me.BrokerInfoView;
import com.yoopoon.home.ui.me.MeFooterView;

/**
 * @ClassName: FramMeFragment
 * @Description: 个人中心fragment
 * @author: guojunjun
 * @date: 2015-7-7 下午2:52:19
 */
@SuppressLint("InflateParams")
@EFragment()
public class FramMeFragment extends FramSuper implements OnClickListener {
	// [start] field
	static String TAG = "FramMeFragment";
	private View rootView;
	/**
	 * @fieldName: isFirst
	 * @fieldType: boolean
	 * @Description: 是否是第一次对用户可见，见方法setUserVisibleHint
	 */
	private BrokerInfoView mBrokerInfoView;
	private MeFooterView mMeFooterView;
	private boolean isBroker = false;
	private String userId = "0";
	private boolean isVisibleToUser = false;
	private int clientCount = 0;
	private TextView tv_task;
	private TextView tv_rec;

	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		super.setUserVisibleHint(isVisibleToUser);
		this.isVisibleToUser = isVisibleToUser;
		Log.i(TAG, "setUserVisibleHint:::isVisibleToUser = " + isVisibleToUser);
		if (isVisibleToUser) {
			User user = User.lastLoginUser(getActivity());
			if (user == null) {
				HomeLoginActivity_.intent(getActivity()).isManual(true).start();
				return;
			} else {
				initUserData();
			}
		}
	}

	@Override
	public void onResume() {
		super.onResume();
		if (isVisibleToUser) {
			initUserData();
		}
	}

	/*
	 * (non Javadoc)
	 * @Title: onStop
	 * @Description: TODO
	 * @see android.support.v4.app.Fragment#onStop()
	 */
	@Override
	public void onStop() {
		super.onStop();
	}

	private void initUserData() {
		SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(getActivity());
		isBroker = sp.getBoolean("isBroker", false);
		userId = sp.getString("userId", "0");
		if ("0".equals(userId)) {
			cleanLayout();
		} else {
			requestClientCount();
		}
	}

	/**
	 * @Title: cleanLayout
	 * @Description: 用户未登陆，清除相关数据
	 */
	private void cleanLayout() {
		mBrokerInfoView.hide();
		mMeFooterView.hide();
	}

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (null != rootView) {
			ViewGroup parent = (ViewGroup) rootView.getParent();
			if (null != parent) {
				parent.removeView(rootView);
			}
		} else {
			rootView = LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_me_fragment, null);
			mBrokerInfoView = (BrokerInfoView) rootView.findViewById(R.id.brokerInfo);
			mMeFooterView = (MeFooterView) rootView.findViewById(R.id.footerView);
			tv_rec = (TextView) rootView.findViewById(R.id.tv_me_building);
			tv_task = (TextView) rootView.findViewById(R.id.tv_me_task);

			tv_rec.setOnClickListener(this);
			tv_task.setOnClickListener(this);

		}
		registerLoginReceiver();
		return rootView;
	}

	private void registerLoginReceiver() {
		IntentFilter filter = new IntentFilter("com.yoopoon.login_action");
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		getActivity().registerReceiver(loginReceiver, filter);
	}

	private void requestClientCount() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data != null) {
					if (data.getResultState() == ResultState.eSuccess) {
						JSONObject dataObj = data.getMRootData();
						try {
							clientCount = dataObj.getInt("totalCount");
						} catch (JSONException e) {
							e.printStackTrace();
						}
					} else {
						HomeLoginActivity_.intent(getActivity()).isManual(true).start();
					}
				} else {
					Toast.makeText(getActivity(), "获取客户信息失败！", Toast.LENGTH_SHORT).show();
				}
				requestBrokerInfo();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_get_my_clients) + "/?page=1&pageSize=10").setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	private BroadcastReceiver loginReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			initUserData();
		}
	};

	/**
	 * @Title: requestBrokerInfo
	 * @Description: 请求当前用户相关信息
	 */
	void requestBrokerInfo() {
		CircleProgressDialog.build(getActivity(), R.style.dialog).show();
		new RequestAdapter() {
			/**
			 * @fieldName: serialVersionUID
			 * @fieldType: long
			 * @Description:
			 */
			private static final long serialVersionUID = 1L;

			@Override
			public void onReponse(ResponseData data) {
				CircleProgressDialog.build(getActivity(), R.style.dialog).hide();
				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData() != null) {
						mBrokerInfoView.initData(data.getMRootData(), isBroker, clientCount);
						mMeFooterView.show(isBroker);
					}
				} else {
					HomeLoginActivity_.intent(getActivity()).isManual(true).start();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_brokerInfo_getBrokerDetails)).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
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
		switch (v.getId()) {
			case R.id.tv_me_building:
				RecBuildActivity_.intent(this).start();
				break;
			case R.id.tv_me_task:
				MeTaskActivity_.intent(this).start();
				break;
			default:
				break;
		}
	}
}
