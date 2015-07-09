/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MeFooterView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午2:14:07 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.widget.LinearLayout;
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
