package com.yoopoon.market.view;

import android.content.Context;
import android.util.AttributeSet;
import android.widget.GridView;

/**
 * @ClassName: NoScrollGridView
 * @Description: 没有scroll的Gridview，用于GridView和Scroll的嵌套问题
 * @author: 徐阳会
 * @date: 2015年9月14日 下午4:18:45
 */
public class NoScrollGridView extends GridView {
	public NoScrollGridView(Context context) {
		super(context);
	}
	public NoScrollGridView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}
	public NoScrollGridView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}
	@Override
	protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
		int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2, MeasureSpec.AT_MOST);
		super.onMeasure(widthMeasureSpec, expandSpec);
	}
}
