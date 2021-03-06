/**   

 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: RecommendActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-14 下午12:54:34 
 * @version: V1.0   
 */
package com.yoopoon.home;

import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import android.app.AlertDialog.Builder;
import android.app.Dialog;
import android.content.Context;
import android.graphics.Color;
import android.os.CountDownTimer;
import android.os.Handler;
import android.os.Vibrator;
import android.text.TextUtils;
import android.util.Log;
import android.view.Gravity;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import com.yoopoon.common.base.utils.RegxUtils;
import com.yoopoon.common.base.utils.SmsUtils;
import com.yoopoon.common.base.utils.SmsUtils.RequestSMSListener;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: RecommendActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午12:54:34
 */
@EActivity(R.layout.activity_irecommend)
public class IRecommendActivity extends MainActionBarActivity implements OnClickListener {
	@ViewById(R.id.bt_irecommend_add)
	Button btn_add;
	@ViewById(R.id.lv_irecommend)
	ListView lv;
	private MyPartnerListAdapter adapter;
	// private String[] names = { "钱德勒", "莫妮卡", "格蕾丝", "威尔", "Grace", "Will", "Chandler", "Rachel",
	// "Monica", "Ross",
	// "Mood", "莫德", "sue", "苏", "Moening", "莫宁", "Alice", "爱丽丝" };
	private static final String TAG = "IRecommendActivity";
	private String[] showNameList;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("推荐经纪人");
		btn_add.setOnClickListener(this);
		lv.setOnItemClickListener(new MyItemClickListener());
		// showNameList = SortNameByOrder.getShowNameList(names);
		// fillData();
	}

	private class MyItemClickListener implements OnItemClickListener {

		/*
		 * (non Javadoc)
		 * @Title: onItemClick
		 * @Description: TODO
		 * @param parent
		 * @param view
		 * @param position
		 * @param id
		 * @see
		 * android.widget.AdapterView.OnItemClickListener#onItemClick(android.widget.AdapterView,
		 * android.view.View, int, long)
		 */
		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

		}
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyPartnerListAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	private static class ViewHolder {
		TextView tv_name;
		ImageView iv_avater;
	}

	private class MyPartnerListAdapter extends BaseAdapter {
		/*
		 * (non Javadoc)
		 * @Title: getCount
		 * @Description: TODO
		 * @return
		 * @see android.widget.Adapter#getCount()
		 */
		@Override
		public int getCount() {
			return showNameList.length;
		}

		/*
		 * (non Javadoc)
		 * @Title: getItem
		 * @Description: TODO
		 * @param position
		 * @return
		 * @see android.widget.Adapter#getItem(int)
		 */
		@Override
		public Object getItem(int position) {
			// TODO Auto-generated method stub
			return position;
		}

		/*
		 * (non Javadoc)
		 * @Title: getItemId
		 * @Description: TODO
		 * @param position
		 * @return
		 * @see android.widget.Adapter#getItemId(int)
		 */
		@Override
		public long getItemId(int position) {
			// TODO Auto-generated method stub
			return 0;
		}

		/*
		 * (non Javadoc)
		 * @Title: getView
		 * @Description: TODO
		 * @param position
		 * @param convertView
		 * @param parent
		 * @return
		 * @see android.widget.Adapter#getView(int, android.view.View, android.view.ViewGroup)
		 */
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			String showName = showNameList[position];
			if (showName.startsWith("*****")) {
				TextView tv = new TextView(IRecommendActivity.this);
				tv.setText(String.valueOf(showName.charAt(showName.length() - 1)));
				tv.setPadding(10, 0, 0, 0);
				return tv;
			}
			ViewHolder holder = null;
			if (convertView == null || !(convertView instanceof LinearLayout))
				convertView = View.inflate(IRecommendActivity.this, R.layout.item_partner, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_patner_name);
				holder.iv_avater = (ImageView) convertView.findViewById(R.id.iv_partner_avater);
				convertView.setTag(holder);
			}
			holder.tv_name.setText(showName);
			return convertView;
		}
	}

	@Override
	public void backButtonClick(View v) {
		finish();
	}

	@Override
	public void titleButtonClick(View v) {
	}

	@Override
	public void rightButtonClick(View v) {
	}

	@Override
	public Boolean showHeadView() {
		return true;
	}

	/*
	 * (non Javadoc)
	 * @Title: onClick
	 * @Description: TODO
	 * @param v
	 * @see android.view.View.OnClickListener#onClick(android.view.View)
	 */
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
			case R.id.bt_irecommend_add:
				showAddPartnerDialog();
				break;
			case R.id.bt_partner_cancel:
				dialog.dismiss();
				break;
			case R.id.bt_partner_invite:
				String phone = et_phone.getText().toString().trim();
				if (TextUtils.isEmpty(phone)) {
					// shake
					Animation ta = AnimationUtils.loadAnimation(this, R.anim.shake);
					Vibrator vibrator = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
					et_phone.startAnimation(ta);
					vibrator.vibrate(500);
					ToastUtils.showToast(IRecommendActivity.this, "亲，你还没输入电话号码呢", 1000, Gravity.BOTTOM);
					return;
				} else {
					if (!RegxUtils.isPhone(phone)) {
						tv_warning.setVisibility(View.VISIBLE);
						tv_warning.setText("请输入正确的手机号码！");
						return;
					}
					recommendBroker(phone);
					setRecommendEnable(false);
					CountDownTimer timer = new CountDownTimer(60000, 1000) {
						@Override
						public void onTick(long millisUntilFinished) {
							bt_invite.setText("邀请(" + millisUntilFinished / 1000 + ")");
						}

						@Override
						public void onFinish() {
							setRecommendEnable(true);
						}
					};
					timer.start();
				}
				break;
		}
	}

	private void setRecommendEnable(boolean enable) {
		bt_invite.setEnabled(enable);
		bt_invite.setBackgroundResource(enable ? R.drawable.cycle_selector : R.drawable.btn_not_enable);
	}

	private int count = 1;
	private long startMills;
	Handler handler = new Handler();
	Timer timer;
	String[] points = { "", ".", "..", "..." };
	TimerTask task;

	private void recommendBroker(String phone) {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).start();
			return;
		} else {
			String smsType = String.valueOf(SmsUtils.RECOMMED_BROKER_IDENTIFY_CODE);
			String json = "{\"Mobile\":\"" + phone + "\",\"SmsType\":\"" + smsType + "\"}";
			startMills = System.currentTimeMillis();
			requesting();
			SmsUtils.requestIdentifyCode(this, json, listener);
		}
	}

	private void requesting() {
		timer = new Timer();
		task = new TimerTask() {
			@Override
			public void run() {
				runOnUiThread(new Runnable() {
					@Override
					public void run() {
						if (count == 5)
							count = 1;
						tv_warning.setVisibility(View.VISIBLE);
						tv_warning.setTextColor(Color.GRAY);
						tv_warning.setText("正在请求，请稍候" + points[count - 1]);
						count++;
					}
				});
			}
		};
		timer.schedule(task, 0, 600);
	}

	RequestSMSListener listener = new RequestSMSListener() {
		@Override
		public void succeed(final String code) {
			long duration = System.currentTimeMillis() - startMills;
			Log.i(TAG, "lis.succeed:" + code);
			if (duration < 2000) {
				handler.postDelayed(new Runnable() {
					@Override
					public void run() {
						timer.cancel();
						task.cancel();
						tv_warning.setVisibility(View.GONE);
						Toast.makeText(IRecommendActivity.this, code, Toast.LENGTH_SHORT).show();
					}
				}, 2000 - duration);
			} else {
				timer.cancel();
				task.cancel();
				tv_warning.setVisibility(View.GONE);
				Toast.makeText(IRecommendActivity.this, code, Toast.LENGTH_SHORT).show();
			}
		}

		@Override
		public void fail(final String msg) {
			Log.i(TAG, "lis.succeed:" + msg);
			long duration = System.currentTimeMillis() - startMills;
			if (duration < 2000) {
				handler.postDelayed(new Runnable() {
					@Override
					public void run() {
						timer.cancel();
						task.cancel();
						tv_warning.setTextColor(Color.RED);
						tv_warning.setText(msg);
						tv_warning.setVisibility(View.VISIBLE);
					}
				}, 2000 - duration);
			} else {
				timer.cancel();
				task.cancel();
				tv_warning.setTextColor(Color.RED);
				tv_warning.setText(msg);
				tv_warning.setVisibility(View.VISIBLE);
			}
		}
	};
	Button bt_cancel;
	Button bt_invite;
	Dialog dialog;
	EditText et_phone;
	TextView tv_warning;
	TextView tv_title;

	private void showAddPartnerDialog() {
		Builder builder = new Builder(IRecommendActivity.this);
		View addView = View.inflate(this, R.layout.dialog_addpartner, null);
		builder.setView(addView);
		bt_cancel = (Button) addView.findViewById(R.id.bt_partner_cancel);
		bt_invite = (Button) addView.findViewById(R.id.bt_partner_invite);
		et_phone = (EditText) addView.findViewById(R.id.et_partner_phone);
		tv_warning = (TextView) addView.findViewById(R.id.tv_addpartner_warning);
		tv_title = (TextView) addView.findViewById(R.id.tv_add_title);
		tv_title.setText("添加经纪人");
		dialog = builder.show();
		bt_cancel.setOnClickListener(this);
		bt_invite.setOnClickListener(this);
	}

	/*
	 * (non Javadoc)
	 * @Title: onDestroy
	 * @Description: TODO
	 * @see android.support.v4.app.FragmentActivity#onDestroy()
	 */
	@Override
	protected void onDestroy() {
		if (dialog != null)
			dialog.dismiss();
		super.onDestroy();
	}
}
