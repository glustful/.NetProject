/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PersonSettingActivity.java 
 * @Project: yoopoon
 * @Package: com.yoopoon.home.ui.me 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-7-7 下午3:19:15 
 * @version: V1.0   
 */
package com.yoopoon.home.ui.me;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.TextChange;
import org.androidannotations.annotations.ViewById;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.ContentResolver;
import android.content.Intent;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.net.Uri;
import android.os.Vibrator;
import android.preference.PreferenceManager;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.makeramen.RoundedImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.common.base.Tools;
import com.yoopoon.common.base.utils.RegxUtils;
import com.yoopoon.common.base.utils.StringUtils;
import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.MainActionBarActivity;
import com.yoopoon.home.MyApplication;
import com.yoopoon.home.R;
import com.yoopoon.home.data.json.ParserJSON;
import com.yoopoon.home.data.json.ParserJSON.ParseListener;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.UploadHeadImg;
import com.yoopoon.home.data.net.UploadHeadImg.OnCompleteListener;
import com.yoopoon.home.data.user.User;
import com.yoopoon.home.domain.Broker2.RequesListener;
import com.yoopoon.home.domain.BrokerEntity;
import com.yoopoon.home.ui.login.HomeLoginActivity_;

/**
 * @ClassName: PersonSettingActivity
 * @Description: 个人资料设置
 * @author: guojunjun
 * @date: 2015-7-7 下午3:19:15
 */
@EActivity(R.layout.setting_person_view)
public class PersonSettingActivity extends MainActionBarActivity {
	private static final String TAG = "PersonSettingActivity";
	@ViewById(R.id.name)
	EditText et_name;
	@ViewById(R.id.rg_person_setting_sex)
	RadioGroup rg_sex;
	@ViewById(R.id.card)
	EditText et_card;
	@ViewById(R.id.email)
	EditText et_email;
	@ViewById(R.id.rb_person_setting_male)
	RadioButton rb_male;
	@ViewById(R.id.rb_person_setting_female)
	RadioButton rb_female;
	@ViewById(R.id.nickname)
	TextView et_nickname;
	@ViewById(R.id.tv_person_setting_phone)
	TextView tv_phone;
	@ViewById(R.id.iv_person_setting_avater)
	RoundedImageView iv_avater;
	@ViewById(R.id.tv_person_setting_uploading)
	TextView tv_uploading;
	@ViewById(R.id.rl_person_setting_progress)
	RelativeLayout rl_progress;
	@ViewById(R.id.ll_progress)
	LinearLayout ll_progress;
	@ViewById(R.id.weixin)
	EditText et_weixin;
	@ViewById(R.id.tv_person_setting_name_warning)
	TextView tv_warning_name;
	@ViewById(R.id.tv_person_setting_email_warning)
	TextView tv_warning_email;
	@ViewById(R.id.tv_person_setting_sfz_warning)
	TextView tv_warning_sfz;
	@ViewById(R.id.invite_code)
	EditText et_code;
	private Animation animation_shake;
	private Vibrator vibrator;
	private BrokerEntity entity;
	private String[] points = { "", ".", "..", "..." };
	private int count = 0;
	private Timer timer;
	private TimerTask task;
	private boolean uploadable = true;
	private User user;

	@TextChange(R.id.card)
	void cardChange() {
		tv_warning_sfz.setVisibility(View.GONE);
	}

	@TextChange(R.id.name)
	void nameChange() {
		tv_warning_name.setVisibility(View.GONE);
	}

	@TextChange(R.id.email)
	void mailChange() {
		tv_warning_email.setVisibility(View.GONE);
	}

	@Click(R.id.iv_person_setting_avater)
	void selectAvater() {
		if (uploadable) {
			Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
			intent.setType("image/*");
			this.startActivityForResult(intent, 1);
		}
	}

	@Click(R.id.save)
	void modifyBrokerInfo() {
		User user = User.lastLoginUser(this);
		if (user == null) {
			HomeLoginActivity_.intent(this).isManual(true).start();
			return;
		}
		String name = et_name.getText().toString();
		String sfz = et_card.getText().toString();
		String email = et_email.getText().toString();
		String nickname = et_nickname.getText().toString();
		String phone = tv_phone.getText().toString();
		if (TextUtils.isEmpty(name)) {
			textWarning(et_name);
			tv_warning_name.setText("请填写姓名");
			tv_warning_name.setVisibility(View.VISIBLE);
			return;
		}
		if (TextUtils.isEmpty(email)) {
			textWarning(et_email);
			tv_warning_email.setText("请填写邮箱");
			tv_warning_email.setVisibility(View.VISIBLE);
			return;
		}
		if (TextUtils.isEmpty(sfz)) {
			textWarning(et_card);
			tv_warning_sfz.setText("请填写身份证号码");
			tv_warning_sfz.setVisibility(View.VISIBLE);
			return;
		}
		if (!RegxUtils.isSfz(sfz)) {
			textWarning(et_card);
			tv_warning_sfz.setText("请填写正确的身份证号码");
			tv_warning_sfz.setVisibility(View.VISIBLE);
			return;
		}

		if (!RegxUtils.isEmail(email)) {
			textWarning(et_email);
			tv_warning_email.setText("请填写正确的邮箱");
			tv_warning_email.setVisibility(View.VISIBLE);
			return;
		}
		rl_progress.setVisibility(View.VISIBLE);
		String sexy = rb_female.isChecked() ? "女士" : "先生";
		entity.setRealname(name);
		entity.setBrokername(name);
		entity.setNickname(nickname);
		entity.setSfz(sfz);
		entity.setPhone(phone);
		entity.setEmail(email);
		entity.setSexy(sexy);
		entity.setHeadphoto(user.getHeadUrl());
		entity.setInviteCode(et_code.getText().toString());
		entity.setWeiXinNumber(et_weixin.getText().toString());

		entity.modifyInfo(new RequesListener() {

			@Override
			public void succeed(String msg) {
				rl_progress.setVisibility(View.GONE);
				Toast.makeText(PersonSettingActivity.this, "修改成功", Toast.LENGTH_SHORT).show();
				finish();
			}

			@Override
			public void fail(String msg) {
				rl_progress.setVisibility(View.GONE);
				Toast.makeText(PersonSettingActivity.this, msg, Toast.LENGTH_SHORT).show();
			}
		});
	}

	private void textWarning(View v) {
		v.startAnimation(animation_shake);
		vibrator.vibrate(500);
	}

	private void parseToBroker(final String json) {
		new ParserJSON(new ParseListener() {

			@Override
			public Object onParse() {
				ObjectMapper om = new ObjectMapper();
				BrokerEntity entity = null;
				try {
					entity = om.readValue(json, BrokerEntity.class);
				} catch (JsonParseException e) {
					e.printStackTrace();
				} catch (JsonMappingException e) {
					e.printStackTrace();
				} catch (IOException e) {
					e.printStackTrace();
				}
				return entity;
			}

			@Override
			public void onComplete(Object parseResult) {
				if (parseResult != null) {
					entity = (BrokerEntity) parseResult;
					et_card.setText(entity.getSfz());
					et_email.setText(entity.getEmail());
					et_name.setText(entity.getRealname());
					et_nickname.setText(entity.getNickname());
					tv_phone.setText(entity.getPhone());
					et_weixin.setText(entity.getWeiXinNumber());
					String photo = entity.getHeadphoto();
					if (!TextUtils.isEmpty(photo)) {
						String url = "http://img.yoopoon.com/" + photo;
						ImageLoader.getInstance().displayImage(url, iv_avater);
					}
				}
				ll_progress.setVisibility(View.GONE);
			}
		}).execute();
	}

	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		if (resultCode == RESULT_OK) {
			Uri uri = data.getData();
			if (uri != null) {
				ContentResolver cr = this.getContentResolver();
				try {
					File file = null;
					if (uri.toString().startsWith("content"))
						file = getFileByUri(uri);
					else {
						String path = uri.toString().substring(7);
						file = new File(path);
					}
					if (StringUtils.isPicFile(file)) {
						Bitmap bitmap = BitmapFactory.decodeStream(cr.openInputStream(uri));
						/* 将Bitmap设定到ImageView */
						iv_avater.setImageBitmap(bitmap);
						if (file.length() > 150000) {
							tv_uploading.setTextColor(Color.BLACK);
							tv_uploading.setText("图片太大啦，换一张小一点的吧");
							tv_uploading.setVisibility(View.VISIBLE);
						} else
							uploadImage(file);
					} else {
						tv_uploading.setTextColor(Color.RED);
						tv_uploading.setText("请选择正确的图片文件");
						tv_uploading.setVisibility(View.VISIBLE);
					}
				} catch (FileNotFoundException e) {
					Log.e("Exception", e.getMessage(), e);
				}
			}
		}
		super.onActivityResult(requestCode, resultCode, data);
	}

	private File getFileByUri(Uri uri) {
		try {
			String[] proj = { MediaStore.Images.Media.DATA };
			Cursor cursor = managedQuery(uri, proj, null, null, null);
			if (cursor != null) {
				int actual_image_column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
				cursor.moveToFirst();
				String img_path = cursor.getString(actual_image_column_index);
				File file = new File(img_path);
				// Uri fileUri = Uri.fromFile(file);
				return file;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return null;
	}

	private void uploadImage(final File file) {
		uploadable = false;
		final String path = getString(R.string.url_host) + getString(R.string.url_upload);
		timer = new Timer();
		task = new TimerTask() {

			@Override
			public void run() {
				runOnUiThread(new Runnable() {

					@Override
					public void run() {
						if (count == 4)
							count = 0;
						tv_uploading.setText("上传中" + points[count++]);
						tv_uploading.setVisibility(View.VISIBLE);
					}
				});
			}
		};
		timer.scheduleAtFixedRate(task, 0, 300);
		new Thread() {

			public void run() {
				new UploadHeadImg().post(path, file, null, new OnCompleteListener() {

					@Override
					public void onSuccess(final String json) {
						uploadable = true;
						runOnUiThread(new Runnable() {

							@Override
							public void run() {
								// {"Status":true,"Msg":"20150722/20150722_175858_383_288.jpg","Object":null}
								try {
									JSONObject obj = new JSONObject(json);
									boolean status = Tools.optBoolean(obj, "Status", false);
									if (!status) {
										tv_uploading.setText("上传失败>_<,请重试");
										tv_uploading.setVisibility(View.VISIBLE);
									} else {
										tv_uploading.setText("上传成功^_^");
										tv_uploading.setVisibility(View.VISIBLE);
										String headUrl = Tools.optString(obj, "Msg", user.getHeadUrl());
										entity.Headphoto = headUrl;
										user.setHeadUrl(headUrl);
									}
								} catch (JSONException e) {
									e.printStackTrace();
								} finally {
									cancelTimer();
								}
							}
						});
					}

					@Override
					public void onFailed() {
						uploadable = true;
						runOnUiThread(new Runnable() {

							@Override
							public void run() {
								tv_uploading.setText("上传失败>_<,请重试");
								tv_uploading.setVisibility(View.VISIBLE);
								cancelTimer();
							}
						});
					}
				});
			};
		}.start();
	}

	private void cancelTimer() {
		if (timer != null)
			timer.cancel();
		if (task != null)
			task.cancel();
	}

	/**
	 * @Title: initUI
	 * @Description: 初始化界面
	 */
	@AfterViews
	void initUI() {
		// setEnable();
		backButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		backButton.setText("返回");
		backButton.setTextColor(Color.WHITE);
		titleButton.setText("个人设置");

		animation_shake = AnimationUtils.loadAnimation(this, R.anim.shake);
		vibrator = (Vibrator) getSystemService(VIBRATOR_SERVICE);

		user = User.lastLoginUser(this);

		requestInfo();
	}

	void requestInfo() {
		ll_progress.setVisibility(View.VISIBLE);
		SharedPreferences sp = PreferenceManager.getDefaultSharedPreferences(MyApplication.getInstance());
		String userId = sp.getString("userId", "0");
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				String json = data.getMRootData().toString();
				parseToBroker(json);
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub
			}
		}.setUrl(getString(R.string.url_brokeInfo_getBrokeInfoById) + userId).setRequestMethod(RequestMethod.eGet)
				.notifyRequest();
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

	@Override
	protected void onDestroy() {
		if (timer != null && task != null) {
			timer.cancel();
			task.cancel();
			timer = null;
			task = null;
		}
		super.onDestroy();
	}

	@Override
	protected void activityYMove() {
		Utils.hiddenSoftBorad(this);
	}
}
