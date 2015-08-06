package com.yoopoon.common.base.utils;

import java.io.File;

public class StringUtils {
	private static String[] pics = { "bmp", "gif", "jpeg", "psd", "png", "jpg" };

	public static boolean isEmpty(String text) {
		if ("".equals(text))
			return true;
		if (text == null)
			return true;
		if ("null".equals(text))
			return true;

		return false;
	}

	public static String trim(String text) {
		return text.replaceAll(" ", "");
	}

	public static boolean isPicFile(File file) {
		String name = file.getName();
		for (String pic : pics) {
			if (name.endsWith(pic))
				return true;
		}

		return false;
	}
}
