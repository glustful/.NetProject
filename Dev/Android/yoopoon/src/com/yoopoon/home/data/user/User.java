package com.yoopoon.home.data.user;

import java.io.IOException;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.widget.Toast;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

public class User
{
	
	 public int Id ;
     public String UserName ;
     public String Password ;

     public boolean Remember ;

     public String Phone ;
     public int Status ;
     

	public int getId() {
		return Id;
	}


	public void setId(int id) {
		Id = id;
	}


	public String getUserName() {
		return UserName;
	}


	public void setUserName(String userName) {
		UserName = userName;
	}


	public String getPassword() {
		return Password;
	}


	public void setPassword(String password) {
		Password = password;
	}


	public boolean isRemember() {
		return Remember;
	}


	public void setRemember(boolean remember) {
		Remember = remember;
	}


	public String getPhone() {
		return Phone;
	}


	public void setPhone(String phone) {
		Phone = phone;
	}


	public int getStatus() {
		return Status;
	}


	public void setStatus(int status) {
		Status = status;
	}


	public static User lastLoginUser(Context context) {
		MyApplication.getInstance();
		SharedPreferences spf = PreferenceManager.getDefaultSharedPreferences(context);
		String user = spf.getString("user", null);
		if(user==null||user.equals(""))
		return null;
		ObjectMapper om = new ObjectMapper();
		try {
			return om.readValue(user, User.class);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} 
		return null;
	}


	public void login(final LoginListener listener) {
		new SerializerJSON(new SerializeListener() {
			
			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					return om.writeValueAsString(User.this);
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return null;
			}
			
			@Override
			public void onComplete(String serializeResult) {
				if(serializeResult==null || serializeResult.equals("")){
					
					return;
				}
				requestLogin(serializeResult,listener);
				
			}
		}).execute();
		
	}


	protected void requestLogin(String serializeResult,final LoginListener lis) {
		new RequestAdapter() {
			
			@Override
			public void onReponse(ResponseData data) {
				if(data.getResultState()==ResultState.eSuccess){
					lis.success(User.this);
				}else{
					lis.faild(data.getMsg());
				}
			}
			
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
				
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_login))
		.SetJSON(serializeResult)
		.setSaveSession(true)
		.notifyRequest();
		
	}

	public interface LoginListener{
		void success(User user);
		void faild(String msg);
	}
}
