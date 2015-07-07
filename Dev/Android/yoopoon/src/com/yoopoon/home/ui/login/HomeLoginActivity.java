package com.yoopoon.home.ui.login;

import java.io.File;
import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.TextChange;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import android.content.Context;
import android.os.Handler;
import android.os.Message;
import android.preference.PreferenceManager;
import android.text.Spannable;
import android.text.SpannableString;
import android.text.TextUtils;
import android.text.style.ForegroundColorSpan;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.RequestTask;
import com.yoopoon.home.data.storage.LocalPath;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.LoginListener;
import com.yoopoon.home.ui.home.FramMainActivity_;

/**
 * @ClassName: HomeLoginActivity
 * @Description: 登陆界面展示
 * @author: guojunjun
 * @date: 2015-7-7 下午12:54:26
 */
@EActivity(R.layout.home_login_activity)
public class HomeLoginActivity extends MainActionBarActivity {
	static String TAG = "HomeLoginActivity";
	// [start]
	@Extra
	boolean isManual;
	// [end]
	// [start]
	@ViewById(R.id.login_id_err)
	TextView mErrorText;
	@ViewById(R.id.login_id_email)
	EditText mEmailText;
	@ViewById(R.id.login_id_pwd)
	EditText mPwdText;
	@ViewById(R.id.login_id_auto)
	CheckBox mAutoCheck;
	@ViewById(R.id.login_id_login)
	Button mLoginBtn;
	@ViewById(R.id.login_id_loading_layout)
	RelativeLayout mLoadingLayout;
	@ViewById(R.id.delMailBtn)
	ImageButton delMailButton;
	@ViewById(R.id.delPwdBtn)
	ImageButton delPassWordButton;
	@ViewById(R.id.loginRegister)
	Button registerButton;
	// [end]
	// [start]
	private Animation animErrOpen = null;
	private Animation animErrClose = null;
	private final int MSG_HIDE_ERROR = 1;
	private Timer timer = null;
	private Boolean auto = false;
	private Context mContext;
	private User mUser;

	// [end]
	@AfterInject
	void afterInject() {
		this.mContext = this;
	}

	@AfterViews
	void crateData() {
		this.titleButton.setText("用户登陆");
		this.titleButton.setVisibility(View.VISIBLE);
		this.rightButton.setVisibility(View.INVISIBLE);
		this.backButton.setVisibility(View.VISIBLE);
		this.backButton.setText("返回");
		SpannableString span = new SpannableString(this.registerButton.getText());
		ForegroundColorSpan fgcs = new ForegroundColorSpan(getResources().getColor(R.color.second_red));
		span.setSpan(fgcs, span.length() - 2, span.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		this.registerButton.setText(span);
		mAutoCheck.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				auto = !auto;
				mAutoCheck.setChecked(auto);
			}
		});
		mUser = User.lastLoginUser(mContext);
		if (mUser == null) {
			mUser = new User();
			mUser.setUserName("");
			mUser.setPassword("");
		} else {
		}
		initUI();
		initData();
	}

	@TextChange(R.id.login_id_email)
	void mailTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delMailButton.setVisibility(View.GONE);
		} else {
			delMailButton.setVisibility(View.VISIBLE);
		}
	}

	@TextChange(R.id.login_id_pwd)
	void passwordTextChange(CharSequence text, TextView textView, int before, int start, int count) {
		if (TextUtils.isEmpty(text)) {
			delPassWordButton.setVisibility(View.GONE);
		} else {
			delPassWordButton.setVisibility(View.VISIBLE);
		}
	}

	@Click(R.id.delMailBtn)
	void delMailClick(View v) {
		mEmailText.setText("");
		mPwdText.setText("");
	}

	@Click(R.id.delPwdBtn)
	void delPwdClick(View v) {
		mPwdText.setText("");
		mPwdText.requestFocus();
	}

	@Click(R.id.loginRegister)
	void registerClick(View v) {
		HomeRegisterActivity_.intent(mContext).start();
		this.finish();
	}

	@Override
	protected void onRestart() {
		super.onRestart();
	}

	private void initUI() {
		animErrOpen = AnimationUtils.loadAnimation(this, R.anim.push_down_in);
		animErrClose = AnimationUtils.loadAnimation(this, R.anim.push_top_out);
		mErrorText.setVisibility(View.GONE);
		mLoadingLayout.setVisibility(View.GONE);
		mLoginBtn.setOnClickListener(onLogin);
	}

	private void initData() {
		// 删除以前记录的cookie信息
		File cookieFile = new File(LocalPath.intance().cacheBasePath + "co");
		if (cookieFile.exists()) {
			cookieFile.delete();
		}
		RequestTask.setmCookieStore(null);
		String eMail = mUser.getUserName();
		String pwd = mUser.getPassword();
		auto = mUser.isRemember();
		auto = (auto == null) ? false : auto;
		if (auto) {
			mEmailText.setText(eMail);
			mPwdText.setText(pwd);
			requestLogin(eMail, pwd, auto);
		} else {
			mPwdText.setText("");
			if (!isManual) {
				FramMainActivity_.intent(mContext).start();
				finish();
			}
		}
		mAutoCheck.setChecked(auto);
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

	@UiThread
	void requestLogin(final String eMail, final String pwd, Boolean auto) {
		mLoadingLayout.setVisibility(View.VISIBLE);
		mUser.setUserName(eMail);
		mUser.setPassword(pwd);
		mUser.setRemember(auto);
		mUser.login(new LoginListener() {
			@Override
			public void success(final User user) {
				new SerializerJSON(new SerializeListener() {
					@Override
					public String onSerialize() {
						ObjectMapper om = new ObjectMapper();
						try {
							return om.writeValueAsString(user);
						} catch (JsonProcessingException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
						return null;
					}

					@Override
					public void onComplete(String serializeResult) {
						if (serializeResult != null)
							PreferenceManager.getDefaultSharedPreferences(mContext).edit()
									.putString("user", serializeResult).commit();
					}
				}).execute();
				mLoadingLayout.setVisibility(View.GONE);
				com.yoopoon.home.ui.home.FramMainActivity_.intent(mContext).start();
				finish();
			}

			@Override
			public void faild(String msg) {
				mLoadingLayout.setVisibility(View.GONE);
				if (isManual)
					showError(msg);
				else {
					com.yoopoon.home.ui.home.FramMainActivity_.intent(mContext).start();
					finish();
				}
			}
		});
	}

	View.OnClickListener onLogin = new View.OnClickListener() {
		@Override
		public void onClick(View view) {
			String eMail = mEmailText.getText().toString();
			String pwd = mPwdText.getText().toString();
			Boolean auto = mAutoCheck.isChecked();
			if (eMail == null || eMail.length() == 0) {
				showError("请输入账户");
				return;
			}
			if (pwd == null || pwd.length() == 0) {
				showError("请输入密码");
				return;
			}
			Utils.hiddenSoftBorad(mContext);
			requestLogin(eMail, pwd, auto);
		}
	};

	@Override
	public boolean onTouchEvent(android.view.MotionEvent event) {
		InputMethodManager imm = (InputMethodManager) getSystemService(INPUT_METHOD_SERVICE);
		if (imm != null) {
			if (this.getCurrentFocus() != null && this.getCurrentFocus().getWindowToken() != null) {
				return imm.hideSoftInputFromWindow(this.getCurrentFocus().getWindowToken(), 0);
			}
		}
		return true;
	}

	@UiThread
	void errorToLogin() {
		showError("登录失败,请重新登录。");
		// **登陆失败删除cookie下一次重新登陆
		setNullCookie();
		// Toast.makeText(HomeLoginActivity.this, "登录失败,请重新登录。", Toast.LENGTH_SHORT).show();
		mLoadingLayout.setVisibility(View.GONE);
	}

	private void setNullCookie() {
		File cookieFile = new File(LocalPath.intance().cacheBasePath + "co");
		if (cookieFile.exists()) {
			cookieFile.delete();
		}
		RequestTask.setmCookieStore(null);
	}

	@Override
	public void backButtonClick(View v) {
		finish();
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
}
