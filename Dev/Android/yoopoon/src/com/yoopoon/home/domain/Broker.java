/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Broker.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-14 下午2:48:26 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: Broker
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午2:48:26
 */
public class Broker {

	// Broker: ""
	// BrokerId: 0
	// Id: 0
	// PartnerId: 0
	// Phone: "18313033523"
	// userId: 0
	public int brokerId;
	public String phone;
	public int userId;
	public int id;
	public int partnerId;

	public int getBrokerId() {
		return brokerId;
	}

	public void setBrokerId(int brokerId) {
		this.brokerId = brokerId;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public int getUserId() {
		return userId;
	}

	public void setUserId(int userId) {
		this.userId = userId;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public int getPartnerId() {
		return partnerId;
	}

	public void setPartnerId(int partnerId) {
		this.partnerId = partnerId;
	}

	public Broker(int brokerId, String phone, int userId, int id, int partnerId) {
		super();
		this.brokerId = brokerId;
		this.phone = phone;
		this.userId = userId;
		this.id = id;
		this.partnerId = partnerId;
	}

	@Override
	public String toString() {
		return "Broker [ brokerId=" + brokerId + ", phone=" + phone + ", userId=" + userId + ", id=" + id
				+ ", partnerId=" + partnerId + "]";
	}

}
