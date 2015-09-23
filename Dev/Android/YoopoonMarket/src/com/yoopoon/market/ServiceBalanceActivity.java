package com.yoopoon.market;

import java.io.IOException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView.LayoutParams;
import android.widget.BaseAdapter;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.OrderDetailEntity;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.Utils;

@EActivity(R.layout.activity_service_balance)
public class ServiceBalanceActivity extends MainActionBarActivity {
	private static final String TAG = "BalanceActivity";
	@ViewById(R.id.lv)
	ListView lv;
	@Extra
	ProductEntity product;
	@Extra
	MemberAddressEntity checkedAddress;
	@ViewById(R.id.tv_name)
	TextView tv_linkman;
	@ViewById(R.id.tv_phone)
	TextView tv_phone;
	@ViewById(R.id.tv_address)
	TextView tv_address;
	@ViewById(R.id.ll_loading)
	View loading;
	@ViewById(R.id.tv_price_total)
	TextView tv_price_total;
	@ViewById(R.id.tv_time)
	TextView tv_time;

	@Click(R.id.tv_time)
	void pickTime() {
		DialogFragment newFragment = new DatePickerFragment();
		newFragment.show(getFragmentManager(), "datePicker");
	}

	EditText et_remark;
	MemberAddressEntity addressEntity;
	List<OrderDetailEntity> details = new ArrayList<OrderDetailEntity>();

	@Click(R.id.btn_ok)
	void confirmOrder() {

		if (User.isLogin(this)) {
			requestAddOrder();
		} else {
			LoginActivity_.intent(this).start();
		}
	}

	void requestAddOrder() {
		if (tv_time.getTag() == null) {
			Toast.makeText(ServiceBalanceActivity.this, "请选择服务时间", Toast.LENGTH_LONG).show();
			return;
		}
		Log.i(TAG, product.toString());
		String time = tv_time.getText().toString();
		String remark = et_remark.getText().toString();
		String json = "{\"MemberAddressId\":" + addressEntity.Id + ",\"Servicetime\":\"" + time + "\",\"Remark\":\""
				+ remark + "\",\"Details\":[{\"Count\":1,\"Price\":" + product.Price + ",\"Product\":{\"Id\":"
				+ product.Id + "}}]}";
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONObject orderObject = object.optJSONObject("Object");
						if (orderObject != null) {
							float price = Float.parseFloat(orderObject.optString("Flee", ""));
							String No = orderObject.optString("OrderNo", "");
							String msg = object.optString("Msg", "");

							Bundle bundle = new Bundle();
							bundle.putFloat("Price", price);
							bundle.putString("No", No);
							bundle.putString("Msg", msg);

							PayDemoActivity_.intent(ServiceBalanceActivity.this).orderBundle(bundle).start();
						}
					}
				} else {
					Toast.makeText(ServiceBalanceActivity.this, "下单失败，请重试", Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_serviceorder_post)).setRequestMethod(RequestMethod.ePost).SetJSON(json)
				.notifyRequest();
	}

	class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {

		final Calendar c = Calendar.getInstance();
		int year = c.get(Calendar.YEAR);
		int month = c.get(Calendar.MONTH);
		int day = c.get(Calendar.DAY_OF_MONTH);
		int hour = c.get(Calendar.HOUR_OF_DAY);

		@Override
		public Dialog onCreateDialog(Bundle savedInstanceState) {
			DatePickerDialog datePickerDialog = new DatePickerDialog(getActivity(), this, year, month, day);
			DatePicker datePicker = datePickerDialog.getDatePicker();
			datePicker.setMinDate(c.getTimeInMillis());
			return datePickerDialog;
		}

		public void onDateSet(DatePicker view, int year, int month, int day) {
			int dataMonth = month + 1;
			tv_time.setText(year + "-" + dataMonth + "-" + day + "");
			tv_time.setTag(true);

		}
	}

	@Click(R.id.rl_address)
	void chooseAddress() {
		ChooseAddressActivity_.intent(ServiceBalanceActivity.this).product(product).start();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("确认订单");

		calcPrice();
		lv.setAdapter(new MyListViewAdapter());

	}

	@Override
	protected void onResume() {
		super.onResume();

		if (checkedAddress != null) {
			addressEntity = checkedAddress;
			setAddress();
		} else {
			requestAddress();
		}
	}

	void calcPrice() {
		DecimalFormat df = new DecimalFormat("#.00");
		tv_price_total.setText("合计：￥" + df.format(product.Price));
		Utils.spanTextStyle(tv_price_total, ServiceBalanceActivity.this);
	}

	void requestAddress() {
		if (!User.isLogin(this)) {
			LoginActivity_.intent(this).start();
			return;
		}
		String userid = User.getUserId(ServiceBalanceActivity.this);
		loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					try {
						if (array != null) {
							JSONObject addressObject = array.getJSONObject(0);
							parseToEntity(addressObject);
						}
					} catch (JSONException e) {
						e.printStackTrace();
					}
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(ServiceBalanceActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_address_byid)).setRequestMethod(RequestMethod.eGet)
				.addParam("userid", userid).notifyRequest();
	}

	void parseToEntity(final JSONObject object) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity = om.readValue(object.toString(), MemberAddressEntity.class);
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
					setAddress();
				}
			}
		}).execute();
	}

	void setAddress() {
		loading.setVisibility(View.GONE);
		tv_linkman.setText("收货人：" + addressEntity.Linkman);
		tv_phone.setText(addressEntity.Tel);
		tv_address.setText(addressEntity.Address);
	}

	static class ViewHolder {
		ImageView iv;
		TextView tv_name;
		TextView tv_category;
		TextView tv_price_counted;
		TextView tv_price_previous;
		TextView tv_count;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return 2;
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
			if (position == 1) {
				View v = View.inflate(ServiceBalanceActivity.this, R.layout.item_remark, null);
				int px = Utils.dp2px(ServiceBalanceActivity.this, 200);
				LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, px);
				v.setLayoutParams(params);
				et_remark = (EditText) v.findViewById(R.id.et_remark);
				return v;
			}
			if (convertView == null || !(convertView instanceof RelativeLayout))
				convertView = View.inflate(ServiceBalanceActivity.this, R.layout.item_cart2, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				holder.tv_count = (TextView) convertView.findViewById(R.id.tv_count);
				convertView.setTag(holder);
			}
			String imageUrl = getString(R.string.url_image) + product.MainImg;
			holder.iv.setTag(imageUrl);
			ImageLoader.getInstance().displayImage(imageUrl, holder.iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			holder.tv_name.setText(product.Subtitte);
			holder.tv_category.setText(product.Name);
			holder.tv_price_counted.setText("￥" + product.Price + "");
			holder.tv_price_previous.setText(product.OldPrice + "");
			holder.tv_count.setText("x" + 1);

			return convertView;
		}
	}

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
