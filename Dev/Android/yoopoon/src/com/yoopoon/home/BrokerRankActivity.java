/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: BrokerRankActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-14 下午3:45:02 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;

/**
 * @ClassName: BrokerRankActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午3:45:02
 */
@EActivity(R.layout.activity_broker_rank)
public class BrokerRankActivity extends MainActionBarActivity {
	@ViewById(R.id.lv_broker)
	ListView lv;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("经纪人排名");
		initListView();
	}

	private void initListView() {
		lv.setAdapter(new MyBrokerRankAdapter());
	}

	private class MyBrokerRankAdapter extends BaseAdapter {

		/*
		 * (non Javadoc)
		 * @Title: getCount
		 * @Description: TODO
		 * @return
		 * @see android.widget.Adapter#getCount()
		 */
		@Override
		public int getCount() {
			return 3;
		}

		/*
		 * (non Javadoc)
		 * @Title: getItem
		 * @Description: TODO
		 * @param position
		 * @return
		 * @see android.widget.Adapter#getItem(int)
		 */
		@Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return null;
		}

		/*
		 * (non Javadoc)
		 * @Title: getItemId
		 * @Description: TODO
		 * @param position
		 * @return
		 * @see android.widget.Adapter#getItemId(int)
		 */
		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}

		/*
		 * (non Javadoc)
		 * @Title: getView
		 * @Description: TODO
		 * @param position
		 * @param convertView
		 * @param parent
		 * @return
		 * @see android.widget.Adapter#getView(int, android.view.View, android.view.ViewGroup)
		 */
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			if (convertView == null)
				convertView = LayoutInflater.from(BrokerRankActivity.this).inflate(R.layout.item_broker_rank, null);
			TextView tv_desc = (TextView) convertView.findViewById(R.id.tv_rank_desc);
			TextView tv_money = (TextView) convertView.findViewById(R.id.tv_rank_money);
			ImageView iv_medal = (ImageView) convertView.findViewById(R.id.iv_rank_medal);
			ImageView iv_trend = (ImageView) convertView.findViewById(R.id.iv_broker_rank_trend);

			if (position == 0) {
				tv_desc.setText("金牌经纪人");
				tv_money.setText("获得佣金：2w");
				iv_medal.setImageResource(R.drawable.gold);
				iv_trend.setImageResource(R.drawable.up_arrow);
			} else {
				tv_desc.setText(position == 1 ? "银牌经纪人" : "銅牌經紀人");
				tv_money.setText("获得佣金：1.8k");
				iv_medal.setImageResource(position == 1 ? R.drawable.silver : R.drawable.cooper);
				iv_medal.setImageResource(position == 1 ? R.drawable.silver : R.drawable.cooper);
				iv_trend.setImageResource(position == 1 ? R.drawable.stabale_icon : R.drawable.trend_down_arrow);
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

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {

		return true;
	}
}
