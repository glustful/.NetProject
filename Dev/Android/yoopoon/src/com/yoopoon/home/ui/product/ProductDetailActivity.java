package com.yoopoon.home.ui.product;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView.ScaleType;
import android.widget.TextView;

import com.etsy.android.grid.util.DynamicHeightImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.listener.ImageLoadingListener;
import com.round.progressbar.CircleProgressDialog;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.ui.view.Html5View;
import com.yoopoon.home.ui.view.MyGridView;
import com.yoopoon.house.ui.broker.BrokerRecommendActivity_;
import com.yoopoon.house.ui.broker.BrokerTakeGuestActivity_;

@EActivity(R.layout.product_detail_view)
public class ProductDetailActivity extends MainActionBarActivity {
	@Extra
	String productId;
	// 如下单个按钮是控制楼盘详情中的我要带客，我要推荐和咨询热线
	@ViewById(R.id.product_detail_take_guest)
	TextView brokerTakeGuestTextView;
	@ViewById(R.id.product_detail_recommend)
	TextView brokerRecommendTextView;
	@ViewById(R.id.product_detail_consultation)
	TextView brokerConsultationTextView;
	@ViewById(R.id.title)
	TextView title;
	@ViewById(R.id.price)
	TextView price;
	@ViewById(R.id.area)
	TextView area;
	@ViewById(R.id.img)
	com.etsy.android.grid.util.DynamicHeightImageView img;
	@ViewById(R.id.imgGrid)
	MyGridView imgGrid;
	@ViewById(R.id.contentWeb)
	Html5View webView;
	Context mContext;
	String header = "<!doctype html><html><head><meta name = \"viewport\" content = \"width = device-width\"/></head><body>";
	String tail = "</body></html>";
	
	@AfterViews
	void initUI() {
		mContext = this;
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("户型详情");
		requestProduct();
	}
	
	ImageLoadingListener listen = new ImageLoadingListener() {
		@Override
		public void onLoadingStarted(String imageUri, View view) {
		}
		@Override
		public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
		}
		@Override
		public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
			if (imageUri.equals(view.getTag().toString())) {
				if (view instanceof DynamicHeightImageView) {
					DynamicHeightImageView new_name = (DynamicHeightImageView) view;
					// ######################郭俊军 编写########################## Start
					// double ratio = loadedImage.getHeight() / loadedImage.getWidth();
					// ######################郭俊军 编写########################## End
					//
					// ######################徐阳会 2015年7月16日 编写########################## Start
					int screenWidth = mContext.getResources().getDisplayMetrics().widthPixels;
					int sceemHeight = mContext.getResources().getDisplayMetrics().heightPixels;
					double ratio = sceemHeight / screenWidth;
					// ######################徐阳会 2015年7月16日 编写########################## End
					new_name.setHeightRatio(ratio);
					new_name.setImageBitmap(loadedImage);
				}
			}
		}
		@Override
		public void onLoadingCancelled(String imageUri, View view) {
		}
	};
	
	void requestProduct() {
		CircleProgressDialog.build(mContext, R.style.dialog).show();
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				CircleProgressDialog.build(mContext, R.style.dialog).dismiss();
				if (data.getResultState() == ResultState.eSuccess) {
					callBack(data.getMRootData());
				} else {
					ToastUtils.showToast(mContext, data.getMsg(), 3000);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_product_GetProductById)).setRequestMethod(RequestMethod.eGet)
				.addParam("productId", productId).notifyRequest();
	}
	@UiThread
	protected void callBack(final JSONObject mRootData) {
		if (mRootData == null)
			return;
		title.setText(Tools.optString(mRootData, "Productname", ""));
		price.setText("均价" + Tools.optDouble(mRootData, "Price", 0) + "元/" + getString(R.string.meter) + "起");
		area.setText(Tools.optString(mRootData, "SubTitle", ""));
		brokerConsultationTextView.setText("咨询热线" + Tools.optString(mRootData, "Phone", ""));
		// ############################### 郭俊军编写 #############################################
		//String url = getString(R.string.url_host_img) + Tools.optString(mRootData, "Productimg", "");
		// ############################### 郭俊军编写 #############################################
		// ############################### 徐阳会修改 2015年7月27日  ############################### Start
		String url = getString(R.string.url_host_img) + Tools.optString(mRootData, "ProductDetailImg", "");
		// ############################### 徐阳会修改 2015年7月27日  ############################### End
		img.setTag(url);
		// img.setHeightRatio(1.5);
		ImageLoader.getInstance().displayImage(url, img, MyApplication.getOptions(), listen);
		webView.loadData(header + Tools.optString(mRootData, "ProductDetailed", "") + tail, "text/html;charset=utf-8",
				null);
		ArrayList<String> tmp = new ArrayList<String>();
		int i = 1;
		while (true) {
			String p = "Productimg" + i;
			if (mRootData.isNull(p))
				break;
			tmp.add(getString(R.string.url_host_img) + mRootData.optString(p));
			i++;
		}
		imgGrid.setAdapter(new ImgAdapter(tmp));
		// ##########################徐阳会 2015年7月22日 添加 Start
		brokerTakeGuestTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				String houseTypeString = null;
				try {
					JSONArray houseJsonArray = mRootData.getJSONArray("ParameterValue");
					for (int i = 0; i < houseJsonArray.length(); i++) {
						JSONObject jsonObject = houseJsonArray.getJSONObject(i);
						String parameterType = jsonObject.getString("ParameterString");
						if (parameterType.equals("户型")) {
							houseTypeString = jsonObject.getString("Value");
						}
					}
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				BrokerTakeGuestActivity_.intent(mContext)
						.intent_properString(Tools.optString(mRootData, "Productname", ""))
						.intent_propretyTypeString(houseTypeString)
						.intent_propretyNumber(Tools.optString(mRootData, "Id", "")).start();
			}
		});
		brokerRecommendTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				String houseTypeString = null;
				try {
					JSONArray houseJsonArray = mRootData.getJSONArray("ParameterValue");
					for (int i = 0; i < houseJsonArray.length(); i++) {
						JSONObject jsonObject = houseJsonArray.getJSONObject(i);
						String parameterType = jsonObject.getString("ParameterString");
						if (parameterType.equals("户型")) {
							houseTypeString = jsonObject.getString("Value");
						}
					}
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				BrokerRecommendActivity_.intent(mContext)
						.intent_properString(Tools.optString(mRootData, "Productname", ""))
						.intent_propretyTypeString(houseTypeString)
						.intent_propretyNumber(Tools.optString(mRootData, "Id", "")).start();
			}
		});
		brokerConsultationTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Tools.callPhone(mContext, Tools.optString(mRootData, "Phone", ""));
			}
		});
		// ##########################徐阳会 2015年7月22日 添加 End
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
		// TODO Auto-generated method stub
		return true;
	}
	
	class ImgAdapter extends BaseAdapter {
		ArrayList<String> urls;
		
		public ImgAdapter(ArrayList<String> tmp) {
			this.urls = new ArrayList<String>();
			this.urls.addAll(tmp);
		}
		@Override
		public int getCount() {
			// TODO Auto-generated method stub
			return urls.size();
		}
		@Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return urls.get(position);
		}
		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return position;
		}
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			com.etsy.android.grid.util.DynamicHeightImageView image;
			if (convertView == null) {
				image = new com.etsy.android.grid.util.DynamicHeightImageView(mContext);
				image.setScaleType(ScaleType.FIT_CENTER);
				image.setLayoutParams(new GridView.LayoutParams(GridView.LayoutParams.MATCH_PARENT,
						GridView.LayoutParams.WRAP_CONTENT));
			} else {
				image = (com.etsy.android.grid.util.DynamicHeightImageView) convertView;
			}
			image.setTag(urls.get(position));
			// image.setHeightRatio(1);
			ImageLoader.getInstance().displayImage(urls.get(position), image, MyApplication.getOptions(), listen);
			return image;
		}
	}
	/*
	 * (non Javadoc)
	 * @Title: onClick
	 * @Description: TODO
	 * @param v
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
}
