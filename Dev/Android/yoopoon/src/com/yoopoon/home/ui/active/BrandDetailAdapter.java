package com.yoopoon.home.ui.active;

import java.util.ArrayList;

import org.androidannotations.annotations.Background;
import org.androidannotations.annotations.EBean;
import org.json.JSONArray;
import org.json.JSONObject;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.etsy.android.grid.util.DynamicHeightImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.product.ProductDetailActivity_;

@EBean
public class BrandDetailAdapter extends BaseAdapter {
	private JSONArray data;// 填充数据的集合
	Context mContext;
	ArrayList<String> names;
	
	public BrandDetailAdapter(Context mContext) {
		super();
		this.mContext = mContext;
		names = new ArrayList<String>();
	}
	public void setData(JSONArray data) {
		this.data = data;
		this.notifyDataSetChanged();
		initImgUrl();
	}
	@Background
	void initImgUrl() {
		if (data == null)
			return;
		names.clear();
		for (int i = 0; i < data.length(); i++) {
			names.add(data.optJSONObject(i).optString("Productimg"));
		}
	}
	
	// 创建一个viewholder类
	public static class ViewHolder extends RecyclerView.ViewHolder {
		DynamicHeightImageView img;
		TextView content;
		TextView price;
		TextView title;
		TextView callPhone;
		
		public ViewHolder(View itemView) {
			super(itemView);
			img = (DynamicHeightImageView) itemView.findViewById(R.id.img);
			title = (TextView) itemView.findViewById(R.id.title);
			content = (TextView) itemView.findViewById(R.id.content);
			price = (TextView) itemView.findViewById(R.id.price);
			callPhone = (TextView) itemView.findViewById(R.id.callPhone);
		}
	}
	
	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return data == null ? 0 : data.length();
	}
	@Override
	public Object getItem(int position) {
		// TODO Auto-generated method stub
		return data.opt(position);
	}
	@Override
	public long getItemId(int position) {
		// TODO Auto-generated method stub
		return position;
	}
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHolder viewHolder = null;
		if (convertView == null) {
			convertView = LayoutInflater.from(mContext).inflate(R.layout.brand_detail_item, parent, false);
			viewHolder = new ViewHolder(convertView);
			convertView.setTag(viewHolder);
		} else {
			viewHolder = (ViewHolder) convertView.getTag();
		}
		final JSONObject item = data.optJSONObject(position);
		viewHolder.title.setText(Tools.optString(item, "Productname", ""));
		viewHolder.title.setTextSize(15);
		viewHolder.content.setText(Tools.optString(item, "SubTitle", ""));
		viewHolder.price.setText("均价" + Tools.optDouble(item, "Price", 0) + "元/m²起");
		viewHolder.callPhone.setTag(Tools.optString(item, "Phone", ""));
		viewHolder.callPhone.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View view) {
				String phone = view.getTag() != null ? view.getTag().toString().trim() : null;
				if (phone == null || phone.equals("")) {
					ToastUtils.showToast(mContext, "手机号码为空，联系管理员", 3000);
					return;
				}
				Tools.callPhone(mContext, phone);
			}
		});
		String photo = item.optString("Productimg", "");
		if (!StringUtils.isEmpty(photo)) {
			String url = mContext.getString(R.string.url_host_img) + item.optString("Productimg");
			viewHolder.img.setTag(url);
			viewHolder.img.setHeightRatio(1);
			ImageLoader.getInstance().displayImage(url, viewHolder.img, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
		}
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				ProductDetailActivity_.intent(mContext).productId(item.optString("Id")).start();
			}
		});
		return convertView;
	}
}
