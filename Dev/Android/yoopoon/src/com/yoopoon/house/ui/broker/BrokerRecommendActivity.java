/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FramBrokerTakeGuest.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.home 
 * @Description: 经纪人带客,红包,推荐和积分等业务
 * @author: 徐阳会  
 * @updater: 徐阳会 
 * @date: 2015年7月14日 上午11:45:48 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.broker;

import java.util.Calendar;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;

import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.Toast;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: BrokerTakeGuestActivity
 * @Description: 经纪人推荐类
 * @author: 徐阳会
 * @date: 2015年7月14日 下午2:09:45
 */
@EActivity(R.layout.activity_broker_recommend)
public class BrokerRecommendActivity extends MainActionBarActivity implements OnClickListener {
	private static final String TAG = "BrokerRecommendActivity:经纪人推荐类";
	// ///////////////////////////////////如下是变量和属性的初始化///////////////////////////////////
	@Extra
	String intent_properString;
	@Extra
	String intent_propretyTypeString;
	@Extra
	String intent_propretyNumber;
	@Extra
	@ViewById(R.id.intent_property)
	EditText intent_propertyEditText;
	@ViewById(R.id.intent_property_type)
	EditText intent_property_typeEditText;
	@ViewById(R.id.guest_name)
	EditText guest_nameEditText;
	@ViewById(R.id.phone_number)
	EditText phone_numberEditText;
	@ViewById(R.id.reservation_time)
	Button reservation_timeButton;
	@ViewById(R.id.detail)
	EditText detailEditText;
	@ViewById(R.id.commit_broker_recommend)
	Button commit_broker_recommend;
	@ViewById(R.id.delete_guest_name)
	ImageButton deleteGuestNameButton;
	@ViewById(R.id.delete_guest_phone_number)
	ImageButton deleteGuestPhoneImageButton;
	@ViewById(R.id.delete_detail)
	ImageButton deleteGuestDetail;
	Context mContext;
	// 时间相干的变量
	// 系列化后得到的经纪人带客Json数据
	String BrokerTakeGuestJson = null;
	
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
		titleButton.setText("推荐");
		// 获取从Adapter中获得的数据,对页面中的楼盘名称和楼盘类型进行初始化,同时设置楼盘的名称和楼盘的类型是不可编辑的.
		intent_propertyEditText.setText(intent_properString);
		intent_propertyEditText.setFocusable(false);
		intent_property_typeEditText.setText(intent_propretyTypeString);
		intent_property_typeEditText.setFocusable(false);
		// 添加"提交信息"按钮的事件绑定
		commit_broker_recommend.setOnClickListener(this);
		reservation_timeButton.setOnClickListener(this);
		guest_nameEditText.addTextChangedListener(deleteGuestNameWatcher);
		phone_numberEditText.addTextChangedListener(deleteGuestPhoneWatcher);
		detailEditText.addTextChangedListener(deleteDetailWatcher);
	}
	
	//创建监听删除客户姓名的右侧删除小图标监听
	private TextWatcher deleteGuestNameWatcher = new TextWatcher() {
		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {
			deleteGuestNameButton.setVisibility(View.VISIBLE);
		}
		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {
		}
		@Override
		public void afterTextChanged(Editable s) {
			String guestNameString = guest_nameEditText.getText().toString();
			if (guestNameString.equals("")) {
				deleteGuestNameButton.setVisibility(View.GONE);
			}
		}
	};
	//创建监听删除客户联系电话的右侧删除小图标监听
	private TextWatcher deleteGuestPhoneWatcher = new TextWatcher() {
		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {
			deleteGuestPhoneImageButton.setVisibility(View.VISIBLE);
		}
		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {
		}
		@Override
		public void afterTextChanged(Editable s) {
			String guestPhoneNumberString = phone_numberEditText.getText().toString();
			if (guestPhoneNumberString.equals("")) {
				deleteGuestPhoneImageButton.setVisibility(View.GONE);
			}
		}
	};
	//创建监听删除备注的右侧删除小图标监听
	private TextWatcher deleteDetailWatcher = new TextWatcher() {
		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {
			deleteGuestDetail.setVisibility(View.VISIBLE);
		}
		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {
		}
		@Override
		public void afterTextChanged(Editable s) {
			String guestDetailString = detailEditText.getText().toString();
			if (guestDetailString.equals("")) {
				deleteGuestDetail.setVisibility(View.GONE);
			}
		}
	};
	
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
					JSONObject jsonObject = data.getMRootData();
					String messageString = jsonObject.optString("Msg");
					if (messageString.equals("提交成功")) {
						Toast.makeText(mContext, messageString, Toast.LENGTH_SHORT).show();
						finish();
					} else {
						Toast.makeText(mContext, messageString, Toast.LENGTH_SHORT).show();
					}
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_brokerlead_client)).setRequestMethod(RequestMethod.ePost)
				.SetJSON(BrokerTakeGuestJson).notifyRequest();
	}
	/**
	 * @Title: serializationCommitinfo
	 * @Description: 将传输到服务器的数据进行序列化
	 */
	private void serializationCommitinfo() {
		// 从SharePreferences中获取当前用户的ID
		SharedPreferences preferences = getSharedPreferences("com.yoopoon.home_preferences", 0);
		String userIdString = preferences.getString("userId", "没有获取到数据");
		// 初始化传入到服务器的参数
		String AddUser = userIdString;
		String Appointmenttime = reservation_timeButton.getText().toString();
		String Clientname = guest_nameEditText.getText().toString();
		String HouseType = intent_property_typeEditText.getText().toString();
		String House = intent_propertyEditText.getText().toString();
		String Note = detailEditText.getText().toString();
		String Phone = phone_numberEditText.getText().toString();
		String Projectid = intent_propretyNumber;
		BrokerTakeGuest brokerTakeGuest = new BrokerTakeGuest(AddUser, Appointmenttime, Clientname, HouseType, House,
				Note, Phone, Projectid);
		ObjectMapper mapper = new ObjectMapper();
		try {
			if (mapper.writeValueAsString(brokerTakeGuest) != null) {
				BrokerTakeGuestJson = mapper.writeValueAsString(brokerTakeGuest);
			}
		} catch (JsonProcessingException e) {
			e.printStackTrace();
		}
	}
	/** 
	 * @Title: deleteGuestClick 
	 * @Description: 添加编辑顾客姓名右侧的删除小图标点击事件
	 * @param view
	 */
	@Click(R.id.delete_guest_name)
	void deleteGuestClick(View view) {
		guest_nameEditText.setText("");
	}
	/** 
	 * @Title: deleteGuestPhoneClick 
	 * @Description: 添加编辑顾客手机号码右侧的删除小图标点击事件
	 * @param view
	 */
	@Click(R.id.delete_guest_phone_number)
	void deleteGuestPhoneClick(View view) {
		phone_numberEditText.setText("");
	}
	/** 
	 * @Title: deleteGuestDetailClick 
	 * @Description: 添加编辑备注右侧的删除小图标点击事件
	 * @param view
	 */
	@Click(R.id.delete_detail)
	void deleteGuestDetailClick(View view) {
		detailEditText.setText("");
	}
	@Override
	public void backButtonClick(View v) {
		finish();
	}
	@Override
	public void titleButtonClick(View v) {
	}
	@Override
	public void rightButtonClick(View v) {
	}
	@Override
	public Boolean showHeadView() {
		return true;
	}
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.reservation_time:
				initCalendar();
				break;
			case R.id.commit_broker_carry_client:
				serializationCommitinfo();
				commitInfo();
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
			reservation_timeButton.setText(year + "/" + tempMonth + "/" + day + "");
		}
	}
	
	/**
	 * @ClassName: BrokerTakeGuest
	 * @Description: 传送到服务器的经纪人带客数据类
	 * @author: 徐阳会
	 * @date: 2015年7月15日 上午10:48:29
	 */
	class BrokerTakeGuest {
		private String AddUser;
		private String Appointmenttime;
		private String Broker;
		private String Brokername;
		private String ClientInfo;
		private String Clientname;
		private String HouseType;
		private String Houses;
		private String Note;
		private String Phone;
		private String Projectid;
		private String Projectname;
		private String Stats;
		
		/**
		 * @return the addUser
		 */
		public String getAddUser() {
			return AddUser;
		}
		/**
		 * @param addUser the addUser to set
		 */
		public void setAddUser(String addUser) {
			AddUser = addUser;
		}
		/**
		 * @return the appointmenttime
		 */
		public String getAppointmenttime() {
			return Appointmenttime;
		}
		/**
		 * @param appointmenttime the appointmenttime to set
		 */
		public void setAppointmenttime(String appointmenttime) {
			Appointmenttime = appointmenttime;
		}
		/**
		 * @return the broker
		 */
		public String getBroker() {
			return Broker;
		}
		/**
		 * @param broker the broker to set
		 */
		public void setBroker(String broker) {
			Broker = broker;
		}
		/**
		 * @return the brokername
		 */
		public String getBrokername() {
			return Brokername;
		}
		/**
		 * @param brokername the brokername to set
		 */
		public void setBrokername(String brokername) {
			Brokername = brokername;
		}
		/**
		 * @return the clientInfo
		 */
		public String getClientInfo() {
			return ClientInfo;
		}
		/**
		 * @param clientInfo the clientInfo to set
		 */
		public void setClientInfo(String clientInfo) {
			ClientInfo = clientInfo;
		}
		/**
		 * @return the clientname
		 */
		public String getClientname() {
			return Clientname;
		}
		/**
		 * @param clientname the clientname to set
		 */
		public void setClientname(String clientname) {
			Clientname = clientname;
		}
		/**
		 * @return the houseType
		 */
		public String getHouseType() {
			return HouseType;
		}
		/**
		 * @param houseType the houseType to set
		 */
		public void setHouseType(String houseType) {
			HouseType = houseType;
		}
		/**
		 * @return the houses
		 */
		public String getHouses() {
			return Houses;
		}
		/**
		 * @param houses the houses to set
		 */
		public void setHouses(String houses) {
			Houses = houses;
		}
		/**
		 * @return the note
		 */
		public String getNote() {
			return Note;
		}
		/**
		 * @param note the note to set
		 */
		public void setNote(String note) {
			Note = note;
		}
		/**
		 * @return the phone
		 */
		public String getPhone() {
			return Phone;
		}
		/**
		 * @param phone the phone to set
		 */
		public void setPhone(String phone) {
			Phone = phone;
		}
		/**
		 * @return the projectid
		 */
		public String getProjectid() {
			return Projectid;
		}
		/**
		 * @param projectid the projectid to set
		 */
		public void setProjectid(String projectid) {
			Projectid = projectid;
		}
		/**
		 * @return the projectname
		 */
		public String getProjectname() {
			return Projectname;
		}
		/**
		 * @param projectname the projectname to set
		 */
		public void setProjectname(String projectname) {
			Projectname = projectname;
		}
		/**
		 * @return the stats
		 */
		public String getStats() {
			return Stats;
		}
		/**
		 * @param stats the stats to set
		 */
		public void setStats(String stats) {
			Stats = stats;
		}
		public BrokerTakeGuest(String AddUser, String Appointmenttime, String Clientname, String HouseType,
				String House, String Note, String Phone, String Projectid) {
			this.AddUser = AddUser;
			this.Appointmenttime = Appointmenttime;
			this.Broker = AddUser;
			this.Brokername = "";
			this.ClientInfo = AddUser;
			this.Clientname = Clientname;
			this.HouseType = HouseType;
			this.Houses = House;
			this.Note = Note;
			this.Phone = Phone;
			this.Projectid = Projectid;
			this.Projectname = House;
			this.Stats = "0";
		}
	}
}
