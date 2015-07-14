/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FramBrokerTakeGuest.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.home 
 * @Description: TODO
 * @author: king  
 * @updater: king 
 * @date: 2015年7月14日 上午11:45:48 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.home;

import java.util.Calendar;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.Context;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;

import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: BrokerTakeGuestActivity
 * @Description: 经纪人带客类
 * @author: 徐阳会
 * @date: 2015年7月14日 下午2:09:45
 */
@EActivity(R.layout.broker_take_guest_view)
public class BrokerTakeGuestActivity extends MainActionBarActivity implements OnClickListener {
	private static final String TAG = "BrokerTakeGuestActivity:经纪人带客类";
	// ///////////////////////////////////如下是变量和属性的初始化///////////////////////////////////
	@Extra
	String intent_properString;
	@Extra
	String intent_propretyTypeString;
	@ViewById(R.id.intent_property)
	EditText intent_propertyEditText;
	@ViewById(R.id.intent_property_type)
	EditText intent_property_typeEditText;
	@ViewById(R.id.guest_name)
	EditText guest_nameEditText;
	@ViewById(R.id.phone_number)
	EditText phone_numberEditText;
	@ViewById(R.id.reservation_time)
	EditText reservation_timeEditText;
	@ViewById(R.id.detail)
	EditText detailEditText;
	@ViewById(R.id.commit_broker_carry_client)
	Button commit_broker_carry_client;
	Context mContext;
	// 时间相干的变量
	private int year;
	private int month;
	private int day;
	private final Calendar calender = Calendar.getInstance();

	// ///////////////////////////////////如上是变量和属性的初始化///////////////////////////////////
	/**
	 * @Title: initUI
	 * @Description: 初始化UI布局和控件
	 */
	@AfterViews
	void initUI() {
		mContext = this;
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("带客");
		// 获取从Adapter中获得的数据,对页面中的楼盘名称和楼盘类型进行初始化,同时设置楼盘的名称和楼盘的类型是不可编辑的.
		intent_propertyEditText.setText(intent_properString);
		intent_propertyEditText.setFocusable(false);
		intent_property_typeEditText.setText(intent_propretyTypeString);
		intent_property_typeEditText.setFocusable(false);
		// 添加"提交信息"按钮的事件绑定
		commit_broker_carry_client.setOnClickListener(this);
		reservation_timeEditText.setOnClickListener(this);
	}
	/**
	 * @Title: initCalendar
	 * @Description: 初始化日期对话框
	 */
	private void initCalendar() {
		DialogFragment newFragment = new DatePickerFragment();
		newFragment.show(getFragmentManager(), "datePicker");
	}
	private void commitInfo() {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1) {
						return;
					}
					for (int i = 0; i < list.length(); i++) {
					}
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
			// 不要忘记JSON参数设置
		}.setUrl(getString(R.string.url_brokerlead_client)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}
	/**
	 * @Title: serializationCommitinfo
	 * @Description: 将传输到服务器的数据进行序列化
	 */
	private void serializationCommitinfo() {
	}
	/*
	 * (non Javadoc)
	 * @Title: backButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#backButtonClick(android.view.View)
	 */
	@Override
	public void backButtonClick(View v) {
		finish();
	}
	/*
	 * (non Javadoc)f
	 * @Title: titleButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#titleButtonClick(android.view.View)
	 */
	@Override
	public void titleButtonClick(View v) {
	}
	/*
	 * (non Javadoc)
	 * @Title: rightButtonClick
	 * @Description: TODO
	 * @param v
	 * @see com.yoopoon.home.MainActionBarActivity#rightButtonClick(android.view.View)
	 */
	@Override
	public void rightButtonClick(View v) {
	}
	/*
	 * (non Javadoc)
	 * @Title: showHeadView
	 * @Description: TODO
	 * @return
	 * @see com.yoopoon.home.MainActionBarActivity#showHeadView()
	 */
	@Override
	public Boolean showHeadView() {
		return true;
	}
	/*
	 * (non Javadoc)
	 * @Title: onClick
	 * @Description: TODO
	 * @param v
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.reservation_time:
				initCalendar();
				break;
			case R.id.commit_broker_carry_client:
				// serializationCommitinfo();
				// commitInfo();
				break;
		}
	}

	/**
	 * @ClassName: DatePickerFragment
	 * @Description: 完成时间对话框的设置
	 * @author: 徐阳会
	 * @date: 2015年7月14日 下午6:34:07
	 */
	class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
		@Override
		public Dialog onCreateDialog(Bundle savedInstanceState) {
			// Use the current date as the default date in the picker
			final Calendar c = Calendar.getInstance();
			int year = c.get(Calendar.YEAR);
			int month = c.get(Calendar.MONTH);
			int day = c.get(Calendar.DAY_OF_MONTH);
			// Create a new instance of DatePickerDialog and return it
			return new DatePickerDialog(getActivity(), this, year, month, day);
		}
		public void onDateSet(DatePicker view, int year, int month, int day) {
			int tempMonth = month + 1;
			reservation_timeEditText.setText(year + "/" + tempMonth + "/" + day + "");
		}
	}
}
