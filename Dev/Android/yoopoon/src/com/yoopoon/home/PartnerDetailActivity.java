/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PartnerDetailActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-14 下午6:02:38 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.view.View;

/**
 * @ClassName: PartnerDetailActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午6:02:38
 */
@EActivity(R.layout.item_partner_detail)
public class PartnerDetailActivity extends MainActionBarActivity {

	@AfterViews
	void initUI() {
		// backButton.setVisibility(View.VISIBLE);
		// titleButton.setVisibility(View.VISIBLE);
		// backButton.setText("返回");
		// titleButton.setText("合伙人详情页");
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
		// TODO Auto-generated method stub

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
		// TODO Auto-generated method stub

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
		// TODO Auto-generated method stub
		return null;
	}

}
