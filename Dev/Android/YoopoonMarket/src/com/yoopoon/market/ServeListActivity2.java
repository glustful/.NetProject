/**
 * 清洁服务Activity
 */

package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.MyGridView;

@EActivity(R.layout.activity_clean_serve)
public class ServeListActivity2 extends MainActionBarActivity {
	private static final String TAG = "CleanServeActivity";
	@Extra
	String[] contents;
	@ViewById(R.id.gv)
	MyGridView gv;
	@ViewById(R.id.ll_loading)
	View loading;
	List<ProductEntity> lists = new ArrayList<ProductEntity>();
	int selectedPosition = -1;
	MyGridViewAdapter adapter;

	@Click(R.id.btn_book)
	void book() {
		if (selectedPosition == -1) {
			Toast.makeText(ServeListActivity2.this, "亲，你还没有选择任何服务呢！", Toast.LENGTH_SHORT).show();
			return;
		} else {
			ServiceBalanceActivity_.intent(this).product(lists.get(selectedPosition)).start();
		}

	}

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText(contents[0]);
		headView.setBackgroundColor(Color.RED);
		titleButton.setTextColor(Color.WHITE);
		requestData();
	}

	void requestData() {
		loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					Log.i(TAG, array.toString());
					parseToList(array);
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(ServeListActivity2.this, data.getMsg(), Toast.LENGTH_SHORT).show();
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
					loading.setVisibility(View.GONE);
					initData();
				}
			}
		}).execute();
	}

	// 数据初始化
	private void initData() {
		adapter = new MyGridViewAdapter();
		gv.setAdapter(adapter);
		gv.setOnItemClickListener(new MyGridViewItemClickListener());
	}

	// GridView的Adapter
	private class MyGridViewAdapter extends BaseAdapter {

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
				convertView = View.inflate(ServeListActivity2.this, R.layout.item_clean_serve, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_title = (TextView) convertView.findViewById(R.id.tv_title);
				holder.tv_price = (TextView) convertView.findViewById(R.id.tv_price);
				convertView.setTag(holder);
			}
			ProductEntity product = lists.get(position);
			holder.tv_name.setText(product.Name);
			holder.tv_title.setText(product.Subtitte);
			holder.tv_price.setText("需付订金：￥" + product.Price);
			LinearLayout ll_bg = (LinearLayout) convertView.findViewById(R.id.ll_bg);
			if (position == selectedPosition) {
				ll_bg.setBackgroundResource(R.drawable.border_with_yes);
			} else {
				ll_bg.setBackgroundColor(Color.WHITE);
			}
			int padding = Utils.dp2px(ServeListActivity2.this, 20);
			ll_bg.setPadding(padding, padding, padding, padding);
			holder.status = !holder.status;

			Utils.spanTextSize(holder.tv_price, "：", false, new int[] { 18, 13 });

			return convertView;

		}

	}

	// GridView的点击事件处理
	private class MyGridViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			selectedPosition = position;
			adapter.notifyDataSetChanged();
			// holder.status = !holder.status;
		}
	}

	static class ViewHolder {

		TextView tv_name;
		TextView tv_title;
		TextView tv_price;
		boolean status = false;
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
