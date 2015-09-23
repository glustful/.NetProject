package com.yoopoon.market.fragment;

import java.util.List;
import org.json.JSONObject;
import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MeOrderActivity;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.CommunityOrderEntity;
import com.yoopoon.market.domain.OrderDetailEntity;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;

public class ReceiveFragment extends Fragment {
	private static final String TAG = "ReceiveFragment";
	PullToRefreshListView lv;
	View rootView;
	List<CommunityOrderEntity> orders;
	MyListViewAdapter adapter;
	View loading;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_receive, null);
		init();
		return rootView;
	}

	void init() {
		loading = rootView.findViewById(R.id.ll_loading);
		MeOrderActivity meOrderActivity = (MeOrderActivity) getActivity();
		orders = meOrderActivity.getOrderList(2);
		TextView tv = (TextView) rootView.findViewById(R.id.tv_empty);

		tv.setVisibility(orders.size() > 0 ? View.GONE : View.VISIBLE);
		lv = (PullToRefreshListView) rootView.findViewById(R.id.lv);
		adapter = new MyListViewAdapter();
		lv.setAdapter(adapter);

		lv.setMode(Mode.PULL_FROM_END);
		lv.setOnRefreshListener(new HowWillIrefresh());
	}

	public void update() {
		MeOrderActivity meOrderActivity = (MeOrderActivity) getActivity();
		orders = meOrderActivity.getOrderList(2);
		adapter.notifyDataSetChanged();
	}

	static class ViewHolder {

		TextView tv_desc;
		TextView tv_order_num;
		TextView tv_btn;
		LinearLayout ll_products;
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
				holder.ll_products = (LinearLayout) convertView.findViewById(R.id.ll_products);
				holder.tv_desc = (TextView) convertView.findViewById(R.id.tv_desc);
				convertView.setTag(holder);
			}
			final CommunityOrderEntity order = orders.get(position);
			holder.tv_btn.setText("确认收货");
			holder.tv_desc.setText("商品已送达");
			holder.tv_order_num.setText("订单号：" + order.No);

			List<OrderDetailEntity> details = order.Details;
			Log.i(TAG, "details = " + details.size());
			for (OrderDetailEntity detail : details) {
				ProductEntity product = detail.Product;
				View productView = View.inflate(getActivity(), R.layout.item_product, null);
				TextView tv_price_counted = (TextView) productView.findViewById(R.id.tv_price_counted);
				TextView tv_price_previous = (TextView) productView.findViewById(R.id.tv_price_previous);
				TextView tv_category = (TextView) productView.findViewById(R.id.tv_category);
				TextView tv_count = (TextView) productView.findViewById(R.id.tv_count);
				TextView tv_name = (TextView) productView.findViewById(R.id.tv_name);
				ImageView iv = (ImageView) productView.findViewById(R.id.iv);

				String imageUrl = getActivity().getString(R.string.url_image) + product.MainImg;
				iv.setTag(imageUrl);
				ImageLoader.getInstance().displayImage(imageUrl, iv, MyApplication.getOptions(),
						MyApplication.getLoadingListener());
				tv_price_counted.setText("￥" + product.Price);
				tv_price_previous.setText("￥" + product.OldPrice);
				tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
				tv_category.setText(product.Subtitte);
				tv_name.setText(product.Name);
				tv_count.setText("x" + detail.Count);

				holder.ll_products.addView(productView);

			}

			holder.tv_btn.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					order.Status = 3;
					serilizerJson(order);
				}
			});
			return convertView;
		}

	}

	void serilizerJson(final CommunityOrderEntity order) {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				String result = null;
				try {
					result = om.writeValueAsString(order);
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return result;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (serializeResult != null) {
					Log.i(TAG, serializeResult);
					requestModifyStatus(serializeResult);
				}
			}
		}).execute();
	}

	void requestModifyStatus(String json) {

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						MeOrderActivity meOrderActivity = (MeOrderActivity) getActivity();
						meOrderActivity.requestOrder(User.getUserId(getActivity()));
					}
				}
				Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_order_put)).setRequestMethod(RequestMethod.ePut).SetJSON(json).notifyRequest();
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
