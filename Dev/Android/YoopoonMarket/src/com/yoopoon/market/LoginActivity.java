package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.content.Intent;
import android.view.View;

@EActivity(R.layout.activity_login)
public class LoginActivity extends MainActionBarActivity {

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		backWhiteButton.setVisibility(View.GONE);
		titleButton.setText("账户登录");
	}

	@Override
	public void backButtonClick(View v) {
		// 点击返回，一定是没有登陆成功，返回首页
		Intent intent = new Intent("com.yoopoon.market.showshop");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		this.sendBroadcast(intent);
		MainActivity_.intent(this).start();
	}

	@Override
	public void titleButtonClick(View v) {
		// TODO Auto-generated method stub

	}

	@Override
	public void rightButtonClick(View v) {
		// TODO Auto-generated method stub

	}

	@Override
	public Boolean showHeadView() {
		return true;
	}

}
