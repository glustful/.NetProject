package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;

import com.makeramen.RoundedImageView;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.R;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.RelativeLayout;
import android.widget.TextView;
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
	@Click(R.id.takeMoney)
	void takeMoney(){
		
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

	public void initData(JSONObject mRootData) {
		name.setText(Tools.optString(mRootData,"Name", "老雷斯"));
		level.setText(Tools.optString(mRootData,"levelStr", "铜牌"));
		order.setText(Tools.optString(mRootData,"orderStr", "1"));
		partnet.setText(Tools.optInt(mRootData,"partnerCount", 0)+"");
		refer.setText(Tools.optInt(mRootData,"refereeCount", 0)+"");
		custom.setText(Tools.optInt(mRootData,"customerCount",0)+"");
		money.setText(Tools.optString(mRootData,"allMoneys", "0.00"));
	}

}
