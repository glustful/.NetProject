package com.yoopoon.market.domain;

public class CategoryEntity {

	public int id;
	public String name;
	public int sort;
	public int fatherId;

	public CategoryEntity(int id, String name, int sort, int fatherId) {
		super();
		this.id = id;
		this.name = name;
		this.sort = sort;
		this.fatherId = fatherId;
	}

	@Override
	public String toString() {
		return "CategoryEntity [id=" + id + ", name=" + name + ", sort=" + sort + ", fatherId=" + fatherId + "]";
	}

}
