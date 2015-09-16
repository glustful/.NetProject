package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.CheckBox;
import android.widget.ListView;
import android.widget.TextView;

@EActivity(R.layout.activity_choose_address)
public class ChooseAddressActivity extends MainActionBarActivity {
	@ViewById(R.id.lv)
	ListView lv;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("选择收货地址");
		lv.setAdapter(new MyListViewAdapter());
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
			return 2;
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
		public View getView(int position, View convertView, ViewGroup parent) {
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
