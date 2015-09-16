package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.graphics.Color;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.MyGridView;

@EActivity(R.layout.activity_delivery)
public class DeliveryActivity extends MainActionBarActivity {

	@ViewById(R.id.gv)
	MyGridView gv;

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("快递代收");
		headView.setBackgroundColor(Color.RED);
		titleButton.setTextColor(Color.WHITE);
		initData();
	}

	// 数据初始化
	private void initData() {
		gv.setAdapter(new MyGridViewAdapter());
		gv.setOnItemClickListener(new MyGridViewItemClickListener());
	}

	// GridView的点击事件处理
	private class MyGridViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			ViewHolder holder = (ViewHolder) view.getTag();
			LinearLayout ll_bg = (LinearLayout) view.findViewById(R.id.ll_bg);
			if (holder.status)
				ll_bg.setBackgroundColor(Color.WHITE);
			else
				ll_bg.setBackgroundResource(R.drawable.border_with_yes);
			int padding = Utils.dp2px(DeliveryActivity.this, 20);
			ll_bg.setPadding(padding, padding, padding, padding);
			holder.status = !holder.status;
		}

	}

	static class ViewHolder {

		TextView tv_name;
		TextView tv_title;
		TextView tv_price;
		boolean status = false;
	}

	// GridView的Adapter
	private class MyGridViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return 5;
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
			ViewHolder holder = null;
			if (convertView == null)
				convertView = View.inflate(DeliveryActivity.this, R.layout.item_clean_serve, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_title = (TextView) convertView.findViewById(R.id.tv_title);
				holder.tv_price = (TextView) convertView.findViewById(R.id.tv_price);
				convertView.setTag(holder);
			}

			Utils.spanTextSize(holder.tv_price, "：", false, new int[] { 18, 13 });

			return convertView;

		}

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
