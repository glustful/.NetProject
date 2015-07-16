package com.yoopoon.home.ui.login;

import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.TextChange;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import android.content.Context;
import android.os.CountDownTimer;
import android.os.Handler;
import android.os.Message;
import android.preference.PreferenceManager;
import android.text.Spannable;
import android.text.SpannableString;
import android.text.TextUtils;
import android.text.style.ForegroundColorSpan;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;

@EActivity(R.layout.home_register_activity)
public class HomeRegisterActivity extends MainActionBarActivity {
	@ViewById(R.id.register_id_err)
	TextView mErrorText;
	@ViewById(R.id.register_id_email)
	EditText mEmailText;
	@ViewById(R.id.register_id_confire_pwd)
	EditText mPwdConfireText;
	@ViewById(R.id.register_id_phone)
	EditText mPhoneText;
	@ViewById(R.id.register_id_pwd)
	EditText mPwdText;
	@ViewById(R.id.register_no)
	EditText mNoText;
	@ViewById(R.id.register_id_register)
	Button mRegisterBtn;
	@ViewById(R.id.login_id_loading_layout)
	RelativeLayout mLoadingLayout;
	@ViewById(R.id.delMailBtn)
	ImageButton delMailButton;
	@ViewById(R.id.delPwdBtn)
	ImageButton delPassWordButton;
	@ViewById(R.id.delConfirePwdBtn)
	ImageButton delConfirePassWordButton;
	@ViewById(R.id.delPhoneBtn)
	ImageButton delPhoneButton;
	@ViewById(R.id.loginRegister)
	Button registerButton;
	@ViewById(R.id.sendSMS)
	Button sendSMS;
	private Animation animErrOpen = null;
	private Animation animErrClose = null;
	Context mContext;
	private Timer timer = null;
	private final int MSG_HIDE_ERROR = 1;
	CountDownTimer countDownTimer = new CountDownTimer(60000, 1000) {

		@Override
		public void onTick(long millisUntilFinished) {
			sendSMS.setEnabled(false);
			sendSMS.setText("还剩" + millisUntilFinished / 1000 + "秒");
		}

		@Override
		public void onFinish() {
			sendSMS.setEnabled(true);
			sendSMS.setText("发送验证码");

		}
	};
	protected String mobileYzm = "";

	@AfterInject
	void initData() {
		this.mContext = this;
	}

	@AfterViews
	void initUI() {
		this.titleButton.setText("用户注册");
		this.titleButton.setVisibility(View.VISIBLE);
		this.rightButton.setVisibility(View.INVISIBLE);
		this.backButton.setVisibility(View.VISIBLE);
		this.backButton.setText("返回");
		SpannableString span = new SpannableString(this.registerButton.getText());

		ForegroundColorSpan fgcs = new ForegroundColorSpan(getResources().getColor(R.color.second_red));
		span.setSpan(fgcs, span.length() - 2, span.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		this.registerButton.setText(span);
		animErrOpen = AnimationUtils.loadAnimation(this, R.anim.push_down_in);
		animErrClose = AnimationUtils.loadAnimation(this, R.anim.push_top_out);
		mErrorText.setVisibility(View.GONE);
		mLoadingLayout.setVisibility(View.GONE);

	}

	@TextChange(R.id.register_id_email)
	void mailTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delMailButton.setVisibility(View.GONE);

		} else {
			delMailButton.setVisibility(View.VISIBLE);

		}
	}

	@TextChange(R.id.register_id_pwd)
	void passwordTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delPassWordButton.setVisibility(View.GONE);

		} else {
			delPassWordButton.setVisibility(View.VISIBLE);

		}
	}

	@TextChange(R.id.register_id_confire_pwd)
	void passwordConfireTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delConfirePassWordButton.setVisibility(View.GONE);

		} else {
			delConfirePassWordButton.setVisibility(View.VISIBLE);

		}
	}

	@TextChange(R.id.register_id_phone)
	void phoneTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delPhoneButton.setVisibility(View.GONE);

		} else {
			delPhoneButton.setVisibility(View.VISIBLE);

		}
	}

	@Click(R.id.delMailBtn)
	void delMailClick(View v) {
		mEmailText.setText("");
		mEmailText.requestFocus();
	}

	@Click(R.id.delPwdBtn)
	void delPwdClick(View v) {
		mPwdText.setText("");
		mPwdText.requestFocus();
	}

	@Click(R.id.delConfirePwdBtn)
	void delConfirePwdClick(View v) {
		mPwdConfireText.setText("");
		mPwdConfireText.requestFocus();
	}

	@Click(R.id.delPhoneBtn)
	void delPhoneClick(View v) {
		mPhoneText.setText("");
		mPhoneText.requestFocus();
	}

	@Click(R.id.loginRegister)
	void registerClick(View v) {
		HomeLoginActivity_.intent(mContext).isManual(true).start();
		this.finish();
	}

	@Click(R.id.sendSMS)
	void sendSMS() {
		String phone = mPhoneText.getText().toString();
		if (phone == null || phone.length() != 11) {
			showError("非法的手机号码");
			return;
		}
		countDownTimer.start();
		requestIdentify("{\"Mobile\":\"" + phone + "\",\"SmsType\":\"0\"}");
	}

	@Click(R.id.register_id_register)
	void register() {

		String eMail = mEmailText.getText().toString();
		String pwd = mPwdText.getText().toString();

		if (eMail == null || eMail.length() == 0) {
			showError("请输入用户名");
			return;
		}
		if (eMail.length() < 6) {
			showError("用户名至少6个字符");
			return;
		}

		if (pwd == null || pwd.length() == 0) {
			showError("请输入密码");
			return;
		}
		if (pwd.length() < 6) {
			showError("密码至少6个字符");
			return;
		}
		String confire = mPwdConfireText.getText().toString();
		if (confire == null || !confire.equals(pwd)) {
			showError("两次输入的密码不一致");
			return;
		}
		String phone = mPhoneText.getText().toString();
		if (phone == null || phone.length() != 11) {
			showError("非法的手机号码");
			return;
		}
		String no = mNoText.getText().toString();
		if (no == null || no.length() == 0) {
			showError("验证码不能为空");
			return;
		}
		Utils.hiddenSoftBorad(mContext);
		String json = "{";
		json += "\"UserName\":\"" + eMail + "\",";
		json += "\"Password\":\"" + pwd + "\",";
		json += "\"SecondPassword\":\"" + confire + "\",";
		json += "\"Phone\":\"" + phone + "\",";
		json += "\"MobileYzm\":\"" + no + "\",";
		json += "\"Hidm\":\"" + mobileYzm + "\",";
		requestRegeter(eMail, pwd, json);
	}

	@UiThread
	void requestRegeter(final String eMail, final String pwd, String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					ObjectMapper om = new ObjectMapper();
					try {
						User mUser = new User();
						mUser.setUserName(eMail);
						mUser.setPassword(pwd);
						mUser.setRemember(true);
						String result = om.writeValueAsString(mUser);
						PreferenceManager.getDefaultSharedPreferences(mContext).edit().putString("user", result)
								.commit();
						HomeLoginActivity_.intent(mContext).start();
					} catch (JsonProcessingException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
					return;
				}

				showError(data.getMsg());

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_user_addBroker)).SetJSON(json).notifyRequest();

	}

	void requestIdentify(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData().optString("Message", "").equals("1")) {
						mobileYzm = data.getMRootData().optString("Desstr");
						return;
					}
				}
				countDownTimer.cancel();
				sendSMS.setEnabled(true);
				sendSMS.setText("发送验证码");
				showError(data.getMsg());

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_sms_sendSMS)).SetJSON(json).notifyRequest();
	}

	@Override
	public void backButtonClick(View v) {
		// TODO Auto-generated method stub

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
		// TODO Auto-generated method stub
		return true;
	}

	protected void activityYMove() {
		Utils.hiddenSoftBorad(this);
	}

	private void showError(String msg) {
		mErrorText.setText(msg);
		mErrorText.setVisibility(View.VISIBLE);
		mErrorText.startAnimation(animErrOpen);
		clearError();
	}

	private void hideError() {
		mErrorText.setVisibility(View.GONE);
		mErrorText.startAnimation(animErrClose);
	}

	public Handler handler = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			if (msg.what == MSG_HIDE_ERROR) {
				hideError();
			}
		}
	};

	private void clearError() {
		TimerTask task = new TimerTask() {
			@Override
			public void run() {
				Message msg = Message.obtain(handler, MSG_HIDE_ERROR, null);
				msg.sendToTarget();
				if (timer != null) {
					timer.cancel();
					timer = null;
				}
			}
		};
		if (timer != null) {
			timer.cancel();
		}
		timer = new Timer();
		timer.schedule(task, 3000);
	}

}
