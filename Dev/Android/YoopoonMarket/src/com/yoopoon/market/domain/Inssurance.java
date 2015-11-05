package com.yoopoon.market.domain;

public class Inssurance {

	public String title;
	public String image;
	public String phone;

	public Inssurance(String title, String image, String phone) {
		super();
		this.title = title;
		this.image = image;
		this.phone = phone;
	}

	@Override
	public String toString() {
		return "Inssurance [title=" + title + ", image=" + image + ", phone=" + phone + "]";
	}

}
