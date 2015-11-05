package com.yoopoon.market.view;

import com.yoopoon.market.R;
import com.yoopoon.market.R.layout;

import android.app.Dialog;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup.LayoutParams;
import android.widget.Button;

/**
 * @ClassName: SetPriceDialog
 * @Description: 设置价格需要用到的Dialog
 * @author: 徐阳会
 * @date: 2015年9月15日 上午9:07:44
 */
public class SetPriceDialog extends Dialog {
	public SetPriceDialog(Context context, int theme) {
		super(context, theme);
	}
	public SetPriceDialog(Context context) {
		super(context);
	}

	public static class Builder {
		private Context mContext;
		private String titleString;
		private String messageString;
		private String positiveButtonText;
		private String negativeButtonText;
		private OnClickListener positiveButtonOnClickListener;
		private OnClickListener negativeButtonClickListener;
		private Button confirmButton, cancelButton;

		public Builder(Context context) {
			mContext = context;
		}
		public Builder setTitle(String title) {
			this.titleString = title;
			return this;
		}
		public Builder setTitle(int resourceId) {
			this.titleString = (String) mContext.getText(resourceId);
			return this;
		}
		public Builder setMessage(String message) {
			this.messageString = message;
			return this;
		}
		public Builder setMessage(int resourceId) {
			this.messageString = (String) mContext.getText(resourceId);
			return this;
		}
		public Builder setPositiveButton(int resourceId, OnClickListener listener) {
			this.positiveButtonText = (String) mContext.getText(resourceId);
			this.positiveButtonOnClickListener = listener;
			return this;
		}
		public Builder setPositiveButton(String positiveButtonTextString, OnClickListener listener) {
			this.positiveButtonText = positiveButtonTextString;
			this.positiveButtonOnClickListener = listener;
			return this;
		}
		public Builder setNegativeButton(int resourceId, OnClickListener listener) {
			this.negativeButtonText = (String) mContext.getText(resourceId);
			this.negativeButtonClickListener = listener;
			return this;
		}
		public Builder setNegativeButton(String negativeButtonTextString, OnClickListener listener) {
			this.negativeButtonText = negativeButtonTextString;
			this.negativeButtonClickListener = listener;
			return this;
		}
		public SetPriceDialog create() {
			LayoutInflater inflater = (LayoutInflater) mContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
			final SetPriceDialog dialog = new SetPriceDialog(mContext);
			View layoutView = inflater.inflate(R.layout.dialog_set_price, null);
			dialog.addContentView(layoutView, new LayoutParams(LayoutParams.MATCH_PARENT, LayoutParams.WRAP_CONTENT));
			confirmButton = (Button) layoutView.findViewById(R.id.btn_confirm);
			cancelButton = (Button) layoutView.findViewById(R.id.btn_cancel);
			confirmButton.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					positiveButtonOnClickListener.onClick(dialog, BUTTON_POSITIVE);
				}
			});
			cancelButton.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					negativeButtonClickListener.onClick(dialog, BUTTON_NEGATIVE);
				}
			});
			dialog.setContentView(layoutView);
			return dialog;
		}
	}
}
