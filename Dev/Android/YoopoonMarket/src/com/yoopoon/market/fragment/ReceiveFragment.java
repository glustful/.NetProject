package com.yoopoon.market.fragment;

import java.util.List;
import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.yoopoon.market.MeOrderActivity;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.CommunityOrderEntity;

public class ReceiveFragment extends Fragment {
	PullToRefreshListView lv;
	View rootView;
	List<CommunityOrderEntity> orders;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_receive, null);
		init();
		return rootView;
	}

	void init() {
		MeOrderActivity meOrderActivity = (MeOrderActivity) getActivity();
		orders = meOrderActivity.getOrderList(2);
		TextView tv = (TextView) rootView.findViewById(R.id.tv_empty);
		tv.setVisibility(orders.size() > 0 ? View.GONE : View.VISIBLE);
		lv = (PullToRefreshListView) rootView.findViewById(R.id.lv);
		lv.setAdapter(new MyListViewAdapter());

		lv.setMode(Mode.PULL_FROM_END);
		lv.setOnRefreshListener(new HowWillIrefresh());
	}

	static class ViewHolder {
		TextView tv_order_num;
		ImageView iv;
		TextView tv_name;
		TextView tv_category;
		TextView tv_count;
		TextView tv_price_counted;
		TextView tv_price_previous;
		TextView tv_btn;
		TextView tv_desc;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return orders.size();
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
				convertView = View.inflate(getActivity(), R.layout.item_order_common, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_btn = (TextView) convertView.findViewById(R.id.tv_btn);
				holder.tv_order_num = (TextView) convertView.findViewById(R.id.tv_order_num);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_count = (TextView) convertView.findViewById(R.id.tv_count);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				holder.tv_desc = (TextView) convertView.findViewById(R.id.tv_desc);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				convertView.setTag(holder);
			}
			CommunityOrderEntity order = orders.get(position);
			holder.tv_order_num.setText("订单号：" + order.No);
			// holder.tv_name.setText(order.Details.)
			holder.tv_price_counted.setText("￥" + order.Actualprice);
			holder.tv_price_previous.setText("￥" + order.Totalprice);
			holder.tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			holder.tv_btn.setText("确认收货");
			holder.tv_desc.setText("商品已送达");
			return convertView;
		}

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
}
