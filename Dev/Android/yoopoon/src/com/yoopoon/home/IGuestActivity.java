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
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.graphics.Color;
import android.text.TextUtils;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

/**
 * @ClassName: IGuestActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午1:12:20
 */
@EActivity(R.layout.activity_guest)
public class IGuestActivity extends MainActionBarActivity {
	@Click(R.id.btn_iguest_commit)
	void commit() {
		Builder builder = new Builder(this);
		View dialogView = View.inflate(this, R.layout.dialog_iguest, null);
		ImageView iv = (ImageView) dialogView.findViewById(R.id.iv_iguest_dialog);
		TextView tv = (TextView) dialogView.findViewById(R.id.tv_iguest_dialog);
		Button btn = (Button) dialogView.findViewById(R.id.btn_iguest_dialog);
		String name = et_name.getText().toString().trim();
		String phone = et_phone.getText().toString().trim();
		String style = et_style.getText().toString().trim();
		String date = et_date.getText().toString().trim();
		if (TextUtils.isEmpty(name) || TextUtils.isEmpty(phone) || TextUtils.isEmpty(style) || TextUtils.isEmpty(date)) {
			iv.setImageResource(R.drawable.iguest_fail);
			tv.setText("对不起，提价信息失败");
		}
		builder.setView(dialogView);
		final AlertDialog dialog = builder.show();
		btn.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				dialog.dismiss();
			}
		});
	}
	
	@ViewById(R.id.et_guest_name)
	EditText et_name;
	@ViewById(R.id.et_guest_date)
	EditText et_date;
	@ViewById(R.id.et_guest_phone)
	EditText et_phone;
	@ViewById(R.id.et_guest_style)
	EditText et_style;
	
	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
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
