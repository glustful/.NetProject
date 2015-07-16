/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerBonus.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.bonus 
 * @Description: 经纪人红包管理的类
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015年7月15日 下午4:08:50 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.bonus;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;

import android.content.Context;
import android.graphics.Color;
import android.view.View;

import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;

/**
 * @ClassName: BrokerBonus
 * @Description: 经纪人红包管理类
 * @author: 徐阳会
 * @date: 2015年7月15日 下午4:08:50
 */
@EActivity(R.layout.broker_bonus)
public class BrokerBonusActivity extends MainActionBarActivity {
	private static final String TAG = "BrokerBonus:经纪人红包";
	// //////////////////////////////////如下是变量和属性的初始化///////////////////////////////////
	Context mContext;

	// //////////////////////////////////如上是变量和属性的初始化///////////////////////////////////
	@AfterViews
	void initUI() {
		mContext = this;
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("带客");
	}
	/*
	 * (non Javadoc)
	 * @Title: backButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#backButtonClick(android.view.View)
	 */
	@Override
	public void backButtonClick(View v) {
		finish();
	}
	/*
	 * (non Javadoc)
	 * @Title: titleButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#titleButtonClick(android.view.View)
	 */
	@Override
	public void titleButtonClick(View v) {
	}
	/*
	 * (non Javadoc)
	 * @Title: rightButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#rightButtonClick(android.view.View)
	 */
	@Override
	public void rightButtonClick(View v) {
	}
	/*
	 * (non Javadoc)
	 * @Title: showHeadView
	 * @Description: TODO
	 * @return
	 * @see com.yoopoon.home.MainActionBarActivity#showHeadView()
	 */
	@Override
	public Boolean showHeadView() {
		return true;
	}
}
