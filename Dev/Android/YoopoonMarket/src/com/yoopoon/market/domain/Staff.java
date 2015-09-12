package com.yoopoon.market.domain;

public class Staff {

	public String title;
	public String category;
	public String image;
	public int count;
	public double price_counted;
	public double price_previous;
	public boolean chosen = true;

	public Staff(String title, String category, String image, int count, double price_counted, double price_previous) {
		super();
		this.title = title;
		this.category = category;
		this.image = image;
		this.count = count;
		this.price_counted = price_counted;
		this.price_previous = price_previous;
	}

	@Override
	public String toString() {
		return "Staff [title=" + title + ", category=" + category + ", image=" + image + ", count=" + count
				+ ", price_counted=" + price_counted + ", price_previous=" + price_previous + ", chosen=" + chosen
				+ "]";
	}

}
