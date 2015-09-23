package com.yoopoon.market.domain;

import java.io.Serializable;
import java.util.List;

public class ServiceOrderEntity implements Serializable {

	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;

	public int Id;
	public String OrderNo;
	public String Addtime;
	public int AddUser;
	public int UpdUser;
	public String UpdTime;
	public float Flee;
	public String Address;
	public String Servicetime;
	public String Remark;
	public int Status;
	public String StatusString;
	public String UserName;
	public int MemberAddressId;
	public List<ServiceOrderDetail> Details;

	@Override
	public String toString() {
		return "ServiceOrderEntity [Id=" + Id + ", OrderNo=" + OrderNo + ", Addtime=" + Addtime + ", AddUser="
				+ AddUser + ", UpdUser=" + UpdUser + ", UpdTime=" + UpdTime + ", Flee=" + Flee + ", Address=" + Address
				+ ", Servicetime=" + Servicetime + ", Remark=" + Remark + ", Status=" + Status + ", StatusString="
				+ StatusString + ", UserName=" + UserName + ", MemberAddressId=" + MemberAddressId + ", Details="
				+ Details + "]";
	}

}
