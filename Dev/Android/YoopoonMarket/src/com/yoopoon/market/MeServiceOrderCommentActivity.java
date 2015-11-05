package com.yoopoon.market;

import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;
import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.graphics.Paint;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.ProductEntity;
import com.yoopoon.market.domain.ServiceOrderDetail;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;

@EActivity(R.layout.activity_me_order_comment)
public class MeServiceOrderCommentActivity extends MainActionBarActivity {
	private static final String TAG = "MeOrderCommentActivity";
	@ViewById(R.id.tv_price_previous)
	TextView tv_price_previous;
	@ViewById(R.id.lv)
	ListView lv;
	@Extra
	List<ServiceOrderDetail> details;

	void comment(ProductEntity product) {
		String comment = et_comment.getText().toString();
		if (TextUtils.isEmpty(comment)) {
			Toast.makeText(this, "亲，你还没发表任何评论呢！", Toast.LENGTH_SHORT).show();
			return;
		}
		String json = "{\"ProductId\":" + product.Id + ",\"Content\":\"" + comment + "\",\"Stars\":4}";
		requestComment(json);
	}

	void requestComment(String json) {
		Log.i(TAG, json);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status) {
						dialog.dismiss();
					}
				}
				Toast.makeText(MeServiceOrderCommentActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_comment_post)).setRequestMethod(RequestMethod.ePost).SetJSON(json)
				.notifyRequest();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("评论");
		lv.setAdapter(new MyListViewAdapter());
		lv.setOnItemClickListener(new MyListViewItemClickListener());
	}

	AlertDialog dialog;

	class MyListViewItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			Builder builder = new Builder(MeServiceOrderCommentActivity.this);
			final ProductEntity product = details.get(position).Product;
			View v = View.inflate(MeServiceOrderCommentActivity.this, R.layout.item_comment, null);
			builder.setView(v);
			tv_name = (TextView) v.findViewById(R.id.tv_name);
			imageView1 = (ImageView) v.findViewById(R.id.imageView1);
			tv_comment = (TextView) v.findViewById(R.id.tv_comment);
			et_comment = (EditText) v.findViewById(R.id.et);
			setDatas();
			dialog = builder.show();
			tv_comment.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					comment(product);
				}
			});
		}
	}
	TextView tv_name;
	ImageView imageView1;
	TextView tv_comment;
	EditText et_comment;

	void setDatas() {
		tv_name.setText(User.getUserName(this));
		String imageUrl = User.getProperty(this, "ImageUrl");
		imageView1.setTag(imageUrl);
		ImageLoader.getInstance().displayImage(imageUrl, imageView1, MyApplication.getOptions(),
				MyApplication.getLoadingListener());
	}

	class ViewHolder {
		TextView tv_name;
		TextView tv_category;
		TextView tv_price_counted;
		TextView tv_price_previous;
		TextView tv_count;
		ImageView iv;
	}

	class MyListViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return details.size();
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
			if (convertView == null) {
				convertView = View.inflate(MeServiceOrderCommentActivity.this, R.layout.item_product, null);
			}
			ViewHolder holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_name);
				holder.tv_category = (TextView) convertView.findViewById(R.id.tv_category);
				holder.tv_price_counted = (TextView) convertView.findViewById(R.id.tv_price_counted);
				holder.tv_price_previous = (TextView) convertView.findViewById(R.id.tv_price_previous);
				holder.tv_count = (TextView) convertView.findViewById(R.id.tv_count);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv);
				convertView.setTag(holder);
			}
			ServiceOrderDetail detail = details.get(position);
			ProductEntity product = detail.Product;
			holder.tv_category.setText(detail.ProductName);
			holder.tv_name.setText(product.Subtitte);
			holder.tv_price_counted.setText("￥" + product.Price);
			holder.tv_price_previous.setText("￥" + product.NewPrice);
			holder.tv_count.setText("x" + detail.Count);
			String imageUrl = getString(R.string.url_image) + product.MainImg;
			holder.iv.setTag(imageUrl);
			ImageLoader.getInstance().displayImage(imageUrl, holder.iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
			holder.tv_price_previous.getPaint().setFlags(Paint.STRIKE_THRU_TEXT_FLAG);

			return convertView;
		}

	}

	@Override
	public void backButtonClick(View v) {
		setResult(RESULT_OK);
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

	@Override
	protected void onStop() {
		super.onStop();
		if (dialog != null) {
			dialog.dismiss();
			dialog = null;
		}
	}

}
