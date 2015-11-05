/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: HousePopuwindow.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.house.ui.houselist 
 * @Description: 继承自Popupwindow，完成
 * @author: 徐阳会 
 * @updater: 徐阳会 
 * @date: 2015年7月22日 下午1:57:53 
 * @version: V1.0   
 */
package com.yoopoon.house.ui.houselist;

import android.graphics.drawable.BitmapDrawable;
import android.widget.PopupWindow;

/** 
 * @ClassName: HousePopuwindow 
 * @Description: 
 * @author: 徐阳会
 * @date: 2015年7月22日 下午1:57:53  
 */
public class HousePopuwindow extends PopupWindow {
	public static HousePopuwindow getHousePopupwindowInstance() {
		HousePopuwindow housePopuwindow = new HousePopuwindow();
		housePopuwindow.setTouchable(true);
		housePopuwindow.setBackgroundDrawable(new BitmapDrawable());
		housePopuwindow.setOutsideTouchable(true);
		housePopuwindow.setFocusable(true);
		return housePopuwindow;
	}
}
