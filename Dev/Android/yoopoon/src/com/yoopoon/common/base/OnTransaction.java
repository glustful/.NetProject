package com.yoopoon.common.base;

/**
 * 数据库事务处理
 *
 * 将接口中的代码放到数据库事务中执行
 * 正常：提交事务
 * 异常：回滚事务
 *
 */
public interface OnTransaction {
	// 此方法中的代码将放到数据库事务中
	// 自动执行事务的提交和回滚
	public void transaction();
}
