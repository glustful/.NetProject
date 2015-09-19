package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.market.anim.ExpandAnimation;
import com.yoopoon.market.domain.CategoryEntity;
import com.yoopoon.market.domain.CategoryList;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;

@EActivity(R.layout.activity_select)
public class SearchActivity extends MainActionBarActivity {
	private static final String TAG = "SearchActivity";
	private static final int INITLIST = 1;
	@ViewById(R.id.ll_areas)
	LinearLayout ll_areas;
	@ViewById(R.id.ll_loading)
	View loading;
	List<CategoryEntity> categoryList = new ArrayList<CategoryEntity>();
	int[] counts;
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };
	List<CategoryList> lists = new ArrayList<CategoryList>();
	int childList = 0;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("选择地区");
		requestTitle();
	}

	void requestTitle() {
		loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						counts = new int[array.length()];
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								String name = categoryObject.optString("Name", "");
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);
								CategoryList list = new CategoryList();
								list.father = entity;
								lists.add(list);

								// requestContents(lists.get(i), i);
							} catch (JSONException e) {
								e.printStackTrace();
							}
						}
						requestContents();

					}
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(SearchActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).addParam("id", "0")
				.notifyRequest();
	}

	void requestContents() {
		for (int i = 0; i < lists.size(); i++) {
			requestContent(lists.get(i), i);
		}
	}

	void requestContent(final CategoryList list, final int index) {
		String id = list.father.id + "";
		Log.i(TAG, "fatherId = " + id);

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						List<CategoryEntity> entities = new ArrayList<CategoryEntity>();
						counts[index] = array.length();
						list.childcount = array.length();
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								Log.i(TAG, categoryObject.toString());
								String name = categoryObject.optString("Name", "");
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);
								entities.add(entity);
								lists.get(index).children = entities;
							} catch (JSONException e) {
								e.printStackTrace();
							}
						}
						childList++;
						if (childList == lists.size()) {
							for (CategoryList list : lists)
								Log.i(TAG, list.toString());
							initList();
						}

					}
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(SearchActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).addParam("id", id)
				.notifyRequest();

	}

	void initList() {

		loading.setVisibility(View.GONE);
		for (int i = 0; i < lists.size(); i++) {
			Log.i(TAG, "i" + i);
			CategoryList list = lists.get(i);
			final TextView tv = new TextView(SearchActivity.this);
			tv.setText(list.father.name);
			tv.setTextColor(colors[i % colors.length]);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setPadding(10, 10, 10, 10);
			tv.setTextSize(20);
			tv.setClickable(true);

			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
			params.setMargins(0, 0, 0, 1);

			ll_areas.addView(tv, params);
			LinearLayout areas = new LinearLayout(SearchActivity.this);
			areas.setOrientation(LinearLayout.VERTICAL);
			areas.setVisibility(View.GONE);
			ll_areas.addView(areas);
			tv.setTag(areas);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {

					LinearLayout areas = (LinearLayout) v.getTag();
					ExpandAnimation animation = new ExpandAnimation(areas, 300);
					areas.startAnimation(animation);
					if (ll_areas.getTag() != null) {
						LinearLayout tag_areas = (LinearLayout) ll_areas.getTag();
						ExpandAnimation ea = new ExpandAnimation(tag_areas, 300);
						boolean toggle = ea.toggle();
						if (!toggle)
							tag_areas.startAnimation(ea);
					}
					Log.i(TAG, "animation:toggle = " + animation.toggle());
					if (animation.toggle())
						ll_areas.setTag(areas);
					else
						ll_areas.setTag(null);
				}
			});
			for (int j = 0; j < list.children.size(); j++) {
				CategoryEntity entity = list.children.get(j);
				TextView childTv = new TextView(SearchActivity.this);
				childTv.setText(entity.name);
				childTv.setBackgroundColor(Color.GRAY);
				childTv.setPadding(10, 10, 10, 10);
				childTv.setTextSize(16);
				areas.addView(childTv);
				childTv.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						TextView childTextView = (TextView) v;
						String text = childTextView.getText().toString().trim();
						Toast.makeText(SearchActivity.this, text, Toast.LENGTH_SHORT).show();
					}
				});
			}
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
