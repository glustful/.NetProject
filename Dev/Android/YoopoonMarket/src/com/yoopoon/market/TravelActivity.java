package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.graphics.Color;
import android.os.Handler;
import android.text.format.DateUtils;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;

@EActivity(R.layout.activity_travel)
public class TravelActivity extends MainActionBarActivity {
	@ViewById(R.id.lv)
	PullToRefreshListView lv;
	MyListViewAdapter adapter;

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("旅游");
		titleButton.setTextColor(Color.WHITE);
		rightButton.setVisibility(View.VISIBLE);
		headView.setBackgroundColor(Color.RED);
		initData();
	}

	// 数据初始化
	private void initData() {
		fillData();
		lv.setOnItemClickListener(new MyListViewItemClickListener());
		lv.setMode(Mode.BOTH);
		lv.setOnRefreshListener(new HowWillIrefresh());
	}

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {

		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(TravelActivity.this, System.currentTimeMillis(),
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

	// 填充ListView的数据
	private void fillData() {
		if (adapter == null) {
			adapter = new MyListViewAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	// ListView的Item点击事件
	private class MyListViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			AssuranceDetailActivity_.intent(TravelActivity.this).start();
		}
	}

	static class ViewHolder {
		TextView tv_title;
		ImageView iv;
		TextView tv_phone;
	}

	// ListView的Adapter
	private class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return 5;
		}

		@Override
		public Object getItem(int position) {
			return null;
		}

		@Override
		public long getItemId(int position) {
			return 0;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder = null;
			if (convertView == null)
				convertView = View.inflate(TravelActivity.this, R.layout.item_assurance, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_phone = (TextView) convertView.findViewById(R.id.tv_phone);
				holder.tv_title = (TextView) convertView.findViewById(R.id.tv_title);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
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
