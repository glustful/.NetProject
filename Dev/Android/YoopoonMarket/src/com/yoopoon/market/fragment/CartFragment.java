/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ShopFragment.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-7 下午4:50:59 
 * @version: V1.0   
 */
package com.yoopoon.market.fragment;

import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.Editable;
import android.text.TextUtils;
import android.text.TextWatcher;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.BalanceActivity_;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.Staff;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.Utils;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class CartFragment extends Fragment implements OnClickListener {
	private static final String TAG = "CartFragment";
	View rootView;
	PullToRefreshListView lv;
	TextView tv_price_total;
	Button btn_balance;
	ListView refreshView;
	MyListViewAdapter adapter;
	List<Staff> staffList = new ArrayList<Staff>();
	View ll_loading;
	TextView tv_title_count;
	CheckBox cb_selectall;
	Button btn_edit;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_cart, null, false);
		init();
		return rootView;
	}

	private void init() {
		lv = (PullToRefreshListView) rootView.findViewById(R.id.lv);
		tv_price_total = (TextView) rootView.findViewById(R.id.tv_price_total);
		btn_balance = (Button) rootView.findViewById(R.id.btn_balance);
		ll_loading = rootView.findViewById(R.id.ll_loading);
		tv_title_count = (TextView) rootView.findViewById(R.id.tv_title_count);
		cb_selectall = (CheckBox) rootView.findViewById(R.id.cb_chooseall);
		btn_edit = (Button) rootView.findViewById(R.id.btn_edit);

		lv.setMode(Mode.BOTH);
		adapter = new MyListViewAdapter();
		lv.setAdapter(adapter);

		btn_balance.setOnClickListener(this);
		cb_selectall.setOnClickListener(this);
		btn_edit.setOnClickListener(this);
		lv.setOnRefreshListener(new HowWillIrefresh());

		if (staffList.size() == 0)
			requestData();
		else
			fillData();

	}

	private void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						try {
							JSONArray array = object.getJSONArray("Object");
							for (int i = 0; i < array.length(); i++) {
								JSONObject staffObject = array.getJSONObject(i);
								int price = staffObject.optInt("Id", 0);
								String title = staffObject.optString("Title", "");
								String img = getString(R.string.url_image) + staffObject.optString("TitleImg", "");
								String category = staffObject.optString("AdSubTitle", "");
								staffList.add(new Staff(title, category, img, i, price + 0.9, price + 20.9));
							}
							fillData();
						} catch (JSONException e) {
							e.printStackTrace();
						}

					} else {
						Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
						ll_loading.setVisibility(View.GONE);
					}
				} else {
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
					ll_loading.setVisibility(View.GONE);
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_test)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	void fillData() {
		adapter.notifyDataSetChanged();
		calcTotalPrice();
		ll_loading.setVisibility(View.GONE);
	}

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {

		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(getActivity(), System.currentTimeMillis(),
					DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
			new Handler().postDelayed(new Runnable() {

				@Override
				public void run() {
					lv.onRefreshComplete();
				}
			}, 1000);
		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			new Handler().postDelayed(new Runnable() {

				@Override
				public void run() {
					lv.onRefreshComplete();
				}
			}, 1000);
		}
	}

	static class ViewHolder {

		CheckBox cb;
		ImageView iv;
		TextView tv_name;
		TextView tv_category;
		TextView tv_price_previous;
		TextView tv_price_counted;
		TextView btn_countdown;
		TextView btn_countup;
		EditText et_count;
	}

	ViewHolder holder = null;

	private class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {

			return staffList.size();
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
			if (convertView == null) {
				convertView = View.inflate(getActivity(), R.layout.item_cart, null);
			}
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				holder.btn_countdown = (TextView) convertView.findViewById(R.id.btn_countdown);
				holder.btn_countup = (TextView) convertView.findViewById(R.id.btn_countup);
				holder.et_count = (EditText) convertView.findViewById(R.id.et_count);
				holder.btn_countdown.setTag(holder.et_count);
				holder.btn_countup.setTag(holder.et_count);
				holder.cb = (CheckBox) convertView.findViewById(R.id.cb);
				convertView.setTag(holder);
			}
			holder.tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			final Staff staff = staffList.get(position);
			holder.tv_price_counted.setText("￥" + staff.price_counted);
			holder.tv_name.setText(staff.title);
			holder.tv_price_previous.setText("￥" + staff.price_previous);
			holder.tv_category.setText(staff.category);
			holder.et_count.setText(staff.count + "");
			holder.iv.setTag(staff.image);
			ImageLoader.getInstance().displayImage(staff.image, holder.iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			holder.iv.setScaleType(ScaleType.CENTER_CROP);
			Utils.spanTextSize(holder.tv_price_counted, "\\.", true, new int[] { 16, 12 });

			holder.cb.setChecked(staff.chosen ? true : false);
			holder.cb.setOnCheckedChangeListener(new OnCheckedChangeListener() {

				@Override
				public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
					staff.chosen = isChecked;
					if (!isChecked)
						cb_selectall.setChecked(false);
					calcTotalPrice();
				}
			});

			holder.btn_countdown.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					EditText et = (EditText) v.getTag();
					String text = et.getText().toString();

					if (!TextUtils.isEmpty(text)) {
						int count = Integer.parseInt(text);
						if (count > 0) {
							et.setText(String.valueOf(--count));
							staff.count--;
							calcTotalPrice();
						}
					}

				}
			});

			holder.btn_countup.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					EditText et = (EditText) v.getTag();
					String text = et.getText().toString();
					if (!TextUtils.isEmpty(text)) {
						int count = Integer.parseInt(text);
						et.setText(String.valueOf(++count));

						staff.count++;
						calcTotalPrice();
					}

				}
			});

			holder.et_count.addTextChangedListener(new TextWatcher() {

				@Override
				public void onTextChanged(CharSequence s, int start, int before, int count) {
					if (!TextUtils.isEmpty(s.toString())) {
						staff.count = Integer.parseInt(s.toString());
						calcTotalPrice();
					}
				}

				@Override
				public void beforeTextChanged(CharSequence s, int start, int count, int after) {
					// TODO Auto-generated method stub

				}

				@Override
				public void afterTextChanged(Editable s) {
					// TODO Auto-generated method stub

				}
			});
			return convertView;
		}
	}

	void calcTotalPrice() {
		double sumPrice = 0;
		int sumCount = 0;
		for (Staff staff : staffList) {
			if (staff.chosen) {
				sumPrice += staff.price_counted * staff.count;
				sumCount += staff.count;
			}
		}
		DecimalFormat df = new DecimalFormat("#.00");
		tv_price_total.setText("合计：￥" + df.format(sumPrice));
		Utils.spanTextStyle(tv_price_total, getActivity());
		btn_balance.setText("结算(" + sumCount + ")");
		tv_title_count.setText("购物车(" + sumCount + ")");
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.btn_balance:
				BalanceActivity_.intent(getActivity()).start();
				break;
			case R.id.cb_chooseall:
				for (Staff staff : staffList)
					staff.chosen = true;
				fillData();
				break;
			case R.id.btn_edit:
				boolean edit = Boolean.parseBoolean((String) v.getTag());
				btn_edit.setTextColor(!edit ? Color.BLACK : getResources().getColor(R.color.text_gray));
				btn_edit.setTag(!edit ? "true" : "false");
				break;
		}
	}

}
