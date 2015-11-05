package com.yoopoon.market.view;

import android.content.Context;
import android.support.v4.view.ViewPager;
import android.util.AttributeSet;
import android.view.View;

public class MyViewPager extends ViewPager {
	public MyViewPager(Context context, AttributeSet attrs) {
		super(context, attrs);
	}
	public MyViewPager(Context context) {
		super(context);
	}
	@Override
	public void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
		int height = 0;
		//下面遍历所有child的高度
		for (int i = 0; i < getChildCount(); i++) {
			View child = getChildAt(i);
			child.measure(widthMeasureSpec, MeasureSpec.makeMeasureSpec(0, MeasureSpec.UNSPECIFIED));
			int h = child.getMeasuredHeight();
			if (h > height) //采用最大的view的高度。
				height = h;
		}
		heightMeasureSpec = MeasureSpec.makeMeasureSpec(height, MeasureSpec.EXACTLY);
		super.onMeasure(widthMeasureSpec, heightMeasureSpec);
	}
}