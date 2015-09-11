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

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import com.yoopoon.market.AboutUActivity_;
import com.yoopoon.market.AddressManageActivity_;
import com.yoopoon.market.PersonalInfoActivity_;
import com.yoopoon.market.R;

/**
 * @ClassName: ShopFragment
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:50:59
 */
public class MeFragment extends Fragment implements OnClickListener {
	View convertView;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		convertView = inflater.inflate(R.layout.fragment_me, null, false);
		init();
		return convertView;
	}

	private void init() {
		Button btn_info = (Button) convertView.findViewById(R.id.btn_info);
		Button btn_address = (Button) convertView.findViewById(R.id.btn_address);
		Button btn_about = (Button) convertView.findViewById(R.id.btn_about);
		btn_info.setOnClickListener(this);
		btn_address.setOnClickListener(this);
		btn_about.setOnClickListener(this);
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
				AboutUActivity_.intent(getActivity()).start();
				break;
		}
	}

}
