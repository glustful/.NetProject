package com.yoopoon.market.domain;

public class CategoryEntity {

	public int id;
	public String name;
	public int sort;

	public CategoryEntity(int id, String name, int sort) {
		super();
		this.id = id;
		this.name = name;
		this.sort = sort;
	}

	@Override
	public String toString() {
		return "CategoryEntity [id=" + id + ", name=" + name + ", sort=" + sort + "]";
	}

}
