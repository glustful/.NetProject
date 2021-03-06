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
import android.graphics.Color;
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
import android.widget.Toast;
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
	@ViewById(R.id.rl_progress)
	View progress;
	private MyBankListAdapter adapter;
	private User user;
	private List<Bank> bankDatas = new ArrayList<Bank>();
	private int amountMoney = 0;
	private AlertDialog deleteDialog;

	@Click(R.id.btn_ipocket_takecash)
	void takeCash() {
		if (amountMoney <= 0) {
			Toast.makeText(this, "亲，你已经没有余额啦！", Toast.LENGTH_LONG).show();
			return;
		}
		TakeCash2Activity_.intent(this).start();
	}

	@ViewById(R.id.tv_ipocket_cash)
	TextView tv_cash;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("我的钱包");
		user = User.lastLoginUser(this);
		if (user == null)
			HomeLoginActivity_.intent(this).isManual(true).start();
		lv.setOnItemLongClickListener(new MyLongClickListener());
	}

	private class MyLongClickListener implements OnItemLongClickListener {

		@Override
		public boolean onItemLongClick(AdapterView<?> parent, View view, int position, long id) {
			final Bank bank = bankDatas.get(position);
			Builder builder = new Builder(IPocketActivity.this);
			View dialogView = View.inflate(IPocketActivity.this, R.layout.dialog_delete, null);
			Button btn_delete = (Button) dialogView.findViewById(R.id.btn_dialog_delete);
			btn_delete.setText("删除银行卡(" + "**** **** ****" + bank.Num + ")");
			builder.setView(dialogView);
			deleteDialog = builder.show();
			btn_delete.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					deleteDialog.dismiss();
					requestDelete(bank);
				}
			});
			return false;
		}
	}

	void requestDelete(final Bank bank) {
		progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				progress.setVisibility(View.GONE);
				Toast.makeText(IPocketActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				if (data.getMRootData() != null) {
					boolean status = data.getMRootData().optBoolean("Status", false);
					if (status) {
						bankDatas.remove(bank);
						fillData();
					}
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_delete_bank)).SetJSON(String.valueOf(bank.Id)).notifyRequest();
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
		progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				progress.setVisibility(View.GONE);
				if (data.getResultState() == ResultState.eSuccess) {
					JSONObject obj = data.getMRootData();
					try {
						JSONArray list = obj.getJSONArray("List");
						bankDatas.clear();
						for (int i = 0; i < list.length(); i++) {
							JSONObject bankObj = list.getJSONObject(i);
							Bank bank = new Bank();
							bank.Id = bankObj.optInt("Id", 0);
							bank.BankName = bankObj.getString("bankName");
							bank.Num = bankObj.getString("Num");
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
			tv_bankname.setText(StringUtils.isEmpty(bank.BankName) ? "" : bank.BankName);
			tv_num.setText("**** **** ****" + bank.Num);
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

	@Override
	protected void onStop() {
		if (deleteDialog != null) {
			deleteDialog.dismiss();
			deleteDialog = null;
		}
		super.onStop();
	}
}
