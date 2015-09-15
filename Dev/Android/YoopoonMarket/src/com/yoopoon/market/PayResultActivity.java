package com.yoopoon.market;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.graphics.Color;
import android.graphics.Paint;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.style.ForegroundColorSpan;
import android.text.style.TextAppearanceSpan;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;

@EActivity(R.layout.pay_succuss)
public class PayResultActivity extends MainActionBarActivity {
	@ViewById(R.id.lv)
	ListView lv;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("支付结果");
		lv.setAdapter(new MyListViewAdapter());
	}

	static class ViewHolder {
		ImageView iv;
		TextView tv_name;
		TextView tv_category;
		TextView tv_price_counted;
		TextView tv_price_previous;
		TextView tv_count;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return 3;
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
			if (position == 2) {
				TextView tv = new TextView(PayResultActivity.this);
				LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(
						LinearLayout.LayoutParams.MATCH_PARENT, LinearLayout.LayoutParams.WRAP_CONTENT);
				tv.setBackgroundColor(Color.WHITE);
				tv.setPadding(10, 10, 10, 10);
				tv.setGravity(Gravity.RIGHT);
				tv.setText("总金额：￥139.8（已省180元）");
				changeTextColor(tv);
				// tv.setLayoutParams(params);
				return tv;
			}
			if (convertView == null || !(convertView instanceof RelativeLayout))
				convertView = View.inflate(PayResultActivity.this, R.layout.item_cart2, null);
			convertView.setBackgroundColor(Color.WHITE);
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				holder.tv_count = (TextView) convertView.findViewById(R.id.tv_count);
				convertView.setTag(holder);
			}
			holder.tv_price_previous.setVisibility(View.VISIBLE);
			holder.tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);
			return convertView;
		}
	}

	void changeTextColor(TextView textView) {
		String text = textView.getText().toString();
		SpannableStringBuilder builder = new SpannableStringBuilder(text);

		String[] former = text.split("：");
		String[] middle = former[1].split("（");

		// ForegroundColorSpan 为文字前景色，BackgroundColorSpan为文字背景色
		TextAppearanceSpan red_span = new TextAppearanceSpan(this, R.style.TextView_LargeSize_RedColor);
		ForegroundColorSpan blackSpan = new ForegroundColorSpan(Color.BLACK);

		ForegroundColorSpan graySpan = new ForegroundColorSpan(Color.GRAY);

		builder.setSpan(blackSpan, 0, former[0].length() + 1, Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		builder.setSpan(red_span, former[0].length() + 1, middle[0].length() + former[0].length() + 1,
				Spannable.SPAN_INCLUSIVE_INCLUSIVE);
		builder.setSpan(graySpan, middle[0].length() + former[0].length() + 1, text.length(),
				Spannable.SPAN_INCLUSIVE_INCLUSIVE);

		textView.setText(builder);
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
