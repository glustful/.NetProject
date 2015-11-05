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
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.style.ForegroundColorSpan;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

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
	@ViewById(R.id.lv)
	PullToRefreshListView lv;
	@ViewById(R.id.ll_loading)
	View ll_loading;

	@Click(R.id.btn_add)
	void newAddress() {
		NewAddressActivity_.intent(AddressManageActivity.this).start();
	}
	List<MemberAddressEntity> addressList = new ArrayList<MemberAddressEntity>();
	MyListViewAdapter adapter;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("地址管理");
		titleButton.setTextColor(Color.WHITE);
		backWhiteButton.setVisibility(View.VISIBLE);
		headView.setBackgroundColor(Color.RED);
	}

	static class ViewHolder {
		TextView tv_name;
		TextView tv_phone;
		TextView tv_address;
		TextView tv_modify;
		TextView tv_delete;
	}

	class MyListViewAdapter extends BaseAdapter {
		@Override
		public int getCount() {
			return addressList.size();
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
			final MemberAddressEntity entity = addressList.get(position);
			if (convertView == null || !(convertView instanceof LinearLayout))
				convertView = View.inflate(AddressManageActivity.this, R.layout.item_address, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_address = (TextView) convertView.findViewById(R.id.tv_address);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_phone = (TextView) convertView.findViewById(R.id.tv_phone);
				holder.tv_delete = (TextView) convertView.findViewById(R.id.tv_delete);
				holder.tv_modify = (TextView) convertView.findViewById(R.id.tv_modify);
				convertView.setTag(holder);
			}
			holder.tv_address.setText(entity.Address);
			holder.tv_name.setText(entity.Linkman);
			holder.tv_phone.setText(entity.Tel);
			if (entity.IsDefault != null && entity.IsDefault.equals("true")) {
				holder.tv_address.setText("[默认]" + entity.Address);
				changeTextColor(holder.tv_address);
			}
			holder.tv_delete.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					delete(entity);
				}
			});
			holder.tv_modify.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					AddressModifyActivity_.intent(AddressManageActivity.this).addressEntity(entity).start();
				}
			});
			return convertView;
		}
	}

	void changeTextColor(TextView textView) {
		String text = textView.getText().toString();
		SpannableStringBuilder builder = new SpannableStringBuilder(text);
		// ForegroundColorSpan 为文字前景色，BackgroundColorSpan为文字背景色
		ForegroundColorSpan redSpan = new ForegroundColorSpan(Color.RED);
		ForegroundColorSpan blackSpan = new ForegroundColorSpan(Color.BLACK);
		builder.setSpan(redSpan, 0, 4, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		builder.setSpan(blackSpan, 4, text.length(), Spannable.SPAN_INCLUSIVE_INCLUSIVE);
		textView.setText(builder);
	}

	void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		String userId = User.getUserId(AddressManageActivity.this);
		if (userId.equals("0"))
			LoginActivity_.intent(this).start();
		else {
			new RequestAdapter() {
				@Override
				public void onReponse(ResponseData data) {
					JSONObject object = data.getMRootData();
					if (object != null) {
						addressList.clear();
						JSONArray array = object.optJSONArray("List");
						parseToEntityList(array);
					} else {
						ll_loading.setVisibility(View.GONE);
						Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
					}
				}

				@Override
				public void onProgress(ProgressMessage msg) {
					// TODO Auto-generated method stub
				}
			}.setUrl(getString(R.string.url_get_address_byid)).setRequestMethod(RequestMethod.eGet)
					.addParam("userid", userId).notifyRequest();
		}
	}

	void parseToEntityList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				if (array != null) {
					for (int i = 0; i < array.length(); i++) {
						MemberAddressEntity addressEntity = null;
						try {
							JSONObject object = array.getJSONObject(i);
							addressEntity = om.readValue(object.toString(), MemberAddressEntity.class);
						} catch (JsonParseException e) {
							e.printStackTrace();
						} catch (JsonMappingException e) {
							e.printStackTrace();
						} catch (IOException e) {
							e.printStackTrace();
						} catch (JSONException e) {
							e.printStackTrace();
						}
						addressList.add(addressEntity);
					}
				}
				return addressList;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					Log.i(TAG, parseResult.toString());
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

	public void delete(final MemberAddressEntity entity) {
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status");
					if (status)
						requestData();
				}
				Toast.makeText(AddressManageActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_delete_address)).setRequestMethod(RequestMethod.eDelete)
				.addParam("id", entity.Id + "").notifyRequest();
	}

	@Override
	protected void onResume() {
		super.onResume();
		requestData();
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
