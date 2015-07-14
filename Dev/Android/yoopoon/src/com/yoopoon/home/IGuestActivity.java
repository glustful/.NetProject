/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: IGuestActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-14 下午1:12:20 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.view.View;

/**
 * @ClassName: IGuestActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午1:12:20
 */

@EActivity(R.layout.activity_guest)
public class IGuestActivity extends MainActionBarActivity {
	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我要带客");
	}

	@Override
	public void backButtonClick(View v) {
		finish();
	}

	@Override
	public void titleButtonClick(View v) {

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {

		return true;
	}
}
