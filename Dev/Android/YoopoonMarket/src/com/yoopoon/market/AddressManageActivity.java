/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PersonalInfoActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-8 上午9:32:55 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.io.IOException;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;

/**
 * @ClassName: PersonalInfoActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-8 上午9:32:55
 */
@EActivity(R.layout.activity_address_manage)
public class AddressManageActivity extends MainActionBarActivity {
	private static final String TAG = "AddressManageActivity";
	@ViewById(R.id.tv)
	TextView tv;
	MemberAddressEntity addressEntity = null;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("地址管理");
		requestData();
	}

	void requestData() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					parseToEntity(object.toString());
				} else {
					Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_address_byid)).setRequestMethod(RequestMethod.eGet).addParam("id", "11")
				.notifyRequest();
	}

	void parseToEntity(final String json) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity = om.readValue(json, MemberAddressEntity.class);
				} catch (JsonParseException e) {
					e.printStackTrace();
				} catch (JsonMappingException e) {
					e.printStackTrace();
				} catch (IOException e) {
					e.printStackTrace();
				}
				return addressEntity;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					tv.setText(parseResult.toString());
				}
			}
		}).execute();
	}

	public void modify(View v) {
		Log.i(TAG, "modify");
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity.Tel = "22222";
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

	void requestModify(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_modify_address)).setRequestMethod(RequestMethod.ePut).SetJSON(json)
				.notifyRequest();
	}

	public void add(View v) {
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
				Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_add_address)).setRequestMethod(RequestMethod.ePost).SetJSON(json)
				.notifyRequest();
	}

	public void delete(View v) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_delete_address)).setRequestMethod(RequestMethod.eDelete).addParam("id", "11")
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
