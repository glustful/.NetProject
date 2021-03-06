package com.yoopoon.market.domain;

import java.io.Serializable;

public class MemberModel implements Serializable {
	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id;
	public String RealName;
	public String IdentityNo;
	public int Gender = 0;
	public String GenderString = "男";
	public String Phone;
	public String Icq;
	public String PostNo;
	public String Thumbnail;
	public float AccountNumber = 0;
	public float Points = 0;
	public int Level = 1;
	public String AddTime;
	public String AddTimeString;
	public int UpdUser;
	public String UpdTime;
	public int UserId;
	public String UserName;
	public String Password;
	public String SecondPassword;
	public String Regtime;

	@Override
	public String toString() {
		return "MemberModel [Id=" + Id + ", RealName=" + RealName + ", IdentityNo=" + IdentityNo + ", Gender=" + Gender
				+ ", GenderString=" + GenderString + ", Phone=" + Phone + ", Icq=" + Icq + ", PostNo=" + PostNo
				+ ", Thumbnail=" + Thumbnail + ", AccountNumber=" + AccountNumber + ", Pointspublic=" + Points
				+ ", Levelpublic=" + Level + ", AddTime=" + AddTime + ", AddTimeString=" + AddTimeString + ", UpdUser="
				+ UpdUser + ", UpdTime=" + UpdTime + ", UserId=" + UserId + ", UserName=" + UserName + ", Password="
				+ Password + ", SecondPassword=" + SecondPassword + ", Regtime=" + Regtime + "]";
	}
}
