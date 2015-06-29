package com.yoopoon.common.base;

import java.io.File;
import java.io.UnsupportedEncodingException;
import java.math.BigInteger;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Date;

import org.json.JSONObject;

import android.os.Environment;


public class Tools {
	public static boolean hasSdCard() {
		String state = Environment.getExternalStorageState();
		if (state.equals(Environment.MEDIA_MOUNTED)) {
			return true;
		} else {
			return false;
		}
	}

	public static String stringOfObject(Object obj) {
		if (obj instanceof String) {
			return (String) obj;
		} else if (obj instanceof Boolean) {
			return String.valueOf(obj);
		} else if (obj instanceof Long) {
			return String.valueOf(obj);
		} else if (obj instanceof Date) {
			return String.valueOf(((Date) obj).getTime());
		} else if (obj != null) {
			return obj.toString();
		} else {
			return "";
		}
	}

	public static String getMd5(String value) {
		if (value != null && value.length() > 0) {
			try {
				MessageDigest md = MessageDigest.getInstance("MD5");
				byte[] hash = md.digest(value.getBytes("UTF8"));
				BigInteger i = new BigInteger(1, hash);
				return String.format("%1$032x", i); 
			} catch (NoSuchAlgorithmException e) {
				e.printStackTrace();
			} catch (UnsupportedEncodingException e) {
				e.printStackTrace();
			}
		}
		return null;
	}

    public static void deleteAllFilesOfDir(File path) {
        if (!path.exists())
            return;
        if (path.isFile()) {
            path.delete();
            return;
        }
        File[] files = path.listFiles();
        for (int i = 0; i < files.length; i++) {
            deleteAllFilesOfDir(files[i]);
        }
        path.delete();
    }
    
    public static String optString(JSONObject obj,String key,String defaultValue){
    	if(obj.isNull(key))
    		return defaultValue;
    	return obj.optString(key, defaultValue);
    }
    
    public static int optInt(JSONObject obj,String key,int defaultValue){
    	if(obj.isNull(key))
    		return defaultValue;
    	return obj.optInt(key, defaultValue);
    }
}