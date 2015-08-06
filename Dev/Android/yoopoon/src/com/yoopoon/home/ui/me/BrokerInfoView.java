package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.content.Context;
import android.text.TextUtils;
import android.util.AttributeSet;
import android.view.View;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.makeramen.RoundedImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.IClientActivity_;
import com.yoopoon.home.IPartnerActivity_;
import com.yoopoon.home.IPocketActivity_;
import com.yoopoon.home.IRecommendActivity_;
import com.yoopoon.home.R;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

@EViewGroup(R.layout.broker_info_view)
public class BrokerInfoView extends RelativeLayout {

	private static final String TAG = "BrokerInfoView";

	@ViewById
	RoundedImageView headImg;
	@ViewById
	TextView name;
	@ViewById
	TextView level;
	@ViewById
	TextView order;
	@ViewById
	TextView partnet;
	@ViewById
	TextView refer;
	@ViewById
	TextView custom;
	@ViewById
	TextView money;
	@ViewById(R.id.name1)
	TextView name1;
	@ViewById(R.id.brokerLayout)
	View bLayout;
	@ViewById(R.id.customLayout)
	View cLayout;

	@Click(R.id.headImg)
	void headImg() {
		if (User.lastLoginUser(getContext()) == null) {
			HomeLoginActivity_.intent(getContext()).isManual(true).start();
			return;
		}
		PersonSettingActivity_.intent(getContext()).start();
	}

	@Click(R.id.takeMoney)
	void takeMoney() {
		IPocketActivity_.intent(getContext()).start();
	}

	@Click(R.id.tv_brokerinfo_client)
	void showClients() {
		IClientActivity_.intent(getContext()).start();
	}

	@Click(R.id.tv_brokerinfo_partner)
	void showPartners() {
		IPartnerActivity_.intent(getContext()).start();
	}

	@Click(R.id.tv_brokerinfo_recommend)
	void showBrokers() {
		IRecommendActivity_.intent(getContext()).start();
	}

	public BrokerInfoView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		// TODO Auto-generated constructor stub
	}

	public BrokerInfoView(Context context, AttributeSet attrs) {
		super(context, attrs);
		// TODO Auto-generated constructor stub
	}

	public BrokerInfoView(Context context) {
		super(context);
		// TODO Auto-generated constructor stub
	}

	public void initData(JSONObject mRootData, boolean isBroker, int clientCount) {
		if (isBroker) {
			// "Name":"xuyanghui",
			// "partnerCount":0,
			// "allMoneys":"0.00",
			// "levelStr":null,
			// "customerCount":0,
			// "orderStr":"1",
			// "photo":"20150714\/20150714_102702_984_384.jpg",
			// "refereeCount":0
			bLayout.setVisibility(View.VISIBLE);
			cLayout.setVisibility(View.GONE);
			name.setText(Tools.optString(mRootData, "Name", "优客惠"));
			level.setText(Tools.optString(mRootData, "levelStr", "铜牌"));
			order.setText(Tools.optString(mRootData, "orderStr", "1"));
			partnet.setText(Tools.optInt(mRootData, "partnerCount", 0) + "");
			refer.setText(Tools.optInt(mRootData, "refereeCount", 0) + "");
			// custom.setText(Tools.optInt(mRootData, "customerCount", 0) + "");
			custom.setText(String.valueOf(clientCount));
			money.setText(Tools.optString(mRootData, "allMoneys", "0.00"));

		} else {
			bLayout.setVisibility(View.GONE);
			cLayout.setVisibility(View.VISIBLE);
			User user = User.lastLoginUser(getContext());

			name1.setText(Tools.optString(mRootData, "Name", user.phone));

		}

		String photo = Tools.optString(mRootData, "photo", null);
		if (!TextUtils.isEmpty(photo)) {
			// 若photo不为空，加载头像
			String url = "http://img.yoopoon.com/" + photo;
			ImageLoader.getInstance().displayImage(url, headImg);
		} else {
			headImg.setImageResource(R.drawable.logo_gray);
		}
	}

	/**
	 * @Title: hide
	 * @Description: 隐藏相应布局
	 */
	public void hide() {
		headImg.setImageResource(R.drawable.logo_gray);
		bLayout.setVisibility(View.GONE);
	}

}
