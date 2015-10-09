package com.yoopoon.market.fragment;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONObject;
import android.graphics.Color;
import android.graphics.Paint;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.format.DateUtils;
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
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.LoginActivity_;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.PayResultActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.domain.ServiceOrderDetail;
import com.yoopoon.market.domain.ServiceOrderEntity;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

public class ServiceOrder2 extends Fragment {
	private static final String TAG = "ServiceOrder2";
	View rootView;
	PullToRefreshListView lv;
	TextView tv_empty;
	List<ServiceOrderEntity> services = new ArrayList<ServiceOrderEntity>();
	MyListViewAdapter adapter = new MyListViewAdapter();

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_serviceoreder, null);
		init();
		return rootView;
	}

	void init() {
		tv_empty = (TextView) rootView.findViewById(R.id.tv_empty);
		tv_empty.setVisibility(services.size() > 0 ? View.GONE : View.VISIBLE);
		lv = (PullToRefreshListView) rootView.findViewById(R.id.lv);
		lv.setAdapter(adapter);

		lv.setMode(Mode.PULL_FROM_END);
		lv.setOnRefreshListener(new HowWillIrefresh());

	}
	int page = 1;
	int pageCount = 5;

	@Override
	public void onResume() {
		super.onResume();
		page = 1;
		requestData();
	}

	void requestData() {
		if (!User.isLogin(getActivity())) {
			LoginActivity_.intent(getActivity()).start();
			return;
		}
		String userId = User.getUserId(getActivity());
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					lv.onRefreshComplete();
					JSONArray array = object.optJSONArray("List");
					if (array != null) {
						if (array.length() == 0 && page > 1)
							Toast.makeText(getActivity(), "已经没有更多数据啦", Toast.LENGTH_SHORT).show();
						if (array.length() > 0)
							parseToList(array);
					}
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_serviceorder_get)).setRequestMethod(RequestMethod.eGet)
				.addParam("userid", userId).addParam("status", "2").addParam("page", String.valueOf(page))
				.addParam("pagecount", String.valueOf(pageCount)).notifyRequest();
	}

	void parseToList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				if (page == 1)
					services.clear();
				for (int i = 0; i < array.length(); i++) {
					JSONObject object = array.optJSONObject(i);
					try {
						ServiceOrderEntity service = om.readValue(object.toString(), ServiceOrderEntity.class);
						services.add(service);
					} catch (JsonParseException e) {
						e.printStackTrace();
					} catch (JsonMappingException e) {
						e.printStackTrace();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
				return services;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					fillData();
				}

			}
		}).execute();
	}

	void fillData() {
		tv_empty.setVisibility(services.size() > 0 ? View.GONE : View.VISIBLE);
		adapter.notifyDataSetChanged();
	}

	static class ViewHolder {

		TextView tv_desc;
		TextView tv_order_num;
		TextView tv_btn;
		LinearLayout ll_products;
		TextView tv_price;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return services.size();
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
				holder.tv_desc = (TextView) convertView.findViewById(R.id.tv_desc);
				holder.tv_btn = (TextView) convertView.findViewById(R.id.tv_btn);
				holder.tv_order_num = (TextView) convertView.findViewById(R.id.tv_order_num);
				holder.ll_products = (LinearLayout) convertView.findViewById(R.id.ll_products);
				holder.tv_price = (TextView) convertView.findViewById(R.id.tv_price);
				convertView.setTag(holder);
			}
			ServiceOrderEntity service = services.get(position);
			holder.tv_desc.setText("商家已接件");
			holder.tv_btn.setText("办理中");
			holder.tv_btn.setTextColor(Color.GRAY);
			holder.tv_price.setVisibility(View.GONE);
			holder.tv_btn.setBackgroundResource(R.drawable.white_bg);

			holder.tv_order_num.setText("订单号：" + service.OrderNo);
			holder.tv_price.setText("￥" + service.Flee);

			List<ServiceOrderDetail> details = service.Details;
			// Log.i(TAG, order.to);
			if (holder.ll_products.getChildCount() > 0)
				holder.ll_products.removeAllViews();
			if (details != null) {
				for (ServiceOrderDetail detail : details) {
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
					tv_price_counted.setText("￥" + detail.Price);
					tv_price_previous.setText("￥" + product.OldPrice);
					tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
					tv_category.setText(product.Subtitte);
					tv_name.setText(product.Name);
					tv_count.setText("x" + detail.Count);

					holder.ll_products.addView(productView);
					View v = new View(getActivity());
					v.setBackgroundResource(R.drawable.line);
					holder.ll_products.addView(v);
				}

				// holder.ll_products.removeViewAt(holder.ll_products.getChildCount() - 1);
			}

			holder.tv_btn.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					PayResultActivity_.intent(getActivity()).start();
				}
			});
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
			page++;
			requestData();
		}
	}

}
