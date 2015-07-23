package com.yoopoon.home.ui.agent;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;

import com.yoopoon.home.IPocketActivity_;
import com.yoopoon.home.R;

public class CommentFunction implements OnClickListener {
	View rootView;
	Context mContext;
	LayoutInflater inflater;
	private TextView tv_ipoctket;
	private TextView tv_iguest;
	
	public View getRootView() {
		if (rootView == null) {
			initView();
		}
		return rootView;
	}
	public CommentFunction(Context context) {
		mContext = context;
		inflater = LayoutInflater.from(mContext);
	}
	private void initView() {
		rootView = inflater.inflate(R.layout.agent_comment_function_view, null);
		tv_ipoctket = (TextView) rootView.findViewById(R.id.tv_agent_ipocket);
		tv_iguest = (TextView) rootView.findViewById(R.id.tv_agent_iguest);
		initClickListener();
	}
	private void initClickListener() {
		tv_ipoctket.setOnClickListener(this);
		tv_iguest.setOnClickListener(this);
	}
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.tv_agent_ipocket:
				IPocketActivity_.intent(mContext).start();
				break;
			case R.id.tv_agent_iguest:
				Intent intent = new Intent("com.yoopoon.broker_takeguest");
				intent.addCategory(Intent.CATEGORY_DEFAULT);
				intent.putExtra("comeFromBroker", true);
				mContext.sendBroadcast(intent);
				break;
			default:
				break;
		}
	}
}
