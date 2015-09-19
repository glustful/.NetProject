package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import android.graphics.Color;
import android.util.Log;
import android.view.View;
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

	@Click(R.id.btn_save)
	void newAddress() {
		MemberAddressEntity entity = new MemberAddressEntity();
		entity.Address = "sdsad";
		entity.Zip = "10011";
		entity.Linkman = "张三";
		entity.Tel = "100595236";
		entity.Adduser = "12";
		entity.Addtime = "\\/Date(1442387652967)\\/";
		entity.Upduser = 12;
		entity.Updtime = "\\/Date(1442387652967)\\/";
		add(entity);
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
				addressEntity.Address = "大理白族自治州";
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
				requestAdd(serializeResult);
			}
		}).execute();
	}

	void requestAdd(String json) {
		Log.i(TAG, json);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
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
