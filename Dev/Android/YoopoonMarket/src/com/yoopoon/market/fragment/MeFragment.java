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

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.makeramen.RoundedImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.AboutUActivity_;
import com.yoopoon.market.AddressManageActivity_;
import com.yoopoon.market.LoginActivity_;
import com.yoopoon.market.MeOrderActivity_;
import com.yoopoon.market.PersonalInfoActivity_;
import com.yoopoon.market.R;
import com.yoopoon.market.domain.CommunityOrderEntity;
import com.yoopoon.market.domain.MemberModel;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

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
	List<CommunityOrderEntity> orders = new ArrayList<CommunityOrderEntity>();
	List<TextView> orderStatus = new ArrayList<TextView>();
	View ll_loading;
	RoundedImageView imageView1;
	MemberModel member;

	@Override
	@Nullable
	public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (rootView == null) {
			rootView = inflater.inflate(R.layout.fragment_me, null, false);
			init();
		}
		return rootView;
	}

	public void requestData() {
		ll_loading.setVisibility(View.VISIBLE);
		imageView1 = (RoundedImageView) rootView.findViewById(R.id.imageView1);
		imageView1.setOnClickListener(this);
		String userid = User.getUserId(getActivity());
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					parseToMember(object.toString());
				} else {
					ll_loading.setVisibility(View.GONE);
					Toast.makeText(getActivity(), data.toString(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_getmemeber_byid)).setRequestMethod(RequestMethod.eGet)
				.addParam("userid", userid).notifyRequest();
	}

	void parseToMember(final String json) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				try {
					member = om.readValue(json, MemberModel.class);
				} catch (JsonParseException e) {
					e.printStackTrace();
				} catch (JsonMappingException e) {
					e.printStackTrace();
				} catch (IOException e) {
					e.printStackTrace();
				}
				return member;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					ll_loading.setVisibility(View.GONE);
					if (!TextUtils.isEmpty(member.Thumbnail)) {
						String imageUrl = getString(R.string.url_image) + member.Thumbnail;
						ImageLoader.getInstance().displayImage(imageUrl, imageView1);
					}
				}
			}
		}).execute();
	}

	void requestOrder(String userId) {
		ll_loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					Log.i(TAG, object.toString());
					JSONArray array = object.optJSONArray("List");
					if (array != null) {

						parseToOrderList(array);
					}
				} else {
					ll_loading.setVisibility(View.GONE);
					Toast.makeText(getActivity(), data.getMsg(), Toast.LENGTH_SHORT).show();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_order_get)).setRequestMethod(RequestMethod.eGet).addParam("userid", userId)
				.notifyRequest();
	}

	void parseToOrderList(final JSONArray array) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject object = array.getJSONObject(i);
						CommunityOrderEntity order = om.readValue(object.toString(), CommunityOrderEntity.class);
						orders.add(order);
					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (JsonParseException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (JsonMappingException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (IOException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
				return orders;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					setOrderStatus();
				}

			}
		}).execute();
	}

	void setOrderStatus() {
		ll_loading.setVisibility(View.GONE);
		int[] status = new int[4];
		for (CommunityOrderEntity entity : orders) {
			switch (entity.Status) {
				case 0:
					status[0]++;
					break;
				case 1:
					status[1]++;
					break;
				case 2:
					status[2]++;
				case 3:
					status[3]++;
				default:
					break;
			}
		}

		for (int i = 0; i < orderStatus.size(); i++) {
			TextView tv = orderStatus.get(i);
			tv.setText(String.valueOf(status[i]));
			tv.setVisibility(status[i] == 0 ? View.GONE : View.VISIBLE);
		}
	}

	private void init() {

		ll_loading = rootView.findViewById(R.id.ll_loading);
		Button btn_info = (Button) rootView.findViewById(R.id.btn_info);
		Button btn_address = (Button) rootView.findViewById(R.id.btn_address);
		Button btn_about = (Button) rootView.findViewById(R.id.btn_about);
		btn_order = (Button) rootView.findViewById(R.id.btn_order);
		if (rls.size() == 0) {
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl1));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl2));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl3));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl4));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl5));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl6));
			rls.add((RelativeLayout) rootView.findViewById(R.id.rl7));
		}

		if (orderStatus.size() == 0) {
			orderStatus.add((TextView) rootView.findViewById(R.id.tv_created));// 待付款
			orderStatus.add((TextView) rootView.findViewById(R.id.tv_payed));// 待发货
			orderStatus.add((TextView) rootView.findViewById(R.id.tv_delivering));// 待收货
			orderStatus.add((TextView) rootView.findViewById(R.id.tv_finished));// 待评价
		}

		for (RelativeLayout rl : rls)
			rl.setOnClickListener(this);

		btn_info.setOnClickListener(this);
		btn_address.setOnClickListener(this);
		btn_about.setOnClickListener(this);
		btn_order.setOnClickListener(this);

		TextView tv_name = (TextView) rootView.findViewById(R.id.tv_name);
		tv_name.setText(User.getUserName(getActivity()));

	}

	@Override
	public void onResume() {
		super.onResume();
		if (!User.isLogin(getActivity())) {
			// 未登录
			LoginActivity_.intent(getContext()).start();
		} else {
			// 若已经登录，需要请求数据
			if (orders.size() == 0) {
				SharedPreferences sp = getActivity().getSharedPreferences(getString(R.string.share_preference),
						Context.MODE_PRIVATE);
				requestOrder(String.valueOf(sp.getInt("UserId", 0)));
				requestData();
			} else {
				setOrderStatus();
			}
		}
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.btn_info:
			case R.id.imageView1:
				PersonalInfoActivity_.intent(getActivity()).member(member).start();
				break;
			case R.id.btn_address:
				AddressManageActivity_.intent(getActivity()).start();
				break;
			case R.id.btn_about:
				AboutUActivity_.intent(getActivity()).start();
				break;
			case R.id.btn_order:
			case R.id.rl1:
				MeOrderActivity_.intent(getActivity()).item(0).orders(orders).start();
				break;

			case R.id.rl2:
				MeOrderActivity_.intent(getActivity()).item(1).orders(orders).start();
				break;
			case R.id.rl3:
				MeOrderActivity_.intent(getActivity()).item(2).orders(orders).start();
				break;
			case R.id.rl4:
				MeOrderActivity_.intent(getActivity()).item(3).orders(orders).start();
				break;

		}
	}

}
