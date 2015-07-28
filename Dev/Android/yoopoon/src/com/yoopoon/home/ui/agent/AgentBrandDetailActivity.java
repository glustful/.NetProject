/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: AgentBrandDetailActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.agent 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-24 下午12:17:05 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.agent;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.LinearLayout;
import android.widget.TextView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.domain.Product;
import com.yoopoon.home.ui.product.ProductDetailActivity_;
import com.yoopoon.house.ui.broker.BrokerRecommendActivity_;
import com.yoopoon.house.ui.broker.BrokerTakeGuestActivity_;

/**
 * @ClassName: AgentBrandDetailActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-24 下午12:17:05
 */
@EActivity(R.layout.activity_agent_brand_detail)
public class AgentBrandDetailActivity extends MainActionBarActivity {
	private static final String TAG = "AgentBrandDetailActivity";
	@ViewById(R.id.gv_agent_style)
	GridView gv;
	@ViewById(R.id.tv_style_name)
	TextView tv_title;
	@ViewById(R.id.tv_style_subtitle)
	TextView tv_subtitle;
	@ViewById(R.id.tv_style_cash)
	TextView tv_cash;
	@ViewById(R.id.iv_style)
	ImageView iv_building;
	@ViewById(R.id.ll_agent_progress)
	LinearLayout ll_progress;

	@Click(R.id.bt_agent_guest)
	void takeGuest() {
		BrokerTakeGuestActivity_.intent(this).intent_properString(productName).intent_propretyTypeString(houseType)
				.intent_propretyNumber(String.valueOf(productId)).start();
	}

	@Click(R.id.bt_agent_rec)
	void recommend() {
		BrokerRecommendActivity_.intent(this).intent_properString(productName).intent_propretyTypeString(houseType)
				.intent_propretyNumber(String.valueOf(productId)).start();
	}

	@Extra
	String brandId;
	@Extra
	int productId;
	@Extra
	String productName;
	@Extra
	String houseType;

	private List<Product> productList = new ArrayList<Product>();
	private MyGridViewAdapter adapter;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("品牌详情");
		initDatas();
	}

	private void initDatas() {
		if (StringUtils.isEmpty(brandId))
			return;
		ll_progress.setVisibility(View.VISIBLE);
		requestProductById();
		requestBrandInfo();
		requestProductInf();
	}

	private void requestProductInf() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject obj = data.getMRootData();
				try {
					JSONArray array = obj.getJSONArray("productList");
					for (int i = 0; i < array.length(); i++) {
						JSONObject productObj = array.getJSONObject(i);

						Product product = paraseToProduct(productObj);
						productList.add(product);
					}
					fillData();
					ll_progress.setVisibility(View.GONE);
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				// paraseToProducts(data.getMRootData());
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_product_GetProductsByBrand)).setRequestMethod(RequestMethod.eGet)
				.addParam("BrandId", brandId).notifyRequest();
	}

	private void requestProductById() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getMRootData() != null) {
					JSONObject obj = data.getMRootData();
					String recCommtion = "推荐佣金：" + obj.optInt("RecCommission", 0) + "  ";
					String dealCommtion = "带客佣金：" + obj.optInt("Dealcommission", 0) + "  ";
					String commition = "最高佣金" + obj.optInt("Commission", 0);
					tv_cash.setText(recCommtion + dealCommtion + commition);
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_product_GetProductById)).setRequestMethod(RequestMethod.eGet)
				.addParam("ProductId", String.valueOf(productId)).notifyRequest();
	}

	private Product paraseToProduct(JSONObject obj) {
		String productName = obj.optString("Productname", "");
		String subTitle = obj.optString("SubTitle", "");
		String productImg = obj.optString("Productimg");
		int id = obj.optInt("Id", 0);
		String phone = obj.optString("Phone", "");
		int price = obj.optInt("Price", 0);
		int dealCommission = obj.optInt("DealCommission", 0);
		int commission = obj.optInt("Commision", 0);
		int recCommission = obj.optInt("RecCommission", 0);

		return new Product(id, phone, subTitle, productName, price, productImg, dealCommission, commission,
				recCommission);
	}

	private void requestBrandInfo() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				if (data.getMRootData() != null) {
					try {
						JSONObject obj = data.getMRootData();
						tv_title.setText(obj.optString("Bname", ""));
						tv_subtitle.setText(obj.optString("SubTitle", ""));

						JSONObject params = obj.getJSONObject("Parameters");

						String photo = params.optString("图片banner", "");
						if (!StringUtils.isEmpty(photo)) {

							String url = "http://img.yoopoon.com/" + photo;
							iv_building.setScaleType(ScaleType.FIT_XY);
							ImageLoader.getInstance().displayImage(url, iv_building);
						}
					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_brand_getBrand)).setRequestMethod(RequestMethod.eGet)
				.addParam("BrandId", brandId).notifyRequest();
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyGridViewAdapter();
			gv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private class MyGridViewAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return productList.size();
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
			ViewHolder holder = null;
			if (convertView == null)
				convertView = View.inflate(AgentBrandDetailActivity.this, R.layout.item_agent_style, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_title = (TextView) convertView.findViewById(R.id.tv_item_style_title);
				holder.tv_commition = (TextView) convertView.findViewById(R.id.tv_item_style_commition);
				holder.tv_price = (TextView) convertView.findViewById(R.id.tv_item_style_price);
				holder.iv = (ImageView) convertView.findViewById(R.id.iv_item_style);
				holder.tv_consult = (TextView) convertView.findViewById(R.id.tv_agent_consult);
				convertView.setTag(holder);
			}

			final Product product = productList.get(position);
			holder.tv_title.setText(product.getProductName());
			holder.tv_commition.setText(product.getSubTitle());
			holder.tv_price.setText("均价" + product.getPrice() + "元/㎡起");

			String productImg = product.getProductImg();
			if (!StringUtils.isEmpty(productImg)) {
				String url = "http://img.yoopoon.com/" + productImg;
				holder.iv.setTag(url);
				ImageLoader.getInstance().displayImage(url, holder.iv, MyApplication.getOptions(),
						MyApplication.getLoadingListener());
			}

			holder.tv_consult.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					Tools.callPhone(AgentBrandDetailActivity.this, product.getPhone());
				}
			});

			convertView.setOnClickListener(new OnClickListener() {

				@Override
				public void onClick(View v) {
					ProductDetailActivity_.intent(AgentBrandDetailActivity.this)
							.productId(String.valueOf(product.getId())).start();

				}
			});

			return convertView;
		}

	}

	static class ViewHolder {
		TextView tv_title;
		TextView tv_commition;
		TextView tv_price;
		ImageView iv;
		TextView tv_consult;

	}

	@Override
	public void backButtonClick(View v) {
		finish();
	}

	@Override
	public void titleButtonClick(View v) {

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {

		return true;
	}

}
