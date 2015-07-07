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
import android.annotation.SuppressLint;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import com.round.progressbar.CircleProgressDialog;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.login.HomeLoginActivity_;
import com.yoopoon.home.ui.me.BrokerInfoView;
import com.yoopoon.home.ui.me.MeFooterView;
import com.yoopoon.home.ui.me.TodyTaskView;

/**
 * @ClassName: FramMeFragment
 * @Description: 个人中心fragment
 * @author: guojunjun
 * @date: 2015-7-7 下午2:52:19
 */
@SuppressLint("InflateParams")
@EFragment()
public class FramMeFragment extends FramSuper {
	// [start] field
	static String tag = "FramMeFragment";
	private View rootView;
	/**
	 * @fieldName: isFirst
	 * @fieldType: boolean
	 * @Description: 是否是第一次对用户可见，见方法setUserVisibleHint
	 */
	private boolean isFirst = true;
	private boolean isVisibleTouser = false;
	private TodyTaskView mTodayTaskView;
	private BrokerInfoView mBrokerInfoView;
	private TextView mTodayTaskCount;
	private MeFooterView mMeFooterView;

	// [end]

	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {

		super.setUserVisibleHint(isVisibleToUser);
		this.isVisibleTouser = isVisibleToUser;
		if (isVisibleToUser && (User.lastLoginUser(getActivity()) == null)) {
			isFirst = false;
			HomeLoginActivity_.intent(getActivity()).isManual(true).start();
			return;
		}
		if (isVisibleToUser && isFirst) {
			isFirst = false;

			requestBrokerInfo();
			requestTodayTask();
		}
	}

	@Override
	public void onResume() {
		super.onResume();
		if (isVisibleTouser && !isFirst) {
			if (User.lastLoginUser(getActivity()) != null) {
				requestBrokerInfo();
				requestTodayTask();
			} else {
				cleanLayout();
			}
		}
	}

	/**
	 * @Title: cleanLayout
	 * @Description: 用户未登陆，清除相关数据
	 */
	private void cleanLayout() {
		mBrokerInfoView.hide();
		mTodayTaskCount.setText("今日 任务（无）");
		mTodayTaskView.setVisibility(View.GONE);
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
			mTodayTaskCount = (TextView) rootView.findViewById(R.id.todayTaskCount);
			mBrokerInfoView = (BrokerInfoView) rootView.findViewById(R.id.brokerInfo);
			mTodayTaskView = (TodyTaskView) rootView.findViewById(R.id.todayTask);
			mMeFooterView = (MeFooterView) rootView.findViewById(R.id.footerView);
		}

		return rootView;
	}

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
					mBrokerInfoView.initData(data.getMRootData(), User.lastLoginUser(getActivity()).isBroker());
					mMeFooterView.show(User.lastLoginUser(getActivity()).isBroker());
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_brokerInfo_getBrokerDetails)).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	/**
	 * @Title: requestTodayTask
	 * @Description: 获取今日任务列表
	 */
	void requestTodayTask() {
		new RequestAdapter() {

			/**
			 * @fieldName: serialVersionUID
			 * @fieldType: long
			 * @Description: TODO
			 */
			private static final long serialVersionUID = 1L;

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData().optJSONArray("list") != null) {
						mTodayTaskCount.setText("今日任务(" + data.getMRootData().optJSONArray("list").length() + ")");
						mTodayTaskView.addChildren(data.getMRootData().optJSONArray("list"));
						mTodayTaskView.setVisibility(View.VISIBLE);
						return;
					}
				}
				mTodayTaskCount.setText("今日任务(无)");
				mTodayTaskView.setVisibility(View.GONE);

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_task_taskListMobile)).setRequestMethod(RequestMethod.eGet)
				.addParam("page", "1").addParam("type", "today").notifyRequest();
	}
}
