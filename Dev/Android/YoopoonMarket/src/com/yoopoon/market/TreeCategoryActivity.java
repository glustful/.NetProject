package com.yoopoon.market;

import java.io.IOException;
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
import android.view.inputmethod.EditorInfo;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.TreeCategory;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.view.LabelGroup;

@EActivity(R.layout.activity_category)
public class TreeCategoryActivity extends MainActionBarActivity {
	private static final String TAG = "TreeCategoryActivity";
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	List<TreeCategory> lists = new ArrayList<TreeCategory>();
	int[] colors = { Color.rgb(236, 109, 23), Color.rgb(40, 174, 62), Color.rgb(39, 127, 194), Color.rgb(175, 97, 163) };

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.GONE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("选择分类");
		// 添加用户输入内容后输入法可以使用键盘上的搜索框搜索
		searchProductEditText.setImeActionLabel("搜索", EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setSingleLine();
		searchProductEditText.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
		searchProductEditText.setOnEditorActionListener(new OnEditorActionListener() {
			@Override
			public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
				if (actionId == EditorInfo.IME_ACTION_SEARCH) {
					Intent intent = new Intent(TreeCategoryActivity.this, ProductKeywordList_.class);
					Bundle bundle = new Bundle();
					bundle.putString("keyword", v.getText().toString());
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
					Toast.makeText(TreeCategoryActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
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
					Log.i(TAG, parseResult.toString());
					initList();
				}
			}
		}).execute();
	}

	void initList() {
		for (int i = 0; i < lists.size(); i++) {
			Log.i(TAG, "i" + i);
			TreeCategory list = lists.get(i);
			final TextView tv = new TextView(TreeCategoryActivity.this);
			tv.setText(list.label);
			tv.setTextColor(colors[i % colors.length]);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setPadding(0, 10, 0, 0);
			tv.setTextSize(20);
			tv.setClickable(true);
			LayoutParams params = new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT);
			params.setMargins(0, 0, 0, 1);
			ll_category.addView(tv, params);
			LabelGroup ll = new LabelGroup(TreeCategoryActivity.this);
			android.view.ViewGroup.LayoutParams ll_params = new android.view.ViewGroup.LayoutParams(-1, -2);
			ll_category.addView(ll, ll_params);
			tv.setTag(ll);
			View v = new View(TreeCategoryActivity.this);
			v.setBackgroundResource(R.drawable.line);
			ll_category.addView(v);
			for (int j = 0; j < list.children.size(); j++) {
				final TreeCategory entity = list.children.get(j);
				TextView childTv = new TextView(TreeCategoryActivity.this);
				childTv.setText(entity.label);
				childTv.setBackgroundResource(R.drawable.white_bg);
				childTv.setPadding(10, 10, 10, 10);
				childTv.setTextSize(16);
				ll.addView(childTv);
				childTv.setOnClickListener(new OnClickListener() {

					@Override
					public void onClick(View v) {
						TextView childTextView = (TextView) v;
						String text = childTextView.getText().toString().trim();
						Intent intent = new Intent(TreeCategoryActivity.this, ProductClassificationList_.class);
						Bundle bundle = new Bundle();
						bundle.putString("classificationId", entity.Id + "");
						bundle.putString("classificationName", entity.label);
						intent.putExtras(bundle);
						startActivity(intent);
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
	}

	@Override
	public void rightButtonClick(View v) {
	}

	@Override
	public Boolean showHeadView() {
		return true;
	}
}
