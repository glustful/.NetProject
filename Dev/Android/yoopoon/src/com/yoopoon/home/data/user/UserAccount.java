package com.yoopoon.home.data.user;

import java.io.File;
import java.util.ArrayList;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;

import android.os.Environment;


public class UserAccount {
    

    private static String mLocalDir = Environment.getExternalStorageDirectory().getPath() + "/yunjoy/";

    private User mCurrentUser;  
    public String mEmail;

    private static UserAccount mInstance;

    public static UserAccount getInstance() {
        if (mInstance == null) {
            synchronized (UserAccount.class) {
                if (mInstance == null) {
                    mInstance = new UserAccount();
                }
            }

        }
        return mInstance;
    }

    public static String getSeverHost() {
        return MyApplication.getInstance().getString(R.string.url_host);
    }

    public static String getLocalDir(String subDir) {
        String dirPath = mLocalDir + subDir;
        File dir = new File(dirPath);
        if (!dir.exists()) {
            dir.mkdirs();
        }
        return dirPath;
    }
    

    private UserAccount() {
        mCurrentUser = null;
        
    }

    public int changeCurrentAccount(String account) {
       
        mCurrentUser = null;
        return readCurrentAccount(account);
    }

    private int readCurrentAccount(String account) {
        int res = -1;
        if (account == null || account.length() < 0) {
            return res;
        }

        try {
            JSONObject jRoot = new JSONObject(account);
            Boolean success = jRoot.optBoolean("succeed");
            if (success) {
                JSONObject jUser = jRoot.optJSONObject("data");
                String name = jUser.optString("userName");
                String code = jUser.optString("userCode");
               // mCurrentUser = new User(name, code);
                
                res = 1;
            } else {
                res = 0;
            }

        } catch (JSONException e) {
            res = -1;
        }

        return res;
    }

    public User getCurrentUser() {
        return mCurrentUser;
    }

}
