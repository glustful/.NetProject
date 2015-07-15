/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: IPocketActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-15 上午9:36:39 
 * @version: V1.0   
 */
package com.yoopoon.home;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ListView;

/**
 * @ClassName: IPocketActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-15 上午9:36:39
 */
@EActivity(R.layout.activity_ipocket)
public class IPocketActivity extends MainActionBarActivity {
	@ViewById(R.id.lv_ipocket_bankcard)
	ListView lv;

	private String[] banks = { "招商", "建设", "建设", "招商" };

	private MyBankListAdapter adapter;

	@Click(R.id.btn_ipocket_takecash)
	void takeCash() {
		TakeCashActivity_.intent(this).start();
	}

	@Click(R.id.btn_ipocket_addbank)
	void addBank() {
		AddBankActivity_.intent(this).start();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我的钱包");
		fillData();
		lv.setOnItemClickListener(new MyBankItemClickListener());
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyBankListAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private class MyBankItemClickListener implements OnItemClickListener {

		/*
		 * (non Javadoc)
		 * @Title: onItemClick
		 * @Description: TODO
		 * @param parent
		 * @param view
		 * @param position
		 * @param id
		 * @see
		 * android.widget.AdapterView.OnItemClickListener#onItemClick(android.widget.AdapterView,
		 * android.view.View, int, long)
		 */
		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			BankCashActivity_.intent(IPocketActivity.this).start();
		}

	}

	private class MyBankListAdapter extends BaseAdapter {

		/*
		 * (non Javadoc)
		 * @Title: getCount
		 * @Description: TODO
		 * @return
		 * @see android.widget.Adapter#getCount()
		 */
		@Override
		public int getCount() {
			return banks.length;
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
			// if (position == banks.length) {
			// Button btn = new Button(IPocketActivity.this);
			// btn.setText("添加银行卡");
			// return btn;
			// }
			if (convertView == null)
				convertView = View.inflate(IPocketActivity.this, R.layout.item_bankcard, null);
			ImageView iv = (ImageView) convertView.findViewById(R.id.iv_bankcard);
			String bank = banks[position];
			iv.setImageResource(bank.contains("招商") ? R.drawable.bankcard2 : R.drawable.bankcard1);
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
