package com.yoopoon.market.domain;

import java.io.Serializable;

public class Staff implements Serializable {

	/**
	 * @fieldName: serialVersionUID
	 * @fieldType: long
	 * @Description: TODO
	 */
	private static final long serialVersionUID = 1L;

	public int id;
	public String title;
	public String category;
	public String image;
	public int count;
	public float price_counted;
	public float price_previous;
	public boolean chosen = false;
	public int productId;

	public Staff(String title, String category, String image, int count, float price_counted, float price_previous,
			int productId) {
		super();
		this.title = title;
		this.category = category;
		this.image = image;
		this.count = count;
		this.price_counted = price_counted;
		this.price_previous = price_previous;
		this.productId = productId;
	}

	public Staff(String title, String category, String image, int count, float price_counted, float price_previous) {
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
		return "Staff [id=" + id + ", title=" + title + ", category=" + category + ", image=" + image + ", count="
				+ count + ", price_counted=" + price_counted + ", price_previous=" + price_previous + ", chosen="
				+ chosen + ", productId=" + productId + "]";
	}

}
