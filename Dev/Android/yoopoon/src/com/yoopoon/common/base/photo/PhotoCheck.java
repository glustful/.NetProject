package com.yoopoon.common.base.photo;

import java.util.ArrayList;
import java.util.HashMap;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.support.v4.view.PagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.PopupWindow;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;


import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.nostra13.universalimageloader.core.assist.FailReason;
import com.nostra13.universalimageloader.core.assist.ImageScaleType;
import com.nostra13.universalimageloader.core.display.FadeInBitmapDisplayer;
import com.nostra13.universalimageloader.core.listener.SimpleImageLoadingListener;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.ResponseData.ResultState;
import com.yoopoon.home.data.user.UserAccount;


@EActivity(R.layout.activity_photo_check)
public class PhotoCheck extends MainActionBarActivity {

	
	static String TAG = "PhotoCheck";
	
	ArrayList<String> bigPictureUrls;
	
	@ViewById(R.id.photo_check_pager)
	ViewPager viewPager;
	 
	
	@Extra
	ArrayList<String> names;
	@Extra
	String name;
	
	MyPagerAdapter pagerAdapter;
	int num;
	Context mContext;
	ProgressDialog mDialog;
	
	@AfterInject
	void afterInject(){
		mContext = this;
		
			pagerAdapter = new MyPagerAdapter();
			num = getNum();
			bigPictureUrls = getBigPictureUri(names);
		
	}
	
	

	@AfterViews
	void afterViews(){
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		titleButton.setText("图片浏览");
		backButton.setText("返回");
			viewPager.setAdapter(pagerAdapter);
			viewPager.setCurrentItem(num);
			titleButton.setText((num+1)+"/"+names.size());
			
		
		 viewPager.setOnPageChangeListener(new OnPageChangeListener() {
			
			@Override
			public void onPageSelected(int arg0) {
				titleButton.setText((arg0+1)+"/"+names.size());
				
			}
			
			@Override
			public void onPageScrolled(int arg0, float arg1, int arg2) {
				// TODO Auto-generated method stub
			}
			
			@Override
			public void onPageScrollStateChanged(int arg0) {
				// TODO Auto-generated method stub
				
			}
		});
	}
	
	
	

	
	
	private int getNum(){
		for(int i = 0 ;i < names.size();i++){
			String nm = names.get(i);
			if(nm.equals(name)){
				return i;
			}
		}
		return 0;
	}
	
	
	private ArrayList<String> getBigPictureUri(ArrayList<String> fileIds){
		ArrayList<String> bigPictureUrls = new ArrayList<String>();
		String url = getString(R.string.url_host_img);
     	for(int i = 0;i <fileIds.size();i++){
     		String fileId = fileIds.get(i);
     	    String fileUrl = url +fileId;
     	    bigPictureUrls.add(fileUrl);
      	}
     	
     	return bigPictureUrls;
     }
	



   

  class MyPagerAdapter extends PagerAdapter {
	    LayoutInflater inflater;
        public MyPagerAdapter(){
        	inflater = LayoutInflater.from(PhotoCheck.this);
        }
        public void refresh(ArrayList<String> str){
        	this.notifyDataSetChanged();
        }
        
        @Override
        public int getCount() {
            return names != null ? names.size() : 0;
        }

        @Override
        public boolean isViewFromObject(View view, Object object) {
            return view == object;
        }

        @Override
        public Object instantiateItem(final ViewGroup container, final int position) {
        	View imageLayout = inflater.inflate(R.layout.picture_bigimage_view, container, false);
			assert imageLayout != null;
			final ImageView imageView = (ImageView) imageLayout.findViewById(R.id.pictureBigImageView);
			final ProgressBar spinner = (ProgressBar) imageLayout.findViewById(R.id.progressBar);
			
			ImageLoader.getInstance().displayImage(bigPictureUrls.get(position), imageView,MyApplication.getOptions(), new SimpleImageLoadingListener() {
				@Override
				public void onLoadingStarted(String imageUri, View view) {
					spinner.setVisibility(View.VISIBLE);
				}

				@Override
				public void onLoadingFailed(String imageUri, View view, FailReason failReason) {
					String message = null;
					
					switch (failReason.getType()) {
						case IO_ERROR:
							message = "图片加载失败，请稍后重试。";
							break;
						case DECODING_ERROR:
							message = "Image can't be decoded";
							break;
						case NETWORK_DENIED:
							message = "Downloads are denied";
							break;
						case OUT_OF_MEMORY:
							message = "Out Of Memory error";
							break;
						case UNKNOWN:
							message = "Unknown error";
							break;
					}
					Toast.makeText(PhotoCheck.this, message, Toast.LENGTH_SHORT).show();
					spinner.setVisibility(View.GONE);
				}

				@Override
				public void onLoadingComplete(String imageUri, View view, Bitmap loadedImage) {
					spinner.setVisibility(View.GONE);
				
				}
			});

			container.addView(imageLayout, 0);
			return imageLayout;
        }

        @Override
        public void destroyItem(ViewGroup container, int position, Object object) {
        	container.removeView((View)object);
        }

        @Override
        public int getItemPosition(Object object) {
            return POSITION_NONE;
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
	// TODO Auto-generated method stub
	return true;
}

}
