/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: AddBankActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 下午4:25:37 
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
import android.app.AlertDialog.Builder;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Color;
import android.os.Handler;
import android.os.Vibrator;
import android.text.TextUtils;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.RegxUtils;
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.domain.Bank;
import com.yoopoon.home.service.SmsService;

/**
 * @ClassName: AddBankActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 下午4:25:37
 */
@EActivity(R.layout.activity_add_bank)
public class AddBankActivity extends MainActionBarActivity {
	@TextChange(R.id.et_addbank_address)
	void addressChange() {
		tv_warning_address.setVisibility(View.GONE);
	}

	@TextChange(R.id.et_addbank_code)
	void codeChange() {
		tv_warning_code.setVisibility(View.GONE);
	}

	@TextChange(R.id.et_addbank_num)
	void numChange() {
		tv_warning_card.setVisibility(View.GONE);
	}

	@Click(R.id.btn_add_bank_getcode)
	void getCode() {
		String smsType = String.valueOf(SmsUtils.ADD_BANKCARD_IDENTIFY_CODE);
		String json = "{\"SmsType\":\"" + smsType + "\"}";
		startSmsService();
		SmsUtils.getCodeForBroker(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				Toast.makeText(AddBankActivity.this, msg, Toast.LENGTH_SHORT).show();
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

	@Click(R.id.btn_addbank_ok)
	void addBank() {
		String address = et_address.getText().toString();
		String code = et_code.getText().toString();
		String num = et_card.getText().toString().trim();
		String type = rb_credit.isChecked() ? "信用卡" : "储蓄卡";

		if (TextUtils.isEmpty(num)) {
			textWarning(et_card);
			tv_warning_card.setText("请填写银行卡号");
			tv_warning_card.setVisibility(View.VISIBLE);
			return;
		}

		if (!RegxUtils.isBankCard(num)) {
			textWarning(et_card);
			tv_warning_card.setText("请填写正确的银行卡号");
			tv_warning_card.setVisibility(View.VISIBLE);
			return;
		}

		if (TextUtils.isEmpty(address)) {
			textWarning(et_address);
			tv_warning_address.setText("请填写开户银行地址");
			tv_warning_address.setVisibility(View.VISIBLE);
			return;
		}

		if (TextUtils.isEmpty(code)) {
			textWarning(et_code);
			tv_warning_code.setText("请输入验证码");
			tv_warning_code.setVisibility(View.VISIBLE);
			return;
		}

		if (StringUtils.isEmpty(hidm)) {
			textWarning(et_code);
			tv_warning_code.setText("请获取验证码");
			tv_warning_code.setVisibility(View.VISIBLE);
			return;
		}

		Bank bank = new Bank();
		bank.Address = address;
		bank.Bank = checkedBank + 1;
		bank.Hidm = hidm;
		bank.MobileYzm = code;
		bank.Num = num;
		bank.Type = "储蓄卡";
		addBankTask(bank);
	}

	@Click(R.id.tv_addbank_bank)
	void selectBank() {
		Builder builder = new Builder(this);
		builder.setTitle("开户银行");
		builder.setSingleChoiceItems(banks, checkedBank, new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				checkedBank = which;
				dialog.dismiss();
				tv_bank.setText(banks[which]);
			}
		});
		builder.show();
	}

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

	void addBankTask(final Bank bank) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					return om.writeValueAsString(bank);
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
				requestAddBank(serializeResult);
			}
		}).execute();
	}

	void requestAddBank(String json) {
		progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				progress.setVisibility(View.GONE);
				String msg = data.getMsg();
				Toast.makeText(AddBankActivity.this, msg, Toast.LENGTH_SHORT).show();
				if (msg.contains("成功"))
					finish();

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_add_bank)).SetJSON(json).notifyRequest();
	}

	@Override
	protected void onResume() {
		super.onResume();
	}

	private String hidm;
	private Timer timer;
	private TimerTask task;
	private int countTimer = 60;
	private Handler handler = new Handler();
	private Animation shake_animation;
	private Vibrator vibrator;
	private String[] banks = { "中国银行", "中国工商银行", "中国农业银行", "中国建设银行", "交通银行", "中国邮政储蓄银行", "民生银行", "农村信用社", "光大银行" };
	private int checkedBank = 0;
	private Intent service;
	@ViewById(R.id.btn_add_bank_getcode)
	Button btn_getcode;
	@ViewById(R.id.et_addbank_address)
	EditText et_address;
	@ViewById(R.id.tv_addbank_bank)
	TextView tv_bank;
	@ViewById(R.id.et_addbank_num)
	EditText et_card;
	@ViewById(R.id.et_addbank_code)
	EditText et_code;
	@ViewById(R.id.rb_addbank_credit)
	RadioButton rb_credit;
	@ViewById(R.id.rb_addbank_saving)
	RadioButton rb_saving;
	@ViewById(R.id.tv_addbank_warning1)
	TextView tv_warning_card;
	@ViewById(R.id.tv_addbank_warning3)
	TextView tv_warning_address;
	@ViewById(R.id.tv_addbank_warning4)
	TextView tv_warning_code;
	@ViewById(R.id.rl_progress)
	View progress;

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
		titleButton.setText("添加银行卡");
		shake_animation = AnimationUtils.loadAnimation(this, R.anim.shake);
		vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

		tv_bank.setText(banks[checkedBank]);
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
