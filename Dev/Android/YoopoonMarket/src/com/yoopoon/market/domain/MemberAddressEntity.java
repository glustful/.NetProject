package com.yoopoon.market.domain;

import java.io.Serializable;

public class MemberAddressEntity implements Serializable {

	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id;
	public int UserId;
	public int Member;
	public String Address;
	public String Zip;
	public String Linkman;
	public String Tel;
	public String Adduser;
	public String Addtime;
	public int Upduser;
	public String Updtime;
	public String AddtimeString;
	public String UpdtimeString;
	public int AreaId = 123;

	@Override
	public String toString() {
		return "MemberAddressEntity [Id=" + Id + ", Member=" + UserId + ", Address=" + Address + ", Zip=" + Zip
				+ ", Linkman=" + Linkman + ", Tel=" + Tel + ", Adduser=" + Adduser + ", Addtime=" + Addtime
				+ ", Upduser=" + Upduser + ", Updtime=" + Updtime + ", AddtimeString=" + AddtimeString
				+ ", UpdtimeString=" + UpdtimeString + ", AreaId=" + AreaId + "]";
	}

}
