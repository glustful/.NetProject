package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.view.inputmethod.EditorInfo;
import android.widget.BaseAdapter;
import android.widget.EditText;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.TreeCategory;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.MyGridView;
import com.yoopoon.market.view.MyListView;

@EActivity(R.layout.activity_category2)
public class CategoryActivity2 extends MainActionBarActivity {
	private static final String TAG = "CategoryActivity2";
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	@ViewById(R.id.lv)
	MyListView lv;
	@ViewById(R.id.ll)
	LinearLayout ll;
	MyListViewAdapter adapter;
	List<TreeCategory> lists = new ArrayList<TreeCategory>();
	int selectedPosition = 0;

	@AfterViews
	void init() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("分类");
		// 添加用户输入内容后输入法可以使用键盘上的搜索框搜索
		searchProductEditText.setImeActionLabel("搜索", EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setSingleLine();
		searchProductEditText.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setOnEditorActionListener(new OnEditorActionListener() {

			@Override
			public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
				if (actionId == EditorInfo.IME_ACTION_SEARCH) {
					/*
					 * Intent intent = new Intent(CategoryActivity.this, ProductList_.class); Bundle
					 * bundle = new Bundle(); bundle.putString("productClassification", "曲靖特产");
					 * intent.putExtras(bundle); startActivity(intent); return true;
					 */
				}
				return false;
			}
		});
		requestData();
	}

	void requestData() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						if (array != null) {
							parseToList(array);
						}
					}
				} else {
					Toast.makeText(CategoryActivity2.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_category_getall)).addParam("ifid", "1").setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	void parseToList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject object = array.getJSONObject(i);
						TreeCategory category = om.readValue(object.toString(), TreeCategory.class);
						lists.add(category);
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
					initList();
				}
			}
		}).execute();
	}

	void initList() {
		adapter = new MyListViewAdapter();
		lv.setAdapter(adapter);
		refreshLinearLayout(selectedPosition);
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return lists.size();
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
			TextView tv = new TextView(CategoryActivity2.this);
			TreeCategory category = lists.get(position);
			tv.setPadding(10, 10, 10, 10);
			tv.setText(category.label);
			tv.setGravity(Gravity.CENTER);
			tv.setBackgroundResource(R.drawable.gray_bg);
			if (selectedPosition == position) {
				tv.setTextColor(Color.RED);
				tv.setBackgroundColor(Color.WHITE);
			}
			tv.setTag(position);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					selectedPosition = (Integer) v.getTag();
					refreshLinearLayout(selectedPosition);
					adapter.notifyDataSetChanged();
				}
			});
			return tv;
		}
	}

	void refreshLinearLayout(int position) {
		ll.removeAllViews();
		TreeCategory category = lists.get(position);
		List<TreeCategory> children = category.children;
		int spacing = Utils.dp2px(this, 10);
		for (final TreeCategory child : children) {
			TextView tv = new TextView(this);
			tv.setText(child.label);
			tv.setPadding(0, 0, 0, spacing);
			ll.addView(tv);
			MyGridView gv = new MyGridView(this);
			gv.setHorizontalSpacing(spacing);
			gv.setColumnWidth(GridView.AUTO_FIT);
			gv.setNumColumns(3);
			ll.addView(gv);
			View v = new View(CategoryActivity2.this);
			LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LayoutParams.MATCH_PARENT, 1);
			v.setBackgroundResource(R.drawable.line);
			ll.addView(v, params);
			requestImages(String.valueOf(child.Id), gv);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					Intent intent = new Intent(CategoryActivity2.this, ProductClassificationList_.class);
					Bundle bundle = new Bundle();
					bundle.putString("classificationId", child.Id + "");
					bundle.putString("classificationName", child.label);
					intent.putExtras(bundle);
					startActivity(intent);
				}
			});
		}
		ll.removeViewAt(ll.getChildCount() - 1);

	}

	static class ViewHolder {
		ImageView iv;
		TextView tv;
	}

	class MyGridViewAdapter extends BaseAdapter {

		List<Map<String, String>> infos;

		public MyGridViewAdapter(List<Map<String, String>> infos) {
			this.infos = infos;
		}

		@Override
		public int getCount() {
			return infos.size();
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
			Map<String, String> item = infos.get(position);
			if (convertView == null)
				convertView = View.inflate(CategoryActivity2.this, R.layout.item_category, null);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				holder.tv = (TextView) convertView.findViewById(R.id.tv);
				convertView.setTag(holder);
			}
			GridView gv = (GridView) parent;
			int width = gv.getColumnWidth();
			LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LayoutParams.WRAP_CONTENT, width);
			holder.iv.setLayoutParams(params);
			ImageLoader.getInstance().displayImage(item.get("url"), holder.iv);
			holder.tv.setText(item.get("name"));
			final String id = item.get("id");
			convertView.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					Bundle bundle = new Bundle();
					bundle.putString("comeFromstatusCode", "productClassificationList");
					bundle.putString("productId", id);
					Intent intent = new Intent(CategoryActivity2.this, ProductDetailActivity_.class);
					intent.putExtras(bundle);
					CategoryActivity2.this.startActivity(intent);
				}
			});
			return convertView;
		}

	}

	void requestImages(String id, final MyGridView gv) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					if (array != null) {
						int length = array.length() >= 6 ? 6 : array.length();
						List<Map<String, String>> infos = new ArrayList<Map<String, String>>();
						for (int i = 0; i < length; i++) {
							Map<String, String> item = new HashMap<String, String>();
							JSONObject product = array.optJSONObject(i);
							String img = product.optString("MainImg");
							String name = product.optString("Name");
							String id = product.optString("Id");
							String url = getString(R.string.url_image) + img;
							item.put("url", url);
							item.put("name", name);
							item.put("id", id);
							infos.add(item);
						}
						gv.setAdapter(new MyGridViewAdapter(infos));
					}
				} else {
					Toast.makeText(CategoryActivity2.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_communityproduct)).addParam("CategoryId", id)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	void requestImages(String id, final ImageView[] ivs) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					if (array != null) {
						int length = array.length() >= 6 ? 6 : array.length();
						for (int i = 0; i < length; i++) {
							JSONObject product = array.optJSONObject(i);
							String img = product.optString("MainImg");
							String name = product.optString("Name");
							String url = getString(R.string.url_image) + img;
							ImageLoader.getInstance().displayImage(url, ivs[i]);
							ivs[i].setScaleType(ScaleType.CENTER);
						}
					}
				} else {
					Toast.makeText(CategoryActivity2.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_communityproduct)).addParam("CategoryId", id)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
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
