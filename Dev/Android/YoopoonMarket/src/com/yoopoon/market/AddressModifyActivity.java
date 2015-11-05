package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.annotation.SuppressLint;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.graphics.Color;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.CheckBox;
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
import com.yoopoon.market.utils.RegxUtils;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;

@EActivity(R.layout.activity_modify_address)
public class AddressModifyActivity extends MainActionBarActivity {
	private static final String TAG = "AddressModifyActivity";
	@Extra
	MemberAddressEntity addressEntity;
	@ViewById(R.id.et_address)
	EditText et_address;
	@ViewById(R.id.et_phone)
	EditText et_phone;
	@ViewById(R.id.et_postno)
	EditText et_postno;
	@ViewById(R.id.et_linkman)
	EditText et_linkman;
	@ViewById(R.id.tv_select)
	TextView tv_select;
	@ViewById(R.id.cb)
	CheckBox cb_isdefault;
	@ViewById(R.id.ll_loading)
	View loading;

	int areaId = 0;

	@Click(R.id.tv_select)
	void choose() {
		SearchActivity_.intent(this).which(2).addressEntity(addressEntity).start();
	}

	@Click(R.id.btn_save)
	void modify() {
		if (areaId == 0) {
			Toast.makeText(this, "请选择地区", Toast.LENGTH_SHORT).show();;
			return;
		}

		String address = et_address.getText().toString();
		String zip = et_postno.getText().toString();
		String linkMan = et_linkman.getText().toString();
		String tel = et_phone.getText().toString();

		if (TextUtils.isEmpty(address)) {
			Toast.makeText(this, "请输入详细地址", Toast.LENGTH_LONG).show();
			return;
		}

		if (!RegxUtils.isName(linkMan)) {
			Toast.makeText(this, "请输入正确的联系人姓名", Toast.LENGTH_LONG).show();
			return;
		}
		if (!RegxUtils.isPhone(tel)) {
			Toast.makeText(this, "请输入正确的联系电话", Toast.LENGTH_LONG).show();
			return;
		}
		if (TextUtils.isEmpty(zip)) {
			Toast.makeText(this, "请输入邮编", Toast.LENGTH_LONG).show();
			return;
		}

		loading.setVisibility(View.VISIBLE);
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity.Address = et_address.getText().toString();
					addressEntity.Linkman = et_linkman.getText().toString();
					addressEntity.Zip = et_postno.getText().toString();
					addressEntity.Tel = et_phone.getText().toString();
					addressEntity.AreaId = areaId;
					addressEntity.IsDefault = cb_isdefault.isChecked() ? "true" : "false";
					String json = om.writeValueAsString(addressEntity);
					return json;
				} catch (JsonProcessingException e) {
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				requestModify(serializeResult);
			}
		}).execute();
	}

	@Override
	@SuppressLint("InflateParams")
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		if (savedInstanceState != null) {
			addressEntity = (MemberAddressEntity) savedInstanceState.get("addressEntity");
		}
		registerAddressReceiver();
	}

	@Override
	protected void onSaveInstanceState(Bundle outState) {
		super.onSaveInstanceState(outState);
		outState.putSerializable("addressEntity", addressEntity);
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
				areaId = Integer.parseInt(intent.getExtras().getString("AreaId", "69"));
				String[] names = intent.getStringArrayExtra("Name");
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < names.length; i++) {
					if (names[i] != null)
						sb.append(names[i].trim() + " ");
				}
				tv_select.setText(sb.toString());
			}
		}
	};

	protected void onDestroy() {
		super.onDestroy();
		if (receiver != null) {
			this.unregisterReceiver(receiver);
		}
	};

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setText("修改地址");
		titleButton.setTextColor(Color.WHITE);
		headView.setBackgroundColor(Color.RED);
		rightButton.setVisibility(View.GONE);
		init();
	}

	void init() {
		et_address.setText(addressEntity.Address);
		et_linkman.setText(addressEntity.Linkman);
		et_phone.setText(addressEntity.Tel);
		et_postno.setText(addressEntity.Zip);
		String isDefault = addressEntity.IsDefault;
		if (TextUtils.isEmpty(isDefault) || "false".equals(isDefault)) {
			cb_isdefault.setChecked(false);
		} else if ("true".equals(isDefault)) {
			cb_isdefault.setChecked(true);
		}
		Log.i(TAG, addressEntity.AreaId + "");
		requestArea("123");
	}

	void requestModify(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				loading.setVisibility(View.GONE);
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();
				}
				Toast.makeText(AddressModifyActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_modify_address)).setRequestMethod(RequestMethod.ePut).SetJSON(json)
				.notifyRequest();
	}

	void requestArea(String areaId) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					StringBuilder builder = new StringBuilder();
					JSONObject parentObject = object.optJSONObject("Parent");
					if (parentObject.optJSONObject("Parent") != null) {
						builder.append(parentObject.optJSONObject("Parent").optString("Name", ""));
					}

					builder.append(parentObject.optString("Name", ""));
					builder.append(object.optString("Name", ""));
					if (!TextUtils.isEmpty(builder.toString()))
						tv_select.setText(builder.toString());

				} else {
					Toast.makeText(AddressModifyActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_area_get)).setRequestMethod(RequestMethod.eGet).addParam("id", areaId)
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
