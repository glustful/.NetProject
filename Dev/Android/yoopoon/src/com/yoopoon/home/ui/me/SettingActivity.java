/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SettingActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午3:19:15 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import java.io.File;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import android.preference.PreferenceManager;
import android.view.View;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.RequestTask;
import com.yoopoon.home.data.storage.LocalPath;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.home.FramMainActivity_;

/**
 * @ClassName: SettingActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-7 下午3:19:15
 */
@EActivity(R.layout.setting_main_view)
public class SettingActivity extends MainActionBarActivity {
	// [start] onClick
	@Click(R.id.person_info)
	void settingPersonInfo() {

	}

	@Click(R.id.security_setting)
	void settingSecurity() {

	}

	@Click(R.id.logout)
	void logout() {
		// 删除以前记录的cookie信息
		File cookieFile = new File(LocalPath.intance().cacheBasePath + "co");
		if (cookieFile.exists()) {
			cookieFile.delete();
		}
		RequestTask.setmCookieStore(null);
		User.mUser = null;
		PreferenceManager.getDefaultSharedPreferences(this).edit().putString("user", "").commit();
		FramMainActivity_.intent(this).start();
		finish();
	}

	// [end]
	@Override
	public void backButtonClick(View v) {

	}

	@Override
	public void titleButtonClick(View v) {

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {

		return null;
	}

}
