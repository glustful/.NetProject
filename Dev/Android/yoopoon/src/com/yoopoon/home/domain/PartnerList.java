/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PartnerList.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 下午1:55:29 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: PartnerList
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 下午1:55:29
 */
public class PartnerList {

	private int id;
	private Broker broker;
	private int partnerId;
	private String brokerName;
	private String phone;

	@Override
	public String toString() {
		return "PartnerList [id=" + id + ", broker=" + broker + ", partnerId=" + partnerId + ", brokerName="
				+ brokerName + ", phone=" + phone + "]";
	}

	public PartnerList(int id, Broker broker, int partnerId, String brokerName, String phone) {
		super();
		this.id = id;
		this.broker = broker;
		this.partnerId = partnerId;
		this.brokerName = brokerName;
		this.phone = phone;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public Broker getBroker() {
		return broker;
	}

	public void setBroker(Broker broker) {
		this.broker = broker;
	}

	public int getPartnerId() {
		return partnerId;
	}

	public void setPartnerId(int partnerId) {
		this.partnerId = partnerId;
	}

	public String getBrokerName() {
		return brokerName;
	}

	public void setBrokerName(String brokerName) {
		this.brokerName = brokerName;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

}
