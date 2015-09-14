/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-21 下午1:44:45 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

import android.util.Log;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestContentType;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.domain.Broker2.RequesListener;

/**
 * @ClassName: BrokerEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-21 下午1:44:45
 */
public class BrokerEntity {
	public static final String TAG = "BrokerEntity";
	public String Address;
	public String Addtime;
	public int Adduser;
	public String Agentlevel;
	public int Amount;
	public String Brokername;
	public String Email;
	public String Headphoto;
	public String Hidm;
	public int Id;
	public int LevelId;
	public String MobileYzm;
	public String Nickname;
	public int PartnersId;
	public String PartnersName;
	public String Password;
	public String Phone;
	public int Qq;
	public String Realname;
	public String Regtime;
	public String Regtime1;
	public boolean Remember;
	public String SecondPassword;
	public String Sexy;
	public String Sfz;
	public String SfzPhoto;
	public int State;
	public int Status;
	public int Totalpoints;
	public String Type;
	public int UcId;
	public String Uptime;
	public int UserId;
	public String UserName;
	public int UserType;
	public String Usertype1;
	public String WeiXinNumber;
	public int Zip;
	public String inviteCode;
	public String rgtime;
	public String strState;
	public int IsInvite;
	public int Upuser;
	public String code;

	@Override
	public String toString() {
		return "BrokerEntity [Address=" + Address + ", Addtime=" + Addtime + ", Adduser=" + Adduser + ", Agentlevel="
				+ Agentlevel + ", Amount=" + Amount + ", Brokername=" + Brokername + ", Email=" + Email
				+ ", Headphoto=" + Headphoto + ", Hidm=" + Hidm + ", Id=" + Id + ", LevelId=" + LevelId
				+ ", MobileYzm=" + MobileYzm + ", Nickname=" + Nickname + ", PartnersId=" + PartnersId
				+ ", PartnersName=" + PartnersName + ", Password=" + Password + ", Phone=" + Phone + ", Qq=" + Qq
				+ ", Realname=" + Realname + ", Regtime=" + Regtime + ", Regtime1=" + Regtime1 + ", Remember="
				+ Remember + ", SecondPassword=" + SecondPassword + ", Sexy=" + Sexy + ", Sfz=" + Sfz + ", SfzPhoto="
				+ SfzPhoto + ", State=" + State + ", Status=" + Status + ", Totalpoints=" + Totalpoints + ", Type="
				+ Type + ", UcId=" + UcId + ", Uptime=" + Uptime + ", UserId=" + UserId + ", UserName=" + UserName
				+ ", UserType=" + UserType + ", Usertype1=" + Usertype1 + ", WeiXinNumber=" + WeiXinNumber + ", Zip="
				+ Zip + ", inviteCode=" + inviteCode + ", rgtime=" + rgtime + ", strState=" + strState + ", IsInvite="
				+ IsInvite + ", Upuser=" + Upuser + ", code=" + code + "]";
	}

	public String getAddress() {
		return Address;
	}

	public void setAddress(String address) {
		Address = address;
	}

	public String getAddtime() {
		return Addtime;
	}

	public void setAddtime(String addtime) {
		Addtime = addtime;
	}

	public int getAdduser() {
		return Adduser;
	}

	public void setAdduser(int adduser) {
		Adduser = adduser;
	}

	public String getAgentlevel() {
		return Agentlevel;
	}

	public void setAgentlevel(String agentlevel) {
		Agentlevel = agentlevel;
	}

	public int getAmount() {
		return Amount;
	}

	public void setAmount(int amount) {
		Amount = amount;
	}

	public String getBrokername() {
		return Brokername;
	}

	public void setBrokername(String brokername) {
		Brokername = brokername;
	}

	public String getEmail() {
		return Email;
	}

	public void setEmail(String email) {
		Email = email;
	}

	public String getHeadphoto() {
		return Headphoto;
	}

	public void setHeadphoto(String headphoto) {
		Headphoto = headphoto;
	}

	public String getHidm() {
		return Hidm;
	}

	public void setHidm(String hidm) {
		Hidm = hidm;
	}

	public int getId() {
		return Id;
	}

	public void setId(int id) {
		Id = id;
	}

	public int getLevelId() {
		return LevelId;
	}

	public void setLevelId(int levelId) {
		LevelId = levelId;
	}

	public String getMobileYzm() {
		return MobileYzm;
	}

	public void setMobileYzm(String mobileYzm) {
		MobileYzm = mobileYzm;
	}

	public String getNickname() {
		return Nickname;
	}

	public void setNickname(String nickname) {
		Nickname = nickname;
	}

	public int getPartnersId() {
		return PartnersId;
	}

	public void setPartnersId(int partnersId) {
		PartnersId = partnersId;
	}

	public String getPartnersName() {
		return PartnersName;
	}

	public void setPartnersName(String partnersName) {
		PartnersName = partnersName;
	}

	public String getPassword() {
		return Password;
	}

	public void setPassword(String password) {
		Password = password;
	}

	public String getPhone() {
		return Phone;
	}

	public void setPhone(String phone) {
		Phone = phone;
	}

	public int getQq() {
		return Qq;
	}

	public void setQq(int qq) {
		Qq = qq;
	}

	public String getRealname() {
		return Realname;
	}

	public void setRealname(String realname) {
		Realname = realname;
	}

	public String getRegtime() {
		return Regtime;
	}

	public void setRegtime(String regtime) {
		Regtime = regtime;
	}

	public String getRegtime1() {
		return Regtime1;
	}

	public void setRegtime1(String regtime1) {
		Regtime1 = regtime1;
	}

	public boolean isRemember() {
		return Remember;
	}

	public void setRemember(boolean remember) {
		Remember = remember;
	}

	public String getSecondPassword() {
		return SecondPassword;
	}

	public void setSecondPassword(String secondPassword) {
		SecondPassword = secondPassword;
	}

	public String getSexy() {
		return Sexy;
	}

	public void setSexy(String sexy) {
		Sexy = sexy;
	}

	public String getSfz() {
		return Sfz;
	}

	public void setSfz(String sfz) {
		Sfz = sfz;
	}

	public String getSfzPhoto() {
		return SfzPhoto;
	}

	public void setSfzPhoto(String sfzPhoto) {
		SfzPhoto = sfzPhoto;
	}

	public int getState() {
		return State;
	}

	public void setState(int state) {
		State = state;
	}

	public int getStatus() {
		return Status;
	}

	public void setStatus(int status) {
		Status = status;
	}

	public int getTotalPoints() {
		return Totalpoints;
	}

	public void setTotalPoints(int totalPoints) {
		Totalpoints = totalPoints;
	}

	public String getType() {
		return Type;
	}

	public void setType(String type) {
		Type = type;
	}

	public int getUcId() {
		return UcId;
	}

	public void setUcId(int ucId) {
		UcId = ucId;
	}

	public String getUptime() {
		return Uptime;
	}

	public void setUptime(String uptime) {
		Uptime = uptime;
	}

	public int getUserId() {
		return UserId;
	}

	public void setUserId(int userId) {
		UserId = userId;
	}

	public String getUserName() {
		return UserName;
	}

	public void setUserName(String userName) {
		UserName = userName;
	}

	public int getUserType() {
		return UserType;
	}

	public void setUserType(int userType) {
		UserType = userType;
	}

	public String getUserType1() {
		return Usertype1;
	}

	public void setUserType1(String userType1) {
		Usertype1 = userType1;
	}

	public String getWeiXinNumber() {
		return WeiXinNumber;
	}

	public void setWeiXinNumber(String weiXinNumber) {
		WeiXinNumber = weiXinNumber;
	}

	public int getZip() {
		return Zip;
	}

	public void setZip(int zip) {
		Zip = zip;
	}

	public String getInviteCode() {
		return inviteCode;
	}

	public void setInviteCode(String inviteCode) {
		this.inviteCode = inviteCode;
	}

	public String getRgtime() {
		return rgtime;
	}

	public void setRgtime(String rgtime) {
		this.rgtime = rgtime;
	}

	public String getStrState() {
		return strState;
	}

	public void setStrState(String strState) {
		this.strState = strState;
	}

	public int getIsInvite() {
		return IsInvite;
	}

	public void setIsInvite(int isInvite) {
		IsInvite = isInvite;
	}

	public int getUpUser() {
		return Upuser;
	}

	public void setUpUser(int upUser) {
		Upuser = upUser;
	}

	public String getCode() {
		return code;
	}

	public void setCode(String code) {
		this.code = code;
	}

	public static String getTag() {
		return TAG;
	}

	public void modifyInfo(final RequesListener listener) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					String json = om.writeValueAsString(BrokerEntity.this);
					return json;
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
				requestModifyInfo(serializeResult, listener);
			}
		}).execute();
	}

	protected void requestModifyInfo(String serializeResult, final RequesListener lis) {
		Log.i(TAG, serializeResult);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				boolean status = data.getMRootData().optBoolean("Status", false);
				String msg = data.getMsg();
				if (status) {
					lis.succeed(msg);
				} else {
					lis.fail(msg);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_update_brokerinfo))
				.setRequestContentType(RequestContentType.eJSON).SetJSON(serializeResult).notifyRequest();
	}
}
