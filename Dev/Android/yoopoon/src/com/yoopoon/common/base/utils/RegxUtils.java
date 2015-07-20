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

}
