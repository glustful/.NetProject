/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PartnerEntity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-7 上午8:59:37 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: PartnerEntity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-7 上午8:59:37
 */
public class Invitation {
	public int PartnerId;
	public int Id;
	public String BrokerName;
	public String HeadPhoto;
	public String AddTime;

	@Override
	public String toString() {
		return "PartnerEntity [PartnerId=" + PartnerId + ", Id=" + Id + ", BrokerName=" + BrokerName + ", HeadPhoto="
				+ HeadPhoto + ", AddTime=" + AddTime + "]";
	}

}
