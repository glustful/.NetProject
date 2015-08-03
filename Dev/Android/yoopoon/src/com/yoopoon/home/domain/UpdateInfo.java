/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: UpdateInfo.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-3 下午3:53:16 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: UpdateInfo
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-3 下午3:53:16
 */
public class UpdateInfo {

	private String version;
	private String url;
	private String description;

	@Override
	public String toString() {
		return "UpdateInfo [version=" + version + ", url=" + url + ", description=" + description + "]";
	}

	public UpdateInfo() {
		super();
	}

	public UpdateInfo(String version, String url, String description) {
		super();
		this.version = version;
		this.url = url;
		this.description = description;
	}

	public String getVersion() {
		return version;
	}

	public void setVersion(String version) {
		this.version = version;
	}

	public String getUrl() {
		return url;
	}

	public void setUrl(String url) {
		this.url = url;
	}

	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

}
