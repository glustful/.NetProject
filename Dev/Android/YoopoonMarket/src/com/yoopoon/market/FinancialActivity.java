package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import android.graphics.Color;
import android.view.View;

@EActivity(R.layout.activity_financial)
public class FinancialActivity extends MainActionBarActivity {

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("金融理财");
		headView.setBackgroundColor(Color.RED);
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
		// TODO Auto-generated method stub
		return true;
	}

}
