package com.yoopoon.common.base.utils;

public class StringUtils {

	public static boolean isEmpty(String text) {
		if ("".equals(text))
			return true;
		if (text == null)
			return true;
		if ("null".equals(text))
			return true;

		return false;
	}

}
