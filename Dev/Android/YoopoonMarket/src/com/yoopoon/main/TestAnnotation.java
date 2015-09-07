/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: TestAnnotation.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.main 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月7日 上午11:39:01 
 * @version: V1.0   
 */
package com.yoopoon.main;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;

import android.app.Activity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.TextView;

import com.yoopoon.yoopoonmarket.R;

/** 
 * @ClassName: TestAnnotation 
 * @Description: 
 * @author: 徐阳会
 * @date: 2015年9月7日 上午11:39:01  
 */
@EActivity(R.layout.test_annotation)
public class TestAnnotation extends Activity {
	@ViewById
	Button button;
	@ViewById
	TextView textview;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
	}
	@Click(R.id.button)
	void settingTextView() {
		textview.setText("testing");
	}
}
