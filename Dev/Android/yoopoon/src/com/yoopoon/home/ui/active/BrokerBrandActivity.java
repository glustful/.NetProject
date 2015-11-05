/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerBrandActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.active 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-14 上午9:35:59 
 * @version: V1.0   
 */
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
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.ui.agent.AgentBrandDetailActivity_;

/**
 * @ClassName: BrokerBrandActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-14 上午9:35:59
 */
@EActivity(R.layout.activity_style)
public class BrokerBrandActivity extends MainActionBarActivity {
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
	@ViewById(R.id.btn_style_consult)
	Button btn_consult;
	@ViewById(R.id.ll_progress)
	View progress;

	@Click(R.id.btn_style)
	void style() {
		if (item != null) {
			try {
				String brandId = item.optString("BrandId", "");
				int productId = item.optInt("ProductId", 0);
				String productName = item.optString("Productname", "");
				String houseType = item.optString("HouseType", "");
				AgentBrandDetailActivity_.intent(this).brandId(brandId).productId(productId).productName(productName)
						.houseType(houseType).start();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	@Click(R.id.btn_style_consult)
	void consult() {
		String phone = (String) btn_consult.getTag();
		if (TextUtils.isEmpty(phone)) {
			Toast.makeText(this, "暂时还没有咨询电话哦！", Toast.LENGTH_SHORT).show();
		} else {
			Tools.callPhone(this, phone);
		}
	}

	private static final String TAG = "BrokerBrandActivity";
	private JSONObject item;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("楼盘详情");
		try {
			item = new JSONObject(mJson);
			requestDetail();
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	void requestDetail() {
		String brandId = item.optString("BrandId", "");
		progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject obj = data.getMRootData();
				if (obj != null) {
					tv_name.setText(obj.optString("Bname", ""));
					tv_subtitle.setText(obj.optString("SubTitle", ""));
					JSONObject params = obj.optJSONObject("Parameters");
					if (params != null) {
						String photo = params.optString("图片banner", "");
						if (!StringUtils.isEmpty(photo)) {
							String url = "http://img.yoopoon.com/" + photo;
							iv_style.setScaleType(ScaleType.FIT_XY);
							ImageLoader.getInstance().displayImage(url, iv_style);
						}
						String phone = params.optString("来电咨询", "");
						btn_consult.setTag(phone);
					}
					wv_detail.setVisibility(View.GONE);
				} else {
					Toast.makeText(BrokerBrandActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

				progress.setVisibility(View.GONE);
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_brand_getDetail)).addParam("brandId", brandId)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
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
