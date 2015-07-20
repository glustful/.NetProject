/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: IClientActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-18 上午10:44:53 
 * @version: V1.0   
 */
package com.yoopoon.home;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.home.anim.ExpandAnimation;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.domain.GuestInfo;

/**
 * @ClassName: IClientActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-18 上午10:44:53
 */
@EActivity(R.layout.activity_iclient)
public class IClientActivity extends MainActionBarActivity {
	@ViewById(R.id.lv_iclient)
	ListView lv;
	private MyClientListAdapter adapter;
	private List<GuestInfo> guestInfos = new ArrayList<GuestInfo>();
	private static final String TAG = "IClientActivity";

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我的客户");
		requestData();
	}

	private void requestData() {
		new Thread() {

			@Override
			public void run() {
				new RequestAdapter() {

					@Override
					public void onReponse(ResponseData data) {
						if (data != null)
							if (data.getResultState() == ResultState.eSuccess) {
								JSONObject dataObj = data.getJsonObject2();
								if (dataObj != null) {
									try {
										int totalCount = dataObj.getInt("totalCount");
										JSONArray listArray = dataObj.getJSONArray("list");
										for (int i = 0; i < listArray.length(); i++) {
											JSONObject guestObj = listArray.getJSONObject(i);
											// "StrType":"带客",
											// "Status":"预约中",
											// "Clientname":"徐阳会",
											// "Id":"43",
											// "Phone":"13508713650",
											// "Houses":"人民路壹号",
											// "Housetype":"2室2厅1厨2卫 书房"
											String strType = guestObj.getString("StrType");
											String status = guestObj.getString("Status");
											String clientName = guestObj.getString("Clientname");
											String id = guestObj.getString("Id");
											String phone = guestObj.getString("Phone");
											String houses = guestObj.getString("Houses");
											String houseType = guestObj.getString("Housetype");
											GuestInfo info = new GuestInfo(clientName, houseType, houses, status,
													phone, id, strType);
											guestInfos.add(info);
										}
									} catch (JSONException e) {
										// TODO Auto-generated catch block
										e.printStackTrace();
									}
									fillData();
								} else {
									Toast.makeText(IClientActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
								}
							}
					}

					@Override
					public void onProgress(ProgressMessage msg) {
						// TODO Auto-generated method stub

					}
				}.setUrl(getString(R.string.url_get_my_clients) + "/?page=1&pageSize=10")
						.setRequestMethod(RequestMethod.eGet).notifyRequest();
			}
		}.start();
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyClientListAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private class MyClientListAdapter extends BaseAdapter {

		/*
		 * (non Javadoc)
		 * @Title: getCount
		 * @Description: TODO
		 * @return
		 * @see android.widget.Adapter#getCount()
		 */
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return guestInfos.size();
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

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			GuestInfo info = guestInfos.get(position);
			convertView = View.inflate(IClientActivity.this, R.layout.item_iclient, null);

			TextView tv_title = (TextView) convertView.findViewById(R.id.tv_iclient_title);
			TextView tv_style = (TextView) convertView.findViewById(R.id.tv_iclient_housestyle);
			TextView tv_phone = (TextView) convertView.findViewById(R.id.tv_iclient_phone);
			TextView tv_showprogress = (TextView) convertView.findViewById(R.id.tv_iclient_showprogress);
			final RelativeLayout rl_progress = (RelativeLayout) convertView.findViewById(R.id.rl_iclient_progress);
			final String status = info.getStatus();

			tv_title.setText(info.getStrType() + "  " + info.getClientName());
			tv_style.setText(info.getHouseType() + "   " + info.getHouses());
			tv_phone.setText(info.getPhone());

			tv_showprogress.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					ExpandAnimation ea = new ExpandAnimation(rl_progress, 300);
					// setProgress(rl_progress, status);
					rl_progress.startAnimation(ea);
				}
			});

			return convertView;
		}
	}

	private void setProgress(RelativeLayout rl_progress, String status) {
		// 1、等待审核
		// 2、等待上访
		// 3、洽谈中
		// 4、交易成功

		TextView tv_progress1_done = (TextView) rl_progress.findViewById(R.id.tv_progress_1);
		TextView tv_progress1_undone = (TextView) rl_progress.findViewById(R.id.tv_progress_1_undone);

		if (status.contains("等待审核"))
			return;
		else {
			tv_progress1_done.setVisibility(View.GONE);
			tv_progress1_undone.setVisibility(View.VISIBLE);
			if (status.contains("等待上访")) {
				TextView tv_progress2_undone = (TextView) rl_progress.findViewById(R.id.tv_progress_2);
				TextView tv_progress2_done = (TextView) rl_progress.findViewById(R.id.tv_progress_2_done);
				tv_progress2_done.setVisibility(View.VISIBLE);
				tv_progress2_undone.setVisibility(View.GONE);
				return;
			}
			if (status.contains("洽谈中")) {
				TextView tv_progress3_undone = (TextView) rl_progress.findViewById(R.id.tv_progress_3);
				TextView tv_progress3_done = (TextView) rl_progress.findViewById(R.id.tv_progress_3_done);
				tv_progress3_done.setVisibility(View.VISIBLE);
				tv_progress3_undone.setVisibility(View.GONE);
				return;
			}
			if (status.contains("交易成功")) {
				TextView tv_progress4_undone = (TextView) rl_progress.findViewById(R.id.tv_progress_4);
				TextView tv_progress4_done = (TextView) rl_progress.findViewById(R.id.tv_progress_4_done);
				tv_progress4_done.setVisibility(View.VISIBLE);
				tv_progress4_undone.setVisibility(View.GONE);
				return;
			}
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
