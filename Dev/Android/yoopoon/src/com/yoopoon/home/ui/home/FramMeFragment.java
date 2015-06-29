package com.yoopoon.home.ui.home;

import org.androidannotations.annotations.EFragment;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.login.HomeLoginActivity_;
import com.yoopoon.home.ui.me.BrokerInfoView;
import com.yoopoon.home.ui.me.TodyTaskView;

@EFragment()
public class FramMeFragment extends FramSuper {
	static String tag = "FramMeFragment";
	@Override
	public void setUserVisibleHint(boolean isVisibleToUser) {
		// TODO Auto-generated method stub
		super.setUserVisibleHint(isVisibleToUser);
		this.isVisibleTouser = isVisibleToUser;
		if(isVisibleToUser&&(User.lastLoginUser(getActivity())==null || !User.lastLoginUser(getActivity()).isBroker())){
			
			HomeLoginActivity_.intent(getActivity())
			.isManual(true)
			.start();  
			return;
		}
		if(isVisibleToUser && isFirst){
			isFirst = false;
			Log.i(tag, "第一次刷新界面");
			requestBrokerInfo();
			requestTodayTask();
		}
	}
	@Override
	public void onResume(){
		super.onResume();
		if(isVisibleTouser){
			Log.i(tag, "返回时刷新界面");
			requestBrokerInfo();
			requestTodayTask();
		}
	}
	View rootView;
	boolean isFirst = true;
	boolean isVisibleTouser = false;
	TodyTaskView mTodayTaskView;
	BrokerInfoView mBrokerInfoView;
	TextView mTodayTaskCount;
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
					R.layout.home_fram_me_fragment, null);
			mTodayTaskCount = (TextView) rootView.findViewById(R.id.todayTaskCount);
			mBrokerInfoView = (BrokerInfoView) rootView.findViewById(R.id.brokerInfo);
			mTodayTaskView = (TodyTaskView) rootView.findViewById(R.id.todayTask);
			mTodayTaskView.addChildren(null);
		}

		return rootView;
	}
	
	void requestBrokerInfo(){
		new RequestAdapter() {
			
			@Override
			public void onReponse(ResponseData data) {
				if(data.getResultState()==ResultState.eSuccess){
					mBrokerInfoView.initData(data.getMRootData());
				}
				
			}
			
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
				
			}
		}.setUrl(getString(R.string.url_brokerInfo_getBrokerDetails))
		.setRequestMethod(RequestMethod.eGet)
		.notifyRequest();
	}
	void requestTodayTask(){
new RequestAdapter() {
			
			@Override
			public void onReponse(ResponseData data) {
				if(data.getResultState()==ResultState.eSuccess){
					if(data.getJsonArray()!=null){
						mTodayTaskCount.setText("今日任务("+data.getJsonArray().length() + ")");
					mTodayTaskView.addChildren(data.getJsonArray());
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
		}.setUrl(getString(R.string.url_task_taskListMobile))
		.setRequestMethod(RequestMethod.eGet)
		.addParam("page", "1")
		.addParam("type", "today")
		.notifyRequest();
	}
}
