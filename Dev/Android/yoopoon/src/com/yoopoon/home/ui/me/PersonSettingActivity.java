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

import java.io.FileNotFoundException;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.content.ContentResolver;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.Toast;
import com.makeramen.RoundedImageView;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.UserInfoListener;
import com.yoopoon.home.domain.Broker2;
import com.yoopoon.home.domain.Broker2.RequesListener;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

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
	@ViewById(R.id.iv_person_setting_avater)
	RoundedImageView iv_avater;
	private Animation animation_shake;

	@Click(R.id.iv_person_setting_avater)
	void selectAvater() {
		Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
		intent.setType("image/*");
		this.startActivityForResult(intent, 1);
	}

	@Click(R.id.save)
	void modifyBrokerInfo() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		}

		String name = et_name.getText().toString();
		String sfz = et_card.getText().toString();
		String email = et_email.getText().toString();
		String nickname = tv_nickname.getText().toString();
		String phone = tv_phone.getText().toString();

		if (TextUtils.isEmpty(name)) {
			et_name.startAnimation(animation_shake);
			return;
		}

		if (TextUtils.isEmpty(email)) {
			et_email.startAnimation(animation_shake);
			return;
		}

		if (TextUtils.isEmpty(sfz)) {
			et_card.startAnimation(animation_shake);
			return;
		}

		String sexy = rb_female.isChecked() ? "女士" : "先生";
		int id = user.getId();
		Broker2 broker = new Broker2(id, id, name, user.getRealName(), nickname, sexy, sfz, email, phone, "");
		broker.modifyInfo(new RequesListener() {

			@Override
			public void succeed(String msg) {
				Toast.makeText(PersonSettingActivity.this, msg, Toast.LENGTH_SHORT).show();
				finish();
			}

			@Override
			public void fail(String msg) {
				Toast.makeText(PersonSettingActivity.this, msg, Toast.LENGTH_SHORT).show();
			}
		});

	}

	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (resultCode == RESULT_OK) {
			Uri uri = data.getData();
			Log.e("uri", uri.toString());
			ContentResolver cr = this.getContentResolver();
			try {
				Bitmap bitmap = BitmapFactory.decodeStream(cr.openInputStream(uri));
				/* 将Bitmap设定到ImageView */
				iv_avater.setImageBitmap(bitmap);
			} catch (FileNotFoundException e) {
				Log.e("Exception", e.getMessage(), e);
			}
		}
		super.onActivityResult(requestCode, resultCode, data);
	}

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
		animation_shake = AnimationUtils.loadAnimation(this, R.anim.shake);
		requestUserInfo();
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
				String phone = user.getPhone();
				String email = user.getEmail();
				tv_nickname.setText(TextUtils.isEmpty(nickName) ? "" : nickName);
				et_name.setText(TextUtils.isEmpty(userName) ? "" : userName);
				et_card.setText(TextUtils.isEmpty(idCard) ? "" : idCard);
				tv_phone.setText(TextUtils.isEmpty(phone) ? "" : phone);
				et_email.setText(TextUtils.isEmpty(email) ? "" : email);
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
