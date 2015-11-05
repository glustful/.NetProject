/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: AddMoneyEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-30 上午10:39:25 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;


public class AddMoneyEntity {
	public int bank;
	public String money;
	public String mobileYzm;
	public String hidm;
	public String ids;

	@Override
	public String toString() {
		return "AddMoneyEntity [bank=" + bank + ", money=" + money + ", mobileYzm=" + mobileYzm + ", hidm=" + hidm
				+ ", ids=" + ids + "]";
	}

	public AddMoneyEntity(int bank, String money, String mobileYzm, String hidm, String ids) {
		super();
		this.bank = bank;
		this.money = money;
		this.mobileYzm = mobileYzm;
		this.hidm = hidm;
		this.ids = ids;
	}

}
