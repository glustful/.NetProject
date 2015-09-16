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
import android.graphics.Color;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
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
	@ViewById(R.id.lv)
	ListView lv;
	@ViewById(R.id.ll_loading)
	View ll_loading;
	Member member = null;
	MyListViewAdapter adapter;
	String[] contents;
	String[] settings;

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

	static class ViewHolder {
		TextView tv_setting;
		TextView tv_content;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return settings.length;
		}

		@Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return null;
		}

		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			if (convertView == null)
				convertView = View.inflate(PersonalInfoActivity.this, R.layout.item_personnal_info, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_setting = (TextView) convertView.findViewById(R.id.tv_setting);
				holder.tv_content = (TextView) convertView.findViewById(R.id.tv_content);
				convertView.setTag(holder);
			}
			holder.tv_setting.setText(settings[position]);
			holder.tv_content.setText(contents[position]);

			return convertView;
		}

	}

	public void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
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
					settings = new String[] { "真实姓名", "身份证号", "性别", "联系电话", "Icq", "邮政编码", "ThumbNail", "账户余额", "积分",
							"级别" };
					contents = new String[settings.length];
					contents[0] = member.RealName;
					contents[1] = member.IdentityNo;
					contents[2] = member.GenderString;
					contents[3] = member.Phone;
					contents[4] = member.Icq;
					contents[5] = member.PostNo;
					contents[6] = member.Thumbnail;
					contents[7] = member.AccountNumber + "";
					contents[8] = member.Points + "";
					contents[9] = member.Level + "";
					fillData();
				}
			}
		}).execute();
	}

	void fillData() {
		ll_loading.setVisibility(View.GONE);
		if (adapter == null) {
			adapter = new MyListViewAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}

	}

	void modify() {
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

	void add() {
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

	void delete() {
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
