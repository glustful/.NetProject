/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerScore.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.broker 
 * @Description: TODO
 * @author: king  
 * @updater: king 
 * @date: 2015年7月15日 下午4:53:14 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.broker;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.content.Context;
import android.graphics.Color;
import android.view.View;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;

/**
 * @ClassName: BrokerScore
 * @Description:
 * @author: 徐阳会
 * @date: 2015年7月15日 下午4:53:14
 */
@EActivity(R.layout.broker_score)
public class BrokerScoreActivity extends MainActionBarActivity {
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
		titleButton.setText("积分");
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
