package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.market.anim.ExpandAnimation;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.SimpleAreaEntity;
import com.yoopoon.market.domain.SimpleAreaList;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.Utils;

@EActivity(R.layout.activity_select)
public class SearchActivity extends MainActionBarActivity {
	private static final String TAG = "SearchActivity";
	@ViewById(R.id.ll_areas)
	LinearLayout ll_areas;
	@ViewById(R.id.ll_loading)
	View loading;
	@Extra
	int which = 0;
	@Extra
	MemberAddressEntity addressEntity;
	int[] counts;
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };
	List<SimpleAreaList> lists = new ArrayList<SimpleAreaList>();
	int childList = 0;
	String[] name = new String[3];

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
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					counts = new int[array.length()];
					for (int i = 0; i < array.length(); i++) {
						try {
							JSONObject categoryObject = array.getJSONObject(i);
							String name = categoryObject.optString("Name", "");
							int id = categoryObject.optInt("Id", 0);
							SimpleAreaEntity entity = new SimpleAreaEntity();
							entity.FatherId = id;
							entity.Name = name;
							SimpleAreaList list = new SimpleAreaList();
							list.father = entity;
							lists.add(list);

						} catch (JSONException e) {
							e.printStackTrace();
						}
					}
					requestContents();

				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(SearchActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_area_get)).setRequestMethod(RequestMethod.eGet).addParam("father", "true")
				.notifyRequest();
	}

	void requestContents() {
		for (int i = 0; i < lists.size(); i++) {
			requestContent(lists.get(i), i);
		}
	}

	void requestContent(final SimpleAreaList list, final int index) {
		String id = list.father.FatherId + "";
		Log.i(TAG, "fatherId = " + id);

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					List<SimpleAreaEntity> entities = new ArrayList<SimpleAreaEntity>();
					counts[index] = array.length();
					list.childCount = array.length();
					for (int i = 0; i < array.length(); i++) {
						try {
							JSONObject categoryObject = array.getJSONObject(i);
							String name = categoryObject.optString("Name", "");
							int id = categoryObject.optInt("Id", 0);
							SimpleAreaEntity entity = new SimpleAreaEntity();
							entity.FatherId = id;
							entity.Name = name;
							entities.add(entity);
							lists.get(index).children = entities;
						} catch (JSONException e) {
							e.printStackTrace();
						}
					}
					childList++;
					if (childList == lists.size()) {
						// for (SimpleAreaList list : lists)
						// Log.i(TAG, list.toString());
						initList();

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
		}.setUrl(getString(R.string.url_area_get)).setRequestMethod(RequestMethod.eGet).addParam("father", "false")
				.addParam("fatherid", id).notifyRequest();

	}

	void initList() {
		int dp10 = Utils.dp2px(this, 10);
		int dp1 = Utils.dp2px(this, 1);
		int dp30 = Utils.dp2px(this, 30);

		loading.setVisibility(View.GONE);
		for (int i = 0; i < lists.size(); i++) {
			// Log.i(TAG, "i" + i);
			SimpleAreaList list = lists.get(i);
			final TextView tv = new TextView(SearchActivity.this);
			tv.setText(list.father.Name);
			tv.setTextColor(colors[i % colors.length]);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setPadding(dp10, dp10, dp10, dp10);
			tv.setTextSize(20);
			tv.setClickable(true);

			Drawable drawable = getResources().getDrawable(R.drawable.right_next_icon);
			int drawableDp = Utils.dp2px(SearchActivity.this, 10);
			drawable.setBounds(1, 1, drawableDp, drawableDp);
			tv.setCompoundDrawables(null, null, drawable, null);

			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
			params.setMargins(0, 0, 0, dp1);

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
					if (animation.toggle()) {
						ll_areas.setTag(areas);
						name[0] = tv.getText().toString().trim();
					} else
						ll_areas.setTag(null);
				}
			});
			for (int j = 0; j < list.children.size(); j++) {
				final SimpleAreaEntity entity = list.children.get(j);
				TextView childTv = new TextView(SearchActivity.this);
				childTv.setText(entity.Name);
				childTv.setBackgroundResource(R.drawable.white_bg);
				childTv.setPadding(dp30, dp10, dp10, dp10);
				childTv.setTextSize(18);
				LayoutParams childParams = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
				childParams.setMargins(0, 0, 0, dp1);
				areas.addView(childTv, childParams);
				childTv.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						String fatherid = String.valueOf(entity.FatherId);
						name[1] = entity.Name;
						requestData(fatherid);
					}
				});
			}
		}
	}

	void requestData(final String fatherid) {
		loading.setVisibility(View.VISIBLE);
		final List<SimpleAreaEntity> areas = new ArrayList<SimpleAreaEntity>();
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				loading.setVisibility(View.GONE);
				Log.i(TAG, data.toString());
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					if (array.length() > 0) {
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								String name = categoryObject.optString("Name", "");
								int id = categoryObject.optInt("Id", 0);
								SimpleAreaEntity entity = new SimpleAreaEntity();
								entity.FatherId = id;
								entity.Name = name;
								areas.add(entity);
							} catch (JSONException e) {
								e.printStackTrace();
							}
						}
						SearchActivity3_.intent(SearchActivity.this).areas(areas).which(which).name(name)
								.addressEntity(addressEntity).start();
					} else {
						// 地区选择完毕，返回
						Intent intent = new Intent("com.yoopoon.market.address");
						intent.addCategory(Intent.CATEGORY_DEFAULT);
						intent.putExtra("AreaId", fatherid);
						intent.putExtra("Name", name);
						SearchActivity.this.sendBroadcast(intent);
						finish();
					}
				} else {
					Toast.makeText(SearchActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_area_get)).setRequestMethod(RequestMethod.eGet).addParam("father", "false")
				.addParam("fatherid", fatherid).notifyRequest();
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
