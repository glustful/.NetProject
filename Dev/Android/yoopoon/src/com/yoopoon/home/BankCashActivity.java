/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BankCashActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 上午11:11:31 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;

import android.graphics.Color;
import android.view.View;

/**
 * @ClassName: BankCashActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 上午11:11:31
 */
@EActivity(R.layout.activity_bank_takecash)
public class BankCashActivity extends MainActionBarActivity {
	/**
	 * @Title: main
	 * @Description: TODO
	 * @param args
	 */
	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("银行卡现金详情");
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
