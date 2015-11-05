package com.yoopoon.market.domain;

import java.io.Serializable;

public class ServiceOrderDetail implements Serializable {
	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;
	public int Id;
	public int Count;
	public float Price;
	public String ProductName;
	public String MainImg;
	public String OrderNo;
	public ProductEntity Product;

	@Override
	public String toString() {
		return "ServiceOrderDetail [Id=" + Id + ", Count=" + Count + ", Price=" + Price + ", ProductName="
				+ ProductName + ", MainImg=" + MainImg + ", OrderNo=" + OrderNo + ", Product=" + Product + "]";
	}

}
