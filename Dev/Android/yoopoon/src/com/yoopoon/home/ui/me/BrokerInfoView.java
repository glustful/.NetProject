package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.makeramen.RoundedImageView;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.R;

@EViewGroup(R.layout.broker_info_view)
public class BrokerInfoView extends RelativeLayout {

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
		PersonSettingActivity_.intent(getContext()).start();
	}

	@Click(R.id.takeMoney)
	void takeMoney() {

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

	public void initData(JSONObject mRootData, boolean isBroker) {
		if (isBroker) {
			bLayout.setVisibility(View.VISIBLE);
			cLayout.setVisibility(View.GONE);
			name.setText(Tools.optString(mRootData, "Name", "老雷斯"));
			level.setText(Tools.optString(mRootData, "levelStr", "铜牌"));
			order.setText(Tools.optString(mRootData, "orderStr", "1"));
			partnet.setText(Tools.optInt(mRootData, "partnerCount", 0) + "");
			refer.setText(Tools.optInt(mRootData, "refereeCount", 0) + "");
			custom.setText(Tools.optInt(mRootData, "customerCount", 0) + "");
			money.setText(Tools.optString(mRootData, "allMoneys", "0.00"));
		} else {
			bLayout.setVisibility(View.GONE);
			cLayout.setVisibility(View.VISIBLE);
			name1.setText(Tools.optString(mRootData, "Name", "老雷斯"));
		}
	}

	/**
	 * @Title: hide
	 * @Description: 隐藏相应布局
	 */
	public void hide() {
		// TODO Auto-generated method stub

	}

}
