/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: TakeCashStep2Activity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-30 上午9:20:44 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.os.CountDownTimer;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.data.json.SerializerJSON;
import com.yoopoon.home.data.json.SerializerJSON.SerializeListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.domain.AddMoneyEntity;

/**
 * @ClassName: TakeCashStep2Activity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-30 上午9:20:44
 */
@EActivity(R.layout.takecash_step2)
public class TakeCashStep2Activity extends MainActionBarActivity {
	@Extra
	String idsText;

	@ViewById(R.id.spinner_step2)
	Spinner spinner;
	@ViewById(R.id.btn_step2_getcode)
	Button btn_getcode;
	@ViewById(R.id.et_step2_code)
	EditText et_code;
	@ViewById(R.id.tv_step2_warning)
	TextView tv_warning;

	@Click(R.id.btn_step2_getcode)
	void getCode() {
		enableGetCode(false);
		CountDownTimer timer = new CountDownTimer(60000, 1000) {

			@Override
			public void onTick(long millisUntilFinished) {
				btn_getcode.setText("重新获取验证码(" + millisUntilFinished / 1000 + ")");

			}

			@Override
			public void onFinish() {
				btn_getcode.setText("获取验证码");
				enableGetCode(true);

			}
		};
		timer.start();
		requestCode();
	}

	@Click(R.id.bt_step2_ok)
	void commit() {
		String code = StringUtils.trim(et_code.getText().toString());
		if (TextUtils.isEmpty(code)) {
			et_code.startAnimation(shakeAnimation);
			tv_warning.setVisibility(View.VISIBLE);
			return;
		}
		serialData(code);

	}

	private String[] banks;
	private static final String TAG = "TakeCashStep2Activity";
	private String hidm;
	private Animation shakeAnimation;
	private String[] ids;
	private double amountMoney;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("提现");
		shakeAnimation = AnimationUtils.loadAnimation(this, R.anim.shake);
		et_code.addTextChangedListener(watcher);
		requestAllBank();
	}
	private TextWatcher watcher = new TextWatcher() {

		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {

		}

		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {

		}

		@Override
		public void afterTextChanged(Editable s) {
			if (!TextUtils.isEmpty(s.toString()))
				tv_warning.setVisibility(View.GONE);
		}
	};

	void enableGetCode(boolean enable) {
		btn_getcode.setEnabled(enable);
		btn_getcode.setBackgroundResource(enable ? R.drawable.cycle_selector : R.drawable.btn_not_enable);

	}

	void initBanks(JSONArray list) {
		// "Name":"中国建设银行[***6795]","Id":13
		banks = new String[list.length()];
		ids = new String[list.length()];
		for (int i = 0; i < list.length(); i++) {
			try {
				JSONObject obj = list.getJSONObject(i);
				banks[i] = obj.optString("Name", "");
				ids[i] = String.valueOf(obj.optInt("Id", 0));
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		initDatas();
	}

	void requestAllBank() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					try {
						JSONArray list = data.getMRootData().getJSONArray("List");
						amountMoney = data.getMRootData().optDouble("AmountMoney", 0);
						initBanks(list);
					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				} else {
					Toast.makeText(TakeCashStep2Activity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_bank_getall)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	void requestCode() {
		String smsType = String.valueOf(SmsUtils.TAKECASH_IDENTIFY_CODE);
		String json = "{\"SmsType\":\"" + smsType + "\"}";
		SmsUtils.getCodeForBroker(this, json, new RequestSMSListener() {

			@Override
			public void succeed(String code) {
				hidm = code;
			}

			@Override
			public void fail(String msg) {
				Toast.makeText(TakeCashStep2Activity.this, msg, Toast.LENGTH_LONG).show();

			}
		});
	}

	void serialData(final String code) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();

				try {
					int bank = Integer.parseInt(ids[spinner.getSelectedItemPosition()]);
					String money = String.valueOf(amountMoney);
					String id = "";
					for (int i = 0; i < ids.length; i++) {
						id += ids[i] + ",";
					}
					return om.writeValueAsString(new AddMoneyEntity(bank, money, code, hidm, idsText));
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

				requestTakeCash(serializeResult);

			}
		}).execute();
	}

	void requestTakeCash(String json) {
		Log.i(TAG, json);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					boolean status = data.getMRootData().optBoolean("Status");
					String msg = data.getMRootData().optString("Msg", "");
					showDialog(msg, status);
				} else {
					showDialog(data.getMsg(), false);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_broker_withdraw_detail)).SetJSON(json).notifyRequest();
	}

	AlertDialog dialog;

	void showDialog(String msg, boolean status) {
		Builder builder = new Builder(this);
		View view = View.inflate(this, R.layout.activity_takecash, null);
		builder.setView(view);
		TextView tv_status = (TextView) view.findViewById(R.id.tv_dialog_status);
		TextView tv_time = (TextView) view.findViewById(R.id.tv_dialog_time);
		ImageView iv = (ImageView) view.findViewById(R.id.iv_dialog);
		Button btn_ok = (Button) view.findViewById(R.id.btn_dialog);
		tv_status.setText(msg);
		iv.setImageResource(status ? R.drawable.take_cash_finish : R.drawable.takecash_no);
		tv_time.setVisibility(status ? View.VISIBLE : View.GONE);
		dialog = builder.show();

		btn_ok.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				dialog.dismiss();
			}
		});

	}

	private void initDatas() {
		ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_dropdown_item,
				banks);
		spinner.setAdapter(adapter);
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
