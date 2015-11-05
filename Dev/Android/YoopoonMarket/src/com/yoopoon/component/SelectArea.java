/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SelectArea.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.component 
 * @Description: TODO
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年9月8日 上午11:14:58 
 * @version: V1.0   
 */
package com.yoopoon.component;

import java.net.URL;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.Toast;

import com.yoopoon.market.R;

/**
 * @ClassName: SelectArea
 * @Description:
 * @author: 徐阳会
 * @date: 2015年9月8日 上午11:14:58
 */
public class SelectArea {
	private View			selectAreaView;
	private Button			selectAreaButton;
	private String			areaNameString;
	private URL				url;
	private Context			mContext;
	private LayoutInflater	inflater;
	
	public String getAreaNameString() {
		return areaNameString;
	}
	public void setAreaNameString(String areaNameString) {
		this.areaNameString = areaNameString;
	}
	public SelectArea(Context context, URL url) {
		mContext = context;
		this.url = url;
		inflater = LayoutInflater.from(mContext);
		//解析出负责选择地区的view布局
		selectAreaView = inflater.inflate(R.layout.select_area_view, null);
		selectAreaButton = (Button) selectAreaView.findViewById(R.id.select_area_button);
		selectAreaButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				selectAreaButton.setText("金尚国际A座");
				Toast.makeText(mContext, "Click The select button", Toast.LENGTH_SHORT).show();
			}
		});
	}
}
