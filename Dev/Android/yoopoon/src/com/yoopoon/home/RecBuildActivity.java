/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RecBuildActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-29 上午8:54:10 
 * @version: V1.0   
 */
package com.yoopoon.home;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.Html;
import android.text.format.DateUtils;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.active.BrandDetail2Activity_;

/**
 * @ClassName: RecBuildActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-29 上午8:54:10
 */
@EActivity(R.layout.activity_rec)
public class RecBuildActivity extends MainActionBarActivity {
	@ViewById(R.id.lv_rec)
	PullToRefreshListView lv;
	@ViewById(R.id.ll_progress)
	LinearLayout ll_progress;
	// private static final String TAG = "RecBuildActivity";
	private List<JSONObject> datas = new ArrayList<JSONObject>();
	private MyRecsBuildAdapter adapter;
	private ListView refreshView;
	private HashMap<String, String> parameter;
	private boolean isBroker = false;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("推荐楼盘");
		init();
		User user = User.lastLoginUser(this);
		isBroker = user.isBroker();
		if (!isBroker)
			lv.setBackgroundColor(Color.WHITE);
		requestBrandList();
	}

	private void init() {
		lv.setOnRefreshListener(new HowWillIrefresh());
		lv.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
		refreshView = lv.getRefreshableView();
		refreshView.setFastScrollEnabled(false);
		refreshView.setFadingEdgeLength(0);
		initParameter();
	}

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {
		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(RecBuildActivity.this, System.currentTimeMillis(),
					DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			autoIncreatePage();
			requestBrandList();
		}
	}

	private void initParameter() {
		if (parameter == null) {
			parameter = new HashMap<String, String>();
		}
		parameter.clear();
		parameter.put("className", "房地产");
		parameter.put("page", "1");
		parameter.put("pageSize", "6");
		parameter.put("type", "all");
	}

	private void autoIncreatePage() {
		parameter.put("page", (Integer.parseInt(parameter.get("page")) + 1) + "");
	}

	void requestBrandList() {
		ll_progress.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				lv.onRefreshComplete();
				if (data.getResultState() == ResultState.eSuccess) {
					JSONArray list = data.getMRootData().optJSONArray("List");
					if (list == null || list.length() < 1) {
						ToastUtils.showToast(RecBuildActivity.this, "亲，这已经是所有推荐的啦！", 3000);
						descCount();
						ll_progress.setVisibility(View.GONE);
						return;
					}
					for (int i = 0; i < list.length(); i++) {
						datas.add(list.optJSONObject(i));
					}
					fillData();
				} else {
					descCount();
				}
				ll_progress.setVisibility(View.GONE);
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_brand_GetAllBrand)).setRequestMethod(RequestMethod.eGet).addParam(parameter)
				.notifyRequest();
	}

	void descCount() {
		int page = Integer.parseInt(parameter.get("page"));
		page = page > 1 ? page - 1 : 1;
		parameter.put("page", page + "");
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyRecsBuildAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private class MyRecsBuildAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return datas.size();
		}

		@Override
		public Object getItem(int position) {
			return datas.get(position);
		}

		@Override
		public long getItemId(int position) {
			return position;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			Holder mHolder;
			if (convertView == null) {
				convertView = LayoutInflater.from(RecBuildActivity.this).inflate(R.layout.item_rec_build, null);
				mHolder = new Holder();
				mHolder.init(convertView);
				convertView.setTag(mHolder);
			} else {
				mHolder = (Holder) convertView.getTag();
			}
			final JSONObject item = datas.get(position);
			// Log.i(TAG, item.toString());
			String url = RecBuildActivity.this.getString(R.string.url_host_img) + item.optString("Bimg");
			mHolder.iv.setTag(url);
			mHolder.iv.setScaleType(ScaleType.FIT_XY);
			ImageLoader.getInstance().displayImage(url, mHolder.iv);
			String title = item.optString("Bname", "") + "    ";
			JSONObject parameter = item.optJSONObject("ProductParamater");
			String city = "[" + parameter.optString("所属城市", "") + "]   ";
			String area = parameter.optString("占地面积", "");
			mHolder.tv_detail2.setText(city + title + area);
			mHolder.tv_detail2.setGravity(Gravity.LEFT);
			String adTitle = Html.fromHtml(item.optString("AdTitle")).toString();
			mHolder.tv_right.setText(StringUtils.isEmpty(adTitle) ? "" : adTitle);
			mHolder.tv_call.setTag(Tools.optString(parameter, "来电咨询", ""));
			String preferential = parameter.optString("最高优惠");
			mHolder.tv_detail1.setText(item.optString("SubTitle", ""));
			mHolder.ll_broker.setVisibility(isBroker ? View.VISIBLE : View.GONE);

			mHolder.tv_call.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					String phone = (String) v.getTag();
					if (!StringUtils.isEmpty(phone)) {
						Tools.callPhone(RecBuildActivity.this, v.getTag().toString());
					} else {
						Toast.makeText(RecBuildActivity.this, "抱歉，暂时没有该楼盘电话", Toast.LENGTH_SHORT).show();
					}
				}
			});
			mHolder.tv_guest.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					BrandDetail2Activity_.intent(RecBuildActivity.this).mJson(item.toString()).start();
				}
			});
			mHolder.iv.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					BrandDetail2Activity_.intent(RecBuildActivity.this).mJson(item.toString()).start();
					// BrandDetailActivity_.intent(mContext).mJson(item.toString()).start();
				}
			});
			return convertView;
		}
	}

	class Holder {

		ImageView iv;
		TextView tv_detail1;
		TextView tv_detail2;
		TextView tv_right;
		TextView tv_call;
		TextView tv_guest;
		LinearLayout ll_broker;

		void init(View root) {
			iv = (ImageView) root.findViewById(R.id.iv_rec);
			tv_detail1 = (TextView) root.findViewById(R.id.tv_recs_detail);
			tv_detail2 = (TextView) root.findViewById(R.id.tv_recs_detail1);
			tv_right = (TextView) root.findViewById(R.id.tv_build_right);
			tv_call = (TextView) root.findViewById(R.id.tv_rec_call);
			tv_guest = (TextView) root.findViewById(R.id.tv_rec_guest);
			ll_broker = (LinearLayout) root.findViewById(R.id.ll_rec_broker);
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
