package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

@EViewGroup(R.layout.brand_detail_header_view)
public class BrandDetailHeaderView extends LinearLayout {
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
	
	@Click(R.id.callPhone)
	void callPhone(View view) {
		String phone = view.getTag() != null ? view.getTag().toString().trim() : null;
		if (phone == null || phone.equals("")) {
			ToastUtils.showToast(getContext(), "手机号码为空，联系管理员", 3000);
			return;
		}
		Tools.callPhone(getContext(), phone);
	}
	public BrandDetailHeaderView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		// TODO Auto-generated constructor stub
	}
	public BrandDetailHeaderView(Context context, AttributeSet attrs) {
		super(context, attrs);
		// TODO Auto-generated constructor stub
	}
	public BrandDetailHeaderView(Context context) {
		super(context);
		// TODO Auto-generated constructor stub
	}
	public void init(JSONObject jsonObject) {
		title.setText(Tools.optString(jsonObject, "Bname", ""));
		subTitle.setText(Tools.optString(jsonObject, "SubTitle", ""));
		callPhone.setTag(Tools.optString(jsonObject.optJSONObject("ProductParamater"), "来电咨询", ""));
		img.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, MyApplication
				.getInstance().getDeviceInfo((Activity) getContext()).heightPixels / 5));
		/*String url = getContext().getString(R.string.url_host_img)+jsonObject.optString("Bimg");*/
		String url = getContext().getString(R.string.url_host_img) + jsonObject.optString("Bimg");
		img.setTag(url);
		ImageLoader.getInstance()
				.displayImage(url, img, MyApplication.getOptions(), MyApplication.getLoadingListener());
	}
	public void setContent(String content) {
		this.content.setText(content);
	}
}
