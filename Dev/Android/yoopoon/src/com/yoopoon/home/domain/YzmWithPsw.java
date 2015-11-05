/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: YzmWithPsw.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 下午5:36:47 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: YzmWithPsw
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 下午5:36:47
 */
public class YzmWithPsw {

	// Hidm: "7bnjqic71CswqRgJnPD1M%2b%2fuDvR8DUPr5RbaUsaHe4Q%3d"
	// Phone: "13508713650"
	// Yzm: "105982"
	// first_password: "123456"
	// second_password: "123456"

	private String hidm;
	private String phone;
	private String yzm;
	private String first_password;
	private String second_password;

	public YzmWithPsw(String hidm, String phone, String yzm, String first_password, String second_password) {
		super();
		this.hidm = hidm;
		this.phone = phone;
		this.yzm = yzm;
		this.first_password = first_password;
		this.second_password = second_password;
	}

	@Override
	public String toString() {
		return "YzmWithPsw [hidm=" + hidm + ", phone=" + phone + ", yzm=" + yzm + ", first_password=" + first_password
				+ ", second_password=" + second_password + "]";
	}

	public String getHidm() {
		return hidm;
	}

	public void setHidm(String hidm) {
		this.hidm = hidm;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getYzm() {
		return yzm;
	}

	public void setYzm(String yzm) {
		this.yzm = yzm;
	}

	public String getFirst_password() {
		return first_password;
	}

	public void setFirst_password(String first_password) {
		this.first_password = first_password;
	}

	public String getSecond_password() {
		return second_password;
	}

	public void setSecond_password(String second_password) {
		this.second_password = second_password;
	}

}
