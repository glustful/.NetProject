/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: MainActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-7 下午4:51:39 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentActivity;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.View.OnTouchListener;
import android.view.ViewGroup;
import android.view.animation.AccelerateInterpolator;
import android.view.animation.Animation;
import android.view.animation.Animation.AnimationListener;
import android.view.animation.AnimationSet;
import android.view.animation.AnimationUtils;
import android.view.animation.LinearInterpolator;
import android.view.animation.ScaleAnimation;
import android.view.animation.TranslateAnimation;
import android.view.inputmethod.EditorInfo;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.TextView;
import android.widget.TextView.OnEditorActionListener;
import android.widget.Toast;

import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.db.dao.DBDao;
import com.yoopoon.market.domain.AreaEntity;
import com.yoopoon.market.fragment.CartFragment;
import com.yoopoon.market.fragment.MeFragment;
import com.yoopoon.market.fragment.ServeFragment;
import com.yoopoon.market.fragment.ShopFragment;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.Utils;
import com.yoopoon.market.view.LazyViewPager;
import com.yoopoon.market.view.LazyViewPager.OnPageChangeListener;

/**
 * @ClassName: MainActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-7 下午4:51:39
 */
@EActivity(R.layout.activity_main)
public class MainActivity extends FragmentActivity implements OnClickListener {
	private static final String TAG = "MainActivity";
	private Context mContext;
	@ViewById(R.id.vp)
	LazyViewPager vp;
	@ViewById(R.id.rg)
	RadioGroup rg;
	@ViewById(R.id.search_layout)
	View searchLayout;
	@ViewById(R.id.btn_select)
	Button btn_select;
	@ViewById(R.id.rightBtn)
	Button btn_category;
	@ViewById(R.id.ll_loading)
	LinearLayout ll_loading;
	@ViewById(R.id.tv_counts)
	TextView tv_counts;
	@ViewById(R.id.tv_shadow1)
	TextView tv_shadow1;
	@ViewById(R.id.rl_shadow2)
	View rl_shadow2;
	@ViewById(R.id.et_search)
	EditText et_search;
	int cartCount = 0;
	ImageView buyImg;
	ViewGroup anim_mask_layout;// 动画层
	private ShopFragment mShopFragment;
	private ImageView clearSearchImageView;//清除按钮
	//创建Watcher监听搜索框中内容的变化，如果变化则设置删除按钮显示以及其他功能
	private TextWatcher searchEditTextWatcher = new TextWatcher() {
		@Override
		public void onTextChanged(CharSequence s, int start, int before, int count) {
		}
		@Override
		public void beforeTextChanged(CharSequence s, int start, int count, int after) {
		}
		@Override
		public void afterTextChanged(Editable s) {
			if (!s.toString().trim().equals("")) {
				clearSearchImageView.setVisibility(View.VISIBLE);
			}
		}
	};

	@Click(R.id.btn_search)
	void search() {
		if (et_search.getVisibility() == View.GONE) {
			Animation animation = AnimationUtils.loadAnimation(this, R.anim.push_right_in);
			animation.setFillAfter(true);
			et_search.setVisibility(View.VISIBLE);
			et_search.startAnimation(animation);
			et_search.setImeActionLabel("搜索", EditorInfo.IME_ACTION_SEARCH);
			et_search.setSingleLine();
			et_search.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
			et_search.setOnEditorActionListener(new OnEditorActionListener() {
				@Override
				public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
					if (actionId == EditorInfo.IME_ACTION_SEARCH) {
						//Toast.makeText(mContext, "搜索" + v.getText().toString(), Toast.LENGTH_SHORT).show();
						searchProduct(v.getText().toString());
						return true;
					}
					return false;
				}
			});
		} else {
			Animation animation = AnimationUtils.loadAnimation(this, R.anim.back_right_out);
			animation.setAnimationListener(new AnimationListener() {
				@Override
				public void onAnimationStart(Animation animation) {
					// TODO Auto-generated method stub
				}
				@Override
				public void onAnimationRepeat(Animation animation) {
					// TODO Auto-generated method stub
				}
				@Override
				public void onAnimationEnd(Animation animation) {
					Intent intent = new Intent("com.yoopoon.market.search.byKeyword");
					Bundle bundle = new Bundle();
					intent.putExtra("keyword", et_search.getText().toString());
					sendBroadcast(intent);
					et_search.setText("");
					et_search.setVisibility(View.GONE);
				}
			});
			et_search.startAnimation(animation);
		}
	}
	@Click(R.id.iv_iknow)
	void iKnow() {
		SharedPreferences sp = getSharedPreferences(getString(R.string.share_preference), MODE_PRIVATE);
		Editor editor = sp.edit();
		editor.putBoolean("isFirst", false);
		editor.commit();
		tv_shadow1.setVisibility(View.GONE);
		rl_shadow2.setVisibility(View.GONE);
	}

	List<Fragment> fragments = new ArrayList<Fragment>();
	List<LinearLayout> lls = new ArrayList<LinearLayout>();
	int checkedItem = 0;
	List<AreaEntity> areaList = new ArrayList<AreaEntity>();
	String[] areaItems;

	@AfterViews
	void initUI() {
		//################################################徐阳会 2015年9月22日修改#########################Start
		//													修改shop_fragment,让ShopFragment成全局变量
		//################################################徐阳会 2015年9月22日修改#########################Start
		mShopFragment = new ShopFragment();
		mContext = MainActivity.this;
		fragments.add(mShopFragment);
		//################################################徐阳会 2015年9月22日修改#########################Start
		//													修改shop_fragment,让ShopFragment成全局变量
		//################################################徐阳会 2015年9月22日修改#########################Start
		fragments.add(new ServeFragment());
		fragments.add(new CartFragment());
		fragments.add(new MeFragment());
		vp.setAdapter(new MyPageAdapter(getSupportFragmentManager()));
		lls.add((LinearLayout) findViewById(R.id.ll1));
		lls.add((LinearLayout) findViewById(R.id.ll2));
		lls.add((LinearLayout) findViewById(R.id.ll3));
		lls.add((LinearLayout) findViewById(R.id.ll4));
		for (LinearLayout ll : lls)
			ll.setOnClickListener(this);
		btn_select.setOnClickListener(new SearchViewClickListener());
		btn_category.setOnClickListener(new SearchViewClickListener());
		vp.setOnPageChangeListener(new MyPagerChangeListener());
		// requestArea();
		new Thread() {
			public void run() {
				DBDao dao = new DBDao(mContext);
				cartCount = dao.getAllCounts();
				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						if (cartCount > 0) {
							tv_counts.setVisibility(View.VISIBLE);
							tv_counts.setText(cartCount + "");
						} else {
							tv_counts.setVisibility(View.GONE);
						}
					}
				});
			};
		}.start();
		//################################################徐阳会 2015年9月22日修改#########################Start
		//									添加搜索框中清除搜索功能
		//################################################徐阳会 2015年9月22日修改#########################Start
		clearSearchImageView = (ImageView) searchLayout.findViewById(R.id.img_clear_search);
		clearSearchImageView.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				Utils.hiddenSoftBorad(mContext);
				et_search.setText("");
				mShopFragment.settingClearSearch();
				clearSearchImageView.setVisibility(View.GONE);
				return true;
			}
		});
		et_search.addTextChangedListener(searchEditTextWatcher);
		//################################################徐阳会 2015年9月22日修改#########################Start
		//									添加搜索框中清除搜索功能
		//################################################徐阳会 2015年9月22日修改#########################Start
	}
	/**
	 * @Title: searchProduct
	 * @Description: 根据关键字搜索商品
	 */
	private void searchProduct(String searchString) {
		Utils.hiddenSoftBorad(mContext);
		if (searchString == null || searchString.equals("")) {
			Toast.makeText(mContext, "输入为空，请输入要搜索的商品", Toast.LENGTH_SHORT).show();
		} else {
			mShopFragment.searchProduct(searchString);
		}
	}
	protected void onCreate(android.os.Bundle arg0) {
		super.onCreate(arg0);
		// 清除密码
		SharedPreferences sp = getSharedPreferences(getString(R.string.share_preference), MODE_PRIVATE);
		Editor editor = sp.edit();
		editor.putString("Password", "");
		editor.putInt("UserId", 0);
		editor.commit();
		registerBroadcast();
	};
	void registerBroadcast() {
		// 展示第一次打开优服务的蒙层
		IntentFilter shadowFilter = new IntentFilter("com.yoopoon.market.show_shadow");
		shadowFilter.addCategory(Intent.CATEGORY_DEFAULT);
		registerReceiver(receiver, shadowFilter);
		// 添加商品到购物车
		IntentFilter cartFilter = new IntentFilter("com.yoopoon.market.add_to_cart");
		cartFilter.addCategory(Intent.CATEGORY_DEFAULT);
		registerReceiver(receiver, cartFilter);
		// 打开首页
		IntentFilter shopFilter = new IntentFilter("com.yoopoon.market.showshop");
		shopFilter.addCategory(Intent.CATEGORY_DEFAULT);
		registerReceiver(receiver, shopFilter);
		// 打开购物车
		IntentFilter cartFilter2 = new IntentFilter("com.yoopoon.market.showcart");
		shopFilter.addCategory(Intent.CATEGORY_DEFAULT);
		registerReceiver(receiver, cartFilter2);
		IntentFilter seviceFilter = new IntentFilter("com.yoopoon.market.service.moreservice");
		seviceFilter.addCategory(Intent.CATEGORY_DEFAULT);
		registerReceiver(receiver, seviceFilter);
	}

	BroadcastReceiver receiver = new BroadcastReceiver() {
		@Override
		public void onReceive(Context context, Intent intent) {
			String action = intent.getAction();
			if (action.equals("com.yoopoon.market.show_shadow")) {
				tv_shadow1.setVisibility(View.VISIBLE);
				rl_shadow2.setVisibility(View.VISIBLE);
			} else if (action.equals("com.yoopoon.market.add_to_cart")) {
				int[] start_loction = (int[]) intent.getExtras().get("start_location");
				ScaleAnimation sa = new ScaleAnimation(0.1f, 1.0f, 0.1f, 1f, Animation.RELATIVE_TO_SELF, 0.5f,
						Animation.RELATIVE_TO_SELF, 0.5f);
				sa.setDuration(300);
				LinearLayout ll = lls.get(2);
				sa.setFillAfter(false);
				ll.startAnimation(sa);
				play(start_loction);
			} else if (action.equals("com.yoopoon.market.showshop")) {
				vp.setCurrentItem(0);
			} else if (action.equals("com.yoopoon.market.showcart")) {
				vp.setCurrentItem(2);
			} else if (action.equals("com.yoopoon.market.service.moreservice")) {
				vp.setCurrentItem(1);
			}
		}
	};

	void play(int[] start_location) {
		buyImg = new ImageView(mContext);// buyImg是动画的图片，我的是一个小球（R.drawable.sign）
		buyImg.setImageResource(R.drawable.sign);// 设置buyImg的图片
		setAnim(buyImg, start_location);// 开始执行动画
	}
	private View addViewToAnimLayout(final ViewGroup vg, final View view, int[] location) {
		int x = location[0];
		int y = location[1];
		LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT,
				LinearLayout.LayoutParams.WRAP_CONTENT);
		lp.leftMargin = x;
		lp.topMargin = y;
		view.setLayoutParams(lp);
		return view;
	}
	private ViewGroup createAnimLayout() {
		ViewGroup rootView = (ViewGroup) this.getWindow().getDecorView();
		LinearLayout animLayout = new LinearLayout(this);
		LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MATCH_PARENT,
				LinearLayout.LayoutParams.MATCH_PARENT);
		animLayout.setLayoutParams(lp);
		animLayout.setId(Integer.MAX_VALUE);
		animLayout.setBackgroundResource(android.R.color.transparent);
		rootView.addView(animLayout);
		return animLayout;
	}
	private void setAnim(final View v, int[] start_location) {
		anim_mask_layout = null;
		anim_mask_layout = createAnimLayout();
		anim_mask_layout.addView(v);// 把动画小球添加到动画层
		final View view = addViewToAnimLayout(anim_mask_layout, v, start_location);
		int[] end_location = new int[2];// 这是用来存储动画结束位置的X、Y坐标
		lls.get(2).getLocationInWindow(end_location);// shopCart是那个购物车
		// 计算位移
		int endX = end_location[0] - start_location[0] + 30;// 动画位移的X坐标
		int endY = end_location[1] - start_location[1];// 动画位移的y坐标
		TranslateAnimation translateAnimationX = new TranslateAnimation(0, endX, 0, 0);
		translateAnimationX.setInterpolator(new LinearInterpolator());
		translateAnimationX.setRepeatCount(0);// 动画重复执行的次数
		translateAnimationX.setFillAfter(true);
		TranslateAnimation translateAnimationY = new TranslateAnimation(0, 0, 0, endY);
		translateAnimationY.setInterpolator(new AccelerateInterpolator());
		translateAnimationY.setRepeatCount(0);// 动画重复执行的次数
		translateAnimationX.setFillAfter(true);
		AnimationSet set = new AnimationSet(false);
		set.setFillAfter(false);
		set.addAnimation(translateAnimationY);
		set.addAnimation(translateAnimationX);
		set.setDuration(800);// 动画的执行时间
		view.startAnimation(set);
		// 动画监听事件
		set.setAnimationListener(new AnimationListener() {
			// 动画的开始
			@Override
			public void onAnimationStart(Animation animation) {
				v.setVisibility(View.VISIBLE);
			}
			@Override
			public void onAnimationRepeat(Animation animation) {
				// TODO Auto-generated method stub
			}
			// 动画的结束
			@Override
			public void onAnimationEnd(Animation animation) {
				v.setVisibility(View.GONE);
				tv_counts.setVisibility(View.VISIBLE);
				tv_counts.setText((++cartCount) + "");
			}
		});
	}
	void requestArea() {
		ll_loading.setVisibility(View.VISIBLE);
		new RequestAdapter() {
			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					JSONArray array = object.optJSONArray("List");
					if (array != null) {
						parseToObject(array);
					} else {
						Toast.makeText(MainActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
						ll_loading.setVisibility(View.GONE);
					}
				} else {
					Toast.makeText(MainActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
					ll_loading.setVisibility(View.GONE);
				}
			}
			@Override
			public void onProgress(ProgressMessage msg) {
			}
		}.setUrl(getString(R.string.url_area_get)).setRequestMethod(RequestMethod.eGet).notifyRequest();
	}
	void parseToObject(final JSONArray array) {
		new ParserJSON(new ParseListener() {
			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				for (int i = 0; i < array.length(); i++) {
					try {
						JSONObject areaObject = array.getJSONObject(i);
						try {
							areaList.add(om.readValue(areaObject.toString(), AreaEntity.class));
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
					} catch (JSONException e) {
						e.printStackTrace();
					}
				}
				return areaList;
			}
			@Override
			public void onComplete(Object parseResult) {
				areaItems = new String[areaList.size()];
				for (int i = 0; i < areaList.size(); i++) {
					AreaEntity entity = areaList.get(i);
					areaItems[i] = entity.Name;
				}
				ll_loading.setVisibility(View.GONE);
			}
		}).execute();
	}

	private class SearchViewClickListener implements OnClickListener {
		@Override
		public void onClick(View v) {
			switch (v.getId()) {
				case R.id.btn_select:
					SearchActivity_.intent(MainActivity.this).start();
					break;
				case R.id.rightBtn:
					TreeCategoryActivity_.intent(MainActivity.this).start();
					break;
				default:
					break;
			}
		}
	}

	private class MyPageAdapter extends FragmentPagerAdapter {
		public MyPageAdapter(FragmentManager fm) {
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

	private class MyPagerChangeListener implements OnPageChangeListener {
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
			onClick(lls.get(arg0));
			searchLayout.setVisibility((arg0 > 1) ? View.GONE : View.VISIBLE);
			if (arg0 != 1) {
				tv_shadow1.setVisibility(View.GONE);
				rl_shadow2.setVisibility(View.GONE);
			}
			btn_category.setVisibility(arg0 == 1 ? View.GONE : View.VISIBLE);
		}
	}

	@Override
	public void onBackPressed() {
		// super.onBackPressed();
		exit();
	}

	long exitTime;

	public void exit() {
		if ((System.currentTimeMillis() - exitTime) > 2000) {
			Toast.makeText(getApplicationContext(), "再按一次退出程序", Toast.LENGTH_SHORT).show();
			exitTime = System.currentTimeMillis();
		} else {
			finish();
			System.exit(0);
		}
	}
	public void toServe(View v) {
		vp.setCurrentItem(1);
	}
	@Override
	public void onClick(View v) {
		for (LinearLayout ll : lls) {
			RadioButton radioButton = (RadioButton) ll.getChildAt(0);
			TextView textView = (TextView) ll.getChildAt(1);
			radioButton.setChecked(false);
			textView.setTextColor(Color.GRAY);
		}
		LinearLayout ll = (LinearLayout) v;
		RadioButton radioButton = (RadioButton) ll.getChildAt(0);
		TextView textView = (TextView) ll.getChildAt(1);
		radioButton.setChecked(true);
		textView.setTextColor(Color.RED);
		switch (v.getId()) {
			case R.id.ll1:
				vp.setCurrentItem(0);
				break;
			case R.id.ll2:
				vp.setCurrentItem(1);
				break;
			case R.id.ll3:
				vp.setCurrentItem(2);
				break;
			case R.id.ll4:
				vp.setCurrentItem(3);
				break;
		}
	}
	@Override
	protected void onDestroy() {
		super.onDestroy();
		if (receiver != null) {
			this.unregisterReceiver(receiver);
		}
	}
}
