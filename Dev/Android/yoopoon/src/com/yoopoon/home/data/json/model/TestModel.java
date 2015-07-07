package com.yoopoon.home.data.json.model;

import com.fasterxml.jackson.annotation.JsonProperty;

public class TestModel {

	public TestModel(int id, String name, String contentType) {
		super();
		this.id = id;
		this.name = name;
		this.contentType = contentType;
	}

	public TestModel() {

	}

	@JsonProperty
	public int id;
	@JsonProperty
	public String name;

	@Override
	public String toString() {
		return "TestModel [id=" + id + ", name=" + name + ", contentType="
				+ contentType + "]";
	}

	@JsonProperty("content-Type")
	public String contentType;

}