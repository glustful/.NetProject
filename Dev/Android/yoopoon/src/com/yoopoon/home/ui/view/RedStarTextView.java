/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RedStarTextView.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午3:19:15 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.view;

import android.content.Context;
import android.graphics.Color;
import android.text.Spannable;
import android.text.SpannableString;
import android.text.Spanned;
import android.text.style.ForegroundColorSpan;
import android.util.AttributeSet;
import android.widget.TextView;

/**
 * @ClassName: RedStarTextView
 * @Description: 首字母为红星的TextView
 * @author: guojunjun
 * @date: 2015-7-8 下午5:37:58
 */
public class RedStarTextView extends TextView {

	public RedStarTextView(Context context, AttributeSet attrs) {
		super(context, attrs);
	}

	@Override
	public void setText(CharSequence text, BufferType type) {

		if (text.length() > 0) {
			Spannable WordtoSpan = new SpannableString(text);

			WordtoSpan.setSpan(new ForegroundColorSpan(Color.RED), 0, 1, Spanned.SPAN_EXCLUSIVE_EXCLUSIVE);
			super.setText(WordtoSpan, type);
		} else {
			super.setText(text, type);
		}
	}

}
