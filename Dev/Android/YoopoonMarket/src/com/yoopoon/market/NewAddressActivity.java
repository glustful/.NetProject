package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;

@EActivity(R.layout.activity_new_address)
public class NewAddressActivity extends MainActionBarActivity {
	private static final String TAG = "NewAddressActivity";
	@ViewById(R.id.et_address)
	EditText et_address;
	@ViewById(R.id.et_phone)
	EditText et_phone;
	@ViewById(R.id.et_linkman)
	EditText et_linkman;
	@ViewById(R.id.et_postno)
	EditText et_postno;
	@ViewById(R.id.tv1)
	TextView tv_select;
	String areaId = "0";
	@ViewById(R.id.ll_loading)
	View loading;

	@Click(R.id.tv1)
	void selectArea() {
		SearchActivity_.intent(this).which(1).start();
	}

	@Click(R.id.btn_save)
	void newAddress() {
		SharedPreferences sp = getSharedPreferences(getString(R.string.share_preference), MODE_PRIVATE);
		int userid = sp.getInt("UserId", 0);

		if (userid != 0) {
			loading.setVisibility(View.VISIBLE);
			if (areaId.equals("0")) {
				Toast.makeText(NewAddressActivity.this, "请选择地区", Toast.LENGTH_SHORT).show();
				return;
			}

			MemberAddressEntity entity = new MemberAddressEntity();
			entity.UserId = userid;
			entity.Address = et_address.getText().toString();
			entity.Zip = et_postno.getText().toString();
			entity.Linkman = et_linkman.getText().toString();
			entity.Tel = et_phone.getText().toString();
			entity.AreaId = Integer.parseInt(areaId);
			entity.IsDefault = "false";

			add(entity);
		} else {
			// 未登录
			LoginActivity_.intent(this).start();
			return;
		}
	}

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		backButton.setVisibility(View.GONE);
		rightButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.VISIBLE);

		headView.setBackgroundColor(Color.RED);
		titleButton.setText("新增地址");
		titleButton.setTextColor(Color.WHITE);

	}

	void add(final MemberAddressEntity addressEntity) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					return om.writeValueAsString(addressEntity);
				} catch (JsonProcessingException e) {
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (serializeResult != null) {
					Log.i(TAG, serializeResult);
					requestAdd(serializeResult);
				}
			}
		}).execute();
	}

	void requestAdd(String json) {
		Log.i(TAG, json);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				loading.setVisibility(View.GONE);
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();// 添加成功，关闭界面
				}

				Toast.makeText(NewAddressActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_add_address)).setRequestMethod(RequestMethod.ePost).SetJSON(json)
				.notifyRequest();
	}

	@Override
	@SuppressLint("InflateParams")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		registerAddressReceiver();
	}

	void registerAddressReceiver() {
		IntentFilter filter = new IntentFilter("com.yoopoon.market.address");
		filter.addCategory(Intent.CATEGORY_DEFAULT);
		this.registerReceiver(receiver, filter);
	}

	BroadcastReceiver receiver = new BroadcastReceiver() {

		@Override
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();
			if (action.equals("com.yoopoon.market.address")) {
				areaId = intent.getExtras().getString("AreaId", "69");
				String[] names = intent.getStringArrayExtra("Name");
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < names.length; i++) {
					if (names[i] != null)
						sb.append(names[i].trim() + " ");
				}
				Toast.makeText(NewAddressActivity.this, sb.toString(), Toast.LENGTH_SHORT).show();
				tv_select.setText(sb.toString());
				Log.i(TAG, tv_select.getText().toString());
			}
		}
	};

	protected void onDestroy() {
		super.onDestroy();
		if (receiver != null) {
			this.unregisterReceiver(receiver);
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
