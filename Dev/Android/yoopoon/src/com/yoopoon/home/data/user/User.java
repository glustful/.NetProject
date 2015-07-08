package com.yoopoon.home.data.user;

import java.util.ArrayList;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

@JsonIgnoreProperties(ignoreUnknown = true)
public class User {

	public int id;

	public String userName;
	public String password;

	public boolean remember;

	public String phone;
	public int status;
	public ArrayList<Role> roles;
	@JsonIgnore
	public static User mUser;

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getUserName() {
		return userName;
	}

	public void setUserName(String userName) {
		this.userName = userName;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public boolean isRemember() {
		return remember;
	}

	public void setRemember(boolean remember) {
		this.remember = remember;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public int getStatus() {
		return status;
	}

	public void setStatus(int status) {
		this.status = status;
	}

	public ArrayList<Role> getRoles() {
		return roles;
	}

	public void setRoles(ArrayList<Role> roles) {
		this.roles = roles;
	}

	@SuppressLint("DefaultLocale")
	public boolean isBroker() {
		if (this.roles == null || this.roles.size() < 1)
			return false;
		for (Role r : roles) {
			if (r.roleName.toLowerCase().equals("broker")) {
				return true;
			}
		}
		return false;
	}

	public static User lastLoginUser(Context context) {
		if (mUser != null)
			return mUser;
		SharedPreferences spf = PreferenceManager.getDefaultSharedPreferences(context);
		String user = spf.getString("user", null);
		if (user == null || user.equals(""))
			return null;
		ObjectMapper om = new ObjectMapper();
		try {
			mUser = om.readValue(user, User.class);
			return mUser;
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
				if (serializeResult == null || serializeResult.equals("")) {

					return;
				}

				requestLogin(serializeResult, listener);

			}
		}).execute();

	}

	protected void requestLogin(String serializeResult, final LoginListener lis) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONObject obj = data.getJsonObject();
					if (obj.isNull("Roles") || obj.optJSONArray("Roles").length() < 1)
						setRoles(null);
					else {
						ArrayList<Role> roles = new ArrayList<Role>();
						for (int i = 0; i < obj.optJSONArray("Roles").length(); i++) {
							Role r = new Role();
							r.roleName = obj.optJSONArray("Roles").optJSONObject(i).optString("RoleName");
							roles.add(r);
						}
						setRoles(roles);
					}
					lis.success(User.this);
				} else {
					lis.faild(data.getMsg());
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_login)).SetJSON(serializeResult)
				.setSaveSession(true).notifyRequest();

	}

	public interface LoginListener {
		void success(User user);

		void faild(String msg);
	}
}
