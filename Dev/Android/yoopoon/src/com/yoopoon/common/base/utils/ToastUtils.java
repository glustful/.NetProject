package com.yoopoon.common.base.utils;

import android.content.Context;
import android.os.Handler;
import android.view.Gravity;
import android.widget.Toast;


public class ToastUtils {

	

	private static Runnable r = new Runnable() {
		@Override
		public void run() {
			if (mToast != null)
				mToast.cancel();
		}
	};

	private static Handler mhandler = new Handler();

	public static void showToast(Context mContext, String text, int duration) {
		if(duration<3000){
			duration = 3000;
		}
		mhandler.removeCallbacks(r);
		if (mToast != null)
			mToast.setText(text);
		else{
			mToast = Toast.makeText(mContext, text, 3);
			mToast.setGravity(Gravity.CENTER, 0, 0);
		}
		
		mhandler.postDelayed(r, duration);

		mToast.show();
	}

	static Toast mToast;

	
}
