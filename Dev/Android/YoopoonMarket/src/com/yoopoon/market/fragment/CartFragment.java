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

import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.TextUtils;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.market.BalanceActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.utils.Utils;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class CartFragment extends Fragment implements OnClickListener {
	View rootView;
	PullToRefreshListView lv;
	TextView tv_price_total;
	Button btn_balance;
	ListView refreshView;
	MyListViewAdapter adpter;

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

		lv.setMode(Mode.BOTH);
		refreshView = lv.getRefreshableView();
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		adpter = new MyListViewAdapter();
		refreshView.setAdapter(adpter);

		lv.setAdapter(new MyListViewAdapter());
		Utils.spanTextStyle(tv_price_total, getActivity());
		btn_balance.setOnClickListener(this);
		lv.setOnRefreshListener(new HowWillIrefresh());
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

			return 10;
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
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				holder.btn_countdown = (TextView) convertView.findViewById(R.id.btn_countdown);
				holder.btn_countup = (TextView) convertView.findViewById(R.id.btn_countup);
				holder.et_count = (EditText) convertView.findViewById(R.id.et_count);
				holder.btn_countdown.setTag(holder.et_count);
				holder.btn_countup.setTag(holder.et_count);
				convertView.setTag(holder);
			}
			holder.tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			Utils.spanTextSize(holder.tv_price_counted, "\\.", true, new int[] { 16, 12 });
			holder.btn_countdown.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					EditText et = (EditText) v.getTag();
					String text = et.getText().toString();

					if (!TextUtils.isEmpty(text)) {
						int count = Integer.parseInt(text);
						if (count > 0)
							et.setText(String.valueOf(--count));
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
					}

				}
			});
			return convertView;
		}
	}

	@Override
	public void onClick(View v) {
		BalanceActivity_.intent(getActivity()).start();
	}

}
