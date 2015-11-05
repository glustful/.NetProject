/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ScrollTextView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.view 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-24 上午11:27:43 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.view;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.TextView;

public class ScrollTextView extends TextView {

	public ScrollTextView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	public ScrollTextView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public ScrollTextView(Context context) {
		super(context);
	}

	/**
	 * 此方法默认返回值为false，在此处将其返回为true就可以使TextView一创建就具有焦点
	 */
	@Override
	public boolean isFocused() {
		return true;
	}

}
