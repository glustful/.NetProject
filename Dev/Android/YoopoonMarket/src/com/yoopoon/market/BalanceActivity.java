/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PersonalInfoActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-8 上午9:32:55 
 * @version: V1.0   
 */
package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import com.yoopoon.market.domain.Staff;

/**
 * @ClassName: PersonalInfoActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-8 上午9:32:55
 */
@EActivity(R.layout.activity_balance)
public class BalanceActivity extends MainActionBarActivity {
	@ViewById(R.id.lv)
	ListView lv;
	@Extra
	Staff staff;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("确认订单");

		lv.setAdapter(new MyListViewAdapter());
	}

	class MyListViewAdapter extends BaseAdapter {

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
			if (convertView == null)
				convertView = View.inflate(BalanceActivity.this, R.layout.item_cart2, null);

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
