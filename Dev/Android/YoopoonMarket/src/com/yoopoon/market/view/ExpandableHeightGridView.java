package com.yoopoon.market.view;

import android.content.Context;
import android.util.AttributeSet;
import android.view.ViewGroup;
import android.widget.GridView;

public class ExpandableHeightGridView extends GridView {
	boolean expanded = false;

	public ExpandableHeightGridView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}
	public boolean isExpanded() {
		return expanded;
	}
	public ExpandableHeightGridView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}
	public ExpandableHeightGridView(Context context) {
		super(context);
	}
	@Override
	protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
		if (isExpanded()) {
			int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2, MeasureSpec.AT_MOST);
			super.onMeasure(widthMeasureSpec, heightMeasureSpec);
			ViewGroup.LayoutParams params = getLayoutParams();
			params.height = getMeasuredHeight();
		} else {
			super.onMeasure(widthMeasureSpec, heightMeasureSpec);
		}
	}
	public void setExpanded(boolean expanded) {
		this.expanded = expanded;
	}
}
