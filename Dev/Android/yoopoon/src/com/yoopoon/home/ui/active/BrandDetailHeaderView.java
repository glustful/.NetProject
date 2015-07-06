package com.yoopoon.home.ui.active;

import org.androidannotations.annotations.EViewGroup;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

import android.app.Activity;
import android.content.Context;
import android.util.AttributeSet;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;
@EViewGroup(R.layout.brand_detail_header_view)
public class BrandDetailHeaderView extends LinearLayout {
	@ViewById(R.id.img)
	ImageView img;
	
	@ViewById(R.id.content)
	TextView content;
	public BrandDetailHeaderView(Context context, AttributeSet attrs,
			int defStyle) {
		super(context, attrs, defStyle);
		// TODO Auto-generated constructor stub
	}

	public BrandDetailHeaderView(Context context, AttributeSet attrs) {
		super(context, attrs);
		// TODO Auto-generated constructor stub
	}

	public BrandDetailHeaderView(Context context) {
		super(context);
		// TODO Auto-generated constructor stub
	
	}

	public void init(JSONObject jsonObject){
		img.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, MyApplication.getInstance().getDeviceInfo((Activity)getContext()).heightPixels/5));
		String url = getContext().getString(R.string.url_host_img)+jsonObject.optString("Bimg");
		img.setTag(url);
		ImageLoader.getInstance().displayImage(url, img,MyApplication.getOptions(),MyApplication.getLoadingListener());
	}
	
	public void setContent(String content){
		this.content.setText(content);
	}
}
