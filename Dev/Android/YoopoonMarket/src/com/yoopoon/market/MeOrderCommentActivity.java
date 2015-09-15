package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import android.widget.TextView;

@EActivity(R.layout.activity_me_order_comment)
public class MeOrderCommentActivity extends MainActionBarActivity {
	@ViewById(R.id.tv_btn)
	TextView tv_btn;
	@ViewById(R.id.tv_desc)
	TextView tv_desc;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("评论");

		tv_btn.setVisibility(View.GONE);
		tv_desc.setVisibility(View.GONE);
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
