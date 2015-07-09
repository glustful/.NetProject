package com.round.progressbar;

import android.app.AlertDialog;
import android.content.Context;
import android.os.Bundle;
import android.os.Handler;

public class CircleProgressDialog extends AlertDialog {

	Handler handler = new Handler();
	RoundProgressBar bar;
	int progress = 0;
	int flag = 1;
	static CircleProgressDialog instance;
	Runnable r = new Runnable() {

		@Override
		public void run() {

			if (progress == 100) {
				flag = -5;
			}
			else if(progress==0){
				flag = 5;
			}
			progress += flag;
			bar.setProgress(progress);

			handler.postDelayed(this, 100);

		}
	};

	@Override
	public void show() {
		// TODO Auto-generated method stub
		super.show();
		handler.postDelayed(r, 0);
	}

	@Override
	public void dismiss() {
		// TODO Auto-generated method stub
		super.dismiss();
		handler.removeCallbacks(r);
		instance = null;
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		init();
	}

	public CircleProgressDialog(Context context, boolean cancelable,
			OnCancelListener cancelListener) {
		super(context, cancelable, cancelListener);

	}

	public CircleProgressDialog(Context context, int theme) {
		super(context, theme);

	}

	public CircleProgressDialog(Context context) {
		super(context);

	}

	private void init() {
		setContentView(R.layout.cricle_progress);
		bar = (RoundProgressBar) findViewById(R.id.roundProgressBar);
	}
	
	public static CircleProgressDialog build(Context mContext){
		if(instance == null){
			instance = new CircleProgressDialog(mContext);
		}
		return instance;
	}
	
	public static CircleProgressDialog build(Context mContext,int style){
		if(instance == null){
			instance = new CircleProgressDialog(mContext,style);
		}
		return instance;
	}

}
