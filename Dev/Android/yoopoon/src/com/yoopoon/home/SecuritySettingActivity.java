/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SecuritySettingActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 上午11:29:44 
 * @version: V1.0   
 */
package com.yoopoon.home;

import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.os.Handler;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.domain.YzmWithPsw2;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: SecuritySettingActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 上午11:29:44
 */
@EActivity(R.layout.activity_security_setting)
public class SecuritySettingActivity extends MainActionBarActivity {
	@ViewById(R.id.et_security_old_psw)
	EditText et_old;
	@ViewById(R.id.et_security_new_psw)
	EditText et_new;
	@ViewById(R.id.et_security_confirm)
	EditText et_confirm;
	@ViewById(R.id.tv_security_warning)
	TextView tv_warning;
	@ViewById(R.id.et_security_setting_code)
	EditText et_code;
	@ViewById(R.id.btn_security_setting_getcode)
	Button btn_getcode;
	@ViewById(R.id.tv_security_time)
	TextView tv_time;
	private Animation shake_animation;
	private String hidm;
	private Handler handler = new Handler();
	private static final String TAG = "SecuritySettingActivity";
	private Timer timer = new Timer();
	private int timercount = 60;
	private TimerTask task;

	@Click(R.id.btn_security_setting_getcode)
	void getCode() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		}
		String smsType = String.valueOf(SmsUtils.CHANGEPSW_IDENTIFY_CODE);
		String json = "{\"SmsType\":\"" + smsType + "\"}";
		SmsUtils.getCodeForBroker(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				clear();
				Toast.makeText(SecuritySettingActivity.this, msg, Toast.LENGTH_SHORT).show();

			}

		});
		setGetCodeEnable(false);
		handler.postDelayed(new Runnable() {

			@Override
			public void run() {
				setGetCodeEnable(true);
				timercount = 60;
			}
		}, 60000);
		timer.schedule(task, 0, 1000);

	}

	private void setGetCodeEnable(boolean enable) {
		btn_getcode.setBackgroundResource(R.drawable.cycle_selector);

		if (!enable) {
			btn_getcode.setBackgroundResource(R.drawable.btn_not_enable);
			timer = new Timer();
			task = new TimerTask() {

				@Override
				public void run() {
					runOnUiThread(new Runnable() {

						@Override
						public void run() {
							btn_getcode.setText("重新获取验证码(" + timercount-- + ")");

						}
					});
				}
			};
		} else {
			task.cancel();
			timer.cancel();
		}
		btn_getcode.setText(enable ? "获取验证码" : "重新获取验证码");
		btn_getcode.setFocusable(enable);
		btn_getcode.setClickable(enable);
		tv_time.setVisibility(enable ? View.GONE : View.VISIBLE);
	}

	@Click(R.id.btn_security_setting_save)
	void changepsw() {
		String newPsw = et_new.getText().toString();
		String oldPsw = et_old.getText().toString();
		String confirmPsw = et_confirm.getText().toString();
		String code = et_code.getText().toString();

		if (TextUtils.isEmpty(oldPsw)) {
			et_old.startAnimation(shake_animation);
			return;
		}

		if (TextUtils.isEmpty(newPsw)) {
			et_new.startAnimation(shake_animation);
			return;
		}

		if (TextUtils.isEmpty(confirmPsw)) {
			et_confirm.startAnimation(shake_animation);
			return;
		}

		if (TextUtils.isEmpty(code)) {
			et_code.startAnimation(shake_animation);
			return;
		}

		tv_warning.setVisibility(newPsw.equals(confirmPsw) ? View.GONE : View.VISIBLE);
		YzmWithPsw2 yzmWithPsw = new YzmWithPsw2(hidm, code, oldPsw, newPsw, confirmPsw);
		changePswTask(yzmWithPsw);
	}

	void changePswTask(final YzmWithPsw2 yzmWithPsw) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();

				try {
					return om.writeValueAsString(yzmWithPsw);
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (serializeResult == null || serializeResult.equals("")) {

					return;
				}

				requestChangePsw(serializeResult);

			}
		}).execute();
	}

	/**
	 * @Title: main
	 * @Description: TODO
	 * @param args
	 */
	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("安全设置");
		shake_animation = AnimationUtils.loadAnimation(this, R.anim.shake);
		et_confirm.addTextChangedListener(new TextWatcher() {

			@Override
			public void onTextChanged(CharSequence s, int start, int before, int count) {
				String newPsd = et_new.getText().toString();
				if (!TextUtils.isEmpty(newPsd)) {
					String confirm = et_confirm.getText().toString();
					if (confirm.equals(newPsd))
						tv_warning.setVisibility(View.GONE);
					else
						tv_warning.setVisibility(View.VISIBLE);

				}
			}

			@Override
			public void beforeTextChanged(CharSequence s, int start, int count, int after) {
				// TODO Auto-generated method stub

			}

			@Override
			public void afterTextChanged(Editable s) {
				// TODO Auto-generated method stub

			}
		});
	}

	private void requestChangePsw(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data != null) {
					if (data.getResultState() == ResultState.eSuccess) {
						if (data.getMsg().contains("成功")) {
							Toast.makeText(SecuritySettingActivity.this, data.getMsg() + ",请重新登陆", Toast.LENGTH_SHORT)
									.show();
							HomeLoginActivity_.intent(SecuritySettingActivity.this).isManual(true).start();
							return;
						} else if (data.getMsg().contains("失败")) {
							clear();
							Toast.makeText(SecuritySettingActivity.this, data.getMsg() + ",请重试", Toast.LENGTH_SHORT)
									.show();
							return;
						}

					}
					clear();
					Toast.makeText(SecuritySettingActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
					return;
				}
				clear();
				Toast.makeText(SecuritySettingActivity.this, "请求失败，请检查网络", Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_change_psw)).SetJSON(json).notifyRequest();
	}

	private void clear() {
		et_code.setText("");
		et_confirm.setText("");
		et_old.setText("");
		et_new.setText("");
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
