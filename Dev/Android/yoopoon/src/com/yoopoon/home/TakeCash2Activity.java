/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: TakeCash2Activity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-29 下午5:12:28 
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
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.CompoundButton.OnCheckedChangeListener;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.domain.TakeCashEntity;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: TakeCash2Activity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-29 下午5:12:28
 */
@EActivity(R.layout.activity_takecash2)
public class TakeCash2Activity extends MainActionBarActivity {
	@ViewById(R.id.lv_takecash)
	ListView lv;

	@Click(R.id.bt_takecash_next)
	void next() {
		TakeCashStep2Activity_.intent(this).start();
	}

	private String userId;
	private MyAdapter adapter;
	private static final String TAG = "TakeCash2Activity";
	private List<TakeCashEntity> datas = new ArrayList<TakeCashEntity>();

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("提现");
		init();
	}

	private void init() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		}
		userId = String.valueOf(user.id);
		requestCash();
	}

	private class MyAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return datas.size();
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
			ViewHolder holder;
			if (convertView == null)
				convertView = View.inflate(TakeCash2Activity.this, R.layout.item_takecash, null);

			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.cb_status = (CheckBox) convertView.findViewById(R.id.cb_takecash);
				holder.tv_cash = (TextView) convertView.findViewById(R.id.tv_takecash_cash);
				holder.tv_type = (TextView) convertView.findViewById(R.id.tv_takecash_type);
				holder.tv_time = (TextView) convertView.findViewById(R.id.tv_takecash_time);
				convertView.setTag(holder);
			}
			final TakeCashEntity item = datas.get(position);
			holder.tv_type.setText(item.getType());
			holder.tv_cash.setText(String.valueOf(item.getBalanceNum()));
			holder.tv_time.setText(item.getAddTime());
			holder.cb_status.setChecked(item.isStatus());

			holder.cb_status.setOnCheckedChangeListener(new OnCheckedChangeListener() {

				@Override
				public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
					item.setStatus(isChecked);

				}
			});

			return convertView;
		}

	}

	static class ViewHolder {
		CheckBox cb_status;
		TextView tv_cash;
		TextView tv_type;
		TextView tv_time;
	}

	void requestCash() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					try {
						JSONArray list = data.getMRootData().getJSONArray("List");
						for (int i = 0; i < list.length(); i++) {
							JSONObject obj = list.getJSONObject(i);
							// "Balancenum":101,"Addtime":"2015-07-27","Type":0,"Id":123
							String type = (obj.optInt("Type") == 0) ? "带客" : "其他";
							String addTime = obj.optString("Addtime", "");
							double balanceNum = obj.optDouble("Balancenum", 0);
							int id = obj.optInt("Id", 0);
							datas.add(new TakeCashEntity(balanceNum, addTime, type, id, false));
						}
						fillData();
					} catch (JSONException e) {
						e.printStackTrace();
					}
				} else {
					Toast.makeText(TakeCash2Activity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_takecash_showall)).setRequestMethod(RequestMethod.eGet)
				.addParam("UserId", userId).notifyRequest();
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
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
