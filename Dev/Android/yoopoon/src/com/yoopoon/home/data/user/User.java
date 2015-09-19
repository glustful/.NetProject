package com.yoopoon.home.data.user;

import java.io.IOException;
import java.util.ArrayList;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.preference.PreferenceManager;
import android.util.Log;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.Tools;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.ParserJSON;
import com.yoopoon.home.data.json.ParserJSON.ParseListener;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.domain.BrokerEntity;
import com.yoopoon.home.domain.PartnerList;

@JsonIgnoreProperties(ignoreUnknown = true)
public class User {
	private static final String TAG = "User";
	public int id;
	public String userName;
	public String password;
	public String nickName;
	public String sex;
	public String idCard;
	public String realName;
	private String email;
	private String headUrl;
	// 微信号码
	private String weiXin;
	// 邀请码
	private String invitationCode;
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

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
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

	public String getNickName() {
		return nickName;
	}

	public void setNickName(String nickName) {
		this.nickName = nickName;
	}

	public String getSex() {
		return sex;
	}

	public void setSex(String sex) {
		this.sex = sex;
	}

	public String getIdCard() {
		return idCard;
	}

	public void setIdCard(String idCard) {
		this.idCard = idCard;
	}

	public String getRealName() {
		return realName;
	}

	public void setRealName(String realName) {
		this.realName = realName;
	}

	public String getHeadUrl() {
		return headUrl;
	}

	public void setHeadUrl(String headUrl) {
		this.headUrl = headUrl;
	}

	public String getWeiXin() {
		return weiXin;
	}

	public void setWeiXin(String weiXin) {
		this.weiXin = weiXin;
	}

	public String getInvitationCode() {
		return invitationCode;
	}

	public void setInvitationCode(String invitationCode) {
		this.invitationCode = invitationCode;
	}

	@Override
	public String toString() {
		return "User [id=" + id + ", userName=" + userName + ", password=" + password + ", nickName=" + nickName
				+ ", sex=" + sex + ",weiXin=" + weiXin + ",invitationCode=" + invitationCode + ", idCard=" + idCard
				+ ", realName=" + realName + ", email=" + email + ", headUrl=" + headUrl + ", remember=" + remember
				+ ", phone=" + phone + ", status=" + status + ", roles=" + roles + "]";
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
		SharedPreferences spf = PreferenceManager.getDefaultSharedPreferences(context);
		String user = spf.getString("user", null);
		if (user == null || user.equals(""))
			return null;
		if (mUser != null)
			return mUser;
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

	public void modifyPsw(final ModifyPswListener lis) {
		new SerializerJSON(new SerializeListener() {
			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					return om.writeValueAsString(User.this);
				} catch (JsonProcessingException e) {
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (serializeResult == null || serializeResult.equals("")) {
					return;
				}
				requestModifyPsw(serializeResult, lis);
			}
		}).execute();
	}

	public void getUserInfo(final UserInfoListener listener) {
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
				requestUserInfo(serializeResult, listener);
			}
		}).execute();
	}

	public void parseToBroker(final String json) {
		new ParserJSON(new ParseListener() {
			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				om.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
				BrokerEntity entity = null;
				try {
					entity = om.readValue(json, BrokerEntity.class);
				} catch (JsonParseException e) {
					Log.i(TAG, "JsonParseException:" + e.getStackTrace());
				} catch (JsonMappingException e) {
					Log.i(TAG, "JsonMappingException:" + e.getMessage());
				} catch (IOException e) {
					Log.i(TAG, "IOException:" + e.getStackTrace());
				}
				return entity;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null)
					Log.i(TAG, parseResult.toString());
			}
		}).execute();
	}

	// Broker: ""
	// BrokerId: 0
	// Id: 0
	// PartnerId: 0
	// Phone: "18313033523"
	// userId: 0
	public void invite(final PartnerList list, final InvitePartnerListener listener) {
		new SerializerJSON(new SerializeListener() {
			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				// String json =
				// "{\"Id\": 0, \"Broker\": \"\", \"PartnerId\": 0, \"userId\": 0, \"BrokerId\": 0, \"Phone\": \"15925149120\"}";
				// return json;
				try {
					return om.writeValueAsString(list);
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
				requestInvite(serializeResult, listener);
			}
		}).execute();
	}

	protected void requestModifyPsw(String serializeResult, final ModifyPswListener lis) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONObject obj = data.getJsonObject();
					if (obj != null) {
						lis.success(data.getMsg());
					} else {
						lis.fail("修改失败，请重试");
					}
				} else {
					lis.fail(data.getMsg());
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_login)).SetJSON(serializeResult).notifyRequest();
	}

	protected void requestInvite(String serializeResult, final InvitePartnerListener lis) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data != null)
					if (data.getResultState() == ResultState.eSuccess) {
						lis.success(data.getMsg());
					} else {
						lis.failed(data.getMsg());
					}
				else
					lis.failed("请求失败，请检查网络");
			}

			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_invite_partner)).SetJSON(serializeResult)
				.notifyRequest();
	}

	protected void requestLogin(String serializeResult, final LoginListener lis) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				if (data.getJsonObject() != null) {
					JSONObject obj = data.getJsonObject();
					User.this.setId(Tools.optInt(obj, "Id", 0));
					User.this.setPhone(User.this.getUserName());
					User.this.setUserName(Tools.optString(obj, "UserName", null));
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
					return;
				}
				lis.faild(data.getMsg());
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_login)).SetJSON(serializeResult)
				.setSaveSession(true).notifyRequest();
	}

	protected void requestUserInfo(String serializeResult, final UserInfoListener lis) {
		SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(MyApplication.getInstance());
		String userId = sp.getString("userId", "0");
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					final JSONObject obj = data.getJsonObject2();
					User.this.setHeadUrl(Tools.optString(obj, "Headphoto", null));
					User.this.setNickName(Tools.optString(obj, "Nickname", null));
					User.this.setIdCard(Tools.optString(obj, "Sfz", null));
					User.this.setWeiXin(Tools.optString(obj, "WeiXinNumber", null));
					User.this.setInvitationCode(Tools.optString(obj, "inviteCode", null));
					User.this.setSex(Tools.optString(obj, "Sexy", null));
					User.this.setRealName(Tools.optString(obj, "Realname", null));
					User.this.setEmail(Tools.optString(obj, "Email", null));
					User.this.setPhone(Tools.optString(obj, "Phone", null));
					// 将拿到的broker信息存储到sp里面
					SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(MyApplication.getInstance());
					Editor editor = sp.edit();
					editor.putString("broker", obj.toString());
					editor.commit();
					if (obj != null) {
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
						lis.failed("获取用户信息失败，请重试");
					}
				} else {
					lis.failed(data.getMsg());
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_brokeInfo_getBrokeInfoById) + userId)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	public interface LoginListener {
		void success(User user);

		void faild(String msg);
	}

	public interface UserInfoListener {
		void success(User user);

		void failed(String msg);
	}

	public interface InvitePartnerListener {
		void success(String msg);

		void failed(String msg);
	}

	public interface ModifyPswListener {
		void success(String msg);

		void fail(String msg);
	}
}
