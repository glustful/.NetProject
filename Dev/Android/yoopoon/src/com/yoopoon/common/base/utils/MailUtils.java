/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MailUtils.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.common.base.utils 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-8-7 上午11:53:59 
 * @version: V1.0   
 */
package com.yoopoon.common.base.utils;


/**
 * @ClassName: MailUtils
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-8-7 上午11:53:59
 */
public class MailUtils {
	String host = "";
	String user = "";
	String password = "";

	public void setHost(String host) {
		this.host = host;
	}

	public void setAccount(String user, String password) {
		this.user = user;
		this.password = password;
	}

	public void send(String from, String to, String subject, String content) {
		// Properties props = new Properties();
		// props.put("mail.smtp.host", host); // 指定SMTP服务器
		// props.put("mail.smtp.auth", "true"); // 指定是否需要SMTP验证
		// try {
		// Session mailSession = Session.getDefaultInstance(props);
		// Message message = new MimeMessage(mailSession);
		// message.setFrom(new InternetAddress(from)); // 发件人
		// message.addRecipient(Message.RecipientType.TO, new InternetAddress(to)); // 收件人
		// message.setSubject(subject); // 邮件主题
		// message.setText(content); // 邮件内容
		// message.saveChanges();
		// Transport transport = mailSession.getTransport("smtp");
		// transport.connect(host, user, password);
		// transport.sendMessage(message, message.getAllRecipients());
		// transport.close();
		// } catch (Exception e) {
		// System.out.println(e);
		// }
	}
}
