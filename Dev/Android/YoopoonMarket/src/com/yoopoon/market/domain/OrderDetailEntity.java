package com.yoopoon.market.domain;

import java.io.Serializable;

public class OrderDetailEntity implements Serializable {
	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id = 1;
	public String No = null;
	public String ProductName;
	public float UnitPrice;
	public float Price = 0;
	public int Count;
	public String Snapshoturl;
	public String Remark;
	public int Adduser = 0;
	public String Adddate;
	public int Upduser = 0;
	public String Upddate;
	public float Totalprice;
	public ProductEntity Product;

	@Override
	public String toString() {
		return "OrderDetailEntity [Id=" + Id + ", No=" + No + ", ProductName=" + ProductName + ", UnitPrice="
				+ UnitPrice + ", Price=" + Price + ", Count=" + Count + ", Snapshoturl=" + Snapshoturl + ", Remark="
				+ Remark + ", Adduser=" + Adduser + ", Adddate=" + Adddate + ", Upduser=" + Upduser + ", Upddate="
				+ Upddate + ", Totalprice=" + Totalprice + ", Product=" + Product + "]";
	}
}
