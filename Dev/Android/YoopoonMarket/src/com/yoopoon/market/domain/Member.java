/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Member.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-12 下午5:33:10 
 * @version: V1.0   
 */
package com.yoopoon.market.domain;

/**
 * @ClassName: Member
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-12 下午5:33:10
 */
public class Member {

	public int Id;
	public String RealName;
	public String IdentityNo;
	public int Gender;
	public String GenderString;
	public String Phone;
	public String Icq;
	public String PostNo;
	public String Thumbnail;
	public int AccountNumber;
	public int Points;
	public int Level;
	public String AddTime;
	public String UpdUser;
	public String UpdTime;

	@Override
	public String toString() {
		return "Member [Id=" + Id + ", RealName=" + RealName + ", IdentityNo=" + IdentityNo + ", Gender=" + Gender
				+ ", GenderString=" + GenderString + ", Phone=" + Phone + ", Icq=" + Icq + ", PostNo=" + PostNo
				+ ", Thumbnail=" + Thumbnail + ", AccountNumber=" + AccountNumber + ", Points=" + Points + ", Level="
				+ Level + ", AddTime=" + AddTime + ", UpdUser=" + UpdUser + ", UpdTime=" + UpdTime + "]";
	}

}
