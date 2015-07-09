package com.yoopoon.home.ui.product;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.UiThread;
import org.androidannotations.annotations.ViewById;
import org.json.JSONObject;

import android.content.Context;
import android.graphics.Bitmap;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;
import android.widget.ImageView.ScaleType;
import android.widget.TextView;

import com.etsy.android.grid.util.DynamicHeightImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.listener.ImageLoadingListener;
import com.round.progressbar.CircleProgressDialog;
import com.round.progressbar.RoundProgressDialog;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.ui.view.Html5View;
import com.yoopoon.home.ui.view.MyGridView;
@EActivity(R.layout.product_detail_view)
public class ProductDetailActivity extends MainActionBarActivity {
	@Extra
	String productId;
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
	void initUI(){
		mContext = this;
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("户型详情");
		requestProduct();
	}
	
	ImageLoadingListener listen = new ImageLoadingListener() {
		
		@Override
		public void onLoadingStarted(String imageUri, View view) {
			// TODO Auto-generated method stub
			
		}
		
		@Override
		public void onLoadingFailed(String imageUri, View view,
				FailReason failReason) {
			// TODO Auto-generated method stub
			
		}
		
		@Override
		public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
			if(imageUri.equals(view.getTag().toString())){
				if(view instanceof DynamicHeightImageView) {
					DynamicHeightImageView new_name = (DynamicHeightImageView) view;
					double ratio = loadedImage.getHeight()/loadedImage.getWidth();
					new_name.setHeightRatio(ratio);
					new_name.setImageBitmap(loadedImage);
				}
			}
			
		}
		
		@Override
		public void onLoadingCancelled(String imageUri, View view) {
			// TODO Auto-generated method stub
			
		}
	};
	
	void requestProduct(){
		CircleProgressDialog.build(mContext, R.style.dialog).show();
		new RequestAdapter() {
			
			@Override
			public void onReponse(ResponseData data) {
				CircleProgressDialog.build(mContext, R.style.dialog).dismiss();
				
				if(data.getResultState()==ResultState.eSuccess){
					callBack(data.getMRootData());
				}else{
					ToastUtils.showToast(mContext, data.getMsg(), 3000);
				}
				
			}
			
			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
				
			}
		}.setUrl(getString(R.string.url_product_GetProductById))
		.setRequestMethod(RequestMethod.eGet)
		.addParam("productId", productId)
		.notifyRequest();
	}
	
	@UiThread
	protected void callBack(JSONObject mRootData) {
		if(mRootData==null)
			return;
		title.setText(Tools.optString(mRootData, "Productname", ""));
		price.setText("均价"+Tools.optDouble(mRootData, "Price", 0)+"元/"+getString(R.string.meter)+"起");
		area.setText(Tools.optString(mRootData, "SubTitle", ""));
		String url = getString(R.string.url_host_img)+Tools.optString(mRootData, "Productimg", "");
		img.setTag(url);
		//img.setHeightRatio(1.5);
		ImageLoader.getInstance().displayImage(url, img,MyApplication.getOptions(),listen);
		webView.loadData(header+Tools.optString(mRootData, "ProductDetailed", "")+tail, "text/html;charset=utf-8", null);
		ArrayList<String> tmp = new ArrayList<String>();
		int i=1;
		while(true){
			String p = "Productimg"+i;
			if(mRootData.isNull(p))
				break;
			tmp.add(getString(R.string.url_host_img)+mRootData.optString(p));
			i++;
		}
		imgGrid.setAdapter(new ImgAdapter(tmp));
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
	
	class ImgAdapter extends BaseAdapter{
		ArrayList<String> urls;
		public ImgAdapter(ArrayList<String> tmp){
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
			if(convertView == null){
				image = new com.etsy.android.grid.util.DynamicHeightImageView(mContext);
				image.setScaleType(ScaleType.FIT_CENTER);
				
				image.setLayoutParams(new GridView.LayoutParams(GridView.LayoutParams.MATCH_PARENT,GridView.LayoutParams.WRAP_CONTENT));
			}
			else{
				image = (com.etsy.android.grid.util.DynamicHeightImageView) convertView;
			}
			image.setTag(urls.get(position));
			//image.setHeightRatio(1);
			ImageLoader.getInstance().displayImage(urls.get(position), image,MyApplication.getOptions(),listen);
			return image;
		}
		
	}

}
