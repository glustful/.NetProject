package com.yoopoon.market.domain;

public class CommunityAreaEntity {
	public int Id;
	public String Codeid;
	public String Adddate;
	public String AdddateString;
	public CommunityAddressEntity Parent;
	public String ParentName;
	public String Name;

	@Override
	public String toString() {
		return "CommunityAreaEntity [Id=" + Id + ", Codeid=" + Codeid + ", Adddate=" + Adddate + ", AdddateString="
				+ AdddateString + ", Parent=" + Parent + ", ParentName=" + ParentName + ", Name=" + Name + "]";
	}
}
