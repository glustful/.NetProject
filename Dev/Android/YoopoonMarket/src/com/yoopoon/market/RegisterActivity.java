package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.os.CountDownTimer;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberModel;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;

@EActivity(R.layout.activity_register)
public class RegisterActivity extends MainActionBarActivity {
	private static final String TAG = "RegisterActivity";

	@Click(R.id.tv_getcode)
	void getCode() {
		new CountDownTimer(60000, 1000) {

			@Override
			public void onTick(long millisUntilFinished) {
				tv_getcode.setBackgroundResource(R.drawable.gray_cycle_selector);
				tv_getcode.setClickable(false);
				tv_getcode.setPadding(5, 5, 5, 5);
				tv_getcode.setText("重新获取验证码(" + millisUntilFinished / 1000 + "s)");

			}

			@Override
			public void onFinish() {
				tv_getcode.setBackgroundResource(R.drawable.cycle_selector);
				tv_getcode.setPadding(5, 5, 5, 5);
				tv_getcode.setText("获取验证码");
				tv_getcode.setClickable(true);

			}
		}.start();
	}

	@ViewById(R.id.tv_getcode)
	TextView tv_getcode;

	@Click(R.id.btn_register)
	void register() {
		String username = et_username.getText().toString();
		String psw = et_psw.getText().toString();
		String confirm = et_confirm.getText().toString();

		model.UserName = username;
		model.Phone = username;
		model.Password = psw;
		model.SecondPassword = confirm;
		serializeJson();

	}

	@ViewById(R.id.et_username)
	EditText et_username;
	@ViewById(R.id.et_code)
	EditText et_code;
	@ViewById(R.id.et_psw)
	EditText et_psw;
	@ViewById(R.id.et_confirm)
	EditText et_confirm;

	MemberModel model = new MemberModel();

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.GONE);
		titleButton.setText("免费注册");
		rightButton.setVisibility(View.GONE);

	}

	void serializeJson() {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				String json = null;
				try {
					json = om.writeValueAsString(model);
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return json;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (!TextUtils.isEmpty(serializeResult)) {
					Log.i(TAG, serializeResult);
					requestRegister(serializeResult);
				}

			}
		}).execute();
	}

	void requestRegister(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();// 注册成功，关闭界面
				}
				Toast.makeText(RegisterActivity.this, data.toString(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_member_register)).setRequestMethod(RequestMethod.ePost).SetJSON(json)
				.notifyRequest();
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
		return true;
	}

}
