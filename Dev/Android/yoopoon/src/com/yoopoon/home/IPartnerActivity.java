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

import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONArray;
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
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.utils.RegxUtils;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.common.base.utils.ToastUtils;
import com.yoopoon.home.data.json.ParserJSON;
import com.yoopoon.home.data.json.ParserJSON.ParseListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.InvitePartnerListener;
import com.yoopoon.home.domain.Broker;
import com.yoopoon.home.domain.Invitation;
import com.yoopoon.home.domain.Partner;
import com.yoopoon.home.domain.PartnerList;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: RecommendActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-7-14 下午12:54:34
 */
@EActivity(R.layout.activity_partner)
public class IPartnerActivity extends MainActionBarActivity implements OnClickListener {
	@ViewById(R.id.bt_partner_add)
	Button btn_add;
	@ViewById(R.id.lv_partner)
	ListView lv;
	TextView tv_warning;
	private static final String TAG = "IPartnerActivity";
	private List<Partner> partners = new ArrayList<Partner>();
	private List<Invitation> invitations = new ArrayList<Invitation>();
	private MyListAdapter adapter;

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("我的合伙人");
		btn_add.setOnClickListener(this);
		lv.setOnItemClickListener(new MyItemClickListener());
		requestList();
	}

	private class MyItemClickListener implements OnItemClickListener {

		@Override
		public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
			if (position < partners.size()) {
				String partnerId = String.valueOf(partners.get(position).PartnerId);
				PartnerDetailActivity_.intent(IPartnerActivity.this).id(partnerId).start();
			}
		}
	}

	private void fillData() {
		if (adapter == null) {
			adapter = new MyListAdapter();
			lv.setAdapter(adapter);
		} else {
			adapter.notifyDataSetChanged();
		}
	}

	static class PartnerHolder {
		ImageView iv_avater;
		TextView tv_name;
	}

	static class InvitationHolder {
		ImageView iv_avater;
		TextView tv_phone;
		Button btn_agree;
		Button btn_ignore;
	}

	private class MyListAdapter extends BaseAdapter {

		@Override
		public int getCount() {
			return partners.size() + invitations.size() + 1;
		}

		@Override
		public Object getItem(int position) {
			return null;
		}

		@Override
		public long getItemId(int position) {
			return 0;
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			if (position == partners.size()) {
				TextView tv = new TextView(IPartnerActivity.this);
				tv.setPadding(10, 5, 0, 5);
				tv.setTextColor(Color.RED);
				tv.setBackgroundColor(Color.WHITE);
				tv.setText("收到的邀请(" + invitations.size() + ")");
				tv.setTextSize(20);
				return tv;
			}
			// 合伙人列表
			if (position < partners.size()) {
				if (convertView == null || !(convertView instanceof RelativeLayout))
					convertView = View.inflate(IPartnerActivity.this, R.layout.item_partner, null);
				PartnerHolder partnerHolder = (PartnerHolder) convertView.getTag();
				if (partnerHolder == null) {
					partnerHolder = new PartnerHolder();
					partnerHolder.iv_avater = (ImageView) convertView.findViewById(R.id.iv_partner_avater);
					partnerHolder.tv_name = (TextView) convertView.findViewById(R.id.tv_patner_name);
					convertView.setTag(partnerHolder);
				}
				Partner entity = partners.get(position);
				String photo = entity.Headphoto;
				if (!StringUtils.isEmpty(photo)) {
					String url = "http://img.yoopoon.com/" + photo;
					ImageLoader.getInstance().displayImage(url, partnerHolder.iv_avater);
				}
				partnerHolder.tv_name.setText(entity.Name);

				return convertView;
			}

			// 邀请列表
			if (convertView == null || !(convertView instanceof LinearLayout))
				convertView = View.inflate(IPartnerActivity.this, R.layout.item_invitation, null);
			InvitationHolder invitationHolder = (InvitationHolder) convertView.getTag();
			if (invitationHolder == null) {
				invitationHolder = new InvitationHolder();
				invitationHolder.iv_avater = (ImageView) convertView.findViewById(R.id.iv_invitation);
				invitationHolder.tv_phone = (TextView) convertView.findViewById(R.id.tv_invitation_phone);
				convertView.setTag(invitationHolder);
			}
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
			case R.id.bt_partner_add:
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
					ToastUtils.showToast(IPartnerActivity.this, "亲，你还没输入电话号码呢", 1000, Gravity.BOTTOM);
					return;
				} else {
					if (!RegxUtils.isPhone(phone)) {
						tv_warning.setText("请输入正确的手机号码！");
						tv_warning.setVisibility(View.VISIBLE);
						return;
					}
					inviting();
					startMills = System.currentTimeMillis();
					invitePartner(phone);
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

	private Timer timer;
	private TimerTask task;
	private int count = 1;
	private String[] points = { "", ".", "..", "..." };
	private long startMills;
	private Handler handler = new Handler();

	private void inviting() {
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

	private void invitePartner(String phone) {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		} else {
			int userId = user.getId();
			String userName = user.getUserName();
			user.invite(new PartnerList(user.getId(), new Broker(userId, user.getPhone(), userId, userId, 0), 0,
					userName, phone), new InvitePartnerListener() {
				@Override
				public void success(final String msg) {
					long duration = System.currentTimeMillis() - startMills;
					if (duration < 2000) {
						handler.postDelayed(new Runnable() {
							@Override
							public void run() {
								responseSucceed(msg);
							}
						}, 2000 - duration);
					} else {
						responseSucceed(msg);
					}
				}

				@Override
				public void failed(final String msg) {
					long duration = System.currentTimeMillis() - startMills;
					if (duration < 2000) {
						handler.postDelayed(new Runnable() {
							@Override
							public void run() {
								responseFail(msg);
							}
						}, 2000 - duration);
					} else {
						responseFail(msg);
					}
				}
			});
		}
	}

	private void responseFail(String msg) {
		timer.cancel();
		task.cancel();
		Toast.makeText(IPartnerActivity.this, msg, Toast.LENGTH_SHORT).show();
	}

	private void responseSucceed(String msg) {
		timer.cancel();
		task.cancel();
		if (msg.contains("成功")) {
			tv_warning.setVisibility(View.GONE);
			Toast.makeText(IPartnerActivity.this, msg, Toast.LENGTH_SHORT).show();
			dialog.dismiss();
		} else {
			tv_warning.setTextColor(Color.RED);
			tv_warning.setText(msg);
			tv_warning.setVisibility(View.VISIBLE);
		}
	}

	Button cancel;
	Button bt_invite;
	Dialog dialog;
	EditText et_phone;

	private void showAddPartnerDialog() {
		Builder builder = new Builder(IPartnerActivity.this);
		View addView = View.inflate(this, R.layout.dialog_addpartner, null);
		builder.setView(addView);
		cancel = (Button) addView.findViewById(R.id.bt_partner_cancel);
		bt_invite = (Button) addView.findViewById(R.id.bt_partner_invite);
		et_phone = (EditText) addView.findViewById(R.id.et_partner_phone);
		tv_warning = (TextView) addView.findViewById(R.id.tv_addpartner_warning);
		dialog = builder.show();
		cancel.setOnClickListener(this);
		bt_invite.setOnClickListener(this);
	}

	void requestList() {
		User user = User.lastLoginUser(this);
		String brokerId = String.valueOf(user.id);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				boolean status = data.getMRootData().optBoolean("Status", false);
				if (status) {
					JSONArray list = data.getMRootData().optJSONArray("list");
					parseInvitations(list);
				} else {
					requestPartners();
				}
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_partner_list)).addParam("brokerId", brokerId)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	private void parseInvitations(final JSONArray list) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				try {
					ObjectMapper om = new ObjectMapper();

					for (int i = 0; i < list.length(); i++) {
						String json = list.getJSONObject(i).toString();
						Invitation invitation = om.readValue(json, Invitation.class);
						invitations.add(invitation);
					}

				} catch (Exception e) {
					e.printStackTrace();
				}
				return invitations;
			}

			@Override
			public void onComplete(Object parseResult) {
				Log.i(TAG, parseResult.toString());
				requestPartners();

			}
		}).execute();
	}

	void requestPartners() {
		User user = User.lastLoginUser(this);
		String brokerId = String.valueOf(user.id);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				Log.i(TAG, data.toString());
				JSONArray list = data.getMRootData().optJSONArray("partnerList");
				parsePartners(list);
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_get_ipartner_list)).addParam("userId", brokerId)
				.setRequestMethod(RequestMethod.eGet).notifyRequest();
	}

	private void parsePartners(final JSONArray list) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				try {
					ObjectMapper om = new ObjectMapper();
					for (int i = 0; i < list.length(); i++) {
						String json = list.getJSONObject(i).toString();
						Partner partner = om.readValue(json, Partner.class);
						partners.add(partner);
					}
				} catch (Exception e) {
					e.printStackTrace();
				}
				return partners;
			}

			@Override
			public void onComplete(Object parseResult) {
				Log.i(TAG, parseResult.toString());
				fillData();

			}
		}).execute();
	}

	/*
	 * (non Javadoc)
	 * @Title: onDestroy
	 * @Description: TODO
	 * @see android.support.v4.app.FragmentActivity#onDestroy()
	 */
	@Override
	protected void onDestroy() {
		if (dialog != null) {
			dialog.dismiss();
			dialog = null;
		}
		if (timer != null) {
			timer.cancel();
			timer = null;
		}
		if (task != null) {
			task.cancel();
			task = null;
		}
		super.onDestroy();
	}
}
