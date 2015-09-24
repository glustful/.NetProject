/**
 * 保险详情页面
 */
package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.content.Intent;
import android.graphics.Color;
import android.net.Uri;
import android.text.TextUtils;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.TextView;
import android.widget.Toast;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.view.MyGridView;

@EActivity(R.layout.activity_assurance_detail)
public class AssuranceDetailActivity extends MainActionBarActivity {
	@ViewById(R.id.gv)
	MyGridView gv;
	// 适合人群
	String[] texts = { "放松", "度假", "旅游", "学生" };
	@Extra
	ProductEntity product;
	@ViewById(R.id.tv_title)
	TextView tv_title;
	@ViewById(R.id.tv_phone)
	TextView tv_phone;
	@ViewById(R.id.iv)
	ImageView iv;
	String[] imgUrls = new String[5];

	@Click(R.id.tv_phone)
	void call() {
		String phoneNumber = (String) tv_phone.getTag();
		if (!TextUtils.isEmpty(phoneNumber)) {
			Intent intent = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + phoneNumber));
			this.startActivity(intent);
		} else {
			Toast.makeText(this, "暂时没有联系电话哦", Toast.LENGTH_SHORT).show();
		}
	}

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("详情");
		titleButton.setTextColor(Color.WHITE);
		headView.setBackgroundColor(Color.RED);
		gv.setAdapter(new MyGridViewAdapter());
		setDatas();
	}

	void setDatas() {
		String name = "【" + product.Name + "】";
		String subtitle = product.Subtitte;
		tv_title.setText(name + subtitle);
		tv_phone.setText("联系电话:" + product.Contactphone);
		tv_phone.setTag(product.Contactphone);
		String imageUrl = getString(R.string.url_image) + product.MainImg;
		iv.setTag(imageUrl);
		ImageLoader.getInstance().displayImage(imageUrl, iv, MyApplication.getOptions(),
				MyApplication.getLoadingListener());
		iv.setScaleType(ScaleType.FIT_XY);
		imgUrls[0] = getString(R.string.url_image) + product.Img;
		imgUrls[1] = getString(R.string.url_image) + product.Img1;
		imgUrls[2] = getString(R.string.url_image) + product.Img2;
		imgUrls[3] = getString(R.string.url_image) + product.Img3;
		imgUrls[4] = getString(R.string.url_image) + product.Img4;
	}

	static class ViewHolder {

		TextView tv;
		ImageView iv;
	}

	// GridView的Adapter
	private class MyGridViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return imgUrls.length;
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
			holder.iv.setTag(imgUrls[position]);
			ImageLoader.getInstance().displayImage(imgUrls[position], holder.iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			holder.tv.setText(texts[position % 4]);

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
