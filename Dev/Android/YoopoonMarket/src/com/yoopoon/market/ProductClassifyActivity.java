package com.yoopoon.market;

import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.TextPaint;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.market.domain.CategoryEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.view.FixGridLayout;

@EActivity(R.layout.activity_category)
public class ProductClassifyActivity extends MainActionBarActivity {
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	@ViewById(R.id.ll_loading)
	LinearLayout ll_loading;
	List<CategoryEntity> categoryList = new ArrayList<CategoryEntity>();
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };

	@AfterViews
	void initProductClassification() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("分类");
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
							int id = categoryObject.optInt("Id", 0);
							int sort = categoryObject.optInt("Sort", 0);
							String name = categoryObject.optString("Name", "");
							StringBuilder builder = new StringBuilder(name);
							for (int j = builder.length() - 1; j < 4; j++)
								builder.append("        ");
							categoryList.add(new CategoryEntity(id, builder.toString(), sort));
							builder.append(categoryObject.toString() + "\n");
						} catch (JSONException e) {
							// TODO Auto-generated catch block
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
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	void initList() {
		Collections.sort(categoryList, comparator);
		int sort = -1;
		int count = -1;
		for (int i = 0; i < categoryList.size(); i++) {
			CategoryEntity entity = categoryList.get(i);
			if (sort != entity.sort) {
				sort = entity.sort;
				TextView tv = new TextView(ProductClassifyActivity.this);
				tv.setText("Sort:" + sort);
				tv.setPadding(0, 10, 0, 0);
				tv.setTextColor(colors[sort % 4]);
				TextPaint paint = tv.getPaint();
				paint.setFakeBoldText(true);
				ll_category.addView(tv);
				FixGridLayout ll = new FixGridLayout(ProductClassifyActivity.this);
				ll.setmCellWidth(100);
				ll.setmCellHeight(30);
				ll_category.addView(ll);
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

			}
			TextView tv = new TextView(ProductClassifyActivity.this);
			LayoutParams params = new LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT);;
			params.setMargins(5, 10, 10, 5);
			tv.setLayoutParams(params);
			tv.setTextSize(16);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setClickable(true);
			tv.setTextColor(Color.GRAY);
			tv.setText(entity.name);
			FixGridLayout ll = (FixGridLayout) ll_category.getChildAt(count - 1);
			ll.addView(tv);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					TextView tv = (TextView) v;
					String text = tv.getText().toString().trim();
					Toast.makeText(ProductClassifyActivity.this, text, Toast.LENGTH_SHORT).show();
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
