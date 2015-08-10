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

	public String Address;
	public int Bank;
	public String Hidm;
	public String MobileYzm;
	public String Num;
	public String Type;
	public String BankName;
	public int Id;

	@Override
	public String toString() {
		return "Bank [Address=" + Address + ", Bank=" + Bank + ", Hidm=" + Hidm + ", MobileYzm=" + MobileYzm + ", Num="
				+ Num + ", Type=" + Type + ", BankName=" + BankName + ", Id=" + Id + "]";
	}

}
