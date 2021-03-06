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
import org.androidannotations.annotations.TextChange;
import org.androidannotations.annotations.ViewById;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Color;
import android.os.Handler;
import android.os.Vibrator;
import android.text.TextUtils;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.RegxUtils;
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.domain.YzmWithPsw;
import com.yoopoon.home.service.SmsService;
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
	@ViewById(R.id.tv_findpsw_err)
	TextView tv_err;
	@ViewById(R.id.ib_findpsw_clean_confirm)
	ImageButton ib_clean_confirm;
	@ViewById(R.id.ib_findpsw_clean_new)
	ImageButton ib_clean_new;
	@ViewById(R.id.ib_findpsw_clean_phone)
	ImageButton ib_clean_phone;
	@ViewById(R.id.rl_findpsw_progress)
	RelativeLayout rl_progress;
	@ViewById(R.id.tv_warning_code)
	TextView tv_warning_code;
	private Animation shake_animation;
	private int countTimer = 60;
	private Handler handler = new Handler();
	private String hidm;
	private Animation anim_open_err;
	private Animation anim_hide_err;
	private Intent service;
	private Vibrator vibrator;

	@TextChange(R.id.et_findpsw_code)
	void codeChange() {
		tv_warning_code.setVisibility(View.GONE);
	}

	@TextChange(R.id.et_findpsw_confirm)
	void confirmChange() {
		String new_psw = et_new.getText().toString();
		String confirm_psw = et_confirm.getText().toString();
		if (!new_psw.equals(confirm_psw)) {
			tv_warning_confirm.setText("两次输入的密码不一致");
			tv_warning_confirm.setVisibility(View.VISIBLE);
		} else {
			tv_warning_confirm.setVisibility(View.VISIBLE);
		}
	}

	@TextChange(R.id.et_findpsw_new)
	void newChange() {
		tv_warning_new.setVisibility(View.GONE);
	}

	@TextChange(R.id.et_findpsw_phone)
	void phoneChange() {
		tv_warning_phone.setVisibility(View.GONE);
	}

	@Click(R.id.btn_findpsw_getcode)
	void getCode() {
		String phone = et_phone.getText().toString();
		if (TextUtils.isEmpty(phone)) {
			et_phone.startAnimation(shake_animation);
			tv_warning_phone.setVisibility(View.VISIBLE);
			vibrator.vibrate(500);
			return;
		}
		if (!RegxUtils.isPhone(phone)) {
			showErr("请输入正确的手机号码");
			return;
		}
		String smsType = String.valueOf(SmsUtils.FINDPSW_IDENTIFY_CODE);
		String json = "{\"Mobile\":\"" + phone + "\",\"SmsType\":\"" + smsType + "\"}";
		startSmsService();
		SmsUtils.requestIdentifyCode(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				showErr(msg);
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

	@Click(R.id.ib_findpsw_clean_confirm)
	void cleanConfirm() {
		et_confirm.setText("");
	}

	@Click(R.id.ib_findpsw_clean_phone)
	void cleanPhone() {
		et_phone.setText("");
	}

	@Click(R.id.ib_findpsw_clean_new)
	void cleanNew() {
		et_new.setText("");
	}

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
			tv_warning_code.setVisibility(View.VISIBLE);
			vibrator.vibrate(500);
			return;
		}
		if (TextUtils.isEmpty(phone)) {
			textWarning(et_phone);
			tv_warning_phone.setVisibility(View.VISIBLE);
			return;
		}
		if (!RegxUtils.isPhone(phone)) {
			showErr("请输入正确的手机号码");
			return;
		}
		if (TextUtils.isEmpty(psw_new)) {
			textWarning(et_new);
			tv_warning_new.setText("请输入新密码");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		int length = psw_new.length();
		if (length > 20 || length < 6) {
			textWarning(et_new);
			tv_warning_new.setText("密码长度必须为6-20位");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		if (TextUtils.isEmpty(psw_confirm)) {
			textWarning(et_confirm);
			tv_warning_confirm.setText("请确认新密码");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}
		if (!psw_confirm.equals(psw_new)) {
			return;
		}
		YzmWithPsw yzmWithPsw = new YzmWithPsw(hidm, phone, code, psw_new, psw_confirm);
		findPswTask(yzmWithPsw);
	}

	void findPswTask(final YzmWithPsw yzmWithPsw) {
		rl_progress.setVisibility(View.VISIBLE);
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					return om.writeValueAsString(yzmWithPsw);
				} catch (JsonProcessingException e) {
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
				rl_progress.setVisibility(View.GONE);
				if (data.getMsg().contains("成功")) {
					Toast.makeText(FindPswActivity.this, "修改成功，请登陆", Toast.LENGTH_SHORT).show();
					HomeLoginActivity_.intent(FindPswActivity.this).isManual(true).start();
					return;
				} else {
					showErr(data.getMsg());
					clear();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_findpsw)).SetJSON(json).notifyRequest();
	}

	private void showErr(String msg) {
		tv_err.setText(msg);
		tv_err.setVisibility(View.VISIBLE);
		tv_err.startAnimation(anim_open_err);
		handler.postDelayed(new Runnable() {

			@Override
			public void run() {
				tv_err.startAnimation(anim_hide_err);
			}
		}, 3000);
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
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("找回密码");
		shake_animation = AnimationUtils.loadAnimation(this, R.anim.shake);

		vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

		anim_open_err = AnimationUtils.loadAnimation(this, R.anim.push_down_in);

		anim_hide_err = AnimationUtils.loadAnimation(this, R.anim.push_top_out);
		anim_hide_err.setAnimationListener(new AnimationListener() {

			@Override
			public void onAnimationStart(Animation animation) {
				// TODO Auto-generated method stub

			}

			@Override
			public void onAnimationRepeat(Animation animation) {
				// TODO Auto-generated method stub

			}

			@Override
			public void onAnimationEnd(Animation animation) {
				tv_err.setVisibility(View.GONE);
			}
		});

	}

	private void textWarning(View v) {
		v.startAnimation(shake_animation);
		vibrator.vibrate(500);
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

	@Override
	protected void activityYMove() {
		Utils.hiddenSoftBorad(this);
	}

	private void startSmsService() {
		service = new Intent(this, SmsService.class);
		startService(service);
		IntentFilter filter = new IntentFilter(Utils.GET_CODE_ACTION);
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, filter);
	}

	private BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();
			if (Utils.GET_CODE_ACTION.equals(action)) {
				String code = intent.getExtras().getString("Code");
				et_code.setText(code);
				if (service != null) {
					stopService(service);
					service = null;
				}
			}
			unregisterReceiver(receiver);
		}
	};
}
