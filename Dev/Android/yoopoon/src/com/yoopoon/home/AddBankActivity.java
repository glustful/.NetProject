/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: AddBankActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 下午4:25:37 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.view.View;

/**
 * @ClassName: AddBankActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 下午4:25:37
 */
@EActivity(R.layout.activity_add_bank)
public class AddBankActivity extends MainActionBarActivity {

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
		titleButton.setText("添加银行卡");
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
