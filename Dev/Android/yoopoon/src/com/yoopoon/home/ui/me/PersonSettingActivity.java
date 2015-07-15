/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PersonSettingActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午3:19:15 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.UserInfoListener;

/**
 * @ClassName: PersonSettingActivity
 * @Description: 个人资料设置
 * @author: guojunjun
 * @date: 2015-7-7 下午3:19:15
 */
@EActivity(R.layout.setting_person_view)
public class PersonSettingActivity extends MainActionBarActivity {
	private static final String TAG = "PersonSettingActivity";
	@ViewById(R.id.name)
	EditText et_name;
	@ViewById(R.id.rg_person_setting_sex)
	RadioGroup rg_sex;
	@ViewById(R.id.card)
	EditText et_card;
	@ViewById(R.id.email)
	EditText et_email;
	@ViewById(R.id.rb_person_setting_male)
	RadioButton rb_male;
	@ViewById(R.id.rb_person_setting_female)
	RadioButton rb_female;
	@ViewById(R.id.tv_person_setting_nickname)
	TextView tv_nickname;
	@ViewById(R.id.tv_person_setting_phone)
	TextView tv_phone;

	/**
	 * @Title: initUI
	 * @Description: 初始化界面
	 */
	@AfterViews
	void initUI() {
		// setEnable();
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("个人设置");
		requestUserInfo();
	}

	private void setEnable() {
		rb_female.setEnabled(false);
		rb_male.setEnabled(false);
		et_name.setEnabled(false);
		et_card.setEnabled(false);
	}

	private void requestUserInfo() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			user = new User();
		}

		user.getUserInfo(new UserInfoListener() {

			@Override
			public void success(User user) {
				Log.i(TAG, user.toString());
				String nickName = user.getNickName();
				String userName = user.getRealName();
				String idCard = user.getIdCard();
				String sex = user.getSex();
				String phone = user.getUserName();
				tv_nickname.setText(TextUtils.isEmpty(nickName) ? "" : nickName);
				et_name.setText(TextUtils.isEmpty(userName) ? "" : userName);
				et_card.setText(TextUtils.isEmpty(idCard) ? "" : idCard);
				tv_phone.setText(TextUtils.isEmpty(phone) ? "" : phone);
				if (sex != null) {
					rb_female.setChecked(sex.equals("先生") ? false : true);
					rb_male.setChecked(sex.equals("女士") ? false : true);
				}
			}

			@Override
			public void failed(String msg) {
				Log.i(TAG, msg);
			}
		});
	}

	@Override
	public void backButtonClick(View v) {
		finish();
	}

	@Override
	public void titleButtonClick(View v) {

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {

		return true;
	}

}
