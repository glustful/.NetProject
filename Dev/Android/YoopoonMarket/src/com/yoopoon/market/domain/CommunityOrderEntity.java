package com.yoopoon.market.domain;

import java.io.Serializable;
import java.util.List;

public class CommunityOrderEntity implements Serializable {
	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id;
	public String No;
	public int Status;
	public String StatusString;
	public String CustomerName;;
	public String Remark;
	public String Adddate;
	public int Adduser = 1;
	public int Upduser = 1;
	public String Upddate;
	public float Totalprice;
	public float Actualprice;
	public List<OrderDetailEntity> Details;
	public String UserName;
	public int MemberAddressId;

	@Override
	public String toString() {
		return "CommunityOrderEntity [Id=" + Id + ", No=" + No + ", Status=" + Status + ", StatusString="
				+ StatusString + ", CustomerName=" + CustomerName + ", Remark=" + Remark + ", Adddate=" + Adddate
				+ ", Adduser=" + Adduser + ", Upduser=" + Upduser + ", Upddate=" + Upddate + ", Totalprice="
				+ Totalprice + ", Actualprice=" + Actualprice + ", Details=" + Details + ", UserName=" + UserName
				+ ", MemberAddressId=" + MemberAddressId + "]";
	}

}
