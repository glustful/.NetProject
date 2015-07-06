package com.yoopoon.home;

import java.util.HashMap;

import org.androidannotations.annotations.EActivity;

import com.yoopoon.home.SearchFunction.OnSearchCallBack;
import com.yoopoon.home.data.net.ResponseData;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.view.Gravity;
import android.view.MotionEvent;
import android.view.View;

@EActivity(R.layout.main_action_bar)
public abstract class SearchActionBarActivity extends ActionBarActivity{

	View headView;
	ActionBar actionBar;
	protected HashMap<String, String> SearchParameter;
	SearchFunction mSearchFunction;
	
	@SuppressLint("InflateParams")
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		actionBar = getSupportActionBar();
		initSearchParam();
		mSearchFunction = new SearchFunction(this, getString(R.string.url_brand_searchBrand));
		mSearchFunction.setParam(SearchParameter);
		mSearchFunction.setSearchCallBack(setSearchCallBack());
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
   
   public abstract void initSearchParam();
   public abstract OnSearchCallBack setSearchCallBack();
   public enum CurrentState{
	   eManual,
	   eSearch
   }
}
