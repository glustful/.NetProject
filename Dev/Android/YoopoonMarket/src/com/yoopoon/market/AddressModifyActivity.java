package com.yoopoon.market;

import java.util.ArrayList;
import java.util.List;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.Click;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.json.JSONObject;
import android.graphics.Color;
import android.text.Spannable;
import android.text.SpannableStringBuilder;
import android.text.style.AbsoluteSizeSpan;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.yoopoon.market.domain.MemberAddressEntity;
import com.yoopoon.market.net.ProgressMessage;
import com.yoopoon.market.net.RequestAdapter;
import com.yoopoon.market.net.RequestAdapter.RequestMethod;
import com.yoopoon.market.net.ResponseData;
import com.yoopoon.market.utils.SerializerJSON;
import com.yoopoon.market.utils.SerializerJSON.SerializeListener;
import com.yoopoon.market.utils.Utils;

@EActivity(R.layout.activity_new_address)
public class AddressModifyActivity extends MainActionBarActivity {
	private static final String TAG = "AddressModifyActivity";
	@Extra
	MemberAddressEntity addressEntity;
	List<TextView> textViews = new ArrayList<TextView>();

	@Click(R.id.btn_save)
	void modify() {
		new SerializerJSON(new SerializeListener() {

			@Override
			public String onSerialize() {
				ObjectMapper om = new ObjectMapper();
				try {
					addressEntity.Tel = "456258963";
					String json = om.writeValueAsString(addressEntity);
					return json;
				} catch (JsonProcessingException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				return null;
			}

			@Override
			public void onComplete(String serializeResult) {
				Log.i(TAG, serializeResult);
				requestModify(serializeResult);
			}
		}).execute();
	}

	@AfterViews
	void initUI() {
		backButton.setVisibility(View.GONE);
		titleButton.setVisibility(View.VISIBLE);
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setText("修改地址");
		titleButton.setTextColor(Color.WHITE);
		headView.setBackgroundColor(Color.RED);
		rightButton.setVisibility(View.GONE);
		init();
	}

	void init() {
		textViews.add((TextView) findViewById(R.id.tv1));
		textViews.add((TextView) findViewById(R.id.tv2));
		// Address
		textViews.add((TextView) findViewById(R.id.tv3));
		// Linkman
		textViews.add((TextView) findViewById(R.id.tv4));
		// Tel
		textViews.add((TextView) findViewById(R.id.tv5));
		// Zip
		textViews.add((TextView) findViewById(R.id.tv6));

		textViews.get(2).setText(addressEntity.Address + "\n修改详细地址");
		textViews.get(3).setText(addressEntity.Linkman + "\n修改收货人");
		textViews.get(4).setText(addressEntity.Tel + "\n修改联系电话");
		textViews.get(5).setText(addressEntity.Zip + "\n修改邮编");

		Utils.spanTextSize(textViews.get(2), "修", true, new int[] { 16, 12 });

	}

	void spanTextSize(TextView textView, String split, boolean former, int[] nums) {
		String text = textView.getText().toString();
		SpannableStringBuilder builder = new SpannableStringBuilder(text);

		String[] price = text.split(split);
		AbsoluteSizeSpan largeSizeSpan = new AbsoluteSizeSpan(nums[0]);
		AbsoluteSizeSpan smallSizeSpan = new AbsoluteSizeSpan(nums[1]);

		if (former) {
			builder.setSpan(largeSizeSpan, 0, price[0].length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
			builder.setSpan(smallSizeSpan, price[0].length(), text.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		} else {
			builder.setSpan(smallSizeSpan, 0, price[0].length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
			builder.setSpan(largeSizeSpan, price[0].length(), text.length(), Spannable.SPAN_EXCLUSIVE_EXCLUSIVE);
		}
		textView.setText(builder);

	}

	void requestModify(String json) {
		new RequestAdapter() {

			@Override
			public void onReponse(ResponseData data) {
				JSONObject object = data.getMRootData();
				if (object != null) {
					boolean status = object.optBoolean("Status", false);
					if (status)
						finish();
				}
				Toast.makeText(AddressModifyActivity.this, data.getMsg(), Toast.LENGTH_SHORT).show();
			}

			@Override
			public void onProgress(ProgressMessage msg) {
				// TODO Auto-generated method stub

			}
		}.setUrl(getString(R.string.url_modify_address)).setRequestMethod(RequestMethod.ePut).SetJSON(json)
				.notifyRequest();
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
