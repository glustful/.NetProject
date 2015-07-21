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

import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.app.AlertDialog.Builder;
import android.content.DialogInterface;
import android.os.Handler;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Log;
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
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.domain.Bank;

/**
 * @ClassName: AddBankActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 下午4:25:37
 */
@EActivity(R.layout.activity_add_bank)
public class AddBankActivity extends MainActionBarActivity {
	@Click(R.id.btn_add_bank_getcode)
	void getCode() {
		String smsType = String.valueOf(SmsUtils.ADD_BANKCARD_IDENTIFY_CODE);
		String json = "{\"SmsType\":\"" + smsType + "\"}";
		SmsUtils.getCodeForBroker(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
				Log.i(TAG, "hidm = " + hidm);
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
		for (int i = 0; i < info_ets.size(); i++) {
			EditText et = info_ets.get(i);
			String text = et.getText().toString();
			if (TextUtils.isEmpty(text)) {
				warning_tvs.get(i).setVisibility(View.VISIBLE);
				et.startAnimation(shake_animation);
				return;
			}
		}

		String address = et_address.getText().toString();
		String code = et_code.getText().toString();
		String num = et_card.getText().toString().trim();
		String type = rb_credit.isChecked() ? "信用卡" : "储蓄卡";

		addBankTask(new Bank(address, checkedBank + 1, hidm, code, num, type));

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
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data != null)
					Log.i(TAG, data.toString());
				if (data.getResultState() == ResultState.eSuccess) {
					String msg = data.getMsg();
					Toast.makeText(AddBankActivity.this, msg, Toast.LENGTH_SHORT).show();
					if (msg.contains("成功")) {
						finish();
					}
				}
				if (data == null) {
					Toast.makeText(AddBankActivity.this, "请检查网络", Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_add_bank)).SetJSON(json).notifyRequest();
	}

	private String hidm;
	private Timer timer;
	private TimerTask task;
	private int countTimer = 60;
	private Handler handler = new Handler();
	private Animation shake_animation;
	private static final String TAG = "AddBankActivity";
	private String[] banks = { "中国银行", "中国工商银行", "中国农业银行", "中国建设银行" };
	private int checkedBank = 0;
	private List<EditText> info_ets = new ArrayList<EditText>();
	private List<TextView> warning_tvs = new ArrayList<TextView>();
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
	@ViewById(R.id.tv_addbank_warning2)
	TextView tv_warning_bank;
	@ViewById(R.id.tv_addbank_warning3)
	TextView tv_warning_address;
	@ViewById(R.id.tv_addbank_warning4)
	TextView tv_warning_code;

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
		titleButton.setText("添加银行卡");
		shake_animation = AnimationUtils.loadAnimation(this, R.anim.shake);
		initDatas();
	}

	private void initDatas() {
		tv_bank.setText(banks[checkedBank]);

		info_ets.add(et_card);
		info_ets.add(et_address);
		info_ets.add(et_code);

		warning_tvs.add(tv_warning_card);
		warning_tvs.add(tv_warning_bank);
		warning_tvs.add(tv_warning_address);
		warning_tvs.add(tv_warning_code);

		for (EditText et : info_ets)
			et.addTextChangedListener(watcher);
	}

	private TextWatcher watcher = new TextWatcher() {

		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {

			for (int i = 0; i < info_ets.size(); i++) {
				EditText et = info_ets.get(i);
				String text = et.getText().toString();
				if (!TextUtils.isEmpty(text))
					warning_tvs.get(i).setVisibility(View.GONE);
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
