/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MeTaskActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-29 上午9:07:06 
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
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;

/**
 * @ClassName: MeTaskActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-29 上午9:07:06
 */
@EActivity(R.layout.activity_task)
public class MeTaskActivity extends MainActionBarActivity {
	@ViewById(R.id.tv_task_count)
	TextView tv_count;
	@ViewById(R.id.todayTask)
	ListView lv_task;
	@ViewById(R.id.ll_progress)
	LinearLayout ll_progress;

	// private static final String TAG = "MeTaskActivity";
	private List<JSONObject> datas = new ArrayList<JSONObject>();
	private MyTaskAdapter adapter;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("我的任务");
		requestTodayTask();
	}

	private void fillData() {
		lv_task.setVisibility(View.VISIBLE);
		if (adapter == null) {
			adapter = new MyTaskAdapter();
			lv_task.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private class MyTaskAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return datas.size();
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
				convertView = View.inflate(MeTaskActivity.this, R.layout.item_task, null);
			JSONObject item = datas.get(position);
			// Log.i(TAG, item.toString());
			TextView tv_name = (TextView) convertView.findViewById(R.id.tv_task_name);
			TextView tv_award = (TextView) convertView.findViewById(R.id.tv_task_award);
			String name = item.optString("Taskname", "");
			String award = item.optString("awardname", "") + ":" + item.optString("awardvalue");
			tv_name.setText(name);
			tv_award.setText(award);
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

	/**
	 * @Title: requestTodayTask
	 * @Description: 获取今日任务列表
	 */
	void requestTodayTask() {
		ll_progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				ll_progress.setVisibility(View.GONE);

				if (data.getResultState() == ResultState.eSuccess) {
					if (data.getMRootData().optJSONArray("list") != null) {
						JSONArray array = data.getMRootData().optJSONArray("list");
						for (int i = 0; i < array.length(); i++) {
							tv_count.setText("今日任务(" + (i + 1) + ")");
							try {
								datas.add(array.getJSONObject(i));
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
						fillData();
						return;
					}
				}
				lv_task.setVisibility(View.GONE);
				tv_count.setText("今日任务(无)");
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_task_taskListMobile)).setRequestMethod(RequestMethod.eGet)
				.addParam("page", "1").addParam("type", "today").notifyRequest();
	}
}
