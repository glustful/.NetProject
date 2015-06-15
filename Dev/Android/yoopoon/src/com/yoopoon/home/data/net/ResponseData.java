package com.yoopoon.home.data.net;

import java.io.InputStream;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class ResponseData
{
    private int mCode;
    private InputStream mIs;
    private String mMsg;
    private ResultState mState;
    private JSONObject mRootData;
    private String mStringData;
    private int responseStatus;

    public ResponseData()
    {
        mCode = -1;
        mMsg = "";
    }

    public int getResponseStatus() {
        return responseStatus;
    }

    public int getCode()
    {
        return mCode;
    }

    public String getMsg()
    {
        return mMsg;
    }

    public ResultState getResultState()
    {
        return mState;
    }

    public void setCode(int code)
    {
        responseStatus = code;
        mCode = code;

        if(mCode < 200 || mCode >= 400)
        {
            mState = ResultState.eException;
        }
        else
        {
            mState = ResultState.eSuccess;
        }
    }

    public void setMsg(String msg)
    {
        mMsg = msg;
    }

    public JSONObject getJsonObject()
    {
      if(mRootData == null)
      {
          return null;
      }
      return mRootData.optJSONObject("data");
    }

    public JSONArray getJsonArray()
    {
        if(mRootData == null)
        {
            return null;
        }
        return mRootData.optJSONArray("data");
    }
    
    public Object getData(){
    	if(mRootData == null){
    		return null;
    	}
    	return mRootData.opt("data");
    }

    public String getStringData()
    {
        return mStringData;
    }

    public void setStringData(String data)
    {
        mStringData = data;
    }

    public void setInstream(InputStream is){
        mIs = is;
    }

    public InputStream getmIs(){
        return mIs;
    }

    public void setEntityData(byte[] receivedData)
    {
        mRootData = null;
        if(receivedData == null)
        {
            return;
        }
        try
        {
            mStringData = new String(receivedData);
            mRootData = new JSONObject(new String(receivedData));
			mCode = mRootData.optInt("code");
			mMsg = mRootData.optString("msg");
            Boolean success = mRootData.optBoolean("succeed");
            if(success)
            {
                mState = ResultState.eSuccess;
            }
            else
            {
                mState = ResultState.eFailure;
            }
        }
        catch (JSONException e)
        {
            mState = ResultState.eException;
            setCode(-1);
            setMsg(e.getMessage());
        }


    }

    public JSONObject getMRootData() {
        return mRootData;
    }

    public enum ResultState
    {
        eSuccess,
        eFailure,
        eException,
        timeout;
    }


}
