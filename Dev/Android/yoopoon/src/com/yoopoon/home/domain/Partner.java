/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PartnerEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-6 下午5:42:54 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: PartnerEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-6 下午5:42:54
 */
public class Partner {

	public String Name;
	public String Headphoto;
	public int Amount;
	public String Phone;
	public String regtime;
	public String Nickname;
	public int PartnerId;
	public int Id;
	public String Sfz;
	public String Agentlevel;
	public String AddTime;

	@Override
	public String toString() {
		return "InvitationEntity [Name=" + Name + ", Headphoto=" + Headphoto + ", Amount=" + Amount + ", Phone="
				+ Phone + ", regtime=" + regtime + ", Nickname=" + Nickname + ", PartnerId=" + PartnerId + ", Id=" + Id
				+ ", Sfz=" + Sfz + ", Agentlevel=" + Agentlevel + ", AddTime=" + AddTime + "]";
	}

}
