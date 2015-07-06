package com.yoopoon.home.ui.home;

import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.util.AttributeSet;
import android.view.View;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.yoopoon.home.R;
import com.yoopoon.home.ui.view.BadgeView;

@EViewGroup(R.layout.main_tab_view)
public class MainTabView extends LinearLayout{

	@ViewById(R.id.imageView)
	ImageView imageView;
	@ViewById(R.id.badgeView)
	BadgeView badgeView;
	@ViewById(R.id.title)
	TextView title;
	
	public MainTabView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
	}

	public MainTabView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	public MainTabView(Context context) {
		super(context);
	}
	
	public void setIndicator(int resId){

		imageView.setImageResource(resId);
		badgeView.setVisibility(View.GONE);

	}
	
	public void setTitle(int resid){
		title.setText(resid);
	}
	
	public void setTitle(String title){
		this.title.setText(title);
	}
	
	public void setBageViewCount(long num){
		badgeView.setVisibility(num == 0 ? View.GONE : View.VISIBLE);
		badgeView.setText(String.valueOf(num));
		if (num > 99) {
			badgeView.setText("99+");
		}
	}
	
	public void showBageViewCount(boolean visiable){
		badgeView.setText("");
		RelativeLayout.LayoutParams params = (android.widget.RelativeLayout.LayoutParams) badgeView.getLayoutParams();
		params.height = (int) getContext().getResources().getDimension(R.dimen.main_tab_bageSize);
		params.width = (int) getContext().getResources().getDimension(R.dimen.main_tab_bageSize);
		badgeView.setLayoutParams(params);
		badgeView.setVisibility(visiable ? View.VISIBLE : View.GONE);
	}
	
	
	public void setIndicator(Drawable icon){
//		imageView.set
	}

}
