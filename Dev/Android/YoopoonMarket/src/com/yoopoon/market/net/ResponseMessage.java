package com.yoopoon.market.net;

import java.io.InputStream;
import java.io.Serializable;

public class ResponseMessage implements Serializable {
	private static final long serialVersionUID = -7060210544600464481L;
	private int mCode;
	private String mMsg;
	private InputStream mIs;
	private byte[] mReceivedData = null;

	final public void setCode(int code) {
		mCode = code;
	}

	final public void setMsg(String msg) {
		mMsg = msg;
	}

	final public void setData(byte[] data) {

		mReceivedData = data;
	}

	final public int getCode() {
		return mCode;
	}

	final public String getMsg() {
		return mMsg;
	}

	final public byte[] getData() {
		return mReceivedData;
	}

	final public void setInstream(InputStream is) {
		mIs = is;
	}

	final public InputStream getmIs() {
		return mIs;
	}
}
