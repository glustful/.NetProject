/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MeJumpView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-28 下午5:40:59 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import android.content.Context;
import android.util.AttributeSet;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.yoopoon.home.R;

/**
 * @ClassName: MeJumpView
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-28 下午5:40:59
 */
@EViewGroup(R.layout.item_foot)
public class MeJumpView extends LinearLayout {

	// [start] viewById
	@ViewById(R.id.tv_me_task)
	TextView tv_task;
	@ViewById(R.id.tv_me_building)
	TextView tv_building;

	public MeJumpView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	public MeJumpView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public MeJumpView(Context context) {
		super(context);
	}

}
