package com.yoopoon.home;

import org.androidannotations.annotations.EActivity;
import android.annotation.SuppressLint;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

@EActivity(R.layout.search_action_bar)
public abstract class SearchActionBarActivity extends ActionBarActivity{

	View headView;
	ActionBar actionBar;
	
	SearchFunction mSearchFunction;
	
	@SuppressLint("InflateParams")
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		actionBar = getSupportActionBar();
		
		//headView = LayoutInflater.from(this).inflate(R.layout.main_action_title, null);
		mSearchFunction = new SearchFunction(this, null);
		headView = mSearchFunction.getSearchView();
		backShow();
	}
	
	private void backShow(){
		
		headView.setVisibility(View.VISIBLE);
		actionBar.setDisplayShowHomeEnabled(false);
		actionBar.setDisplayShowCustomEnabled(true);
		actionBar.setDisplayShowTitleEnabled(false);
		actionBar.setDisplayHomeAsUpEnabled(false);
		
		ActionBar.LayoutParams lp = new ActionBar.LayoutParams(
				android.view.ViewGroup.LayoutParams.MATCH_PARENT, android.view.ViewGroup.LayoutParams.MATCH_PARENT);
		lp.gravity = lp.gravity & ~Gravity.HORIZONTAL_GRAVITY_MASK
				| Gravity.LEFT;
		actionBar.setCustomView(headView, lp);
	}
	
	
	
	public ActionBar getMainActionBar(){
		return actionBar;
	}
	
	@Override
	public boolean dispatchTouchEvent(MotionEvent ev) {

		if(ev.getAction() == MotionEvent.ACTION_MOVE ){
			
			activityYMove();

		}

		return super.dispatchTouchEvent(ev);
	}
	
	@Override
	public boolean onTouchEvent(MotionEvent event) {
		
		return true;
		}

	
   protected void activityYMove(){
		
	}
	
}
