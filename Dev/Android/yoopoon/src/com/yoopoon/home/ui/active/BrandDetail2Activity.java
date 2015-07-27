package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONException;
import org.json.JSONObject;
import android.text.Html;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;

@EActivity(R.layout.activity_style)
public class BrandDetail2Activity extends MainActionBarActivity {
	private static final String TAG = "BrandDetail2Activity";
	@Extra
	String mJson;
	@ViewById(R.id.tv_style_name)
	TextView tv_name;
	@ViewById(R.id.tv_style_detail2)
	TextView tv_detail;
	@ViewById(R.id.tv_style_subtitle)
	TextView tv_subtitle;
	@ViewById(R.id.iv_style)
	ImageView iv_style;

	@Click(R.id.tv_style)
	void style() {
		Log.i(TAG, mJson);
		if (!TextUtils.isEmpty(mJson))
			BrandDetailActivity_.intent(this).mJson(mJson).start();
	}

	@AfterViews
	void initView() {
		backButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("楼盘详情");

		initDatas();
	}

	private void initDatas() {
		if (!StringUtils.isEmpty(mJson))
			try {
				JSONObject obj = new JSONObject(mJson);
				String title = obj.optString("Bname", "");
				String subTitle = obj.optString("SubTitle", "");
				String content = obj.optString("Content", "");
				tv_name.setText(title);
				tv_subtitle.setText(subTitle);
				tv_detail.setText(Html.fromHtml(content));

				String photo = obj.optString("Bimg", "");
				if (TextUtils.isEmpty(photo)) {
					String url = "http://img.yoopoon.com/" + photo;
					ImageLoader.getInstance().displayImage(url, iv_style);
				}
			} catch (JSONException e) {
				// TODO Auto-generated catch block
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
