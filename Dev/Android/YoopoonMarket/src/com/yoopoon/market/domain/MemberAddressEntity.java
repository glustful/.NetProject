/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MemberAddressEntity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-12 下午6:39:23 
 * @version: V1.0   
 */
package com.yoopoon.market.domain;

/**
 * @ClassName: MemberAddressEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-12 下午6:39:23
 */
public class MemberAddressEntity {

	public int Id;
	public int Member;
	public String Address;
	public String Zip;
	public String Linkman;
	public String Tel;
	public String Adduser;
	public String Addtime;
	public int Upduser;
	public String Updtime;

	@Override
	public String toString() {
		return "MemberAddressEntity [Id=" + Id + ", Member=" + Member + ", Address=" + Address + ", Zip=" + Zip
				+ ", Linkman=" + Linkman + ", Tel=" + Tel + ", Adduser=" + Adduser + ", Addtime=" + Addtime
				+ ", Upduser=" + Upduser + ", Updtime=" + Updtime + "]";
	}

}
