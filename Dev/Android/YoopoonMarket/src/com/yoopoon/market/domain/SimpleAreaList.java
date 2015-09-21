package com.yoopoon.market.domain;

import java.io.Serializable;
import java.util.List;

public class SimpleAreaList implements Serializable {
	public List<SimpleAreaEntity> children;
	public int childCount;
	public SimpleAreaEntity father;
}
