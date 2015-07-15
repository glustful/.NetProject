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

import java.util.Arrays;
import java.util.Comparator;

import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;

import android.app.AlertDialog.Builder;
import android.app.Dialog;
import android.preference.PreferenceManager;
import android.text.TextUtils;
import android.view.LayoutInflater;
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

import com.yoopoon.home.data.user.User;
import com.yoopoon.home.data.user.User.InvitePartnerListener;
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
	View addView;
	private MyPartnerListAdapter adapter;
	private String[] names = { "钱德勒", "莫妮卡", "格蕾丝", "威尔", "Grace", "Will", "Chandler", "Rachel", "Monica", "Ross",
			"Mood" };
	private int[] num = new int[26];
	private static final String TAG = "IPartnerActivity";
	private static int currentLetterLine = 0;
	private char currentShowChar = 'A';
	private int totalLetterLines;
	private CharacterParser parser = new CharacterParser();

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		titleButton.setText("添加合伙人");
		addView = LayoutInflater.from(this).inflate(R.layout.dialog_addpartner, null);
		btn_add.setOnClickListener(this);
		lv.setOnItemClickListener(new MyItemClickListener());
		setTotalLetterLines();
		testNames();
		fillData();
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
			PartnerDetailActivity_.intent(IPartnerActivity.this).start();
		}
	}

	private void testNames() {
		Arrays.sort(names, comparator);
	}

	Comparator<String> comparator = new Comparator<String>() {
		@Override
		public int compare(String lhs, String rhs) {
			String lHs = parser.getSelling(lhs).toUpperCase();
			String rHs = parser.getSelling(rhs).toUpperCase();
			if (lHs.charAt(0) > rHs.charAt(0))
				return 1;
			else
				return lHs.charAt(0) < rHs.charAt(0) ? -1 : 0;
		}
	};

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
			return names.length + totalLetterLines;
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
			if (position == 0) {
				TextView tv = new TextView(IPartnerActivity.this);
				tv.setText(String.valueOf(names[0].charAt(0)));
				tv.setPadding(10, 0, 0, 0);
				String upperName = parser.getSelling(names[0]).toUpperCase();
				currentShowChar = upperName.charAt(0);
				currentLetterLine++;
				return tv;
			} else {
				String upperName = parser.getSelling(names[position - currentLetterLine]).toUpperCase();
				char c = upperName.charAt(0);
				if (c != currentShowChar) {
					TextView tv = new TextView(IPartnerActivity.this);
					tv.setText(String.valueOf(c));
					tv.setPadding(10, 0, 0, 0);
					currentShowChar = upperName.charAt(0);
					currentLetterLine++;
					return tv;
				}
			}
			ViewHolder holder = null;
			if (convertView == null || convertView instanceof LinearLayout)
				convertView = View.inflate(IPartnerActivity.this, R.layout.item_partner, null);
			holder = (ViewHolder) convertView.getTag();
			if (holder == null) {
				holder = new ViewHolder();
				holder.tv_name = (TextView) convertView.findViewById(R.id.tv_patner_name);
				holder.iv_avater = (ImageView) convertView.findViewById(R.id.iv_partner_avater);
				convertView.setTag(holder);
			}
			holder.tv_name.setText(names[position - currentLetterLine]);
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
					et_phone.startAnimation(ta);
					Toast.makeText(this, "亲，你还没输入电话呢！", Toast.LENGTH_SHORT).show();
					return;
				} else {
					invitePartner(phone);
				}
				break;
		}
	}
	private void invitePartner(String phone) {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).start();
			return;
		} else {
			String userId = PreferenceManager.getDefaultSharedPreferences(this).getString("userId", null);
			user.invite(new Broker("", 0, phone, Integer.parseInt(userId), 0, 0), new InvitePartnerListener() {
				@Override
				public void success() {
					Toast.makeText(IPartnerActivity.this, "恭喜你，邀请成功啦！", Toast.LENGTH_SHORT).show();
					dialog.dismiss();
				}
				@Override
				public void failed(String msg) {
					Toast.makeText(IPartnerActivity.this, msg, Toast.LENGTH_SHORT).show();
				}
			});
		}
	}

	Button cancel;
	Button invite;
	Dialog dialog;
	EditText et_phone;

	private void showAddPartnerDialog() {
		Builder builder = new Builder(IPartnerActivity.this);
		builder.setView(addView);
		cancel = (Button) addView.findViewById(R.id.bt_partner_cancel);
		invite = (Button) addView.findViewById(R.id.bt_partner_invite);
		et_phone = (EditText) addView.findViewById(R.id.et_partner_phone);
		dialog = builder.show();
		cancel.setOnClickListener(this);
		invite.setOnClickListener(this);
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
	private void setTotalLetterLines() {
		int[] nums = new int[26];
		int count = 0;
		for (String name : names) {
			String newName = parser.getSelling(name);
			char c = newName.toUpperCase().charAt(0);
			int index = c - 65;
			nums[index]++;
		}
		for (int num : nums) {
			if (num > 0)
				count++;
		}
		this.totalLetterLines = count;
	}
}
