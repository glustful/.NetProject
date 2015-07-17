/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Broker2.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 上午9:55:19 
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
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: Broker2
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 上午9:55:19
 */
public class Broker2 {

	// Brokername:'',
	// Realname:'',
	// Nickname:'',
	// Sexy:'',
	// Sfz:'',
	// Email:'',
	// Phone:'',
	// Headphoto:''

	private int brokerId;
	private int id;
	private String brokername;
	private String realname;
	private String nickname;
	private String sexy;
	private String sfz;
	private String email;
	private String phone;
	private String headphoto;

	private static final String TAG = "Broker2";

	public Broker2(int brokerId, int id, String brokername, String realname, String nickname, String sexy, String sfz,
			String email, String phone, String headphoto) {
		super();
		this.brokerId = brokerId;
		this.id = id;
		this.brokername = brokername;
		this.realname = realname;
		this.nickname = nickname;
		this.sexy = sexy;
		this.sfz = sfz;
		this.email = email;
		this.phone = phone;
		this.headphoto = headphoto;
	}

	@Override
	public String toString() {
		return "Broker2 [brokerId=" + brokerId + ", id=" + id + ", brokername=" + brokername + ", realname=" + realname
				+ ", nickname=" + nickname + ", sexy=" + sexy + ", sfz=" + sfz + ", email=" + email + ", phone="
				+ phone + ", headphoto=" + headphoto + "]";
	}

	public String getBrokername() {
		return brokername;
	}

	public int getBrokerId() {
		return brokerId;
	}

	public void setBrokerId(int brokerId) {
		this.brokerId = brokerId;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public void setBrokername(String brokername) {
		this.brokername = brokername;
	}

	public String getRealname() {
		return realname;
	}

	public void setRealname(String realname) {
		this.realname = realname;
	}

	public String getNickname() {
		return nickname;
	}

	public void setNickname(String nickname) {
		this.nickname = nickname;
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

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getHeadphoto() {
		return headphoto;
	}

	public void setHeadphoto(String headphoto) {
		this.headphoto = headphoto;
	}

	public void modifyInfo(final RequesListener listener) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();

				try {
					String json = om.writeValueAsString(Broker2.this);
					Log.i(TAG, json);
					return json;
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

				requestModifyInfo(serializeResult, listener);

			}
		}).execute();

	}

	protected void requestModifyInfo(String serializeResult, final RequesListener lis) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					Log.i(TAG, data.toString());
					lis.succeed(data.getMsg());

				} else {
					Log.i(TAG, data.toString());
					lis.fail(data.getMsg());
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_update_brokerinfo)).SetJSON(serializeResult)
				.setSaveSession(true).notifyRequest();

	}

	public interface RequesListener {

		void succeed(String msg);

		void fail(String msg);
	}

}
