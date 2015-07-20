/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Bank.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-17 下午3:02:05 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: Bank
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-17 下午3:02:05
 */
public class Bank {
	//
	// Address: "大理古城建设银行"
	// Bank: 4
	// Hidm: "UQ9qrKR8g37r9FCkof%2f0hA%3d%3d"
	// MobileYzm: "796313"
	// Num: "6217003950001286795"
	// Type: "储蓄卡"

	private String Address;
	private int bank;
	private String hidm;
	private String mobileYzm;
	private String num;
	private String type;
	private String bankName;

	/**
	 * 中国银行 Bank of China
	 */
	public static final int BC = 1;
	/**
	 * 中国工商银行 Industrial and Commercial Bank of China
	 */
	public static final int ICBC = 2;
	/**
	 * 中国农业银行 Agricultural Bank of China
	 */
	public static final int ABC = 3;
	/**
	 * 中国建设银行 China Construction Bank
	 */
	public static final int CCB = 4;

	public Bank() {

	}

	@Override
	public String toString() {
		return "Bank [Address=" + Address + ",bankName = " + bankName + ", bank=" + bank + ", hidm=" + hidm
				+ ", mobileYzm=" + mobileYzm + ", num=" + num + ", type=" + type + "]";
	}

	public Bank(String address, int bank, String hidm, String mobileYzm, String num, String type) {
		super();
		Address = address;
		this.bank = bank;
		this.hidm = hidm;
		this.mobileYzm = mobileYzm;
		this.num = num;
		this.type = type;
	}

	public String getAddress() {
		return Address;
	}

	public void setAddress(String address) {
		Address = address;
	}

	public int getBank() {
		return bank;
	}

	public void setBank(int bank) {
		this.bank = bank;
	}

	public String getHidm() {
		return hidm;
	}

	public void setHidm(String hidm) {
		this.hidm = hidm;
	}

	public String getMobileYzm() {
		return mobileYzm;
	}

	public void setMobileYzm(String mobileYzm) {
		this.mobileYzm = mobileYzm;
	}

	public String getNum() {
		return num;
	}

	public void setNum(String num) {
		this.num = num;
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public String getBankName() {
		return bankName;
	}

	public void setBankName(String bankName) {
		this.bankName = bankName;
	}

}
