package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.webkit.WebView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.TextView;
import android.widget.Toast;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

@EActivity(R.layout.activity_style)
public class BrandDetail2Activity extends MainActionBarActivity {
	private static final String TAG = "BrandDetail2Activity";
	@Extra
	String mJson;
	@ViewById(R.id.tv_style_name)
	TextView tv_name;
	@ViewById(R.id.wv_style_detail2)
	WebView wv_detail;
	@ViewById(R.id.tv_style_subtitle)
	TextView tv_subtitle;
	@ViewById(R.id.iv_style)
	ImageView iv_style;
	@ViewById(R.id.tv_style_consult)
	Button btn_consult;

	@Click(R.id.tv_style)
	void style() {
		if (!TextUtils.isEmpty(mJson))
			BrandDetailActivity_.intent(this).mJson(mJson).start();
	}

	@Click(R.id.tv_style_consult)
	void consult() {
		String phone = (String) btn_consult.getTag();
		if (TextUtils.isEmpty(phone)) {
			Toast.makeText(this, "该楼盘暂时还没有咨询热线哦", Toast.LENGTH_SHORT).show();
			return;
		}
		Tools.callPhone(this, phone);
	}

	@AfterViews
	void initView() {
		backButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("楼盘详情");
		initDatas();
	}

	private void initDatas() {
		Log.i(TAG, mJson);
		if (!StringUtils.isEmpty(mJson))
			try {
				JSONObject obj = new JSONObject(mJson);
				String title = obj.optString("Bname", "");
				String subTitle = obj.optString("SubTitle", "");
				final String content = obj.optString("Content", "");
				tv_name.setText(title);
				tv_subtitle.setText(subTitle);
				// ######################## 徐阳会 2015年7月28日 修改 ######################### Start
				tv_name.setText(title);
				tv_subtitle.setText(subTitle);
				wv_detail.loadDataWithBaseURL(null, content, "text/html", "utf-8", null);
				JSONObject parametersJsonObject = obj.optJSONObject("ProductParamater");
				// ######################## 彭佳媛 编写 ######################### Start
				/*
				 * String photo = obj.optString("Bimg", ""); if (TextUtils.isEmpty(photo)) {
				 * Log.i(TAG, "11111111"); String url = "http://img.yoopoon.com/" + photo;
				 * Log.i(TAG, url); ImageLoader.getInstance().displayImage(url, iv_style); }
				 */
				// ######################## 彭佳媛 编写 ######################### End
				//
				//
				// ######################## 徐阳会 2015年7月27日 修改 ######################### Start
				String photo = parametersJsonObject.optString("图片banner");
				btn_consult.setTag(parametersJsonObject.optString("来电咨询", ""));
				if (!photo.equals("")) {
					String url = "http://img.yoopoon.com/" + photo;
					iv_style.setTag(url);
					iv_style.setScaleType(ScaleType.FIT_XY);
					ImageLoader.getInstance().displayImage(url, iv_style, MyApplication.getOptions(),
							MyApplication.getLoadingListener());
				}
				// ######################## 徐阳会 2015年7月27日 修改 ######################### End
			} catch (JSONException e) {
				e.printStackTrace();
			}
	}

	@Override
	public void backButtonClick(View v) {
		this.finish();
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
