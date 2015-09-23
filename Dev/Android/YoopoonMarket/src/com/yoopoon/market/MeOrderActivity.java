package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.ViewPager;
import android.support.v4.view.ViewPager.OnPageChangeListener;
import android.util.Log;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.CommunityOrderEntity;
import com.yoopoon.market.fragment.CommentFragment;
import com.yoopoon.market.fragment.PayFragment;
import com.yoopoon.market.fragment.ReceiveFragment;
import com.yoopoon.market.fragment.SendFragment;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;

@EActivity(R.layout.activity_me_order)
public class MeOrderActivity extends MainActionBarActivity implements OnClickListener {
	private static final String TAG = "MeOrderActivity";
	@ViewById(R.id.vp)
	ViewPager vp;
	@ViewById(R.id.ll_loading)
	View loading;
	@Extra
	int item;
	@Extra
	List<CommunityOrderEntity> orders;
	List<CommunityOrderEntity> createdOrders = new ArrayList<CommunityOrderEntity>(); // 待付款
	List<CommunityOrderEntity> payedOrders = new ArrayList<CommunityOrderEntity>();// 待发货
	List<CommunityOrderEntity> deliveringOrders = new ArrayList<CommunityOrderEntity>();// 待收货
	List<CommunityOrderEntity> finishedOrders = new ArrayList<CommunityOrderEntity>();// 待评价
	List<Fragment> fragments = new ArrayList<Fragment>();
	List<TextView> textViews = new ArrayList<TextView>();

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("我的订单");

		fragments.add(new PayFragment());
		fragments.add(new SendFragment());
		fragments.add(new ReceiveFragment());
		fragments.add(new CommentFragment());

		textViews.add((TextView) findViewById(R.id.tv1));
		textViews.add((TextView) findViewById(R.id.tv2));
		textViews.add((TextView) findViewById(R.id.tv3));
		textViews.add((TextView) findViewById(R.id.tv4));

		vp.setAdapter(new MyViewPagerAdapter(getSupportFragmentManager()));
		vp.setOnPageChangeListener(new MyPageChangeListener());

		for (TextView tv : textViews)
			tv.setOnClickListener(this);
		vp.setCurrentItem(item);

		initList();
	}

	void initList() {
		for (CommunityOrderEntity order : orders) {
			switch (order.Status) {
				case 0:
					createdOrders.add(order);
					break;
				case 1:
					payedOrders.add(order);
					break;
				case 2:
					deliveringOrders.add(order);
					break;
				case 3:
					finishedOrders.add(order);
					break;
				default:
					break;
			}
		}
	}

	public void requestOrder(String userId) {
		Log.i(TAG, "requestData(");
		orders.clear();
		loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {

					JSONArray array = object.optJSONArray("List");
					if (array != null) {
						parseToOrderList(array);
					}
				} else {
					loading.setVisibility(View.GONE);
					Toast.makeText(MeOrderActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
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
				orders.clear();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject object = array.getJSONObject(i);
						CommunityOrderEntity order = om.readValue(object.toString(), CommunityOrderEntity.class);
						orders.add(order);
					} catch (JSONException e) {
						e.printStackTrace();
					} catch (JsonParseException e) {
						e.printStackTrace();
					} catch (JsonMappingException e) {
						e.printStackTrace();
					} catch (IOException e) {
						e.printStackTrace();
					}
				}
				return orders;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					loading.setVisibility(View.GONE);
					initList();
					notifyAllFragments();
				}

			}
		}).execute();
	}

	void notifyAllFragments() {
		PayFragment payFragment = (PayFragment) fragments.get(0);
		SendFragment sendFragment = (SendFragment) fragments.get(1);
		ReceiveFragment receiveFragment = (ReceiveFragment) fragments.get(2);
		CommentFragment commentFragment = (CommentFragment) fragments.get(3);

		payFragment.update();
		sendFragment.update();
		receiveFragment.update();
		commentFragment.update();
	}

	public List<CommunityOrderEntity> getOrderList(int item) {
		switch (item) {
			case 0:
				return createdOrders;
			case 1:
				return payedOrders;
			case 2:
				return deliveringOrders;
			case 3:
				return finishedOrders;

			default:
				break;
		}
		return null;
	}

	class MyPageChangeListener implements OnPageChangeListener {

		@Override
		public void onPageScrollStateChanged(int arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onPageScrolled(int arg0, float arg1, int arg2) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onPageSelected(int arg0) {
			for (TextView tv : textViews)
				tv.setBackgroundResource(R.drawable.white_tv_bg);
			textViews.get(arg0).setBackgroundResource(R.drawable.red_line_bg);
		}

	}

	class MyViewPagerAdapter extends FragmentPagerAdapter {

		public MyViewPagerAdapter(FragmentManager fm) {
			super(fm);
		}

		@Override
		public Fragment getItem(int arg0) {
			return fragments.get(arg0);
		}

		@Override
		public int getCount() {
			return fragments.size();
		}

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
		return true;
	}

	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.tv1:
			case R.id.tv2:
			case R.id.tv3:
			case R.id.tv4:

				for (TextView tv : textViews)
					tv.setBackgroundResource(R.drawable.white_tv_bg);
				v.setBackgroundResource(R.drawable.red_line_bg);
				int item = Integer.parseInt((String) v.getTag());
				vp.setCurrentItem(item);
				break;
		}
	}

}
