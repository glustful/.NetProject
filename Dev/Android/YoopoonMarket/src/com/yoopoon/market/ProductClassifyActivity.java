package com.yoopoon.market;

import java.util.ArrayList;
import java.util.Comparator;
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
import android.text.TextPaint;
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
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.FixGridLayout;

/**
 * @ClassName: ProductClassifyActivity
 * @Description: 产品分类activity
 * @author: 徐阳会
 * @date: 2015年9月16日 上午9:09:25
 */
@EActivity(R.layout.activity_category)
public class ProductClassifyActivity extends MainActionBarActivity {
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	private static final String TAG = "ProductClassifyActivity";
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	@ViewById(R.id.ll_loading)
	LinearLayout ll_loading;
	List<CategoryEntity> categoryList = new ArrayList<CategoryEntity>();
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };
	List<Integer> counts = new ArrayList<Integer>();
	int fatherId = 0;

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
					Intent intent = new Intent(ProductClassifyActivity.this, ProductList_.class);
					Bundle bundle = new Bundle();
					bundle.putString("productClassification", "曲靖特产");
					intent.putExtras(bundle);
					startActivity(intent);
					return true;
				}
				return false;
			}
		});
		requestData();
	}

	void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray categoryArray = object.optJSONArray("Object");
					for (int i = 0; i < categoryArray.length(); i++) {
						try {
							JSONObject categoryObject = categoryArray.getJSONObject(i);
							Log.i(TAG, categoryObject.toString());
							int id = categoryObject.optInt("Id", 0);
							int sort = categoryObject.optInt("Sort", 0);
							String name = categoryObject.optString("Name", "");
							int fatherid = categoryObject.optInt("FatherId", 0);
							StringBuilder builder = new StringBuilder(name);
							categoryList.add(new CategoryEntity(id, builder.toString(), sort, fatherid));
						} catch (JSONException e) {
							e.printStackTrace();
							ll_loading.setVisibility(View.GONE);
						}
					}
					initList();

				} else {
					Toast.makeText(ProductClassifyActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
					ll_loading.setVisibility(View.GONE);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_category_get)).addParam("id", "1").setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
	}

	void calcCount() {
		int sort = categoryList.get(0).sort;
		int count = 0;
		for (CategoryEntity entity : categoryList) {
			Log.i(TAG, entity.toString());
			if (entity.sort != sort) {
				sort = entity.sort;
				counts.add(count);
				count = 0;
			}
			count++;
		}
		for (Integer i : counts) {
			i = i / 4 + 1;
		}
	}

	void initList() {
		WindowManager wm = (WindowManager) getSystemService(WINDOW_SERVICE);
		int width = wm.getDefaultDisplay().getWidth() / 5;
		for (CategoryEntity entity : categoryList)
			Log.i(TAG, entity.toString());
		// Collections.sort(categoryList, comparator);
		for (CategoryEntity entity : categoryList)
			Log.i(TAG, entity.toString());
		calcCount();
		int sort = -1;
		int count = -1;
		int index = -1;
		for (int i = 0; i < categoryList.size(); i++) {
			CategoryEntity entity = categoryList.get(i);
			Log.i(TAG, entity.toString());
			if (sort != entity.sort) {
				index++;
				sort = entity.sort;
				TextView tv = new TextView(ProductClassifyActivity.this);
				tv.setText(entity.name);
				tv.setPadding(0, 10, 0, 0);
				tv.setTextColor(colors[sort % 4]);
				TextPaint paint = tv.getPaint();
				paint.setFakeBoldText(true);
				ll_category.addView(tv);
				FixGridLayout ll = new FixGridLayout(ProductClassifyActivity.this);
				ll.setmCellWidth(width);
				int px = Utils.dp2px(ProductClassifyActivity.this, 20);
				ll.setmCellHeight(px);
				int columns = ((counts.get(index) % 4) == 0) ? counts.get(index) / 4 : counts.get(index) / 4 + 1;
				int px2 = Utils.dp2px(ProductClassifyActivity.this, 10);
				ll_category.addView(ll, new LayoutParams(android.view.ViewGroup.LayoutParams.MATCH_PARENT, px * columns
						+ px2));
				if (sort >= 0) {
					View v = new View(ProductClassifyActivity.this);
					android.view.ViewGroup.LayoutParams params = new android.view.ViewGroup.LayoutParams(
							LayoutParams.MATCH_PARENT, 1);
					v.setLayoutParams(params);
					v.setBackgroundResource(R.drawable.line);
					ll_category.addView(v);
					count++;
				}
				count += 2;
				continue;
			}
			TextView tv = new TextView(ProductClassifyActivity.this);
			LayoutParams params = new LayoutParams(200, LayoutParams.WRAP_CONTENT);;
			params.setMargins(5, 10, 10, 5);
			tv.setLayoutParams(params);
			tv.setTextSize(14);
			tv.setLines(1);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setClickable(true);
			tv.setTextColor(Color.GRAY);
			tv.setText(entity.name);
			tv.setTag(entity.id);
			FixGridLayout ll = (FixGridLayout) ll_category.getChildAt(count - 1);
			ll.addView(tv, params);
			tv.setOnClickListener(new OnClickListener() {
				@Override
				public void onClick(View v) {
					TextView tv = (TextView) v;
					String text = tv.getText().toString().trim();
					// Toast.makeText(ProductClassifyActivity.this, text,
					// Toast.LENGTH_SHORT).show();*/
					Bundle bundle = new Bundle();
					bundle.putString("classificationName", text);
					bundle.putString("classificationId", tv.getTag().toString());
					Intent intent = new Intent(ProductClassifyActivity.this, ProductList_.class);
					intent.putExtras(bundle);
					startActivity(intent);
				}
			});
		}
		ll_category.removeViewAt(ll_category.getChildCount() - 1);
		ll_loading.setVisibility(View.GONE);
	}

	Comparator<CategoryEntity> comparator = new Comparator<CategoryEntity>() {
		@Override
		public int compare(CategoryEntity lhs, CategoryEntity rhs) {
			if (lhs.sort > rhs.sort)
				return 1;
			return (lhs.sort < rhs.sort) ? -1 : 0;
		}
	};

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
