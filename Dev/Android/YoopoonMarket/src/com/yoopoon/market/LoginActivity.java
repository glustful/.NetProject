package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.Utils;

@EActivity(R.layout.activity_login)
public class LoginActivity extends MainActionBarActivity {
	private static final String TAG = "LoginActivity";
	@ViewById(R.id.ll_loading)
	View loading;

	@Click(R.id.tv_register)
	void register() {
		RegisterActivity_.intent(this).start();
	}

	@Click(R.id.btn_login)
	void login() {
		Utils.hiddenSoftBorad(this);
		loading.setVisibility(View.VISIBLE);
		final String account = et_account.getText().toString();
		final String psw = et_psw.getText().toString();
		String json = "{\"Username\":\"" + account + "\",\"password\":\"" + psw + "\"}";
		Log.i(TAG, json);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				loading.setVisibility(View.GONE);
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					JSONObject userObject = object.optJSONObject("Object");
					if (status && userObject != null) {
						Log.i(TAG, userObject.toString());
						SharedPreferences sp = getSharedPreferences(getString(R.string.share_preference), MODE_PRIVATE);
						Editor editor = sp.edit();
						editor.putString("UserName", account);
						editor.putString("Password", psw);
						editor.putInt("UserId", userObject.optInt("Id", 0));
						editor.commit();
						finish();// 登录成功，关闭界面
					}
				}
				Toast.makeText(LoginActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_login)).SetJSON(json).setSaveSession(true)
				.notifyRequest();

	}

	@ViewById(R.id.et_account)
	EditText et_account;
	@ViewById(R.id.et_password)
	EditText et_psw;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		backWhiteButton.setVisibility(View.GONE);
		titleButton.setText("账户登录");

		String userName = User.getUserName(this);
		if (!TextUtils.isEmpty(userName))
			et_account.setText(userName);
		String password = User.getProperty(this, "Password");
		if (!TextUtils.isEmpty(password))
			et_psw.setText(password);
		if (!TextUtils.isEmpty(userName) && !TextUtils.isEmpty(password)) {
			login();
		}
	}

	@Override
	public void backButtonClick(View v) {
		sendBroadcast();
		MainActivity_.intent(this).start();
		// finish();
	}

	void sendBroadcast() {
		Intent intent = new Intent("com.yoopoon.market.showshop");
		intent.addCategory(Intent.CATEGORY_DEFAULT);
		this.sendBroadcast(intent);
	}

	@Override
	public void onBackPressed() {
		sendBroadcast();
		MainActivity_.intent(this).start();
		// finish();
	}

	@Override
	public void titleButtonClick(View v) {
		// TODO Auto-generated method stub

	}

	@Override
	public void rightButtonClick(View v) {
		// TODO Auto-generated method stub

	}

	@Override
	public Boolean showHeadView() {
		return true;
	}

}
