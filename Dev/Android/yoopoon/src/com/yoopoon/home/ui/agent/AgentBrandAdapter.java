package com.yoopoon.home.ui.agent;

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
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.house.ui.broker.BrokerTakeGuestActivity_;

public class AgentBrandAdapter extends BaseAdapter {
	Context mContext;
	ArrayList<JSONObject> datas;
	int height = 0;

	public AgentBrandAdapter(Context context) {
		this.mContext = context;
		datas = new ArrayList<JSONObject>();
		height = MyApplication.getInstance().getDeviceInfo((Activity) mContext).heightPixels / 6;
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return datas.size();
	}

	@Override
	public Object getItem(int position) {
		// TODO Auto-generated method stub
		return datas.get(position);
	}

	@Override
	public long getItemId(int position) {
		// TODO Auto-generated method stub
		return position;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		Holder mHolder;
		if (convertView == null) {
			convertView = LayoutInflater.from(mContext).inflate(R.layout.agent_page_brand_item, null);
			mHolder = new Holder();
			mHolder.init(convertView);
			convertView.setTag(mHolder);
		} else {
			mHolder = (Holder) convertView.getTag();
		}
		// "BrandId":"134",
		// "BrandName":"华润中央公园",
		// "Bimg":"20150701/20150701_161420_163_911.jpg",
		// "ProductId":33,
		// "Productname":"公园城11",
		// "HouseType":"",
		// "Price":"5000.00",
		// "Commition":"1500.00",
		// "SubTitle":"一万抵三万"

		final JSONObject item = datas.get(position);
		String url = mContext.getString(R.string.url_host_img) + item.optString("Bimg");
		mHolder.img.setLayoutParams(new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MATCH_PARENT, height));
		mHolder.img.setTag(url);
		ImageLoader.getInstance().displayImage(url, mHolder.img, MyApplication.getOptions(),
				MyApplication.getLoadingListener());
		mHolder.title.setText("[" + item.optString("BrandName") + "]");
		String price = item.optString("Price");
		String commition = item.optString("Commition");
		mHolder.tv_price.setText("价格：" + price);
		mHolder.tv_commition.setText("最高佣金" + commition + "元/套");

		mHolder.tv_iguest.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				BrokerTakeGuestActivity_.intent(mContext).intent_properString(item.optString("Productname"))
						.intent_propretyTypeString(item.optString("HouseType"))
						.intent_propretyNumber(item.optString("ProductId")).start();

			}
		});

		mHolder.tv_irecommend.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				BrokerTakeGuestActivity_.intent(mContext).intent_properString(item.optString("Productname"))
						.intent_propretyTypeString(item.optString("HouseType"))
						.intent_propretyNumber(item.optString("ProductId")).start();

			}
		});

		return convertView;
	}

	class Holder {
		ImageView img;
		TextView title;
		TextView tv_price;
		TextView tv_commition;
		TextView tv_iguest;
		TextView tv_irecommend;

		void init(View root) {
			img = (ImageView) root.findViewById(R.id.img);
			title = (TextView) root.findViewById(R.id.title);
			tv_price = (TextView) root.findViewById(R.id.tv_agent_brand_price);
			tv_commition = (TextView) root.findViewById(R.id.tv_agent_brand_commition);
			tv_iguest = (TextView) root.findViewById(R.id.tv_agent_brand_iguest);
			tv_irecommend = (TextView) root.findViewById(R.id.tv_agent_brand_irecommend);
		}

	}

	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		datas.addAll(mJsonObjects);
		this.notifyDataSetChanged();
	}

}
