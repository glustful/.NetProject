package com.miicaa.home.ui.home;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.ViewById;

import android.content.Intent;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.miicaa.common.base.Tools;
import com.miicaa.home.R;
import com.miicaa.home.data.business.account.AccountInfo;
import com.miicaa.home.data.net.ProgressMessage;
import com.miicaa.home.data.net.RequestAdpater;
import com.miicaa.home.data.net.ResponseData;
import com.miicaa.home.data.net.ResponseData.ResultState;
import com.miicaa.home.ui.frame.FrameMe;
import com.miicaa.home.ui.person.PersonMeDetail;
import com.miicaa.utils.AllUtils;

@EFragment(R.layout.home_fram_me_fragment)
public class FramMeFragment extends Fragment {
	String meCode;
	@ViewById(R.id.homeFramMe)
	LinearLayout homeFramMe;
	@ViewById(R.id.home_fram_me_head_view)
	ImageView head;
	@ViewById(R.id.home_fram_me_name_text)
	TextView name;
	@ViewById(R.id.edit_me)
	ImageButton editme;
	FrameMe me;
	boolean isAdmin = false;

	@AfterInject
	void afterInject() {
		meCode = AccountInfo.instance().getLastUserInfo().getCode();
	}

	@AfterViews
	void initData() {
		me = new FrameMe(homeFramMe.getContext());
		homeFramMe.addView(me.getRootView());
		/*
		 * if(!isAdmin) requestManager(); else{ if(me != null)
		 * me.showPay(View.VISIBLE); else me.showPay(View.GONE); }
		 */

	}

	@Override
	public void onResume() {

		super.onResume();
		
		requestPackageExpir("/home/phone/message/getalert");
	}

	private void requestPackageExpir(String url) {
		new RequestAdpater() {

			@Override
			public void onReponse(ResponseData data) {
				
				if (data.getResultState() == ResultState.eSuccess) {
					
					if (data.getMRootData().isNull("data")
							|| data.getMRootData().optString("data").trim().equals("")) {
						
						if (me != null) {
							me.showPackageExpir(View.GONE);
						} 
					} else {
						if(me != null)
						me.showPackageExpir(View.VISIBLE);
					}
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(url).notifyRequest();

	}

	@Override
	public void onStart() {
		Tools.setHeadImgWithoutClick(AccountInfo.instance().getLastUserInfo().getCode(),head);
		name.setText(AccountInfo.instance().getLastUserInfo().getName());
		requestManager();
		super.onStart();
	}

	@Click(R.id.home_fram_me_head_view)
	void headViewClick(View v) {
		editme(v);
	}

	@Click(R.id.edit_me)
	void editMeClick(View v) {
		editme(v);
	}

	void editme(View v) {
		AllUtils.hiddenSoftBorad(getActivity());
		Intent intent = new Intent(getActivity(), PersonMeDetail.class);
		Bundle bundle = new Bundle();
		bundle.putString("userCode", meCode);
		intent.putExtra("bundle", bundle);
		startActivity(intent);
		getActivity().overridePendingTransition(R.anim.my_slide_in_right,
				R.anim.my_slide_out_left);
	}

	private void requestManager() {
		String url = this.getString(R.string.pay_isadmin_url);
		// PayUtils.showDialog(getActivity());
		new RequestAdpater() {

			@Override
			public void onReponse(ResponseData data) {
				if (data.getResultState() == ResultState.eSuccess) {
					// PayUtils.closeDialog();
					if (!data.getJsonObject().isNull("isAdmin")
							&& data.getJsonObject().optBoolean("isAdmin")) {
						isAdmin = true;
						if (me != null) {
							me.showPay(View.VISIBLE);
						} 
					} else {
						isAdmin = false;
						me.showPay(View.GONE);
					}
				}

			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(url).notifyRequest();
	}
	// @Click(R.id.framMeExit)
	// void exitClick(){
	// Activity activity = (Activity) getActivity();
	// Intent intent = new Intent(getActivity(), HomeLoginActivity_.class);
	// Bundle bundle = new Bundle();
	// bundle.putString("exit", "exit");
	// intent.putExtra("bundle", bundle);
	// getActivity().startActivity(intent);
	// PushMessage.stopPushMessage(getActivity());//停止消息推送服务
	// activity.finish();
	// }
	// @Click(R.id.framMeComp)
	// void compSel(){
	// Intent intent = new Intent(getActivity(), PersonUnitChange.class);
	// ((Activity) getActivity()).startActivityForResult(intent, 99);
	// ((Activity)
	// getActivity()).overridePendingTransition(R.anim.my_slide_in_right,
	// R.anim.my_slide_out_left);
	// }

	// View rootView;
	// @Override
	// public void onCreate(Bundle savedInstanceState) {
	// // TODO Auto-generated method stub
	// super.onCreate(savedInstanceState);
	// }
	//
	// @Override
	// public View onCreateView(LayoutInflater inflater, ViewGroup container,
	// Bundle savedInstanceState) {
	// rootView =
	// LayoutInflater.from(getActivity()).inflate(R.layout.home_fram_me_fragment,
	// null);
	// if(container == null){
	// container = (ViewGroup)rootView.getParent();
	// container.removeAllViewsInLayout();
	// }
	//
	// // TODO Auto-generated method stub
	// return rootView;
	// }

}
