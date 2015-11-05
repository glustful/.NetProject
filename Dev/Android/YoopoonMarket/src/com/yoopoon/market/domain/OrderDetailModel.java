package com.yoopoon.market.domain;

public class OrderDetailModel {
	// / ID
	public int Id;

	// / 商品名字
	public String ProductName;

	// / 单价
	public float UnitPrice;

	// / 数量
	public int Count;

	// / 商品页面快照
	public String Snapshoturl;

	// / 商品备注
	public String Remark;

	// / 添加人员
	public int Adduser;

	// / 添加时间
	public String Adddate;

	// / 修改人
	public int Upduser;

	// / 修改时间
	public String Upddate;

	// / 总价
	public float Totalprice;

	// / 订单主体
	// public ProductModel Product { get; set; }
}
