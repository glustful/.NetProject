package com.yoopoon.market.utils;

import java.util.regex.Pattern;

;
public class SplitStringWithDot {
	/**
	 * @Title: split 分割处理末尾.0字符
	 * @Description: 传入字符串，判断是否末尾为.0字符串
	 * @param str 传入字符串
	 * @return
	 */
	public static String split(String str) {
		if (str.contains(".")) {
			String[] stringArray = str.split("\\.");
			if (stringArray[1].equals("0")) {
				return stringArray[0];
			}
		}
		return str;
	}
	public static boolean isInteger(String str) {
		Pattern pattern = Pattern.compile("^[-\\+]?[\\d]*$");
		return pattern.matcher(str).matches();
	}
}
