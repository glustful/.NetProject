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
import org.androidannotations.annotations.ViewById;

import android.content.Intent;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.TextView.OnEditorActionListener;

/**
 * @ClassName: ProductClassifyActivity
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月10日 上午11:10:02
 */
@EActivity(R.layout.activity_product_classification)
public class ProductClassifyActivity extends MainActionBarActivity {
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;

	@AfterViews
	void initProductClassification() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("分类");
		//添加用户输入内容后输入法可以使用键盘上的搜索框搜索
		searchProductEditText.setImeActionLabel("搜索", EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setSingleLine();
		searchProductEditText.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setOnEditorActionListener(new OnEditorActionListener() {
			@Override
			public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
				if (actionId == EditorInfo.IME_ACTION_SEARCH) {
					Intent intent=new Intent(ProductClassifyActivity.this,ProductList_.class);
					Bundle  bundle=new Bundle();
					bundle.putString("productClassification", "曲靖特产");
					intent.putExtras(bundle);
					startActivity(intent);
					return true;
				}
				return false;
			}
		});
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
