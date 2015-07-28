package com.yoopoon.home.ui.html5;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.view.View;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.R;
import com.yoopoon.home.ui.view.Html5View;

/**
 * @author guojunjun E-mail: guojunjun520@gmail.com
 * @version 创建时间：2015-7-6 下午3:39:31 类说明 加载网页类
 */
@EActivity(R.layout.html5_view)
public class HTML5Activity extends MainActionBarActivity {
	@Extra
	String url;
	@Extra
	String title;
	@ViewById(R.id.webview)
	Html5View mHtml5View;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText(title);
		mHtml5View.loadUrl(url);

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
