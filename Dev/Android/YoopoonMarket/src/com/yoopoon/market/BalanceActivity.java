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
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView.LayoutParams;
import android.widget.BaseAdapter;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.CommunityOrderEntity;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.OrderDetailEntity;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;
import com.yoopoon.market.utils.Utils;

/**
 * @ClassName: PersonalInfoActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-8 上午9:32:55
 */
@EActivity(R.layout.activity_balance)
public class BalanceActivity extends MainActionBarActivity {
	private static final String TAG = "BalanceActivity";
	@ViewById(R.id.lv)
	ListView lv;
	@Extra
	List<Staff> staffList;
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

	EditText et_remark;
	MemberAddressEntity addressEntity;
	List<OrderDetailEntity> details = new ArrayList<OrderDetailEntity>();

	@Click(R.id.btn_ok)
	void confirmOrder() {

		if (User.isLogin(this)) {

			for (Staff staff : staffList) {
				ProductEntity product = new ProductEntity();
				product.Name = staff.category;
				product.Subtitte = staff.title;
				product.Id = staff.productId;
				product.Price = staff.price_counted;
				OrderDetailEntity detail = new OrderDetailEntity();
				detail.Product = product;
				detail.Remark = et_remark.getText().toString();
				detail.Totalprice = product.Price * staff.count;
				detail.ProductName = product.Name;
				detail.Count = staff.count;
				detail.Price = product.Price;
				detail.UnitPrice = product.Price;
				details.add(detail);
			}
			CommunityOrderEntity orderEntity = new CommunityOrderEntity();
			orderEntity.Totalprice = total_price;
			orderEntity.MemberAddressId = addressEntity.Id;
			orderEntity.Details = details;
			orderEntity.Remark = et_remark.getText().toString();
			orderEntity.UserName = User.getUserName(BalanceActivity.this);
			orderEntity.Actualprice = orderEntity.Totalprice;
			serializeOrder(orderEntity);
		} else {
			LoginActivity_.intent(this).start();
		}
	}

	void serializeOrder(final CommunityOrderEntity orderEntity) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				String json = null;
				try {
					json = om.writeValueAsString(orderEntity);
				} catch (JsonProcessingException e) {
					e.printStackTrace();
				}
				return json;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (!TextUtils.isEmpty(serializeResult)) {
					Log.i(TAG, serializeResult);
					requestAddOrder(serializeResult);
				}
			}
		}).execute();
	}

	void requestAddOrder(String serial) {
		Log.i(TAG, serial);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONObject orderEntity = object.optJSONObject("Object");
						float price = Float.parseFloat(orderEntity.optString("Actualprice", ""));
						String No = orderEntity.optString("No", "");
						String msg = object.optString("Msg", "");

						Bundle bundle = new Bundle();
						bundle.putFloat("Price", price);
						bundle.putString("No", No);
						bundle.putString("Msg", msg);
						int[] staffId = new int[staffList.size()];
						for (int i = 0; i < staffList.size(); i++) {
							staffId[i] = staffList.get(i).id;
						}
						bundle.putIntArray("StaffIds", staffId);

						PayDemoActivity_.intent(BalanceActivity.this).orderBundle(bundle).start();
					}
				} else {
					Toast.makeText(BalanceActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_order_post)).setRequestMethod(RequestMethod.ePost).SetJSON(serial)
				.notifyRequest();
	}

	@Click(R.id.rl_address)
	void chooseAddress() {
		ChooseAddressActivity_.intent(BalanceActivity.this).staffList(staffList).start();
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

		Log.i(TAG, "onResume()");
		if (checkedAddress != null) {
			Log.i(TAG, checkedAddress.toString());
			addressEntity = checkedAddress;
			setAddress();
		} else {
			requestAddress();
		}
	}

	float total_price = 0;

	void calcPrice() {
		for (Staff staff : staffList) {
			total_price += staff.price_counted * staff.count;
		}
		DecimalFormat df = new DecimalFormat("#.00");
		tv_price_total.setText("合计：￥" + df.format(total_price));
		Utils.spanTextStyle(tv_price_total, BalanceActivity.this);
	}

	void requestAddress() {
		if (!User.isLogin(BalanceActivity.this)) {
			LoginActivity_.intent(this).start();
			return;
		}
		String userid = User.getUserId(BalanceActivity.this);
		loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				JSONObject object = data.getMRootData();
				if (object != null) {
					parseToEntity(object);

				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(BalanceActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_address_byid)).setRequestMethod(RequestMethod.eGet)
				.addParam("memberid", "").notifyRequest();
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
					Log.i(TAG, parseResult.toString());
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
			return staffList.size() + 1;
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
			if (position == staffList.size()) {
				View v = View.inflate(BalanceActivity.this, R.layout.item_remark, null);
				int px = Utils.dp2px(BalanceActivity.this, 200);
				LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, px);
				v.setLayoutParams(params);
				et_remark = (EditText) v.findViewById(R.id.et_remark);
				return v;
			}
			if (convertView == null || !(convertView instanceof RelativeLayout))
				convertView = View.inflate(BalanceActivity.this, R.layout.item_cart2, null);
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
			Staff staff = staffList.get(position);
			holder.iv.setTag(staff.image);
			ImageLoader.getInstance().displayImage(staff.image, holder.iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			holder.tv_name.setText(staff.title);
			holder.tv_category.setText(staff.category);
			holder.tv_price_counted.setText("￥" + staff.price_counted + "");
			holder.tv_price_previous.setText(staff.price_previous + "");
			holder.tv_count.setText("x" + staff.count);

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
