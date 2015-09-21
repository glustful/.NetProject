package com.yoopoon.market.domain;

import java.util.List;

public class TreeCategory {
	public int Id;
	public List<TreeCategory> children;
	public String label;

	@Override
	public String toString() {
		return "TreeCategory [Id=" + Id + ", children=" + children + ", label=" + label + "]";
	}

}
