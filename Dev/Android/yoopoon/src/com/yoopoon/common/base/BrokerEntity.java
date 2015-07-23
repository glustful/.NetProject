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
package com.yoopoon.common.base;

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

	private static final String TAG = "BrokerEntity";

	private String address = null;
	private String addtime = "/Date(-62135596800000)/";
	private int adduser = 0;
	private String agentlevel = null;
	private int amount = 0;
	private String brokername;
	private String email;
	// private String headphoto;
	private String Headphoto;
	private String hidm = null;
	private int id = 2033;
	private int levelId = 0;
	private String mobileYzm = null;
	private String nickname;
	private int partnersId = 0;
	private String partnersName = null;
	private String password = null;
	private String phone;
	private int qq;
	private String realname;
	private String regtime = "/Date(-62135596800000)/";
	private String regtime1 = null;
	private boolean remember = false;
	private String secondPassword = null;
	private String sexy;
	private String sfz;
	private String sfzPhoto = null;
	private int state = 0;
	private int status = 0;
	private int totalPoints = 0;
	private String type = null;
	private int ucId = 0;
	private String uptime = "/Date(-62135596800000)/";
	private int userId = 0;
	private String userName = null;
	private int userType = 0;
	private String userType1 = null;
	private String weiXinNumber = null;
	private int zip = 0;
	private String inviteCode = null;
	private String rgtime = null;
	private String strState = null;

	@Override
	public String toString() {
		return "BrokerEntity [address=" + address + ", addtime=" + addtime + ", adduser=" + adduser + ", agentlevel="
				+ agentlevel + ", amount=" + amount + ", brokername=" + brokername + ", email=" + email
				+ ", headphoto=" + Headphoto + ", hidm=" + hidm + ", id=" + id + ", levelId=" + levelId
				+ ", mobileYzm=" + mobileYzm + ", nickname=" + nickname + ", partnersId=" + partnersId
				+ ", partnersName=" + partnersName + ", password=" + password + ", phone=" + phone + ", qq=" + qq
				+ ", realname=" + realname + ", regtime=" + regtime + ", regtime1=" + regtime1 + ", remember="
				+ remember + ", secondPassword=" + secondPassword + ", sexy=" + sexy + ", sfz=" + sfz + ", sfzPhoto="
				+ sfzPhoto + ", state=" + state + ", status=" + status + ", totalPoints=" + totalPoints + ", type="
				+ type + ", ucId=" + ucId + ", uptime=" + uptime + ", userId=" + userId + ", userName=" + userName
				+ ", userType=" + userType + ", userType1=" + userType1 + ", weiXinNumber=" + weiXinNumber + ", zip="
				+ zip + ", inviteCode=" + inviteCode + ", rgtime=" + rgtime + ", strState=" + strState + "]";
	}

	public String toString2() {
		return "{Address: null,Addtime: \"/Date(-62135596800000)/\",Adduser: 0,Agentlevel: null,Amount: 0,Brokername: \"xuyanghui\",Email: \"xuyanghui@live.com\","
				+ "Headphoto: \"20150721/20150721_100516_222_173.jpg\",Hidm: null,Id: 2033,LevelId: 0,MobileYzm: null,Nickname: \"xuyanghui\",PartnersId: 0,"
				+ "PartnersName: null,Password: null,Phone: \"13508713650\",Qq: 0,Realname: \"徐阳会\",Regtime: \"/Date(-62135596800000)/\",Regtime1: null,"
				+ "Remember: false,SecondPassword: null,Sexy: \"先生\",Sfz: \"530302000000022\",SfzPhoto: null,State: 0,Status: 0,Totalpoints: 0,Type: null,"
				+ "UcId: 0,Uptime: \"/Date(-62135596800000)/\",Upuser: 0,UserId: 0,UserName: null,UserType: 0,Usertype1: null,WeiXinNumber: null,Zip: 0,"
				+ "inviteCode: null,rgtime: null,strState: null}";
	}

	public BrokerEntity() {

	}

	public BrokerEntity(int id, String brokername, String email, String headphoto, String nickname, String phone,
			String realname, String sexy, String sfz) {
		super();
		this.id = id;
		this.brokername = brokername;
		this.email = email;
		this.Headphoto = headphoto;
		this.nickname = nickname;
		this.phone = phone;
		this.realname = realname;
		this.sexy = sexy;
		this.sfz = sfz;
	}

	public String getAddress() {
		return address;
	}

	public void setAddress(String address) {
		this.address = address;
	}

	public String getAddtime() {
		return addtime;
	}

	public void setAddtime(String addtime) {
		this.addtime = addtime;
	}

	public int getAdduser() {
		return adduser;
	}

	public void setAdduser(int adduser) {
		this.adduser = adduser;
	}

	public String getAgentlevel() {
		return agentlevel;
	}

	public void setAgentlevel(String agentlevel) {
		this.agentlevel = agentlevel;
	}

	public int getAmount() {
		return amount;
	}

	public void setAmount(int amount) {
		this.amount = amount;
	}

	public String getBrokername() {
		return brokername;
	}

	public void setBrokername(String brokername) {
		this.brokername = brokername;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getHeadphoto() {
		return Headphoto;
	}

	public void setHeadphoto(String headphoto) {
		// this.headphoto = headphoto;
		this.Headphoto = headphoto;
	}

	public String getHidm() {
		return hidm;
	}

	public void setHidm(String hidm) {
		this.hidm = hidm;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public int getLevelId() {
		return levelId;
	}

	public void setLevelId(int levelId) {
		this.levelId = levelId;
	}

	public String getMobileYzm() {
		return mobileYzm;
	}

	public void setMobileYzm(String mobileYzm) {
		this.mobileYzm = mobileYzm;
	}

	public String getNickname() {
		return nickname;
	}

	public void setNickname(String nickname) {
		this.nickname = nickname;
	}

	public int getPartnersId() {
		return partnersId;
	}

	public void setPartnersId(int partnersId) {
		this.partnersId = partnersId;
	}

	public String getPartnersName() {
		return partnersName;
	}

	public void setPartnersName(String partnersName) {
		this.partnersName = partnersName;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public int getQq() {
		return qq;
	}

	public void setQq(int qq) {
		this.qq = qq;
	}

	public String getRealname() {
		return realname;
	}

	public void setRealname(String realname) {
		this.realname = realname;
	}

	public String getRegtime() {
		return regtime;
	}

	public void setRegtime(String regtime) {
		this.regtime = regtime;
	}

	public String getRegtime1() {
		return regtime1;
	}

	public void setRegtime1(String regtime1) {
		this.regtime1 = regtime1;
	}

	public boolean isRemember() {
		return remember;
	}

	public void setRemember(boolean remember) {
		this.remember = remember;
	}

	public String getSecondPassword() {
		return secondPassword;
	}

	public void setSecondPassword(String secondPassword) {
		this.secondPassword = secondPassword;
	}

	public String getSexy() {
		return sexy;
	}

	public void setSexy(String sexy) {
		this.sexy = sexy;
	}

	public String getSfz() {
		return sfz;
	}

	public void setSfz(String sfz) {
		this.sfz = sfz;
	}

	public String getSfzPhoto() {
		return sfzPhoto;
	}

	public void setSfzPhoto(String sfzPhoto) {
		this.sfzPhoto = sfzPhoto;
	}

	public int getState() {
		return state;
	}

	public void setState(int state) {
		this.state = state;
	}

	public int getStatus() {
		return status;
	}

	public void setStatus(int status) {
		this.status = status;
	}

	public int getTotalPoints() {
		return totalPoints;
	}

	public void setTotalPoints(int totalPoints) {
		this.totalPoints = totalPoints;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public int getUcId() {
		return ucId;
	}

	public void setUcId(int ucId) {
		this.ucId = ucId;
	}

	public String getUptime() {
		return uptime;
	}

	public void setUptime(String uptime) {
		this.uptime = uptime;
	}

	public int getUserId() {
		return userId;
	}

	public void setUserId(int userId) {
		this.userId = userId;
	}

	public String getUserName() {
		return userName;
	}

	public void setUserName(String userName) {
		this.userName = userName;
	}

	public int getUserType() {
		return userType;
	}

	public void setUserType(int userType) {
		this.userType = userType;
	}

	public String getUserType1() {
		return userType1;
	}

	public void setUserType1(String userType1) {
		this.userType1 = userType1;
	}

	public String getWeiXinNumber() {
		return weiXinNumber;
	}

	public void setWeiXinNumber(String weiXinNumber) {
		this.weiXinNumber = weiXinNumber;
	}

	public int getZip() {
		return zip;
	}

	public void setZip(int zip) {
		this.zip = zip;
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
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());

				if (data.getMsg().contains("成功")) {
					lis.succeed(data.getMsg());
				} else {
					lis.fail(data.getMsg());
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
