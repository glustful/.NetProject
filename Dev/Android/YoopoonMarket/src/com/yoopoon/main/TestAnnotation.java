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
import android.graphics.Bitmap;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.listener.ImageLoadingListener;
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
	@ViewById
	ImageView imageview;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		String imageURL = "http://img.iyookee.cn/20150824/20150824_100142_474_271.png";
		ImageLoader.getInstance().loadImage(imageURL, new ImageLoadingListener() {
			@Override
			public void onLoadingStarted(String imageUri, View view) {
			}
			@Override
			public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
			}
			@Override
			public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
				imageview.setImageBitmap(loadedImage);
			}
			@Override
			public void onLoadingCancelled(String imageUri, View view) {
			}
		});
	}
	@Click(R.id.button)
	void settingTextView() {
		textview.setText("testing");
	}
}
