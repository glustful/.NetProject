package com.round.progressbar;
import android.content.Context;
import android.graphics.Color;
import android.graphics.drawable.ColorDrawable;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.LinearLayout.LayoutParams;
import android.widget.PopupWindow;

public class RoundProgressDialog {
	PopupWindow popup;
	Context mContext;
	static RoundProgressDialog instance;
	private RoundProgressDialog(Context context){
		this.mContext = context;
		popup = new PopupWindow(mContext);
		popup.setContentView(LayoutInflater.from(mContext).inflate(R.layout.dialog_cricle_progress, null));
		popup.setWidth(LayoutParams.WRAP_CONTENT);
		popup.setHeight(LayoutParams.WRAP_CONTENT);
		popup.setBackgroundDrawable(new ColorDrawable(Color.TRANSPARENT));
		popup.setOutsideTouchable(true);
	}
	
	public static RoundProgressDialog build(Context context){
		if(instance==null)
		 instance = new RoundProgressDialog(context);
		return instance;
	}
	
	public void show(View parent,int gravity,int x,int y){
		popup.showAtLocation(parent, gravity, x, y);
	}
	
	public void hide(){
		popup.dismiss();
	}
}
