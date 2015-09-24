/**   
 * Copyright ? 2015 yoopoon. All rights reserved.
 * 
 * @Title: PersonalInfoActivity.java 
 * @Project: YoopoonMarket
 * @Package: com.yoopoon.market 
 * @Description: TODO
 * @author: guojunjun  
 * @updater: guojunjun 
 * @date: 2015-9-8 上午9:32:55 
 * @version: V1.0   
 */
package com.yoopoon.market;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.ViewById;
import org.json.JSONException;
import org.json.JSONObject;
import android.content.ContentResolver;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.net.Uri;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView.ScaleType;
import android.widget.RadioButton;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.makeramen.RoundedImageView;
import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.domain.MemberModel;
import com.yoopoon.market.domain.User;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.ParserJSON;
import com.yoopoon.market.utils.ParserJSON.ParseListener;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;
import com.yoopoon.market.utils.StringUtils;
import com.yoopoon.market.utils.UploadHeadImg;
import com.yoopoon.market.utils.UploadHeadImg.OnCompleteListener;
import com.yoopoon.market.utils.UploadHeadImg.OnProgressListener;

/**
 * @ClassName: PersonalInfoActivity
 * @Description: TODO
 * @author: guojunjun
 * @date: 2015-9-8 上午9:32:55
 */
@EActivity(R.layout.activity_modify_info)
public class PersonalInfoActivity extends MainActionBarActivity {
	private static final String TAG = "PersonalInfoActivity";
	@ViewById(R.id.et_name)
	EditText et_name;
	@ViewById(R.id.et_phone)
	EditText et_phone;
	@ViewById(R.id.et_postno)
	EditText et_postno;
	@ViewById(R.id.rb_male)
	RadioButton rb_male;
	@ViewById(R.id.rb_female)
	RadioButton rb_female;
	@ViewById(R.id.imageView1)
	RoundedImageView iv;
	@ViewById(R.id.tv_upload)
	TextView tv_upload;
	boolean uploadable = true;
	MemberModel member;

	@Click(R.id.imageView1)
	void selectAvater() {
		if (uploadable) {
			Intent intent = new Intent(Intent.ACTION_GET_CONTENT);
			intent.setType("image/*");
			this.startActivityForResult(intent, 1);
		}
	}

	@Click(R.id.btn_save)
	void save() {
		modify();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.GONE);
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);

		headView.setBackgroundColor(Color.RED);
		titleButton.setText("个人资料");
		titleButton.setTextColor(Color.WHITE);
	}

	@Override
	protected void onResume() {
		super.onResume();
		if (!User.isLogin(this)) {
			LoginActivity_.intent(this).start();
			return;
		}
		String userid = User.getUserId(this);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					parseToMember(object.toString());
				} else {
					// ll_loading.setVisibility(View.GONE);
					Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
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
					fillData();
				}
			}
		}).execute();
	}

	void fillData() {

		et_name.setText(member.RealName);
		et_phone.setText(member.Phone);
		et_postno.setText(member.PostNo);
		rb_male.setChecked((member.Gender == 0));
		rb_female.setChecked((member.Gender == 1));
		if (!TextUtils.isEmpty(member.Thumbnail)) {
			String imageUrl = getString(R.string.url_image) + member.Thumbnail;
			iv.setTag(imageUrl);
			ImageLoader.getInstance().displayImage(imageUrl, iv, MyApplication.getOptions(),
					MyApplication.getLoadingListener());
		}
	}

	void modify() {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					member.Phone = et_phone.getText().toString();
					member.PostNo = et_postno.getText().toString();
					member.RealName = et_name.getText().toString();
					member.Gender = rb_male.isChecked() ? 0 : 1;
					String json = om.writeValueAsString(member);
					return json;
				} catch (JsonProcessingException e) {
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				if (serializeResult == null || serializeResult.equals("")) {
					return;
				}
				requestModifyInfo(serializeResult);
			}
		}).execute();

	}

	void requestModifyInfo(String serializeResult) {
		Log.i(TAG, serializeResult);
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();// 修改成功，返回“我”
				}
				Toast.makeText(PersonalInfoActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {

			}
		}.setUrl(getString(R.string.url_member_modify)).setRequestMethod(RequestMethod.ePut).SetJSON(serializeResult)
				.notifyRequest();
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
						iv.setImageBitmap(bitmap);
						iv.setScaleType(ScaleType.CENTER_CROP);
						if (file.length() > 150000) {
							tv_upload.setText("图片太大啦，换一张小一点的吧");
						} else {
							uploadImage(file);
						}
					} else {
						tv_upload.setText("请选择正确的图片文件");
					}

				} catch (FileNotFoundException e) {
					Log.e("Exception", e.getMessage(), e);
				}
			}
		}
		super.onActivityResult(requestCode, resultCode, data);
	}

	String text = "上传失败><";

	void uploadImage(final File file) {
		final String path = getString(R.string.url_host) + getString(R.string.url_upload);
		tv_upload.setText("上传中..");
		uploadable = false;
		new Thread() {
			public void run() {
				new UploadHeadImg().post(path, file, new OnProgressListener() {

					@Override
					public void onProgress(int progress) {
						// TODO Auto-generated method stub

					}
				}, new OnCompleteListener() {

					@Override
					public void onSuccess(String json) {
						uploadable = true;
						try {
							JSONObject object = new JSONObject(json);
							boolean status = object.optBoolean("Status", false);
							if (status) {
								text = "上传成功^^";
								member.Thumbnail = object.optString("Msg", "");
							}
							runOnUiThread(new Runnable() {

								@Override
								public void run() {
									tv_upload.setText(text);
								}
							});
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						}

					}

					@Override
					public void onFailed() {
						uploadable = true;
						Log.i(TAG, "onFailed()");
						runOnUiThread(new Runnable() {

							@Override
							public void run() {
								tv_upload.setText("上传失败><");
							}
						});
					}
				});
			};
		}.start();
	}

	File getFileByUri(Uri uri) {
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

}
