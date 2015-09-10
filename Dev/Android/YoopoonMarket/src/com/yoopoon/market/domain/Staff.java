package com.yoopoon.market.domain;

public class Staff {

	public String title;
	public String category;
	public String image;
	public int count;
	public float price_counted;
	public float price_previous;

	@Override
	public String toString() {
		return "Staff [title=" + title + ", category=" + category + ", image=" + image + ", count=" + count
				+ ", price_counted=" + price_counted + ", price_previous=" + price_previous + "]";
	}

}
