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
import android.view.ViewGroup.LayoutParams;
import android.view.WindowManager;
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
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.FixGridLayout;

@EActivity(R.layout.activity_category)
public class CategoryActivity extends MainActionBarActivity {
	private static final String TAG = "CategoryActivity";
	@ViewById(R.id.et_search_product)
	EditText searchProductEditText;
	@ViewById(R.id.ll_category)
	LinearLayout ll_category;
	List<CategoryEntity> categoryList = new ArrayList<CategoryEntity>();
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
								String name = categoryObject.optString("Name", "")
										+ "                                        ";
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);

								new Thread(new MyRequestContentThread(entity, i, new Object(), new Object())).start();
								try {
									Thread.sleep(10);
								} catch (InterruptedException e) {
									// TODO Auto-generated catch block
									e.printStackTrace();
								}
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

	boolean start = true;

	class MyRequestContentThread implements Runnable {

		private CategoryEntity name;
		private Integer index;
		private Object prev;
		private Object self;

		private MyRequestContentThread(CategoryEntity name, Integer index, Object prev, Object self) {
			this.name = name;
			this.index = index;
			this.prev = prev;
			this.self = self;
		}

		public void run() {
			int count = 1;
			while (count > 0) {
				synchronized (prev) {
					synchronized (self) {
						requestContents(name, name.id + "", index);
						count--;
						try {
							Thread.sleep(10);
						} catch (InterruptedException e) {
							e.printStackTrace();
						}

						self.notify();
					}
					try {
						prev.wait();
					} catch (InterruptedException e) {
						e.printStackTrace();
					}
				}

			}
		}
	}

	void requestContents(final CategoryEntity titleEntity, String id, final int index) {

		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						JSONArray array = object.optJSONArray("Object");
						categoryList.add(titleEntity);
						counts[index] = array.length();
						for (int i = 0; i < array.length(); i++) {
							try {
								JSONObject categoryObject = array.getJSONObject(i);
								String name = categoryObject.optString("Name", "")
										+ "                                        ";
								int id = categoryObject.optInt("Id", 0);
								int sort = categoryObject.optInt("Sort", 0);
								int fatherId = categoryObject.optInt("FatherId", 0);
								CategoryEntity entity = new CategoryEntity(id, name, sort, fatherId);
								categoryList.add(entity);
							} catch (JSONException e) {
								// TODO Auto-generated catch block
								e.printStackTrace();
							}
						}
						start = true;
						if (index == counts.length - 1) {

							Log.i(TAG, "for循环完成啦！");
							for (CategoryEntity entity : categoryList)
								Log.i(TAG, entity.toString());
							for (int j = 0; j < counts.length; j++)
								Log.i(TAG, "count = " + counts[j]);
							initList();
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
		}.setUrl(getString(R.string.url_category_get)).setRequestMethod(RequestMethod.eGet).addParam("id", id)
				.notifyRequest();

	}

	void initList() {
		WindowManager wm = (WindowManager) getSystemService(WINDOW_SERVICE);
		int width = wm.getDefaultDisplay().getWidth() / 5;
		int kind = -1;
		boolean flag = true;
		int childCount = 0;
		for (int i = 0; i < categoryList.size(); i++) {
			CategoryEntity entity = categoryList.get(i);
			if (flag) {
				kind++;
				TextView tv = new TextView(CategoryActivity.this);
				tv.setTextColor(colors[kind % 4]);
				tv.getPaint().setFakeBoldText(true);
				tv.setPadding(0, 10, 0, 0);
				tv.setText(entity.name);
				ll_category.addView(tv);
				FixGridLayout ll = new FixGridLayout(CategoryActivity.this);
				int px = Utils.px2dp(CategoryActivity.this, 50);
				int px2 = Utils.px2dp(CategoryActivity.this, 10);
				ll.setmCellWidth(width);
				ll.setmCellHeight(px);
				int columns = ((counts[kind] % 4) == 0) ? counts[kind] / 4 : (counts[kind] / 4 + 1);

				ll_category.addView(ll, new LayoutParams(LayoutParams.MATCH_PARENT, (columns * px) + px2));
				View v = new View(CategoryActivity.this);
				v.setBackgroundResource(R.drawable.line);
				ll_category.addView(v);
				flag = false;
				childCount = 0;
				continue;
			}
			childCount++;
			FixGridLayout ll = (FixGridLayout) ll_category.getChildAt(ll_category.getChildCount() - 2);
			TextView tv = new TextView(CategoryActivity.this);
			tv.setText(entity.name);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setClickable(true);
			tv.setPadding(5, 5, 5, 5);
			ll.addView(tv);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					String text = ((TextView) v).getText().toString().trim();
					Toast.makeText(CategoryActivity.this, text, Toast.LENGTH_SHORT).show();
				}
			});

			if (childCount == counts[kind] || counts[kind] == 0) {
				flag = true;
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
