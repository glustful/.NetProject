package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
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
import android.widget.CheckBox;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

@EActivity(R.layout.activity_choose_address)
public class ChooseAddressActivity extends MainActionBarActivity {
	private static final String TAG = "ChooseAddressActivity";
	@ViewById(R.id.lv)
	ListView lv;
	@ViewById(R.id.loading)
	View loading;
	@Extra
	List<Staff> staffList;

	@Click(R.id.btn_manage)
	void manageAddress() {
		AddressManageActivity_.intent(ChooseAddressActivity.this).start();
	}
	List<MemberAddressEntity> addressList = new ArrayList<MemberAddressEntity>();
	MyListViewAdapter adapter;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("选择收货地址");
	}

	@Override
	protected void onResume() {
		super.onResume();
		requestAddress();
	}

	void requestAddress() {
		loading.setVisibility(View.VISIBLE);
		String userid = User.getUserId(ChooseAddressActivity.this);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					parseToEntityList(array);
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(ChooseAddressActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_address_byid)).setRequestMethod(RequestMethod.eGet)
				.addParam("userid", userid).notifyRequest();
	}

	void parseToEntityList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
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
		loading.setVisibility(View.GONE);
		if (adapter == null) {
			adapter = new MyListViewAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	static class ViewHolder {

		TextView tv_name;
		TextView tv_phone;
		TextView tv_address;
		CheckBox cb;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return addressList.size();
		}

		@Override
		public Object getItem(int position) {
			return null;
		}

		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}

		@Override
		public View getView(final int position, View convertView, ViewGroup parent) {
			if (convertView == null)
				convertView = View.inflate(ChooseAddressActivity.this, R.layout.item_choose_addres, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_address = (TextView) convertView.findViewById(R.id.tv_address);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_phone = (TextView) convertView.findViewById(R.id.tv_phone);
				holder.cb = (CheckBox) convertView.findViewById(R.id.cb);
				convertView.setTag(holder);
			}
			final MemberAddressEntity entity = addressList.get(position);
			holder.tv_address.setText(entity.Address);
			holder.tv_name.setText(entity.Linkman);
			holder.tv_phone.setText(entity.Tel);

			holder.cb.setChecked(position == selectedPosition);

			if (position == 0) {
				holder.tv_address.setText("[默认]" + entity.Address);
				changeTextColor(holder.tv_address);
			}

			final CheckBox cb = holder.cb;
			convertView.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					cb.setChecked(!cb.isChecked());
					if (cb.isChecked()) {
						selectedPosition = position;
						adapter.notifyDataSetChanged();
					}

				}
			});
			return convertView;
		}

	}

	int selectedPosition = 0;

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

	@Override
	public void backButtonClick(View v) {
		// 选择地址的前面是确认订单页面，需要传入选择的地址，不能只是简单地finish()掉
		BalanceActivity_.intent(ChooseAddressActivity.this).checkedAddress(addressList.get(selectedPosition))
				.staffList(staffList).start();
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
