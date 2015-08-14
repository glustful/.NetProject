/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RegxUtil.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-20 下午4:10:44 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

/**
 * @ClassName: RegxUtil
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-20 下午4:10:44
 */
public class RegxUtils {

	public static boolean isPhone(String phone) {
		String regx = "(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\\d{8}$";
		return phone.matches(regx);
	}

	public static boolean isSfz(String sfz) {
		String sfzRegx = "^(\\d{15}$|^\\d{18}$|^\\d{17}(\\d|X|x))$";
		return sfz.matches(sfzRegx);
	}

	public static boolean isEmail(String email) {
		String emailRegx = "^([a-zA-Z0-9_\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9]{2,4})+$";
		return email.matches(emailRegx);
	}

	public static boolean isName(String name) {
		int length = name.length();

		return (length <= 5) && (length >= 2);
	}

	public static boolean isBankCard(String number) {
		String regx = "^(\\d{16}|\\d{19})$";
		if (!number.matches(regx))
			return false;
		int[] nums = { 10, 18, 30, 35, 37, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 58, 60,
				62, 65, 68, 69, 84, 87, 88, 94, 95, 98, 99 };
		int num = Integer.parseInt(number.substring(0, 2));
		for (int i = 0; i < nums.length; i++) {
			if (num == nums[i])
				return true;
		}
		return false;
	}

}
