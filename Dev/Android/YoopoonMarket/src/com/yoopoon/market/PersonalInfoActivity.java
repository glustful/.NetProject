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
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.Member;
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
@EActivity(R.layout.activity_personal_info)
public class PersonalInfoActivity extends MainActionBarActivity {
	private static final String TAG = "PersonalInfoActivity";
	@ViewById(R.id.tv)
	TextView tv;
	Member member = null;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("个人资料");
		requestData();
	}

	public void requestData() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					// JSONArray array = object.optJSONArray("List");
					// try {
					// JSONObject memberJson = array.getJSONObject(0);
					// parseToMember(memberJson.toString());
					// } catch (JSONException e) {
					// // TODO Auto-generated catch block
					// e.printStackTrace();
					// }
					parseToMember(object.toString());

				} else {
					Toast.makeText(PersonalInfoActivity.this, data.toString(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_getmemeber_byid)).setRequestMethod(RequestMethod.eGet).addParam("id", "3")
				.notifyRequest();
	}

	void parseToMember(final String json) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				try {
					member = om.readValue(json, Member.class);
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
					tv.setText(parseResult.toString());
				}
			}
		}).execute();
	}

	public void modify(View v) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					member.RealName = "加班";
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
				Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_member_modify)).setRequestMethod(RequestMethod.ePut).SetJSON(serializeResult)
				.notifyRequest();
	}

	public void add(View v) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					member.RealName = "didadi";
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
				requestAdd(serializeResult);
			}
		}).execute();
	}

	void requestAdd(String serializeResult) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_member_add)).setRequestMethod(RequestMethod.ePost).SetJSON(serializeResult)
				.notifyRequest();
	}

	public void delete(View v) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_member_delete)).setRequestMethod(RequestMethod.eDelete).addParam("id", "6")
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
