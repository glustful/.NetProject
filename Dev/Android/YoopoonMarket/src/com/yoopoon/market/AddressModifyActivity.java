package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.graphics.Color;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
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

	@Click(R.id.btn_save)
	void modify() {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity.Address = et_address.getText().toString();
					addressEntity.Linkman = et_address.getText().toString();
					addressEntity.Zip = et_postno.getText().toString();
					addressEntity.Tel = et_phone.getText().toString();
					String json = om.writeValueAsString(addressEntity);
					return json;
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				Log.i(TAG, serializeResult);
				requestModify(serializeResult);
			}
		}).execute();
	}

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
	}

	void requestModify(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
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
