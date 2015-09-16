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
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;
import com.yoopoon.market.domain.CategoryEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;

@EActivity(R.layout.activity_category)
public class CategoryActivity extends MainActionBarActivity {
	private static final String TAG = "CategoryActivity";
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	List<CategoryEntity> titleEntity = new ArrayList<CategoryEntity>();
	List<CategoryEntity> contentEntity = new ArrayList<CategoryEntity>();

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
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								String name = categoryObject.optString("Name", "");
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);
								Log.i(TAG, entity.toString());
								titleEntity.add(entity);
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
					}
				} else {
					Toast.makeText(CategoryActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).addParam("id", "0")
				.notifyRequest();
	}

	void requestContents(String id) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								String name = categoryObject.optString("Name", "");
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);
								Log.i(TAG, entity.toString());
								contentEntity.add(entity);
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
						initList();
					}
				} else {
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
