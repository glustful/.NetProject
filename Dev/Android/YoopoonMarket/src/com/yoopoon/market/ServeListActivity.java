package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.os.Handler;
import android.text.TextUtils;
import android.text.format.DateUtils;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.handmark.pulltorefresh.library.PullToRefreshBase;
import com.handmark.pulltorefresh.library.PullToRefreshBase.Mode;
import com.handmark.pulltorefresh.library.PullToRefreshListView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

@EActivity(R.layout.activity_charge)
public class ServeListActivity extends MainActionBarActivity {
	private static final String TAG = "ChargeActivity";

	@Extra
	String[] contents;
	@ViewById(R.id.lv)
	PullToRefreshListView lv;
	MyListViewAdapter adapter;
	List<ProductEntity> lists = new ArrayList<ProductEntity>();

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText(contents[0]);
		titleButton.setTextColor(Color.WHITE);
		rightButton.setVisibility(View.VISIBLE);
		headView.setBackgroundColor(Color.RED);
		initData();
		requestData();
	}

	void requestData() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					Log.i(TAG, array.toString());
					parseToList(array);
				} else {
					Toast.makeText(ServeListActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_service_get)).setRequestMethod(RequestMethod.eGet).addParam("type", "1")
				.addParam("name", contents[1]).notifyRequest();
	}

	void parseToList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject object = array.getJSONObject(i);
						ProductEntity entity = om.readValue(object.toString(), ProductEntity.class);
						lists.add(entity);
					} catch (JSONException e) {
						e.printStackTrace();
					} catch (JsonParseException e) {
						e.printStackTrace();
					} catch (JsonMappingException e) {
						e.printStackTrace();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
				return lists;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					Log.i(TAG, parseResult.toString());
					fillData();
				}
			}
		}).execute();
	}

	// 数据初始化
	private void initData() {
		lv.setOnItemClickListener(new MyListViewItemClickListener());
		lv.setMode(Mode.BOTH);
		lv.setOnRefreshListener(new HowWillIrefresh());
	}

	class HowWillIrefresh implements PullToRefreshBase.OnRefreshListener2<ListView> {

		@Override
		public void onPullDownToRefresh(PullToRefreshBase<ListView> refreshView) {
			String label = DateUtils.formatDateTime(ServeListActivity.this, System.currentTimeMillis(),
					DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
			refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
			new Handler().postDelayed(new Runnable() {

				@Override
				public void run() {
					lv.onRefreshComplete();
				}
			}, 1000);
		}

		@Override
		public void onPullUpToRefresh(PullToRefreshBase<ListView> refreshView) {
			new Handler().postDelayed(new Runnable() {

				@Override
				public void run() {
					lv.onRefreshComplete();
				}
			}, 1000);
		}
	}

	// 填充ListView的数据
	private void fillData() {
		if (adapter == null) {
			adapter = new MyListViewAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	// ListView的Item点击事件
	private class MyListViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			ProductEntity product = lists.get(position - 1);
			AssuranceDetailActivity_.intent(ServeListActivity.this).product(product).start();
		}
	}

	static class ViewHolder {
		TextView tv_title;
		ImageView iv;
		TextView tv_phone;
	}

	// ListView的Adapter
	private class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return lists.size();
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
			ViewHolder holder = null;
			if (convertView == null)
				convertView = View.inflate(ServeListActivity.this, R.layout.item_assurance, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_phone = (TextView) convertView.findViewById(R.id.tv_phone);
				holder.tv_title = (TextView) convertView.findViewById(R.id.tv_title);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				convertView.setTag(holder);
			}
			ProductEntity product = lists.get(position);
			holder.tv_phone.setText("联系电话：" + product.Contactphone);
			String name = "【" + product.Name + "】";
			String subtitle = product.Subtitte;
			holder.tv_title.setText(name + subtitle);
			if (!TextUtils.isEmpty(product.MainImg)) {
				String imageUrl = getString(R.string.url_image) + product.MainImg;
				holder.iv.setTag(imageUrl);
				ImageLoader.getInstance().displayImage(imageUrl, holder.iv, MyApplication.getOptions(),
						MyApplication.getLoadingListener());
				holder.iv.setScaleType(ScaleType.CENTER_CROP);
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
