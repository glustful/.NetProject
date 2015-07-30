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

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.AdapterView.OnItemLongClickListener;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.domain.Bank;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

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

	private MyBankListAdapter adapter;
	private User user;
	private static final String TAG = "IPocketActivity";
	private List<Bank> bankDatas = new ArrayList<Bank>();
	private int amountMoney = 0;

	@Click(R.id.btn_ipocket_takecash)
	void takeCash() {
		TakeCash2Activity_.intent(this).start();
	}

	@ViewById(R.id.tv_ipocket_cash)
	TextView tv_cash;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我的钱包");

		user = User.lastLoginUser(this);
		if (user == null)
			HomeLoginActivity_.intent(this).isManual(true).start();

		// fillData();
		// lv.setOnItemClickListener(new MyBankItemClickListener());
		lv.setOnItemLongClickListener(new MyLongClickListener());
	}

	private class MyLongClickListener implements OnItemLongClickListener {

		@Override
		public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
			Builder builder = new Builder(IPocketActivity.this);
			View dialogView = View.inflate(IPocketActivity.this, R.layout.dialog_delete, null);

			builder.setView(dialogView);
			final AlertDialog dialog = builder.show();
			Button btn_delete = (Button) dialogView.findViewById(R.id.btn_dialog_delete);
			btn_delete.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					dialog.dismiss();
				}
			});

			return false;
		}

	}

	private void fillData() {
		tv_cash.setText(String.valueOf(amountMoney));
		if (adapter == null) {
			adapter = new MyBankListAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	protected void onResume() {
		request();
		super.onResume();
	}

	protected void request() {

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					JSONObject obj = data.getJsonObject2();
					try {
						JSONArray list = obj.getJSONArray("List");
						bankDatas.clear();
						for (int i = 0; i < list.length(); i++) {
							JSONObject bankObj = list.getJSONObject(i);
							Bank bank = new Bank();
							bank.setBankName(bankObj.getString("bankName"));
							bank.setNum(bankObj.getString("Num"));
							bankDatas.add(bank);
						}
						amountMoney = obj.getInt("AmountMoney");
						fillData();

					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}

				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(MyApplication.getInstance().getString(R.string.url_get_all_banks))
				.setRequestMethod(RequestMethod.eGet).notifyRequest();

	}

	private class MyBankItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			BankCashActivity_.intent(IPocketActivity.this).start();
		}

	}

	private class MyBankListAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return bankDatas.size() + 1;
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
			if (position == bankDatas.size()) {
				Button btn = new Button(IPocketActivity.this);
				btn.setBackgroundResource(R.drawable.addbankcard_selector);
				btn.setText("+添加银行卡");
				btn.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						AddBankActivity_.intent(IPocketActivity.this).start();
					}
				});
				return btn;
			}
			if (convertView == null || !(convertView instanceof RelativeLayout))
				convertView = View.inflate(IPocketActivity.this, R.layout.item_bankcard, null);
			Bank bank = bankDatas.get(position);
			TextView tv_bankname = (TextView) convertView.findViewById(R.id.tv_bankcard_bank);
			TextView tv_num = (TextView) convertView.findViewById(R.id.tv_bankcard_card);

			tv_bankname.setText(StringUtils.isEmpty(bank.getBankName()) ? "" : bank.getBankName());
			tv_num.setText("**** **** ****" + bank.getNum());

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
