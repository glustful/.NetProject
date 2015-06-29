package com.yoopoon.home.ui.agent;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import com.yoopoon.home.R;

public class CommentFunction {
	View rootView;

	Context mContext;

	LayoutInflater inflater;
	
	

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

		
	}

	
}
