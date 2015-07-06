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
      return mRootData.optJSONObject("Object");
    }

    public JSONArray getJsonArray()
    {
        if(mRootData == null)
        {
            return null;
        }
        return mRootData.optJSONArray("Object");
    }
    
    public Object getData(){
    	if(mRootData == null){
    		return null;
    	}
    	return mRootData.opt("Object");
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
            mStringData = checkJSONArray(mStringData);
            mRootData = new JSONObject(mStringData);
			//mCode = mRootData.optInt("code");
			mMsg = mRootData.optString("Msg");
			if(mRootData.isNull("Status")||mRootData.isNull("Object")){
				mState = ResultState.eSuccess;
				return;
			}
            Boolean success = mRootData.optBoolean("Status");
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

    private String checkJSONArray(String mStringData2) {
		try {
			new JSONArray(mStringData2);
			String tmp = "{\"Status\":"+true+",\"Object\":"+mStringData2+"}";
			return tmp;
		} catch (JSONException e) {
			
			e.printStackTrace();
		}
		return mStringData2;
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
