/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: DensityUtil.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-31 上午8:55:39 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

import android.content.Context;

/**
 * @ClassName: DensityUtil
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-31 上午8:55:39
 */
public class DensityUtil {

	public static int dip2px(Context context, float dpValue) {
		final float scale = context.getResources().getDisplayMetrics().density;
		return (int) (dpValue * scale + 0.5f);
	}

	public static int px2dip(Context context, float pxValue) {
		final float scale = context.getResources().getDisplayMetrics().density;
		return (int) (pxValue / scale + 0.5f);
	}

}
