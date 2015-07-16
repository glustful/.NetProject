/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MeFooterView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: 经纪人个人页面功能
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午2:14:07 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.util.AttributeSet;
import android.view.View;
import android.widget.LinearLayout;

import com.yoopoon.home.BrokerRankActivity_;
import com.yoopoon.home.IPartnerActivity_;
import com.yoopoon.home.IRecommendActivity_;
import com.yoopoon.home.R;

/**
 * @ClassName: MeFooterView
 * @Description: 个人中心尾部布局
 * @author: guojunjun
 * @date: 2015-7-7 下午2:14:07
 */
@EViewGroup(R.layout.me_footer_view)
public class MeFooterView extends LinearLayout {
	// [start] viewById
	@ViewById(R.id.brokerLayout)
	View brokerLayout;
	@ViewById(R.id.customLayout)
	View customLayout;

	// [end]
	// [start] onClick
	@Click(R.id.registerToBroker)
	void registerToBroker() {
		PersonSettingActivity_.intent(getContext()).start();
	}
	@Click(R.id.setting)
	void setting() {
		SettingActivity_.intent(getContext()).start();
	}
	@Click(R.id.brokerSetting)
	void brokerSetting() {
		setting();
	}
	@Click(R.id.tv_footer_recommend)
	void iRecommend() {
		// ################彭佳媛 编写 ################### Start
		// RecommendActivity_.intent(getContext()).start();
		// #################彭佳媛 编写 ################## End
		//
		//
		// ################徐阳会 修改 2015年7月16日 ##################### Start
		// 创建Broadast,发送广播, 让mainPage将页面切换到经纪人推荐页面(和经纪人带客页面一样)(FramHouseFragment)
		Intent intent = new Intent("com.yoopoon.broker_takeguest");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		Activity currentActivity = (Activity) getContext();
		currentActivity.sendBroadcast(intent);
		// ###############徐阳会 修改 2015年7月16日 ##################### End
	}
	// ################ 彭佳媛 编写 #################
	/*
	 * @Click(R.id.tv_footer_guest) void iGuest() { IGuestActivity_.intent(getContext()).start(); }
	 */
	// ################ 彭佳媛 编写 #################
	// ################ 徐阳会 修改 2015年7月16日 Start #################
	@Click(R.id.tv_footer_guest)
	void iGuest() {
		// 创建Broadast,发送广播, 让mainPage将页面切换到经纪人带客页面(FramHouseFragment)
		Intent intent = new Intent("com.yoopoon.broker_takeguest");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		Activity currentActivity = (Activity) getContext();
		currentActivity.sendBroadcast(intent);
	}
	// ################ 徐阳会 修改 2015年7月16日 End #################
	@Click(R.id.tv_footer_partner)
	void iPartner() {
		IPartnerActivity_.intent(getContext()).start();
	}
	@Click(R.id.tv_footer_rank)
	void iRank() {
		BrokerRankActivity_.intent(getContext()).start();
	}
	@Click(R.id.tv_footer_recommend_agent)
	void recommendAgent() {
		IRecommendActivity_.intent(getContext()).start();
	}
	// [end]
	// [start] constructor
	public MeFooterView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}
	public MeFooterView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}
	public MeFooterView(Context context) {
		super(context);
	}
	// [end]
	// [start] public method
	public void show(boolean isBroker) {
		if (isBroker) {
			brokerLayout.setVisibility(View.VISIBLE);
			customLayout.setVisibility(View.GONE);
		} else {
			brokerLayout.setVisibility(View.GONE);
			customLayout.setVisibility(View.VISIBLE);
		}
	}
	// [end]
	/**
	 * @Title: hide
	 * @Description: 隐藏布局文件
	 */
	public void hide() {
		brokerLayout.setVisibility(View.GONE);
		customLayout.setVisibility(View.GONE);
	}
}
