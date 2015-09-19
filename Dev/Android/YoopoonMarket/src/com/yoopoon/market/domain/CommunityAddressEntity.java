package com.yoopoon.market.domain;

import java.util.List;

public class CommunityAddressEntity {
	public int Id;

	// / 订单编码
	public String No;

	// / 状态
	public enum Status {
		Created, Payed, Delivering, Successed, Canceled,
	}

	public Status EnumOrderStatus;

	public String StatusString() {
		switch (EnumOrderStatus) {
			case Created:
				return "新建";

			case Payed:
				return "已付款";

			case Delivering:
				return "配送中";

			case Successed:
				return "订单完成";

			case Canceled:
				return "订单关闭";

			default:
				return "";
		}

	}

	// / 客户名称
	public String CustomerName;

	// / 订单备注
	public String Remark;

	// / 开单时间
	public String Adddate;

	// / 开单人员
	public int Adduser;

	// / 修改人员
	public int Upduser;

	// / 修改时间
	public String Upddate;

	// / 订单总价
	public float Totalprice;

	// / 订单实价
	public float Actualprice;

	// / 订单明细
	public List<OrderDetailModel> Details;

	public String UserName;

	public int MemberAddressId;
}
