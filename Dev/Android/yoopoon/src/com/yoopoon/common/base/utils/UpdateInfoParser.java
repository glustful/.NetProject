/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: UpdateInfoParser.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-3 下午3:47:12 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;

import java.io.InputStream;
import org.xmlpull.v1.XmlPullParser;
import android.util.Xml;
import com.yoopoon.home.domain.UpdateInfo;

/**
 * @ClassName: UpdateInfoParser
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-3 下午3:47:12
 */
public class UpdateInfoParser {
	public static UpdateInfo getUpdataInfo(InputStream is) throws Exception {
		XmlPullParser parser = Xml.newPullParser();
		parser.setInput(is, "utf-8");
		int type = parser.getEventType();
		UpdateInfo info = new UpdateInfo();
		while (type != XmlPullParser.END_DOCUMENT) {
			switch (type) {
				case XmlPullParser.START_TAG:
					if ("version".equals(parser.getName())) {
						info.setVersion(parser.nextText());
					} else if ("url".equals(parser.getName())) {
						info.setUrl(parser.nextText());
					} else if ("description".equals(parser.getName())) {
						info.setDescription(parser.nextText());
					}
					break;
			}
			type = parser.next();
		}
		return info;
	}
}
