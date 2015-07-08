package com.yoopoon.home;

import android.app.Activity;
import android.content.Context;
import android.graphics.drawable.BitmapDrawable;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.widget.PopupWindow;
import android.widget.PopupWindow.OnDismissListener;
import android.widget.RelativeLayout;


public class SearchPopup
{
    protected PopupWindow mPopupWindow = null;
    protected View mRoot = null;
    protected int  height = RelativeLayout.LayoutParams.WRAP_CONTENT;
    protected OnSearchDismissListener listener;
    public View getmRoot() {
		return mRoot;
	}

	public void setmRoot(View mRoot) {
		this.mRoot = mRoot;
	}
	

	public void setmRoot(int layoutId) {
		mRoot = LayoutInflater.from(mContext).inflate(layoutId, null);
		
	}

	protected Context mContext = null;
   


	private static Builder mBuilder;
    public static Builder builder(Context context)
    {
        mBuilder = new Builder(context);
        return mBuilder;
    }
   
	private SearchPopup(Context context)
    {
        this.mContext = context;
    }
	
	

   
    private void DrawContent()
    {
       
        mPopupWindow = new PopupWindow(mRoot, LayoutParams.MATCH_PARENT, height, false);
        mPopupWindow.setWidth(LayoutParams.MATCH_PARENT);
       
        mPopupWindow.setHeight(height);
        mPopupWindow.setBackgroundDrawable(new BitmapDrawable());
        mPopupWindow.setFocusable(true);
        mPopupWindow.setOutsideTouchable(false);
        mPopupWindow.setOnDismissListener(new OnDismissListener() {
			
			@Override
			public void onDismiss() {
				if(listener != null){
					listener.onDimiss();
				}
			}
		});
        mRoot.setOnTouchListener(new View.OnTouchListener() {
			
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if(event.getAction()==MotionEvent.ACTION_DOWN){
					mPopupWindow.dismiss();
				}
				return true;
			}
		});
        
    }

   public void setOnDismissListener(OnSearchDismissListener l){
	   this.listener = l;
   }

    public void show()
    {
        DrawContent();
        Activity activity = (Activity)mContext;
             
        mPopupWindow.setAnimationStyle(R.style.AnimationPreview);
        mPopupWindow.showAtLocation(activity.getWindow().getDecorView(), Gravity.BOTTOM, 0, 0);
    }
    
    public void show(View parentView, int gravity, int x, int y) {
    	 DrawContent();
    	 mPopupWindow.setAnimationStyle(R.style.AnimationPreview);
         mPopupWindow.showAtLocation(parentView, gravity, x, y);
		
	}

    public void dismiss(){
    	mPopupWindow.dismiss();
    }

    public static class Builder
    {
        SearchPopup mPopup;
        public Builder (Context context)
        {
            mPopup = new SearchPopup(context);
        }

        public Builder setContentView(int layoutId){
        	mPopup.setmRoot(layoutId);
        	return this;
        }
        
        public Builder setContentView(View layout){
        	mPopup.setmRoot(layout);
        	return this;
        }
        public Builder setOnDismissListener(OnSearchDismissListener l){
     	   mPopup.setOnDismissListener(l);
     	   return this;
        }
        public void show()
        {
            mPopup.show();
        }
        
        public void show(View parentView, int gravity, int x, int y)
        {
            mPopup.show(parentView,gravity,x,y);
        }
        
        public void dismiss(){
        	mPopup.dismiss();
        }

		public Builder setHeight(int height) {
			
			mPopup.setHeight(height);
			return this;
		}
		
		
    }

	public void setHeight(int height) {
		this.height = height;
		
	}

	public interface OnSearchDismissListener{
		void onDimiss();
	}

}
