/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: YzmWithPsw2.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 下午6:09:59 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: YzmWithPsw2
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 下午6:09:59
 */
public class YzmWithPsw2 {

	// Hidm: "oPqEuQzRN1xFuutVqq4RkQ%3d%3d"
	// MobileYzm: "435193"
	// OldPassword: "123456"
	// Password: "12345678"
	// SecondPassword: "12345678"

	private String hidm;
	private String MobileYzm;
	private String OldPassword;
	private String Password;
	private String SecondPassword;

	@Override
	public String toString() {
		return "YzmWithPsw2 [hidm=" + hidm + ", MobileYzm=" + MobileYzm + ", OldPassword=" + OldPassword
				+ ", Password=" + Password + ", SecondPassword=" + SecondPassword + "]";
	}

	public YzmWithPsw2(String hidm, String mobileYzm, String oldPassword, String password, String secondPassword) {
		super();
		this.hidm = hidm;
		MobileYzm = mobileYzm;
		OldPassword = oldPassword;
		Password = password;
		SecondPassword = secondPassword;
	}

	public String getHidm() {
		return hidm;
	}

	public void setHidm(String hidm) {
		this.hidm = hidm;
	}

	public String getMobileYzm() {
		return MobileYzm;
	}

	public void setMobileYzm(String mobileYzm) {
		MobileYzm = mobileYzm;
	}

	public String getOldPassword() {
		return OldPassword;
	}

	public void setOldPassword(String oldPassword) {
		OldPassword = oldPassword;
	}

	public String getPassword() {
		return Password;
	}

	public void setPassword(String password) {
		Password = password;
	}

	public String getSecondPassword() {
		return SecondPassword;
	}

	public void setSecondPassword(String secondPassword) {
		SecondPassword = secondPassword;
	}

}
