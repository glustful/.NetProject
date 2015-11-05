/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MyListView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.view 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-24 下午4:49:05 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.view;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.ListView;

/**
 * @ClassName: MyListView
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-24 下午4:49:05
 */
public class MyListView extends ListView {
	public MyListView(Context context) {
		super(context);
	}

	public MyListView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public MyListView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	@Override
	/**
	 * 重写该方法，达到使ListView适应ScrollView的效果
	 */
	protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
		int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2, MeasureSpec.AT_MOST);
		super.onMeasure(widthMeasureSpec, expandSpec);
	}
}
