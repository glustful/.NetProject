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
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.graphics.Color;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberModel;
import com.yoopoon.market.domain.User;
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
@EActivity(R.layout.activity_modify_info)
public class PersonalInfoActivity extends MainActionBarActivity {
	private static final String TAG = "PersonalInfoActivity";
	@ViewById(R.id.ll_loading)
	View ll_loading;
	@ViewById(R.id.et_name)
	EditText et_name;
	@ViewById(R.id.et_phone)
	EditText et_phone;
	@ViewById(R.id.et_postno)
	EditText et_postno;
	@ViewById(R.id.rb_male)
	RadioButton rb_male;
	@ViewById(R.id.rb_female)
	RadioButton rb_female;
	MemberModel member;

	@Click(R.id.btn_save)
	void save() {
		modify();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.GONE);
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);

		headView.setBackgroundColor(Color.RED);
		titleButton.setText("个人资料");
		titleButton.setTextColor(Color.WHITE);
		requestData();
	}

	public void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		String userid = User.getUserId(PersonalInfoActivity.this);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				JSONObject object = data.getMRootData();
				if (object != null) {
					parseToMember(object.toString());

				} else {
					ll_loading.setVisibility(View.GONE);
					Toast.makeText(PersonalInfoActivity.this, data.toString(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_getmemeber_byid)).setRequestMethod(RequestMethod.eGet).addParam("id", "17")
				.notifyRequest();
	}

	void parseToMember(final String json) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				try {
					member = om.readValue(json, MemberModel.class);
				} catch (JsonParseException e) {
					e.printStackTrace();
				} catch (JsonMappingException e) {
					e.printStackTrace();
				} catch (IOException e) {
					e.printStackTrace();
				}
				return member;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					Log.i(TAG, parseResult.toString());
					ll_loading.setVisibility(View.GONE);
					fillData();
				}
			}
		}).execute();
	}

	void fillData() {
		et_name.setText(member.RealName);
		et_phone.setText(member.Phone);
		et_postno.setText(member.PostNo);
		rb_male.setChecked((member.Gender == 0));
		rb_female.setChecked((member.Gender == 1));
	}

	void modify() {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					member.Phone = et_phone.getText().toString();
					member.PostNo = et_postno.getText().toString();
					member.RealName = et_name.getText().toString();
					member.Gender = rb_male.isChecked() ? 0 : 1;
					String json = om.writeValueAsString(member);
					return json;
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
				requestModifyInfo(serializeResult);
			}
		}).execute();

	}

	void requestModifyInfo(String serializeResult) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();// 修改成功，返回“我”
				}
				Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_member_modify)).setRequestMethod(RequestMethod.ePut).SetJSON(serializeResult)
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
