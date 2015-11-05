/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: Agent.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.domain 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 下午4:38:07 
 * @version: V1.0   
 */
package com.yoopoon.home.domain;

/**
 * @ClassName: Agent
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 下午4:38:07
 */
public class Agent {

	private String name;
	private boolean checked;

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public boolean isChecked() {
		return checked;
	}

	public void setChecked(boolean checked) {
		this.checked = checked;
	}

	public Agent(String name, boolean checked) {
		super();
		this.name = name;
		this.checked = checked;
	}

	@Override
	public String toString() {
		return "Agent [name=" + name + ", checked=" + checked + "]";
	}

}
