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
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.CommunityAreaEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

@EActivity(R.layout.activity_area)
public class AreaActivity extends MainActionBarActivity {
	private static final String TAG = "AreaActivity";
	@ViewById(R.id.lv)
	ListView lv;
	List<CommunityAreaEntity> areas = new ArrayList<CommunityAreaEntity>();
	MyListViewAdapter adapter;
	boolean father = true;
	int fatherid = 0;
	String[] areaNames;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		backButton.setText("选择省份");
		requestAreas();
	}

	void requestAreas() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					areaNames = new String[array.length()];
					for (int i = 0; i < array.length(); i++) {
						try {
							JSONObject areaObject = array.getJSONObject(i);
							areaNames[i] = areaObject.optString("Name", "");
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}
					}
					parseToEntityList(array);
				} else {
					Toast.makeText(AreaActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_area_get)).addParam("father", String.valueOf(father))
				.addParam("fatherid", String.valueOf(fatherid)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	void parseToEntityList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				areas.clear();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject object = array.getJSONObject(i);
						CommunityAreaEntity entity = om.readValue(object.toString(), CommunityAreaEntity.class);
						areas.add(entity);
					} catch (JsonParseException e) {
						e.printStackTrace();
					} catch (JsonMappingException e) {
						e.printStackTrace();
					} catch (IOException e) {
						e.printStackTrace();
					} catch (JSONException e) {
						e.printStackTrace();
					}
				}
				return areas;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					Log.i(TAG, parseResult.toString());
					fillData();
				}
			}
		}).execute();
	}

	void fillData() {
		if (adapter == null) {
			father = false;
			adapter = new MyListViewAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return areaNames.length;
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
		public View getView(final int position, View convertView, ViewGroup parent) {
			TextView tv = new TextView(AreaActivity.this);
			tv.setPadding(10, 10, 10, 10);
			// final CommunityAreaEntity entity = areas.get(position);
			tv.setText(areaNames[position]);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setClickable(true);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					if (areas.size() != 0) {
						CommunityAreaEntity entity = areas.get(position);
						fatherid = entity.Id;

					}
					requestAreas();
				}
			});
			return tv;
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
