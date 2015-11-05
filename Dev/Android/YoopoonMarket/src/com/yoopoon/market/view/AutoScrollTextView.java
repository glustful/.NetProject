package com.yoopoon.market.view;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.TextView;

//自动滚动的TextView
public class AutoScrollTextView extends TextView {

	public AutoScrollTextView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	public AutoScrollTextView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public AutoScrollTextView(Context context) {
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
