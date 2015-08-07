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
import android.util.Log;
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
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.domain.YzmWithPsw2;
import com.yoopoon.home.service.SmsService;
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
	@ViewById(R.id.et_security_code)
	EditText et_code;
	@ViewById(R.id.btn_security_setting_getcode)
	Button btn_getcode;
	@ViewById(R.id.tv_security_code)
	TextView tv_warning_code;
	@ViewById(R.id.ib_security_clean_confirm)
	ImageButton ib_clean_confirm;
	@ViewById(R.id.ib_security_clean_new)
	ImageButton ib_clean_new;
	@ViewById(R.id.ib_security_clean_old)
	ImageButton ib_clean_old;
	@ViewById(R.id.tv_security_err)
	TextView tv_err;
	@ViewById(R.id.tv_security_new)
	TextView tv_warning_new;
	@ViewById(R.id.tv_security_confirm)
	TextView tv_warning_confirm;
	@ViewById(R.id.tv_security_old)
	TextView tv_warning_old;
	@ViewById(R.id.rl_security_progress)
	RelativeLayout rl_progress;
	private Animation shake_animation;
	private String hidm;
	private Handler handler = new Handler();
	private static final String TAG = "SecuritySettingActivity";
	private Timer timer = new Timer();
	private int timercount = 60;
	private TimerTask task;
	private Animation anim_open_err;
	private Animation anim_hide_err;
	private Intent service;
	private Vibrator vibrator;

	@TextChange(R.id.et_security_new_psw)
	void newChange() {
		String psw = et_new.getText().toString();
		if (psw.length() > 20 || psw.length() < 6) {
			tv_warning_new.setText("密码必须为6-20位");
			tv_warning_new.setVisibility(View.VISIBLE);
		} else {
			tv_warning_new.setVisibility(View.GONE);
		}
	}

	@TextChange(R.id.et_security_confirm)
	void confirmChange() {
		String new_psw = et_new.getText().toString();
		String cofirm_psw = et_confirm.getText().toString();
		if (!new_psw.equals(cofirm_psw)) {
			tv_warning_confirm.setVisibility(View.VISIBLE);
		} else {
			tv_warning_confirm.setVisibility(View.GONE);
		}
	}

	@TextChange(R.id.et_security_old_psw)
	void changeOld() {
		tv_warning_old.setVisibility(View.GONE);
	}

	@TextChange(R.id.et_security_code)
	void changeCode() {
		tv_warning_code.setVisibility(View.GONE);
	}

	@Click(R.id.btn_security_setting_getcode)
	void getCode() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		}
		String smsType = String.valueOf(SmsUtils.CHANGEPSW_IDENTIFY_CODE);
		String json = "{\"SmsType\":\"" + smsType + "\"}";
		startSmsService();
		// if (!user.isBroker())
		// json = "1";

		Log.i(TAG, "user.isBroker:" + user.isBroker() + ",json = " + json);
		SmsUtils.getCodeForBroker(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				clear();
				showErr(msg);
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

	@Click(R.id.ib_security_clean_confirm)
	void cleanConfirm() {
		et_confirm.setText("");
	}

	@Click(R.id.ib_security_clean_new)
	void cleanNew() {
		et_new.setText("");
	}

	@Click(R.id.ib_security_clean_old)
	void cleanOld() {
		et_old.setText("");
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
	}

	@Click(R.id.btn_security_setting_save)
	void changepsw() {
		String newPsw = et_new.getText().toString();
		String oldPsw = et_old.getText().toString();
		String confirmPsw = et_confirm.getText().toString();
		String code = et_code.getText().toString();
		if (TextUtils.isEmpty(oldPsw)) {
			textWarning(et_old);
			tv_warning_old.setVisibility(View.VISIBLE);
			return;
		}
		if (TextUtils.isEmpty(newPsw)) {
			textWarning(et_new);
			tv_warning_new.setText("请输入新密码");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		int length = newPsw.length();
		if (length < 6 || length > 20) {
			textWarning(et_new);
			tv_warning_new.setText("密码长度必须为6-20位");
			tv_warning_new.setVisibility(View.VISIBLE);
			return;
		}

		if (TextUtils.isEmpty(confirmPsw)) {
			textWarning(et_confirm);
			return;
		}
		if (TextUtils.isEmpty(code)) {
			textWarning(et_code);
			tv_warning_code.setVisibility(View.VISIBLE);
			return;
		}
		if (!newPsw.equals(confirmPsw)) {
			return;
		}
		YzmWithPsw2 yzmWithPsw = new YzmWithPsw2(hidm, code, oldPsw, newPsw, confirmPsw);
		changePswTask(yzmWithPsw);
	}

	void changePswTask(final YzmWithPsw2 yzmWithPsw) {
		rl_progress.setVisibility(View.VISIBLE);
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
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("安全设置");
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

	private void requestChangePsw(String json) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				rl_progress.setVisibility(View.GONE);
				if (data.getMsg().contains("成功")) {
					Toast.makeText(SecuritySettingActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
					HomeLoginActivity_.intent(SecuritySettingActivity.this).isManual(true).start();
					return;
				} else {
					clear();
					showErr(data.getMsg());
				}
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

	private void showErr(String msg) {
		tv_err.setText(msg);
		tv_err.setVisibility(View.VISIBLE);
		tv_err.startAnimation(anim_open_err);
		handler.postDelayed(new Runnable() {
			@Override
			public void run() {
				tv_err.startAnimation(anim_hide_err);
				// tv_err.setVisibility(View.GONE);
			}
		}, 3000);
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
