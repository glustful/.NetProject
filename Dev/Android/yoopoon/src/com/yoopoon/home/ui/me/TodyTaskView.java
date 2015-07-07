package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.EViewGroup;
import org.json.JSONArray;

import com.yoopoon.home.R;

import android.content.Context;
import android.util.AttributeSet;
import android.view.LayoutInflater;
import android.view.ViewGroup.MarginLayoutParams;
import android.widget.LinearLayout;
@EViewGroup
public class TodyTaskView extends LinearLayout {
	LayoutInflater inflater;
	LinearLayout mRoot;
	public TodyTaskView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		initRoot();
	}

	public TodyTaskView(Context context, AttributeSet attrs) {
		super(context, attrs);
		initRoot();
	}

	public TodyTaskView(Context context) {
		super(context);
		initRoot();
	}
	
	void initRoot(){
		mRoot = this;
		mRoot.setOrientation(LinearLayout.VERTICAL);
		mRoot.setLayoutParams(new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT));
		inflater = LayoutInflater.from(getContext());
	}
	
	public void addChildren(JSONArray children){
		mRoot.removeAllViews();
		for(int i=0;i<3;i++){
			LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT); 
			LinearLayout child = (LinearLayout) inflater.inflate(R.layout.today_task_item_view,null);
			lp.setMargins(0, 2, 0, 0);
			mRoot.addView(child,lp);
		}
	}

}
