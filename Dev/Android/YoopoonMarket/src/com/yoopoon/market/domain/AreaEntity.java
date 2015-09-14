package com.yoopoon.market.domain;

public class AreaEntity {

	public int Id;
	public String Codeid;
	public String Adddate;
	public String Parent;
	public String Name;

	@Override
	public String toString() {
		return "AreaEntity [Id=" + Id + ", Codeid=" + Codeid + ", Adddate=" + Adddate + ", Parent=" + Parent
				+ ", Name=" + Name + "]";
	}

}
