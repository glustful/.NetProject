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
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
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
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.Staff;
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
	boolean editable = false;
	LinearLayout ll_balance;
	int limit = 5;
	int offset = 0;
	int[] checkedIds;

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
		ll_balance = (LinearLayout) rootView.findViewById(R.id.ll_balance);

		lv.setMode(Mode.BOTH);
		adapter = new MyListViewAdapter();
		lv.setAdapter(adapter);

		btn_balance.setOnClickListener(this);
		cb_selectall.setOnClickListener(this);
		btn_edit.setOnClickListener(this);
		lv.setOnRefreshListener(new HowWillIrefresh());

	}

	@Override
	public void onResume() {
		super.onResume();
		staffList.clear();
		offset = 0;
		requestData(offset);
		requestCount();
	}

	void requestCount() {
		new Thread() {
			@Override
			public void run() {
				DBDao dao = new DBDao(getActivity());
				int count = dao.getAllCounts();
				tv_title_count.setText("购物车(" + count + ")");
			}
		}.start();
	}

	private void requestData(final int offset) {
		if (offset == 0)
			ll_loading.setVisibility(View.VISIBLE);
		new Thread() {

			public void run() {
				DBDao dao = new DBDao(getActivity());
				List<Staff> list = dao.findPart(offset, limit);
				if (list.size() == 0) {
					getActivity().runOnUiThread(new Runnable() {

						@Override
						public void run() {
							if (offset != 0) {
								Toast.makeText(getActivity(), "已经没有更多数据啦", Toast.LENGTH_SHORT).show();
								lv.onRefreshComplete();
							}
							ll_loading.setVisibility(View.GONE);
						}
					});
				}

				else {
					for (Staff staff : list) {
						staffList.add(staff);
					}
					getActivity().runOnUiThread(new Runnable() {

						@Override
						public void run() {
							fillData();
							lv.onRefreshComplete();
						}
					});
				}
			};
		}.start();

	}

	void fillData() {
		Log.i(TAG, "fillData()");
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
					offset += 5;
					requestData(offset);

				}
			}, 1000);
		}
	}

	static class ViewHolder {

		TextView tv_delete;
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
				holder.tv_delete = (TextView) convertView.findViewById(R.id.tv_delete);
				convertView.setTag(holder);
			}
			holder.tv_delete.setVisibility(editable ? View.VISIBLE : View.GONE);

			holder.cb.setVisibility(!editable ? View.VISIBLE : View.GONE);
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
			holder.iv.setScaleType(ScaleType.FIT_XY);
			Utils.spanTextSize(holder.tv_price_counted, "\\.", true, new int[] { 16, 12 });

			holder.cb.setChecked(staff.chosen);
			holder.cb.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					staff.chosen = !staff.chosen;
					if (!staff.chosen)
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

			holder.tv_delete.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					staffList.remove(staff);
					fillData();
					new Thread() {
						public void run() {
							DBDao dao = new DBDao(getActivity());
							dao.delete(staff.id);
						};
					}.start();
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
		btn_balance.setTag(sumCount);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.btn_balance:
				int count = (Integer) btn_balance.getTag();
				if (count == 0) {
					Toast.makeText(getActivity(), "亲，你还没有选择任何商品呢!", Toast.LENGTH_SHORT).show();
				} else {
					List<Staff> chosenList = new ArrayList<Staff>();
					for (Staff staff : staffList) {
						if (staff.chosen)
							chosenList.add(staff);
					}
					BalanceActivity_.intent(getActivity()).staffList(chosenList).start();
				}
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
				editable = !editable;
				ll_balance.setVisibility(editable ? View.GONE : View.VISIBLE);
				fillData();
				break;
		}
	}

}
