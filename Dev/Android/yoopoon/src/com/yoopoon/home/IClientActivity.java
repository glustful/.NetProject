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
import java.util.HashMap;
import java.util.List;
import java.util.Random;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.os.Handler;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AbsListView.OnScrollListener;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
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
	@ViewById(R.id.ll_loading)
	LinearLayout ll_loading;
	private MyClientListAdapter adapter;
	private List<GuestInfo> guestInfos = new ArrayList<GuestInfo>();
	private static final String TAG = "IClientActivity";
	private LinearLayout last_shown_progress;
	private int totalCount;
	private int page = 1;
	private final int pageSize = 10;
	private Handler handler = new Handler();

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我的客户");
		initParams();
		lv.setOnScrollListener(new MyScrollListener());
		requestData();
	}

	private class MyScrollListener implements OnScrollListener {

		@Override
		public void onScrollStateChanged(AbsListView view, int scrollState) {
			if (scrollState == OnScrollListener.SCROLL_STATE_IDLE) {
				if (lv.getLastVisiblePosition() == (guestInfos.size() - 1)) {
					if (guestInfos.size() < totalCount) {
						initParams();
						requestData();
					} else {
						Toast.makeText(IClientActivity.this, "这已经是所有客户啦", Toast.LENGTH_SHORT).show();
					}
				}
			}
		}

		@Override
		public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {

		}
	}

	private HashMap<String, String> params = new HashMap<String, String>();

	private void initParams() {
		params.clear();
		params.put("page", String.valueOf(page++));
		params.put("pageSize", "10");
	}

	private void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		handler.postDelayed(new Runnable() {

			@Override
			public void run() {
				new RequestAdapter() {

					@Override
					public void onReponse(ResponseData data) {

						if (data.getResultState() == ResultState.eSuccess) {
							JSONObject dataObj = data.getJsonObject2();
							if (dataObj != null) {
								try {
									totalCount = dataObj.getInt("totalCount");
									JSONArray listArray = dataObj.getJSONArray("list");
									Random r = new Random();
									for (int i = 0; i < listArray.length(); i++) {
										JSONObject guestObj = listArray.getJSONObject(i);
										Log.i(TAG, guestObj.toString());
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
										GuestInfo info = new GuestInfo(clientName, houseType, houses, status, phone,
												id, strType);
										if ("预约中".equals(status))
											info.num = 2;
										else if (status.contains("审核")) {
											info.num = 1;
										} else if (status.contains("成功")) {
											info.num = 4;
										} else {
											info.num = 3;
										}
										guestInfos.add(info);
									}
								} catch (JSONException e) {
									e.printStackTrace();
								} finally {
									ll_loading.setVisibility(View.GONE);
								}
								fillData();
							} else {
								ll_loading.setVisibility(View.GONE);
								Toast.makeText(IClientActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
							}
						}
					}

					@Override
					public void onProgress(ProgressMessage msg) {
						// TODO Auto-generated method stub

					}
					// + "/?page=1&pageSize=10"
				}.setUrl(getString(R.string.url_get_my_clients)).addParam(params).setRequestMethod(RequestMethod.eGet)
						.notifyRequest();

			}
		}, 2000);

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
		public View getView(final int position, View convertView, ViewGroup parent) {
			final GuestInfo info = guestInfos.get(position);
			convertView = View.inflate(IClientActivity.this, R.layout.item_iclient, null);

			TextView tv_title = (TextView) convertView.findViewById(R.id.tv_iclient_title);
			TextView tv_style = (TextView) convertView.findViewById(R.id.tv_iclient_housestyle);
			TextView tv_phone = (TextView) convertView.findViewById(R.id.tv_iclient_phone);
			TextView tv_showprogress = (TextView) convertView.findViewById(R.id.tv_iclient_showprogress);
			final LinearLayout progress = (LinearLayout) convertView.findViewById(R.id.ll_iclient_progress);
			final String status = info.getStatus();

			progress.setVisibility(View.GONE);
			progress.setTag(position);

			View detail_progress = null;
			switch (info.num) {
				case 1:
					detail_progress = View.inflate(IClientActivity.this, R.layout.progress, null);
					break;
				case 2:
					detail_progress = View.inflate(IClientActivity.this, R.layout.progress2, null);
					break;
				case 3:
					detail_progress = View.inflate(IClientActivity.this, R.layout.progress3, null);
					break;
				case 4:
					detail_progress = View.inflate(IClientActivity.this, R.layout.progress4, null);
					break;
			}
			progress.addView(detail_progress);
			if (info.isProgressShown())
				progress.setVisibility(View.VISIBLE);

			tv_title.setText(info.getStrType() + "  " + info.getClientName());
			tv_style.setText(info.getHouseType() + "   " + info.getHouses());
			tv_phone.setText(info.getPhone());

			tv_showprogress.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					if (last_shown_progress != null && last_shown_progress.getTag() != progress.getTag()) {
						ExpandAnimation ea_hide = new ExpandAnimation(last_shown_progress, 300);
						last_shown_progress.startAnimation(ea_hide);
						int last_position = (Integer) last_shown_progress.getTag();
						guestInfos.get(last_position).setProgressShown(false);
					}

					ExpandAnimation ea = new ExpandAnimation(progress, 300);

					progress.startAnimation(ea);
					info.setProgressShown(ea.toggle());
					last_shown_progress = ea.toggle() ? progress : null;

				}
			});

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
