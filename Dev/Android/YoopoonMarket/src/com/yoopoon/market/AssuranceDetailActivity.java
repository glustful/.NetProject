/**
 * 保险详情页面
 */
package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;
import com.yoopoon.market.view.MyGridView;

@EActivity(R.layout.activity_assurance_detail)
public class AssuranceDetailActivity extends MainActionBarActivity {
	@ViewById(R.id.gv)
	MyGridView gv;
	// 适合人群
	String[] texts = { "放松", "度假", "旅游", "学生" };

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("保险详情");
		gv.setAdapter(new MyGridViewAdapter());
	}

	static class ViewHolder {
		TextView tv;
		ImageView iv;
	}

	// GridView的Adapter
	private class MyGridViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return 4;
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
		public View getView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder = null;
			if (convertView == null)
				convertView = View.inflate(AssuranceDetailActivity.this, R.layout.item_assurance_detail, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv = (TextView) convertView.findViewById(R.id.tv);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				convertView.setTag(holder);
			}
			holder.tv.setText(texts[position]);

			return convertView;
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
