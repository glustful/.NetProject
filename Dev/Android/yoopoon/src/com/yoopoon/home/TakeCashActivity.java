/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: TakeCashActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 上午10:29:27 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import android.view.View;
import com.yoopoon.home.data.net.ResponseData;

/**
 * @ClassName: TakeCashActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 上午10:29:27
 */
@EActivity(R.layout.activity_takecash)
public class TakeCashActivity extends MainActionBarActivity {
	@Extra
	ResponseData mData;

	// @Click(R.id.btn_takecash_finish)
	// void takenCash() {
	// finish();
	// }

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
		titleButton.setText("提取现金");
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
