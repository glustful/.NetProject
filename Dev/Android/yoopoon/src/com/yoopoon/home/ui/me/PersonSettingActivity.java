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

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.apache.http.Header;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.protocol.HTTP;
import org.apache.http.util.EntityUtils;
import android.content.ContentResolver;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.AsyncTask;
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
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.UserInfoListener;
import com.yoopoon.home.domain.Broker2.RequesListener;
import com.yoopoon.home.domain.BrokerEntity;
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
		BrokerEntity broker = new BrokerEntity(id, nickname, nickname, phone, sfz, email, name, sexy);
		broker.modifyInfo(new RequesListener() {

			@Override
			public void succeed(String msg) {
				Toast.makeText(PersonSettingActivity.this, msg, Toast.LENGTH_SHORT).show();
				SettingActivity_.intent(PersonSettingActivity.this).start();
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
			if (uri != null) {
				ContentResolver cr = this.getContentResolver();
				try {
					File file = new File(uri.toString());
					String filename = file.getName();
					Bitmap bitmap = BitmapFactory.decodeStream(cr.openInputStream(uri));
					/* 将Bitmap设定到ImageView */
					iv_avater.setImageBitmap(bitmap);
					uploadImage(filename);
				} catch (FileNotFoundException e) {
					Log.e("Exception", e.getMessage(), e);
				}
			}
		}
		super.onActivityResult(requestCode, resultCode, data);
	}

	private void uploadImage(final String filename) {
		new AsyncTask<String, Integer, String>() {

			@Override
			protected String doInBackground(String... params) {
				String uriAPI = params[0];// Post方式没有参数在这里
				String result = "";
				HttpPost httpRequst = new HttpPost(uriAPI);// 创建HttpPost对象

				// Content-Disposition: form-data; name="fileToUpload"; filename="head_img1.jpg"
				// Content-Type: image/jpeg

				List<NameValuePair> postParams = new ArrayList<NameValuePair>();
				HttpParams httpParameters = new BasicHttpParams();
				HttpConnectionParams.setConnectionTimeout(httpParameters, 10 * 1000);// 设置请求超时10秒
				HttpConnectionParams.setSoTimeout(httpParameters, 10 * 1000); // 设置等待数据超时10秒
				HttpConnectionParams.setSocketBufferSize(httpParameters, 8192);

				httpRequst.setHeader("Content-Type", "image/jpeg");

				String disposition = "form-data; name=\"fileToUpload\"; filename=\"" + filename + "\"";
				httpRequst.setHeader("Content-Disposition", disposition);

				Header[] positionHeaders = httpRequst.getHeaders("Content-Disposition");
				for (Header header : positionHeaders) {
					Log.i(TAG, header.toString());
				}

				Header[] contentHeaders = httpRequst.getHeaders("Content-Type");
				for (Header header : contentHeaders) {
					Log.i(TAG, header.toString());
				}

				try {

					httpRequst.setEntity(new UrlEncodedFormEntity(postParams, HTTP.UTF_8));
					HttpResponse httpResponse = new DefaultHttpClient(httpParameters).execute(httpRequst);
					if (httpResponse.getStatusLine().getStatusCode() == 200) {
						HttpEntity httpEntity = httpResponse.getEntity();
						result = EntityUtils.toString(httpEntity);// 取出应答字符串
					} else {
						HttpEntity httpEntity = httpResponse.getEntity();
						Log.i(TAG, "请求码 ：" + httpResponse.getStatusLine().getStatusCode());
						result = EntityUtils.toString(httpEntity);
					}
				} catch (UnsupportedEncodingException e) {
					e.printStackTrace();
					result = e.getMessage().toString();
				} catch (ClientProtocolException e) {
					e.printStackTrace();
					result = e.getMessage().toString();
				} catch (IOException e) {
					e.printStackTrace();
					result = e.getMessage().toString();
				}

				return result;
			}

			@Override
			protected void onPostExecute(String result) {
				Toast.makeText(PersonSettingActivity.this, result, Toast.LENGTH_LONG).show();
				super.onPostExecute(result);
			}

		}.execute(getString(R.string.url_host) + getString(R.string.url_upload));
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
				String nickName = user.getNickName();
				String userName = user.getRealName();
				String idCard = user.getIdCard();
				String sex = user.getSex();
				String phone = user.getPhone();
				String email = user.getEmail();
				String photo = user.getHeadUrl();
				tv_nickname.setText(TextUtils.isEmpty(nickName) ? "" : nickName);
				et_name.setText(TextUtils.isEmpty(userName) ? "" : userName);
				et_card.setText(TextUtils.isEmpty(idCard) ? "" : idCard);
				tv_phone.setText(TextUtils.isEmpty(phone) ? "" : phone);
				et_email.setText(TextUtils.isEmpty(email) ? "" : email);
				if (sex != null) {
					rb_female.setChecked(sex.equals("先生") ? false : true);
					rb_male.setChecked(sex.equals("女士") ? false : true);
				}
				if (!TextUtils.isEmpty(photo)) {
					String url = "http://img.yoopoon.com/" + photo;
					ImageLoader.getInstance().displayImage(url, iv_avater);
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
