/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PartnerDetailActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-7 上午10:11:18 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.style.ForegroundColorSpan;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;

/**
 * @ClassName: PartnerDetailActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-7 上午10:11:18
 */
@EActivity(R.layout.item_partner_detail)
public class PartnerDetailActivity extends MainActionBarActivity {
	@Extra
	String id;

	@ViewById(R.id.tv_partde_name)
	TextView tv_name;
	@ViewById(R.id.tv_partde_phone)
	TextView tv_phone;
	@ViewById(R.id.tv_partde_regtime)
	TextView tv_regtime;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("合伙人详情");
		requestDatas();
	}

	void requestDatas() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONObject obj = data.getMRootData().optJSONObject("List");
					initDatas(obj);

				} else {
					Toast.makeText(PartnerDetailActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_broker)).addParam("Id", id).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	private void initDatas(JSONObject obj) {
		// "Brokername":"15587119911",
		// "Phone":"15587119911"
		// "rgtime":"2015/7/28"
		String brokerName = obj.optString("Brokername", "");
		String phone = obj.optString("Phone", "");
		String regTime = obj.optString("rgtime", "");

		tv_name.setText("姓名：" + brokerName);
		tv_phone.setText("电话：" + phone);
		tv_regtime.setText("注册时间：" + regTime);

		modifyColor(tv_name, 3);
		modifyColor(tv_phone, 3);
		modifyColor(tv_regtime, 5);
	}

	private void modifyColor(TextView tv, int blackLength) {
		String text = tv.getText().toString();
		SpannableStringBuilder builder = new SpannableStringBuilder(text);

		// ForegroundColorSpan 为文字前景色，BackgroundColorSpan为文字背景色
		ForegroundColorSpan graySpan = new ForegroundColorSpan(Color.GRAY);
		ForegroundColorSpan blackSpan = new ForegroundColorSpan(Color.BLACK);

		builder.setSpan(blackSpan, 0, blackLength, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		builder.setSpan(graySpan, blackLength, text.length(), Spannable.SPAN_INCLUSIVE_INCLUSIVE);

		tv.setText(builder);
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
