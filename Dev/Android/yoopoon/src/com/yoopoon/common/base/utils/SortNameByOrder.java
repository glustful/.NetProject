/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SortNameByOrder.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 上午8:54:22 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

import java.util.Arrays;
import java.util.Comparator;

/**
 * @ClassName: SortNameByOrder
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 上午8:54:22
 */
public class SortNameByOrder {

	/**
	 * @Title: main
	 * @Description: TODO
	 * @param args
	 */
	public static String[] getShowNameList(String[] names) {
		int length = names.length + getTotalLetters(names);
		String[] showNameList = new String[length];
		CharacterParser parser = new CharacterParser();
		Arrays.sort(names, comparator);

		showNameList[0] = String.valueOf(parser.getSelling(names[0]).toUpperCase().charAt(0));
		String lastName = showNameList[0];
		int i = 1;
		for (String name : names) {
			if (parser.getSelling(name).toUpperCase().charAt(0) != parser.getSelling(lastName).toUpperCase().charAt(0))
				showNameList[i++] = "*****" + parser.getSelling(name).toUpperCase().charAt(0);
			showNameList[i++] = name;
			lastName = name;
		}
		showNameList[0] = "*****" + parser.getSelling(names[0]).toUpperCase().charAt(0);
		return showNameList;
	}

	static Comparator<String> comparator = new Comparator<String>() {

		public int compare(String arg0, String arg1) {
			CharacterParser parser = new CharacterParser();
			char c1 = parser.getSelling(arg0).toUpperCase().charAt(0);
			char c2 = parser.getSelling(arg1).toUpperCase().charAt(0);
			if (c1 > c2)
				return 1;
			else
				return (c1 < c2) ? -1 : 0;
		}
	};

	private static int getTotalLetters(String[] names) {
		int count = 0;
		int[] nums = new int[26];
		CharacterParser parser = new CharacterParser();

		for (String name : names) {
			char c = parser.getSelling(name).toUpperCase().charAt(0);
			int index = c - 65;
			nums[index]++;
		}

		for (int num : nums) {
			if (num > 0)
				count++;
		}

		return count;
	}

}
