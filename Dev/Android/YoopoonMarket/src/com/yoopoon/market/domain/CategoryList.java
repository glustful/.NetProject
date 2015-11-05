package com.yoopoon.market.domain;

import java.util.ArrayList;
import java.util.List;

public class CategoryList {
	public CategoryEntity father;
	public List<CategoryEntity> children = new ArrayList<CategoryEntity>();
	public int childcount = 0;

	@Override
	public String toString() {
		return "CategoryList [father=" + father + ", children=" + children + ", childcount=" + childcount + "]";
	}

}
