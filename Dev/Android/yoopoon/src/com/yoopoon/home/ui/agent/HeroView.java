package com.yoopoon.home.ui.agent;

import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;

import com.yoopoon.home.R;

import android.content.Context;
import android.util.AttributeSet;
import android.view.View;
import android.widget.LinearLayout;
import android.widget.TextView;
@EViewGroup(R.layout.hero_head_view)
public class HeroView extends LinearLayout {
	@ViewById(R.id.content)
	TextView content;
	public HeroView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		// TODO Auto-generated constructor stub
	}

	public HeroView(Context context, AttributeSet attrs) {
		super(context, attrs);
		// TODO Auto-generated constructor stub
	}

	public HeroView(Context context) {
		super(context);
		// TODO Auto-generated constructor stub
	}

	public HeroView setText(String string) {
		content.setText(string);
		return this;
	}

	public View setTextColor(int color) {
		content.setTextColor(color);
		return this;
	}

}
