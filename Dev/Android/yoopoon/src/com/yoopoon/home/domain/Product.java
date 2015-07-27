/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Product.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-24 下午1:22:35 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: Product
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-24 下午1:22:35
 */
public class Product {

	// //"Phone":"18787140549",
	// "Type":null,
	// "ProductDetailImg":null,
	// "StockRule":0,
	// "Bname":null,
	// "BrandImg":null,
	// "acreage":null,
	// "Classify":null,
	// "Recommend":0,
	// "area":null,
	// "Acreage":null,
	// "Productimg4":null,
	// "Content":null,
	// "Addtime":"\/Date(-62135596800000)\/",
	// "ClassId":0,
	// "ProductDetailed":null,
	// "Productimg1":null,
	// "Productimg3":null,
	// "Productimg2":null,
	// "RecCommission":0,
	// "Productname":"公园城3",
	// "ClassifyName":null,
	// "BrandId":0,
	// "Dealcommission":0,
	// "Status":0,
	// "Commission":0,
	// "Stockrule":0,
	// "Advertisement":null,
	// "ParameterValue":null,
	// "Price":9000,
	// "SubTitle":"四室两厅一厨两卫双阳台",
	// "Sericeinstruction":null,
	// "Id":35,
	// "Productimg":"20150619\/20150619_115115_607_974.png"}
	private int id;
	private String phone;
	private String subTitle;
	private String productName;
	private int price;
	private String productImg;
	private int dealCommission;
	private int commission;
	private int recCommission;

	public Product(int id, String phone, String subTitle, String productName, int price, String productImg,
			int dealCommission, int commission, int recCommission) {
		super();
		this.id = id;
		this.phone = phone;
		this.subTitle = subTitle;
		this.productName = productName;
		this.price = price;
		this.productImg = productImg;
		this.dealCommission = dealCommission;
		this.commission = commission;
		this.recCommission = recCommission;
	}

	@Override
	public String toString() {
		return "Product [id=" + id + ", phone=" + phone + ", subTitle=" + subTitle + ", productName=" + productName
				+ ", price=" + price + ", productImg=" + productImg + ", dealCommission=" + dealCommission
				+ ", commission=" + commission + ", recCommission=" + recCommission + "]";
	}

	public int getId() {
		return id;
	}

	public void setId(int id) {
		this.id = id;
	}

	public String getPhone() {
		return phone;
	}

	public void setPhone(String phone) {
		this.phone = phone;
	}

	public String getSubTitle() {
		return subTitle;
	}

	public void setSubTitle(String subTitle) {
		this.subTitle = subTitle;
	}

	public String getProductName() {
		return productName;
	}

	public void setProductName(String productName) {
		this.productName = productName;
	}

	public int getPrice() {
		return price;
	}

	public void setPrice(int price) {
		this.price = price;
	}

	public String getProductImg() {
		return productImg;
	}

	public void setProductImg(String productImg) {
		this.productImg = productImg;
	}

	public int getDealCommission() {
		return dealCommission;
	}

	public void setDealCommission(int dealCommission) {
		this.dealCommission = dealCommission;
	}

	public int getCommission() {
		return commission;
	}

	public void setCommission(int commission) {
		this.commission = commission;
	}

	public int getRecCommission() {
		return recCommission;
	}

	public void setRecCommission(int recCommission) {
		this.recCommission = recCommission;
	}

}
