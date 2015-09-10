package com.yoopoon.market.view;


import android.content.Context;
import android.util.AttributeSet;
import android.widget.GridView;

public class MyGridView extends GridView {
 public MyGridView(Context context, AttributeSet attrs) {
  super(context, attrs);
 }

public MyGridView(Context context) {
  super(context);
 }

public MyGridView(Context context, AttributeSet attrs, int defStyle) {
  super(context, attrs, defStyle);
 }

@Override
 public void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {

 //重新排大小
 int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2,
    MeasureSpec.AT_MOST);
//	 int expandSpec = MeasureSpec.makeMeasureSpec(MeasureSpec.UNSPECIFIED,
//			    MeasureSpec.UNSPECIFIED);
  super.onMeasure(widthMeasureSpec, expandSpec);
 }

}

