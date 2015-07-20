/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: GuestInfo.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-18 上午10:49:30 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

import android.widget.RelativeLayout;

/**
 * @ClassName: GuestInfo
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-18 上午10:49:30
 */
public class GuestInfo {

	// "Clientname":"徐阳会",
	// "Housetype":"2室2厅1厨2卫 书房",
	// "Houses":"人民路壹号",
	// "Status":"预约中",
	// "Phone":"13508713650",
	// "Id":"43",
	// "StrType":"带客"

	private String clientName;
	private String houseType;
	private String houses;
	private String status;
	private String phone;
	private String id;
	private String strType;
	private RelativeLayout rl_progress;
	private boolean isProgressShown = false;

	@Override
	public String toString() {
		return "GuestInfo [clientName=" + clientName + ", houseType=" + houseType + ", houses=" + houses + ", status="
				+ status + ", phone=" + phone + ", id=" + id + ", strType=" + strType + "]";
	}

	public GuestInfo(String clientName, String houseType, String houses, String status, String phone, String id,
			String strType) {
		super();
		this.clientName = clientName;
		this.houseType = houseType;
		this.houses = houses;
		this.status = status;
		this.phone = phone;
		this.id = id;
		this.strType = strType;
	}

	public String getClientName() {
		return clientName;
	}

	public void setClientName(String clientName) {
		this.clientName = clientName;
	}

	public String getHouseType() {
		return houseType;
	}

	public void setHouseType(String houseType) {
		this.houseType = houseType;
	}

	public String getHouses() {
		return houses;
	}

	public void setHouses(String houses) {
		this.houses = houses;
	}

	public String getStatus() {
		return status;
	}

	public void setStatus(String status) {
		this.status = status;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getId() {
		return id;
	}

	public void setId(String id) {
		this.id = id;
	}

	public String getStrType() {
		return strType;
	}

	public void setStrType(String strType) {
		this.strType = strType;
	}

	public RelativeLayout getRl_progress() {
		return rl_progress;
	}

	public void setRl_progress(RelativeLayout rl_progress) {
		this.rl_progress = rl_progress;
	}

	public boolean isProgressShown() {
		return isProgressShown;
	}

	public void setProgressShown(boolean isProgressShown) {
		this.isProgressShown = isProgressShown;
	}

}
