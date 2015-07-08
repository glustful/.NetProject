package com.yoopoon.home.ui.home;

import java.util.ArrayList;

import org.json.JSONObject;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.TextView;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.product.ProductDetailActivity_;

/**
 * 房源中实体对应的Adapter
 * @author king
 */
public class FramHouseListViewAdapter extends BaseAdapter {
	Context mContext;
	ArrayList<JSONObject> datas;
	int height = 0;

	public FramHouseListViewAdapter(Context mContext) {
		this.mContext = mContext;
		datas = new ArrayList<JSONObject>();
		height = MyApplication.getInstance().getDeviceInfo((Activity) mContext).heightPixels / 6;
	}
	@Override
	public int getCount() {
		return datas.size();
	}
	@Override
	public Object getItem(int position) {
		return datas.get(position);
	}
	@Override
	public long getItemId(int position) {
		return position;
	}
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		ViewHandler viewHandler;
		if (convertView == null) {
			convertView = LayoutInflater.from(mContext).inflate(R.layout.home_fram_house_listview_item, null);
			LinearLayout listViewLinearLayout = (LinearLayout) convertView
					.findViewById(R.id.house_listview_item_linearlayout);
			LayoutParams layoutParams = (LayoutParams) listViewLinearLayout.getLayoutParams();
			int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
			layoutParams.height = screenWidth / 3;
			listViewLinearLayout.setLayoutParams(layoutParams);
			viewHandler = new ViewHandler();
			viewHandler.init(convertView);
			convertView.setTag(viewHandler);
		} else {
			viewHandler = (ViewHandler) convertView.getTag();
		}
		final JSONObject item = datas.get(position);
		String url = mContext.getString(R.string.url_host_img) + item.optString("Productimg");
		viewHandler.houseImageView.setLayoutParams(new LinearLayout.LayoutParams(
				LinearLayout.LayoutParams.MATCH_PARENT, height));
		viewHandler.houseImageView.setTag(url);
		ImageLoader.getInstance().displayImage(url, viewHandler.houseImageView, MyApplication.getOptions(),
				MyApplication.getLoadingListener());
		// viewHandler.houseImageView.
		viewHandler.houseProductnameTextView.setText(item.optString("Productname"));
		viewHandler.housePriceTextView.setText(item.optString("Price") + "元/m²");
		viewHandler.houseTypeAcreaqeStockRuleTextView.setText(item.optString("Type") + "/" + item.optString("Acreage")
				+ "m²" + "/" + "在售" + item.optString("StockRule") + "套");
		viewHandler.houseAdvertisementTextView.setText(item.optString("Advertisement"));
		// 添加点击事件
		convertView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				ProductDetailActivity_.intent(mContext).productId(item.optString("Id")).start();
			}
		});
		return convertView;
	}

	private class ViewHandler {
		private ImageView houseImageView;
		private TextView houseProductnameTextView;
		private TextView housePriceTextView;
		private TextView houseTypeAcreaqeStockRuleTextView;
		private TextView houseAdvertisementTextView;

		void init(View root) {
			houseImageView = (ImageView) root.findViewById(R.id.house_image);
			houseProductnameTextView = (TextView) root.findViewById(R.id.house_productname);
			housePriceTextView = (TextView) root.findViewById(R.id.house_price);
			houseTypeAcreaqeStockRuleTextView = (TextView) root.findViewById(R.id.tpye_area_stockRule);
			houseAdvertisementTextView = (TextView) root.findViewById(R.id.house_advertisement);
		}
	}

	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		datas.addAll(mJsonObjects);
		this.notifyDataSetChanged();
	}
}
