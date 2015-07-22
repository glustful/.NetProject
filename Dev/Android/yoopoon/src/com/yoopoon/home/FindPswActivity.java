/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: FindPswActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-16 下午4:47:25 
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
import com.yoopoon.home.domain.YzmWithPsw;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: FindPswActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-16 下午4:47:25
 */
@EActivity(R.layout.activity_findpsw)
public class FindPswActivity extends MainActionBarActivity {
	@ViewById(R.id.et_findpsw_phone)
	EditText et_phone;
	@ViewById(R.id.tv_findpsw_phone)
	TextView tv_warning_phone;
	@ViewById(R.id.tv_findpsw_confirm)
	TextView tv_warning_confirm;
	@ViewById(R.id.btn_findpsw_getcode)
	Button btn_getcode;
	@ViewById(R.id.et_findpsw_new)
	EditText et_new;
	@ViewById(R.id.et_findpsw_confirm)
	EditText et_confirm;
	@ViewById(R.id.et_findpsw_code)
	EditText et_code;
	@ViewById(R.id.tv_findpsw_new)
	TextView tv_warning_new;

	private Animation shake_animation;
	private int countTimer = 60;
	private Handler handler = new Handler();
	private String hidm;
	private static final String TAG = "FindPswActivity";

	@Click(R.id.btn_findpsw_getcode)
	void getCode() {
		String phone = et_phone.getText().toString();
		if (TextUtils.isEmpty(phone)) {
			et_phone.startAnimation(shake_animation);
			return;
		}
		String smsType = String.valueOf(SmsUtils.FINDPSW_IDENTIFY_CODE);
		String json = "{\"Mobile\":\"" + phone + "\",\"SmsType\":\"" + smsType + "\"}";

		SmsUtils.requestIdentifyCode(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				Toast.makeText(FindPswActivity.this, msg, Toast.LENGTH_SHORT).show();
			}

		});
		setGetCodeEnable(false);

		handler.postDelayed(new Runnable() {

			@Override
			public void run() {
				setGetCodeEnable(true);
				countTimer = 60;
			}
		}, 60000);

	}

	// Hidm: "7bnjqic71CswqRgJnPD1M%2b%2fuDvR8DUPr5RbaUsaHe4Q%3d"
	// Phone: "13508713650"
	// Yzm: "105982"
	// first_password: "123456"
	// second_password: "123456"

	@Click(R.id.btn_findpsw_ok)
	void findPsw() {
		String code = et_code.getText().toString().trim();
		String psw_new = et_new.getText().toString();
		String psw_confirm = et_confirm.getText().toString();
		String phone = et_phone.getText().toString().trim();

		tv_warning_new.setVisibility(View.GONE);
		tv_warning_new.setVisibility(View.GONE);

		if (TextUtils.isEmpty(code)) {
			et_code.startAnimation(shake_animation);
			return;
		}

		if (TextUtils.isEmpty(phone)) {
			et_phone.startAnimation(shake_animation);
			return;
		}

		if (TextUtils.isEmpty(psw_new)) {
			tv_warning_new.setText("请输入新密码");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		if (TextUtils.isEmpty(psw_confirm)) {
			tv_warning_confirm.setText("请确认新密码");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		YzmWithPsw yzmWithPsw = new YzmWithPsw(hidm, phone, code, psw_new, psw_confirm);

		findPswTask(yzmWithPsw);

	}

	void findPswTask(final YzmWithPsw yzmWithPsw) {
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

				requestFindPsw(serializeResult);

			}
		}).execute();
	}

	void requestFindPsw(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data != null)
					if (data.getResultState() == ResultState.eSuccess) {
						if (data.getMsg().contains("成功")) {
							Toast.makeText(FindPswActivity.this, "修改成功，请登陆", Toast.LENGTH_SHORT).show();
							HomeLoginActivity_.intent(FindPswActivity.this).isManual(true).start();
						} else {
							Toast.makeText(FindPswActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
							clear();
						}
					}
				if (data == null) {
					Toast.makeText(FindPswActivity.this, "请检查网络", Toast.LENGTH_SHORT).show();
					clear();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_findpsw)).SetJSON(json).notifyRequest();
	}

	private void clear() {
		et_phone.setText("");
		et_new.setText("");
		et_confirm.setText("");
		et_code.setText("");
	}

	private Timer timer;
	private TimerTask task;

	private void setGetCodeEnable(boolean enable) {
		if (enable) {
			btn_getcode.setBackgroundResource(R.drawable.cycle_selector);
			btn_getcode.setText("获取验证码");
			timer.cancel();
			task.cancel();
		} else {
			btn_getcode.setBackgroundResource(R.drawable.btn_not_enable);
			timer = new Timer();
			task = new TimerTask() {

				@Override
				public void run() {
					runOnUiThread(new Runnable() {

						@Override
						public void run() {
							String text = "重新获取验证码(" + countTimer-- + "s)";
							btn_getcode.setText(text);
						}
					});
				}
			};
			timer.schedule(task, 0, 1000);
		}
		btn_getcode.setClickable(enable);
		btn_getcode.setFocusable(enable);
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("找回密码");
		shake_animation = AnimationUtils.loadAnimation(this, R.anim.shake);
		et_confirm.addTextChangedListener(watcher);
		et_new.addTextChangedListener(watcher);
	}

	private TextWatcher watcher = new TextWatcher() {

		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {
			String psw_new = et_new.getText().toString();
			String psw_confirm = et_confirm.getText().toString();
			if (psw_new.equals(psw_confirm)) {
				tv_warning_confirm.setVisibility(View.GONE);
			} else {
				tv_warning_confirm.setText("两次输入的密码不一致");
				tv_warning_confirm.setVisibility(View.VISIBLE);
			}

		}

		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {

		}

		@Override
		public void afterTextChanged(Editable s) {

		}
	};

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
