package com.yoopoon.market;

import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.Random;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EActivity;
import org.androidannotations.annotations.Extra;
import org.androidannotations.annotations.ViewById;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;
import com.alipay.sdk.app.PayTask;
import com.yoopoon.market.alipay.PayResult;
import com.yoopoon.market.alipay.SignUtils;
import com.yoopoon.market.db.dao.DBDao;

@EActivity(R.layout.pay_main)
public class PayDemoActivity extends MainActionBarActivity {
	@Extra
	Bundle orderBundle;
	@Extra
	boolean isService;

	@ViewById(R.id.product_subject)
	TextView tv_name;
	@ViewById(R.id.product_price)
	TextView tv_price;
	@ViewById(R.id.tv_desc)
	TextView tv_desc;
	private static final String TAG = "PayDemoActivity";

	// 商户PID
	public static final String PARTNER = "2088311414553838";
	// 商户收款账号
	public static final String SELLER = "yunjoy@yunjoy.cn";
	// 支付宝公钥
	public static String RSA_PUBLIC = "";
	// 商户私钥，pkcs8格式
	public static String RSA_PRIVATE = "MIICeAIBADANBgkqhkiG9w0BAQEFAASCAmIwggJeAgEAAoGBANFzvfp10O8RbgWr0A0o0zgVj/m4xhiVmvW6YT3jlcC+8WtljdRipAMkPe8hfnj07s9EfyI+Grw9jKh5teL4njxBCogPpthrFWM1j3B7ItHQF90JUsCrOfosmqGkx/rdOhfvgIWpP5k2wGsj0KEiaOy1SZ/qh74s29J2EN0qLtxJAgMBAAECgYBoEb08XBvDHYLwOG04jKdeP4B5EPEEuBj1rXSxnooC6hzkQuJUu+pIUVKgpaDEktaxj5QnvHnmPCLOdyMDsopT+VBi4JKGY1DWyaRhuBuDTppsLq2nuTYbJbEXVWAObYS58I6ieTAlLWcFknWeNLfJfDHvE8zKrvpl4fir+BxITQJBAPeeW46GXjuLAyXPxcS8QC6PcAXiPS0/eQrsbyVzZDBtZSyc0mlQwFvPpgFJ3lNpiGFCeSLXlQAwEgq/UFH/vBMCQQDYiqr1vG4IZTpcaBm/QEd6ioTLUvdd01m08ZJZ6eIskQRC2XzVIpD0sZ4K9KFw4q3v9AmfhhjW+FVZGULQDZmzAkEA9TJad0eXCF8fPtH/hFDlPTXMOAdPjP7NXYPCi9M34rxw8zxXHvJXiJKWT7BV90MJSUYJrfbMFOOE+h936brTAQJBAJzqZ5apVEcLK+54lWfM6b84D6DTX2QDWvdPMxGq9XX8JE1ZEyfT450d9PvVaAPIj+jZO/v4jZmB3T8ymgLwSBcCQQCz9H+dP4baUmDShChuwFSdnRLoP1Z4G0VcMvpCKM0/Ciu2nqXTQe9nuKdafavCGXOZO/jYFkXsPvAq3YK5TDPf";

	private static final int SDK_PAY_FLAG = 1;

	private static final int SDK_CHECK_FLAG = 2;

	private Handler mHandler = new Handler() {

		public void handleMessage(Message msg) {
			switch (msg.what) {
				case SDK_PAY_FLAG: {
					PayResult payResult = new PayResult((String) msg.obj);

					// 支付宝返回此次支付结果及加签，建议对支付宝签名信息拿签约时支付宝提供的公钥做验签
					String resultInfo = payResult.getResult();

					String resultStatus = payResult.getResultStatus();
					// 9000 订单支付成功
					// 8000 正在处理中
					// 4000 订单支付失败
					// 6001 用户中途取消
					// 6002 网络连接出错

					// 判断resultStatus 为“9000”则代表支付成功，具体状态码代表含义可参考接口文档
					if (TextUtils.equals(resultStatus, "9000")) {
						Toast.makeText(PayDemoActivity.this, "支付成功", Toast.LENGTH_SHORT).show();
						// 支付成功后，购物车应删除相应商品。然后返回购物车
						final int[] ids = orderBundle.getIntArray("StaffIds");
						if (ids != null) {
							new Thread() {

								@Override
								public void run() {

									DBDao dao = new DBDao(PayDemoActivity.this);
									for (int i = 0; i < ids.length; i++) {
										int id = ids[i];
										dao.delete(id);
									}
								}
							}.start();

							// 发送广播打开购物车
							MainActivity_.intent(PayDemoActivity.this).start();
							Intent intent = new Intent("com.yoopoon.market.showcart");
							intent.addCategory(Intent.CATEGORY_DEFAULT);
							PayDemoActivity.this.sendBroadcast(intent);
						} else {

							Intent intent = new Intent("com.yoopoon.market.pay_succeed");
							intent.addCategory(Intent.CATEGORY_DEFAULT);
							PayDemoActivity.this.sendBroadcast(intent);
							finish();
						}

					} else {
						// 判断resultStatus 为非“9000”则代表可能支付失败
						// “8000”代表支付结果因为支付渠道原因或者系统原因还在等待支付结果确认，最终交易是否成功以服务端异步通知为准（小概率状态）
						if (TextUtils.equals(resultStatus, "8000")) {
							Toast.makeText(PayDemoActivity.this, "支付结果确认中", Toast.LENGTH_SHORT).show();

						} else {
							if (TextUtils.equals(resultStatus, "4000"))
								Toast.makeText(PayDemoActivity.this, "支付失败", Toast.LENGTH_SHORT).show();
							if (TextUtils.equals(resultStatus, "6001"))
								Toast.makeText(PayDemoActivity.this, "用户中途取消", Toast.LENGTH_SHORT).show();
							if (TextUtils.equals(resultStatus, "6002"))
								Toast.makeText(PayDemoActivity.this, "网络连接出错", Toast.LENGTH_SHORT).show();

						}
					}
					break;
				}
				case SDK_CHECK_FLAG: {
					Toast.makeText(PayDemoActivity.this, "检查结果为：" + msg.obj, Toast.LENGTH_SHORT).show();
					break;
				}
				default:
					break;
			}
		};
	};

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
	}

	@AfterViews
	void initUI() {
		backWhiteButton.setVisibility(View.VISIBLE);
		titleButton.setVisibility(View.VISIBLE);
		rightButton.setVisibility(View.GONE);
		titleButton.setText("支付");
		titleButton.setTextColor(Color.WHITE);
		headView.setBackgroundColor(Color.RED);

		String price = orderBundle.getFloat("Price") + "";
		tv_price.setText(price);
		tv_desc.setText("优朋数字社区商品支付");
		tv_name.setText("优朋社区");
	}

	/**
	 * call alipay sdk pay. 调用SDK支付
	 */
	public void pay(View v) {
		// 订单
		String price = orderBundle.getFloat("Price") + "";
		String No = orderBundle.getString("No");
		String notifyUrl = orderBundle.getString("Msg");
		Log.i(TAG, price);
		Log.i(TAG, No);
		Log.i(TAG, notifyUrl);
		String orderInfo = getOrderInfo("优朋社区", "优朋数字社区商品支付", "0.01", notifyUrl, No);

		// 对订单做RSA 签名
		String sign = sign(orderInfo);
		try {
			// 仅需对sign 做URL编码
			sign = URLEncoder.encode(sign, "UTF-8");
		} catch (Exception e) {
			e.printStackTrace();
		}

		// 完整的符合支付宝参数规范的订单信息
		final String payInfo = orderInfo + "&sign=\"" + sign + "\"&" + getSignType();

		Runnable payRunnable = new Runnable() {

			@Override
			public void run() {
				// 构造PayTask 对象
				PayTask alipay = new PayTask(PayDemoActivity.this);
				// 调用支付接口，获取支付结果
				Log.i(TAG, payInfo);
				String result = alipay.pay(payInfo);
				Log.i(TAG, result);

				Message msg = new Message();
				msg.what = SDK_PAY_FLAG;
				msg.obj = result;
				mHandler.sendMessage(msg);
			}
		};

		// 必须异步调用
		Thread payThread = new Thread(payRunnable);
		payThread.start();
	}

	/**
	 * check whether the device has authentication alipay account. 查询终端设备是否存在支付宝认证账户
	 */
	public void check(View v) {
		Runnable checkRunnable = new Runnable() {

			@Override
			public void run() {
				// 构造PayTask 对象
				PayTask payTask = new PayTask(PayDemoActivity.this);
				// 调用查询接口，获取查询结果
				boolean isExist = payTask.checkAccountIfExist();
				Log.i(TAG, "isExist = " + isExist);

				Message msg = new Message();
				msg.what = SDK_CHECK_FLAG;
				msg.obj = isExist;
				mHandler.sendMessage(msg);
			}
		};

		Thread checkThread = new Thread(checkRunnable);
		checkThread.start();

	}

	/**
	 * get the sdk version. 获取SDK版本号
	 */
	public void getSDKVersion() {
		PayTask payTask = new PayTask(this);
		String version = payTask.getVersion();
		Toast.makeText(this, version, Toast.LENGTH_SHORT).show();
	}

	/**
	 * create the order info. 创建订单信息
	 */
	public String getOrderInfo(String subject, String body, String price, String notifyUrl, String No) {
		// 签约合作者身份ID
		String orderInfo = "partner=" + "\"" + PARTNER + "\"";

		// 签约卖家支付宝账号
		orderInfo += "&seller_id=" + "\"" + SELLER + "\"";

		// 商户网站唯一订单号
		orderInfo += "&out_trade_no=" + "\"" + No + "\"";

		// 商品名称
		orderInfo += "&subject=" + "\"" + subject + "\"";

		// 商品详情
		orderInfo += "&body=" + "\"" + body + "\"";

		// 商品金额
		orderInfo += "&total_fee=" + "\"" + price + "\"";

		// 服务器异步通知页面路径
		if (TextUtils.isEmpty(notifyUrl))
			orderInfo += "&notify_url=" + "\"" + "http://notify.msp.hk/notify.htm" + "\"";
		else {
			orderInfo += "&notify_url=" + "\"" + notifyUrl + "\"";
		}
		// 服务接口名称， 固定值
		orderInfo += "&service=\"mobile.securitypay.pay\"";

		// 支付类型， 固定值
		orderInfo += "&payment_type=\"1\"";

		// 参数编码， 固定值
		orderInfo += "&_input_charset=\"utf-8\"";

		// 设置未付款交易的超时时间
		// 默认30分钟，一旦超时，该笔交易就会自动被关闭。
		// 取值范围：1m～15d。
		// m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。
		// 该参数数值不接受小数点，如1.5h，可转换为90m。
		orderInfo += "&it_b_pay=\"30m\"";

		// extern_token为经过快登授权获取到的alipay_open_id,带上此参数用户将使用授权的账户进行支付
		// orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

		// 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空
		// orderInfo += "&return_url=\"m.alipay.com\"";

		// 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能使用）
		// orderInfo += "&paymethod=\"expressGateway\"";

		return orderInfo;
	}

	/**
	 * get the out_trade_no for an order. 生成商户订单号，该值在商户端应保持唯一（可自定义格式规范）
	 */
	public String getOutTradeNo() {
		SimpleDateFormat format = new SimpleDateFormat("MMddHHmmss", Locale.getDefault());
		Date date = new Date();
		String key = format.format(date);

		Random r = new Random();
		key = key + r.nextInt();
		key = key.substring(0, 15);
		return key;
	}

	/**
	 * sign the order info. 对订单信息进行签名
	 * @param content 待签名订单信息
	 */
	public String sign(String content) {
		return SignUtils.sign(content, RSA_PRIVATE);
	}

	/**
	 * get the sign type we use. 获取签名方式
	 */
	public String getSignType() {
		return "sign_type=\"RSA\"";
	}

	@Override
	public void backButtonClick(View v) {
		// 订单已经确认成功，点击返回说明没有成功付款，应跳到未付款界面
		if (!isService)
			MeOrderActivity_.intent(this).item(0).start();
		// 关闭自己
		finish();
	}

	@Override
	public void titleButtonClick(View v) {
		// TODO Auto-generated method stub

	}

	@Override
	public void rightButtonClick(View v) {

	}

	@Override
	public Boolean showHeadView() {
		return true;
	}

}
