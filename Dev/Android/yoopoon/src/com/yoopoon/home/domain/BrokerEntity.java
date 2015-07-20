/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-19 上午11:02:27 
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
import com.yoopoon.home.domain.Broker2.RequesListener;

/**
 * @ClassName: BrokerEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-19 上午11:02:27
 */
public class BrokerEntity {

	// brokerModel.Headphoto = broker.Headphoto;
	// brokerModel.Nickname = broker.Nickname;
	// brokerModel.Phone = broker.Phone;
	// brokerModel.Sfz = broker.Sfz;
	// brokerModel.Email = broker.Email;
	// brokerModel.Realname = broker.Realname;
	// brokerModel.Sexy = broker.Sexy;

	// Brokername:'',
	// Realname:'',
	// Nickname:'',
	// Sexy:'',
	// Sfz:'',
	// Email:'',
	// Phone:'',
	// Headphoto:''

	private static final String TAG = "BrokerEntity";

	private int id;
	private String brokername;
	private String nickname;
	private String phone;
	private String sfz;
	private String email;
	private String realname;
	private String sexy;

	public BrokerEntity(int id, String brokername, String nickname, String phone, String sfz, String email,
			String realname, String sexy) {
		super();
		this.id = id;
		this.brokername = brokername;
		this.nickname = nickname;
		this.phone = phone;
		this.sfz = sfz;
		this.email = email;
		this.realname = realname;
		this.sexy = sexy;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getBrokername() {
		return brokername;
	}

	public void setBrokername(String brokername) {
		this.brokername = brokername;
	}

	public String getNickname() {
		return nickname;
	}

	public void setNickname(String nickname) {
		this.nickname = nickname;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
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

	public String getRealname() {
		return realname;
	}

	public void setRealname(String realname) {
		this.realname = realname;
	}

	public String getSexy() {
		return sexy;
	}

	public void setSexy(String sexy) {
		this.sexy = sexy;
	}

	public void modifyInfo(final RequesListener listener) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();

				try {
					String json = om.writeValueAsString(BrokerEntity.this);
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
				System.out.println("code=" + data.getResponseStatus());

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_update_brokerinfo))

		.SetJSON("{}").notifyRequest();

	}

}
