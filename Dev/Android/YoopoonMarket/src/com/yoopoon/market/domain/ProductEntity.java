package com.yoopoon.market.domain;

public class ProductEntity {
	public int id;
	public String title;
	public String imgUrl;
	public String category;
	public float price_counted;
	public float price_previous;
	public int amount;

	public ProductEntity(String title, String imgUrl, String category, float price_counted, float price_previous,
			int amount) {
		super();
		this.title = title;
		this.imgUrl = imgUrl;
		this.category = category;
		this.price_counted = price_counted;
		this.price_previous = price_previous;
		this.amount = amount;
	}

	@Override
	public String toString() {
		return "ProductEntity [id=" + id + ", title=" + title + ", imgUrl=" + imgUrl + ", category=" + category
				+ ", price_counted=" + price_counted + ", price_previous=" + price_previous + ", amount=" + amount
				+ "]";
	}

}
