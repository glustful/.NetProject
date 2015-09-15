package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.graphics.Color;
import android.view.View;

@EActivity(R.layout.activity_new_address)
public class NewAddressActivity extends MainActionBarActivity {
	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		backButton.setVisibility(View.GONE);
		rightButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.VISIBLE);

		headView.setBackgroundColor(Color.RED);
		titleButton.setText("新增地址");
		titleButton.setTextColor(Color.WHITE);

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
