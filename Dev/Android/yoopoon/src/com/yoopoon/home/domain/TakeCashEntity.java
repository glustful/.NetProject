/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: TakeCashEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-29 下午5:49:44 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: TakeCashEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-29 下午5:49:44
 */
public class TakeCashEntity {

	// "Balancenum":101,"Addtime":"2015-07-27","Type":0,"Id":123
	private double balanceNum;
	private String addTime;
	private String type;
	private int id;
	private boolean status = false;

	@Override
	public String toString() {
		return "TakeCashEntity [balanceNum=" + balanceNum + ", addTime=" + addTime + ", type=" + type + ", id=" + id
				+ ", status=" + status + "]";
	}

	public TakeCashEntity(double balanceNum, String addTime, String type, int id, boolean status) {
		super();
		this.balanceNum = balanceNum;
		this.addTime = addTime;
		this.type = type;
		this.id = id;
		this.status = status;
	}

	public double getBalanceNum() {
		return balanceNum;
	}

	public void setBalanceNum(double balanceNum) {
		this.balanceNum = balanceNum;
	}

	public String getAddTime() {
		return addTime;
	}

	public void setAddTime(String addTime) {
		this.addTime = addTime;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public boolean isStatus() {
		return status;
	}

	public void setStatus(boolean status) {
		this.status = status;
	}

}
