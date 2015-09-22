package com.yoopoon.market.view;
import com.yoopoon.market.R;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnCancelListener;
import android.content.res.TypedArray;
import android.graphics.Bitmap;
import android.net.http.SslError;
import android.util.AttributeSet;
import android.util.Log;
import android.view.KeyEvent;
import android.webkit.JsPromptResult;
import android.webkit.JsResult;
import android.webkit.SslErrorHandler;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.EditText;

public class HtmlWebView extends WebView {

	boolean autoHeight = false;
	public HtmlWebView(Context context, AttributeSet attrs, int defStyle) {
		super(context, attrs, defStyle);
		TypedArray ta = context.obtainStyledAttributes(attrs, R.styleable.html5View, defStyle, 0);
		autoHeight = ta.getBoolean(R.styleable.html5View_auto_height, false);
		ta.recycle();
		initWebView();
	}

	public HtmlWebView(Context context, AttributeSet attrs) {
		this(context, attrs,0);
		
	}

	public HtmlWebView(Context context) {
		super(context);
		initWebView();
	}
	
	private void initWebView() {
		setWebChromeClient(new MyWebChromeClient());

		setWebViewClient(new MyWebViewClient());
		
		// Configure the webview
		WebSettings s = getSettings();
		s.setBuiltInZoomControls(false);
		s.setDefaultTextEncodingName("gbk");
		s.setLayoutAlgorithm(WebSettings.LayoutAlgorithm.NORMAL);
		s.setUseWideViewPort(true);
		s.setLoadWithOverviewMode(true);
		s.setSavePassword(true);
		s.setSaveFormData(true);
		s.setJavaScriptEnabled(true);
		
		s.setCacheMode(WebSettings.LOAD_NO_CACHE);
		s.setGeolocationEnabled(true);
		s.setGeolocationDatabasePath("/data/data/org.itri.html5webview/databases/");

		s.setDomStorageEnabled(true);
		
		
	}
	
	private class MyWebChromeClient extends WebChromeClient {

		@Override
		public void onReceivedTitle(WebView view, String title) {
			
		}
		
		/**
		 * 覆盖默认的window.alert展示界面，避免title里显示为“：来自file:////”
		 */
		public boolean onJsAlert(WebView view, String url, String message,
				JsResult result) {

			final AlertDialog.Builder builder = new AlertDialog.Builder(
					view.getContext());

			builder.setTitle("对话框").setMessage(message)
					.setPositiveButton("确定", null);

			// 不需要绑定按键事件
			// 屏蔽keycode等于84之类的按键
			builder.setOnKeyListener(new DialogInterface.OnKeyListener() {
				public boolean onKey(DialogInterface dialog, int keyCode,
						KeyEvent event) {
					Log.v("onJsAlert", "keyCode==" + keyCode + "event=" + event);
					return true;
				}

				
			});
			// 禁止响应按back键的事件
			builder.setCancelable(false);
			AlertDialog dialog = builder.create();
			dialog.show();
			result.confirm();// 因为没有绑定事件，需要强行confirm,否则页面会变黑显示不了内容。
			return true;

		}

		public boolean onJsBeforeUnload(WebView view, String url,
				String message, JsResult result) {
			return super.onJsBeforeUnload(view, url, message, result);
		}

		/**
		 * 覆盖默认的window.confirm展示界面，避免title里显示为“：来自file:////”
		 */
		public boolean onJsConfirm(WebView view, String url, String message,
				final JsResult result) {
			final AlertDialog.Builder builder = new AlertDialog.Builder(
					view.getContext());

			builder.setTitle("对话框")
					.setMessage(message)
					.setPositiveButton("确定",
							new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,
										int which) {
									result.confirm();
								}
							})
					.setNeutralButton("取消",
							new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,
										int which) {
									result.cancel();
								}
							});
			builder.setOnCancelListener(new OnCancelListener() {
				@Override
				public void onCancel(DialogInterface dialog) {
					result.cancel();
				}
			});

			// 屏蔽keycode等于84之类的按键，避免按键后导致对话框消息而页面无法再弹出对话框的问题
			builder.setOnKeyListener(new DialogInterface.OnKeyListener() {
				@Override
				public boolean onKey(DialogInterface dialog, int keyCode,
						KeyEvent event) {
					Log.v("onJsConfirm", "keyCode==" + keyCode + "event="
							+ event);
					return true;
				}

				
			});
			// 禁止响应按back键的事件
			builder.setCancelable(false);
			AlertDialog dialog = builder.create();
			dialog.show();
			return true;

		}

		/**
		 * 覆盖默认的window.prompt展示界面，避免title里显示为“：来自file:////”
		 * window.prompt('请输入您的域名地址', '618119.com');
		 */
		public boolean onJsPrompt(WebView view, String url, String message,
				String defaultValue, final JsPromptResult result) {
			final AlertDialog.Builder builder = new AlertDialog.Builder(
					view.getContext());

			builder.setTitle("对话框").setMessage(message);

			final EditText et = new EditText(view.getContext());
			et.setSingleLine();
			et.setText(defaultValue);
			builder.setView(et)
					.setPositiveButton("确定",
							new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,
										int which) {
									result.confirm(et.getText().toString());
								}

							})
					.setNeutralButton("取消",
							new DialogInterface.OnClickListener() {
								public void onClick(DialogInterface dialog,
										int which) {
									result.cancel();
								}
							});

			// 屏蔽keycode等于84之类的按键，避免按键后导致对话框消息而页面无法再弹出对话框的问题
			
			builder.setOnKeyListener(new DialogInterface.OnKeyListener() {
				public boolean onKey(DialogInterface dialog, int keyCode,
						KeyEvent event) {
					Log.v("onJsPrompt", "keyCode==" + keyCode + "event="
							+ event);
					return true;
				}

				
			});

			// 禁止响应按back键的事件
			builder.setCancelable(false);
			AlertDialog dialog = builder.create();
			dialog.show();
			return true;

		}

	}

	private class MyWebViewClient extends WebViewClient {
		@Override
		public void onPageStarted(WebView view, String url, Bitmap favicon) {

			super.onPageStarted(view, url, favicon);
			
		}
		
	

		@Override
		public void onPageFinished(WebView view, String url) {
			
			super.onPageFinished(view, url);
			
			
		}


		@Override
		public void onReceivedSslError(WebView view, SslErrorHandler handler,
				SslError error) {

			handler.proceed();
		}
		@Override
		public boolean shouldOverrideUrlLoading(WebView view, String url) {
			
			view.loadUrl(url);
			return false;
		}
	}
	
	@SuppressWarnings("deprecation")
	@Override
	 public void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {

	 //重新排大小
	 int expandSpec = MeasureSpec.makeMeasureSpec(Integer.MAX_VALUE >> 2,
	    MeasureSpec.AT_MOST);
	 if(autoHeight)
	  super.onMeasure(widthMeasureSpec, expandSpec);
	 else{
		 
		 super.onMeasure(widthMeasureSpec, heightMeasureSpec);
	 }
	 }
	
	
}
