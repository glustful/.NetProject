package com.yoopoon.market.utils;

import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONObject;

public class JSONArrayConvertToArrayList {
	private static ArrayList<JSONObject> jsonObjects = new ArrayList<JSONObject>();

	public static ArrayList<JSONObject> convertToArrayList(JSONArray array) {
		if (array == null) {
			return null;
		}
		for (int i = 0; i < array.length(); i++) {
			JSONObject jsonObject = array.optJSONObject(i);
			jsonObjects.add(jsonObject);
		}
		return jsonObjects;
	}
}
