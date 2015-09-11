/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ProductClassifyActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月10日 上午11:10:02 
 * @version: V1.0   
 */
package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;

import android.view.View;

/**
 * @ClassName: ProductClassifyActivity
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月10日 上午11:10:02
 */
@EActivity(R.layout.activity_product_classification)
public class ProductClassifyActivity extends MainActionBarActivity {
	@AfterViews
	void initProductClassification() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("分类");
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
