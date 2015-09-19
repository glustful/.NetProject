package com.yoopoon.market.domain;

import java.util.List;

public class CommunityOrderEntity {
	public int Id = 1;
	public String No = "00212654646512";
	public int Status = 0;
	public String StatusString = "新建";
	public String CustomerName = "客户姓名";
	public String Remark = "test";
	public String Adddate = "\\/Date(1442572933790)\\/";
	public int Adduser = 1;
	public int Upduser = 1;
	public String Upddate = "\\/Date(1442572933790)\\/";
	public float Totalprice = 100.0f;
	public float Actualprice = 100.0f;
	public List<OrderDetailEntity> Details;
	public String UserName = "admin";
	public int MemberAddressId = 0;

}
