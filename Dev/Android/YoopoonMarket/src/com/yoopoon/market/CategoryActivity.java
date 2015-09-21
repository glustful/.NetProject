package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;
import com.yoopoon.market.domain.CategoryEntity;
import com.yoopoon.market.domain.CategoryList;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.FixGridLayout;

@EActivity(R.layout.activity_category)
public class CategoryActivity extends MainActionBarActivity {
	private static final String TAG = "CategoryActivity";
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	@ViewById(R.id.ll_loading)
	View loading;
	List<CategoryList> lists = new ArrayList<CategoryList>();
	int childList = 0;

	int[] counts;
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };

	@AfterViews
	void initProductClassification() {
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
					Intent intent = new Intent(CategoryActivity.this, ProductList_.class);
					Bundle bundle = new Bundle();
					bundle.putString("productClassification", "曲靖特产");
					intent.putExtras(bundle);
					startActivity(intent);
					return true;
				}
				return false;
			}
		});
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
					Toast.makeText(CategoryActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).notifyRequest();
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
					Toast.makeText(CategoryActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
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
		WindowManager wm = (WindowManager) getSystemService(WINDOW_SERVICE);
		int width = Utils.dp2px(CategoryActivity.this, wm.getDefaultDisplay().getWidth() / 4);

		int height = Utils.dp2px(CategoryActivity.this, 40);
		// int px2 = Utils.px2dp(context, px)

		loading.setVisibility(View.GONE);
		for (int i = 0; i < lists.size(); i++) {
			Log.i(TAG, "i" + i);
			CategoryList list = lists.get(i);
			final TextView tv = new TextView(CategoryActivity.this);
			tv.setText(list.father.name);
			tv.setTextColor(colors[i % colors.length]);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setPadding(0, 10, 0, 0);
			tv.setTextSize(20);
			tv.setClickable(true);

			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
			params.setMargins(0, 0, 0, 1);

			ll_category.addView(tv, params);
			FixGridLayout ll = new FixGridLayout(CategoryActivity.this);
			ll.setmCellWidth(width);
			ll.setmCellHeight(height);
			int columns = (list.childcount % 4 == 0) ? list.childcount / 4 : list.childcount / 4 + 1;
			android.view.ViewGroup.LayoutParams ll_params = new android.view.ViewGroup.LayoutParams(-1, height
					* columns);
			ll_category.addView(ll, ll_params);
			tv.setTag(ll);
			View v = new View(CategoryActivity.this);
			v.setBackgroundResource(R.drawable.line);
			ll_category.addView(v);
			for (int j = 0; j < list.children.size(); j++) {
				final CategoryEntity entity = list.children.get(j);
				TextView childTv = new TextView(CategoryActivity.this);
				childTv.setText(entity.name + "                           ");
				childTv.setBackgroundResource(R.drawable.white_bg);
				childTv.setPadding(10, 10, 10, 10);
				childTv.setTextSize(16);
				ll.addView(childTv);
				childTv.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						TextView childTextView = (TextView) v;
						String text = childTextView.getText().toString().trim();
						Toast.makeText(CategoryActivity.this, text, Toast.LENGTH_SHORT).show();
						Intent intent = new Intent(CategoryActivity.this, ProductList.class);
						Bundle bundle = new Bundle();
						bundle.putString("classificationId", entity.id + "");
						bundle.putString("classificationName", entity.name);

					}
				});
			}
		}
		ll_category.removeViewAt(ll_category.getChildCount() - 1);
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
