package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import android.view.View;

@EActivity(R.layout.activity_payway)
public class PayWayActivity extends MainActionBarActivity {

	@Click(R.id.tv_alipay)
	void pay() {
		PayDemoActivity_.intent(PayWayActivity.this).start();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("支付方式");
		rightButton.setVisibility(View.GONE);
	}

	@Override
	public void backButtonClick(View v) {
		finish();
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
