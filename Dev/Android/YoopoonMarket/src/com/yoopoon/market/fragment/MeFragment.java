/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: ShopFragment.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market.fragment 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-7 下午4:50:59 
 * @version: V1.0   
 */
package com.yoopoon.market.fragment;

import java.util.ArrayList;
import java.util.List;
import org.json.JSONObject;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.Toast;
import com.yoopoon.market.AddressManageActivity_;
import com.yoopoon.market.LoginActivity_;
import com.yoopoon.market.MeOrderActivity_;
import com.yoopoon.market.PayDemoActivity_;
import com.yoopoon.market.PersonalInfoActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class MeFragment extends Fragment implements OnClickListener {
	private static final String TAG = "MeFragment";
	View rootView;
	Button btn_order;
	List<RelativeLayout> rls = new ArrayList<RelativeLayout>();
	boolean isUserVisiable = false;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		rootView = inflater.inflate(R.layout.fragment_me, null, false);
		init();
		return rootView;
	}

	@Override
	public void onResume() {
		super.onResume();
		if (!User.isLogin(getActivity())) {
			// 未登录
			LoginActivity_.intent(getContext()).start();
		} else {
			// 若已经登录，需要请求数据
			// requestOrder();
		}
	}

	void requestOrder() {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
				} else {
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_order_get)).setRequestMethod(RequestMethod.eGet).addParam("id", "1")
				.notifyRequest();
	}

	private void init() {
		Button btn_info = (Button) rootView.findViewById(R.id.btn_info);
		Button btn_address = (Button) rootView.findViewById(R.id.btn_address);
		Button btn_about = (Button) rootView.findViewById(R.id.btn_about);
		btn_order = (Button) rootView.findViewById(R.id.btn_order);
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl1));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl2));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl3));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl4));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl5));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl6));
		rls.add((RelativeLayout) rootView.findViewById(R.id.rl7));

		for (RelativeLayout rl : rls)
			rl.setOnClickListener(this);

		btn_info.setOnClickListener(this);
		btn_address.setOnClickListener(this);
		btn_about.setOnClickListener(this);
		btn_order.setOnClickListener(this);
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.btn_info:
				PersonalInfoActivity_.intent(getActivity()).start();
				break;
			case R.id.btn_address:
				AddressManageActivity_.intent(getActivity()).start();
				break;
			case R.id.btn_about:
				PayDemoActivity_.intent(getActivity()).start();
				break;
			case R.id.btn_order:
			case R.id.rl1:
				MeOrderActivity_.intent(getActivity()).item(0).start();
				break;

			case R.id.rl2:
				MeOrderActivity_.intent(getActivity()).item(1).start();
				break;
			case R.id.rl3:
				MeOrderActivity_.intent(getActivity()).item(2).start();
				break;
			case R.id.rl4:
				MeOrderActivity_.intent(getActivity()).item(3).start();
				break;

		}
	}

}
