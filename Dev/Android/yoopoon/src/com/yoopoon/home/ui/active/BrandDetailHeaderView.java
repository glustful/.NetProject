package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.preference.PreferenceManager;
import android.util.AttributeSet;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.home.FramMainActivity_;

@EViewGroup(R.layout.brand_detail_header_view)
public class BrandDetailHeaderView extends RelativeLayout {
	private static final String TAG = "BrandDetailHeaderView";
	@ViewById(R.id.img)
	ImageView img;
	@ViewById(R.id.content)
	TextView content;
	@ViewById(R.id.title)
	TextView title;
	@ViewById(R.id.subTitle)
	TextView subTitle;
	@ViewById(R.id.callPhone)
	TextView callPhone;
	// ######################如下是经纪人登陆后在品牌详情页顶端的我要推荐，我要带客，咨询热线功能初始化######
	// 控制只有当经纪人登陆时候才显示该功能（visiable）
	@ViewById(R.id.broker_takeguest_recommend_brandDetail)
	RelativeLayout brokerTakeGuestRecommendLinearLayout;
	@ViewById(R.id.broker_take_guest_from_brand)
	Button brokerTakeGuestButton;
	@ViewById(R.id.broker_recommend_from_brand)
	Button brokerRecommendButton;
	@ViewById(R.id.broker_consultation_from_brand)
	Button brokerConsultationButton;
	// 是否是经纪人状态（从SharedPreference获取）
	boolean brokerStatusBoolean;

	// ######################如上是经纪人登陆后在品牌详情页顶端的我要推荐，我要带客，咨询热线功能初始化######
	@Click(R.id.callPhone)
	void callPhone() {
		String phone = callPhone.getTag() != null ? callPhone.getTag().toString().trim() : null;
		if (phone == null || phone.equals("")) {
			ToastUtils.showToast(getContext(), "手机号码为空，联系管理员", 3000);
			return;
		}
		Tools.callPhone(getContext(), phone);
	}

	// #############################徐阳会 2015年7月30日 添加################################# Start
	// 点击咨询热线事件绑定
	@Click(R.id.broker_consultation_from_brand)
	void consultClick(View view) {
		String phone = view.getTag() != null ? view.getTag().toString().trim() : null;
		if (phone == null || phone.equals("")) {
			ToastUtils.showToast(getContext(), "手机号码为空，联系管理员", 3000);
			return;
		}
		Tools.callPhone(getContext(), phone);
	}

	// 点击“我要带客”按钮事件绑定
	@Click(R.id.broker_take_guest_from_brand)
	void brokerTakeGuestClick(View view) {
		Intent intent = new Intent("com.yoopoon.broker_takeguest");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		intent.putExtra("comeFromBroker", true);
		Activity currentActivity = (Activity) getContext();
		currentActivity.sendBroadcast(intent);
		// 启动房源库页面对应的Activity
		FramMainActivity_.intent(currentActivity).start();
	}

	// 点击“我要推荐”按钮事件绑定
	@Click(R.id.broker_recommend_from_brand)
	void brokerRecommendClick(View view) {
		Intent intent = new Intent("com.yoopoon.broker_takeguest");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		intent.putExtra("comeFromBroker", true);
		Activity currentActivity = (Activity) getContext();
		currentActivity.sendBroadcast(intent);
		// 启动房源库页面对应的Activity
		FramMainActivity_.intent(currentActivity).start();
	}

	// #############################徐阳会 2015年7月30日 添加################################# End
	public BrandDetailHeaderView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	public BrandDetailHeaderView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public BrandDetailHeaderView(Context context) {
		super(context);
	}

	public void init(JSONObject jsonObject) {
		// Log.i(TAG, jsonObject.toString());
		title.setText(Tools.optString(jsonObject, "Bname", ""));
		subTitle.setText(Tools.optString(jsonObject, "SubTitle", ""));
		callPhone.setTag(Tools.optString(jsonObject.optJSONObject("ProductParamater"), "来电咨询", ""));
		brokerConsultationButton.setTag(Tools.optString(jsonObject.optJSONObject("ProductParamater"), "来电咨询", ""));
		img.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, MyApplication
				.getInstance().getDeviceInfo((Activity) getContext()).heightPixels / 5));
		/* String url = getContext().getString(R.string.url_host_img)+jsonObject.optString("Bimg"); */
		String url = getContext().getString(R.string.url_host_img) + jsonObject.optString("Bimg");
		img.setTag(url);
		ImageLoader.getInstance()
				.displayImage(url, img, MyApplication.getOptions(), MyApplication.getLoadingListener());
		brokerStatusBoolean = PreferenceManager.getDefaultSharedPreferences(getContext()).getBoolean("isBroker", false);
		// Log.i(LOGTAG, brokerStatusBoolean + "");
		if (brokerStatusBoolean) {
			brokerTakeGuestRecommendLinearLayout.setVisibility(View.VISIBLE);
		} else {
			brokerTakeGuestRecommendLinearLayout.setVisibility(View.GONE);
		}
	}

	public void setContent(String content) {
		this.content.setText(content);
	}
}
