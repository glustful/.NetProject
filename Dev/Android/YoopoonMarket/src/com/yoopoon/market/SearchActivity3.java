/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: SearchActivity3.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-21 上午9:29:28 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.content.Intent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.domain.SimpleAreaEntity;

@EActivity(R.layout.activity_area3)
public class SearchActivity3 extends MainActionBarActivity {
	private static final String TAG = "SearchActivity3";
	@ViewById(R.id.lv)
	ListView lv;
	@Extra
	List<SimpleAreaEntity> areas;
	@Extra
	int which = 0;
	@Extra
	String[] name;
	@Extra
	MemberAddressEntity addressEntity;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.GONE);
		rightButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.GONE);
		backButton.setText("选择市区");
		lv.setAdapter(new MyListViewAdapter());
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return areas.size();
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
			final SimpleAreaEntity area = areas.get(position);
			TextView tv = new TextView(SearchActivity3.this);
			tv.setText(area.Name);
			tv.setPadding(10, 10, 10, 10);
			tv.setBackgroundResource(R.drawable.white_bg);
			tv.setClickable(true);
			tv.setTag(area.FatherId);
			tv.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					Intent intent = new Intent("com.yoopoon.market.address");
					intent.addCategory(Intent.CATEGORY_DEFAULT);
					intent.putExtra("AreaId", String.valueOf(v.getTag()));
					name[2] = area.Name;
					intent.putExtra("Name", name);
					SearchActivity3.this.sendBroadcast(intent);

					switch (which) {
						case 0:
							MainActivity_.intent(SearchActivity3.this).start();
							break;
						case 1:
							NewAddressActivity_.intent(SearchActivity3.this).start();
							break;
						case 2:
							AddressModifyActivity_.intent(SearchActivity3.this).addressEntity(addressEntity).start();
							break;
						default:
							break;
					}

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
