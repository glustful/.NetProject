package com.yoopoon.home.data.net;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.KeyManagementException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.UnrecoverableKeyException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.IdentityHashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;

import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.HttpVersion;
import org.apache.http.NameValuePair;
import org.apache.http.client.CookieStore;
import org.apache.http.client.HttpClient;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.cookie.Cookie;
import org.apache.http.entity.BufferedHttpEntity;
import org.apache.http.entity.ByteArrayEntity;
import org.apache.http.entity.StringEntity;
import org.apache.http.entity.mime.MultipartEntity;
import org.apache.http.entity.mime.content.FileBody;
import org.apache.http.entity.mime.content.StringBody;
import org.apache.http.impl.client.BasicCookieStore;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.impl.cookie.BasicClientCookie;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.protocol.BasicHttpContext;
import org.apache.http.protocol.HttpContext;
import org.apache.http.util.EncodingUtils;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONObject;

import com.yoopoon.home.data.net.RequestAdapter.RequestContentType;
import com.yoopoon.home.data.storage.LocalPath;

import android.graphics.Bitmap;
import android.net.http.AndroidHttpClient;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;

public class RequestTask implements Runnable {
	private static CookieStore mCookieStore = null;

	private static final int mRevBufferSize = 1024 * 2;
	private boolean mStopRunning;
	private RequestAdapter mRequestData;
	private DefaultHttpClient mHttpClient;
	private HttpContext mHttpContext = null;
	private Handler mRevedHander;
	private String mId;
	private InputStream mIs;

	public RequestTask(RequestAdapter requestData, Handler revedHandler) {
		mRevedHander = revedHandler;
		mRequestData = requestData;
		this.mStopRunning = false;
		mHttpContext = new BasicHttpContext();
		mId = UUID.randomUUID().toString();
	}

	public String getId() {
		return mId;
	}

	public void cancel() {
		this.mStopRunning = true;
		mHttpClient.getConnectionManager().shutdown();
	}

	@Override
	public void run() {
		if (mRequestData == null || mStopRunning) {
			return;
		}

		mHttpClient = getHttpClient(mRequestData);
		if (mCookieStore == null) {// 如果为空，尝试从文件获取
			File cookieFile = new File(LocalPath.intance().cacheBasePath + "co");
			String res = "";
			try {
				FileInputStream fin = new FileInputStream(cookieFile);

				int length = fin.available();

				byte[] buffer = new byte[length];
				fin.read(buffer);

				res = EncodingUtils.getString(buffer, "UTF-8");
				fin.close();
				if (res != null && !"".equals(res.trim())) {
					BasicCookieStore basicCookieStore = new BasicCookieStore();
					JSONArray array = new JSONArray(res);
					for (int i = 0; i < array.length(); i++) {
						JSONObject o = array.getJSONObject(i);
						String name = o.optString("name");
						String value = o.optString("value");
						if (name != null && value != null) {
							BasicClientCookie basicClientCookie = new BasicClientCookie(
									name, value);
							basicClientCookie
									.setComment(o.optString("comment"));
							basicClientCookie.setDomain(o.optString("domain"));
							basicClientCookie.setPath(o.optString("path"));
							basicClientCookie.setVersion(o.optInt("version"));
							basicClientCookie.setSecure(o.optBoolean("secure"));
							basicCookieStore.addCookie(basicClientCookie);
						}
						mCookieStore = basicCookieStore;
					}

					mHttpClient.setCookieStore(mCookieStore);
				}
			} catch (Exception e) {
				e.printStackTrace();
			}

		}
		if (mCookieStore != null) {
			mHttpClient.setCookieStore(mCookieStore);
		}

		String urlString = "";
		if (mRequestData.getCallMethod() == RequestAdapter.CallMethod.eUnity) {
			urlString = mRequestData.getUnityUrl();
			mRequestData.addParam("url", mRequestData.getUrl());
		} else {
			urlString = mRequestData.getHost() + mRequestData.getUrl();
		}

		if (mRequestData.getRequestMethod() == RequestAdapter.RequestMethod.eGet) {
			RequestGetMethod(urlString);
		} else if (mRequestData.getRequestMethod() == RequestAdapter.RequestMethod.ePost) {
			try {
				RequestPostMethod(urlString);
			} catch (UnsupportedEncodingException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		} else {
			upLoadRequestPostMethod(urlString);
		}

	}

	private void RequestGetMethod(String urlString) {
		IdentityHashMap<String, String> params = mRequestData.getParams();
		if ((null != params) && (params.size() > 0)) {
			urlString += "?";
			for (Map.Entry<String, String> entry : params.entrySet()) {
				String key = entry.getKey();
				String value = entry.getValue();
				if (value instanceof String) {
					urlString += key + "=" + value;
					urlString += "&";
				}
			}
			urlString = urlString.substring(0, urlString.length() - 1);// 去掉最后的&字符
		}
		HttpResponse httpResponse = null;
		
		try {
			HttpGet get = new HttpGet(urlString);
			get.setHeader("x-requested-with", "XMLHttpRequest");
			// get.setHeader("Content-Type", "application/json");
			httpResponse = mHttpClient.execute(get, mHttpContext);
			int responseCode = httpResponse.getStatusLine().getStatusCode();
			if (responseCode >= 200 && responseCode < 400) {
				if (mRequestData.getSaveSession()) {
					CookieStore cookie = mHttpClient.getCookieStore();
					if (null != cookie) {
						saveCookie(cookie);
					}
				}
				if (mRequestData.getRequestType() == RequestAdapter.RequestType.eGeneral) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg("获取数据成功");
					msg.setData(EntityUtils.toByteArray(httpResponse
							.getEntity()));
					sendResponseMessage(msg);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileDown) {
					HttpEntity entity = httpResponse.getEntity();
					BufferedHttpEntity bufHttpEntity = new BufferedHttpEntity(
							entity);
					InputStream is = bufHttpEntity.getContent();
					downloadFile(is, responseCode);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileUp) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
					msg.setData(null);
					sendResponseMessage(msg);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileStream) {
					HttpEntity entity = httpResponse.getEntity();
					BufferedHttpEntity bufferedHttpEntity = new BufferedHttpEntity(
							entity);
					InputStream is = bufferedHttpEntity.getContent();
					mIs = is;
					Log.d("RequestTask",
							"InputStream size is +" + "..." + is.available());
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setInstream(mIs);
					sendResponseMessage(msg);

				}
			} else {
				ResponseMessage msg = new ResponseMessage();
				msg.setCode(responseCode);
				msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
				msg.setData(null);
				sendResponseMessage(msg);
			}
		} catch (IOException e) {
			ResponseMessage msg = new ResponseMessage();
			msg.setCode(-1);
			msg.setMsg(e.getMessage());
			msg.setData(null);
			sendResponseMessage(msg);
		}
	}

	private void RequestPostMethod(String urlString)
			throws UnsupportedEncodingException {
		IdentityHashMap<String, String> params = mRequestData.getParams();
		HttpPost httpPost = new HttpPost(urlString);
		httpPost.setHeader("x-requested-with", "XMLHttpRequest");
		if (mRequestData.getContentType() == RequestContentType.eJSON) {
			httpPost.setHeader("Content-Type", "application/json");
		}

		List<NameValuePair> reqPar = new ArrayList<NameValuePair>();
		if (null != params) {

			for (Map.Entry<String, String> entry : params.entrySet()) {
				String key = entry.getKey();
				String value = entry.getValue();
				reqPar.add(new BasicNameValuePair(key, value));
			}
		}

		try {
			if (mRequestData.getContentType() == RequestContentType.eGeneral) {
				httpPost.setEntity(new UrlEncodedFormEntity(reqPar, "UTF-8"));
			} else {
				httpPost.setEntity(new StringEntity(mRequestData.getJSON(),
						"UTF-8"));
			}

			HttpResponse httpResponse = mHttpClient.execute(httpPost);
			int responseCode = httpResponse.getStatusLine().getStatusCode();
			if ((responseCode > 199) && (responseCode < 400)) {
				if (!mStopRunning && mRequestData.getSaveSession()) {
					CookieStore cookie = mHttpClient.getCookieStore();
					if (null != cookie) {
						saveCookie(cookie);
					}
				}

				if (mRequestData.getRequestType() == RequestAdapter.RequestType.eGeneral) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg("获取数据成功");
					// System.out.println("response="+httpResponse.getEntity());

					msg.setData(EntityUtils.toByteArray(httpResponse
							.getEntity()));
					sendResponseMessage(msg);
					/*
					 * InputStream in = httpResponse.getEntity().getContent();
					 * BufferedReader bw = new BufferedReader(new
					 * InputStreamReader(in)); String line = bw.readLine();
					 * while(line != null){ System.out.println("line="+line);
					 * line = bw.readLine(); } bw.close();
					 */
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileDown) {
					HttpEntity entity = httpResponse.getEntity();
					BufferedHttpEntity bufHttpEntity = new BufferedHttpEntity(
							entity);
					InputStream is = bufHttpEntity.getContent();
					downloadFile(is, responseCode);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileUp) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
					msg.setData(null);
					sendResponseMessage(msg);
				} else {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
					msg.setData(null);
					sendResponseMessage(msg);
				}
			} else {
				ResponseMessage msg = new ResponseMessage();
				msg.setCode(responseCode);
				msg.setMsg("请求错误");
				msg.setData(null);
				sendResponseMessage(msg);
			}
		} catch (Exception e) {

			ResponseMessage msg = new ResponseMessage();
			msg.setCode(-1);
			// msg.setMsg(e.getMessage());
			msg.setMsg("网络不给力，请稍后重试！");
			msg.setData(null);
			sendResponseMessage(msg);
		}

	}

	private void upLoadRequestPostMethod(String urlString) {
		HttpEntity entityWhole;
		MultipartEntity mpEntity = new MultipartEntity();
		HttpPost httpPost = new HttpPost(urlString);
		httpPost.setHeader("x-requested-with", "XMLHttpRequest");
		// httpPost.setHeader("Content-Type","multipart/form-data");

		try {
			if (mRequestData.getmAttPath() != null
					&& !"".equals(mRequestData.getmAttPath().trim())
					&& mRequestData.getmBitmap() == null) {

				IdentityHashMap<String, String> params = mRequestData
						.getParams();
				if (null != params) {
					for (Map.Entry<String, String> entry : params.entrySet()) {
						String key = entry.getKey();
						String value = entry.getValue();
						try {
							StringBody par = new StringBody(value);
							mpEntity.addPart(key, par);
						} catch (Exception e) {
							e.printStackTrace();
						}
					}
				}
				FileBody file = new FileBody(new File(
						mRequestData.getmAttPath()));
				mpEntity.addPart("file", file);
				httpPost.setEntity(mpEntity);

			} else if (mRequestData.getmAttPath() == null
					&& mRequestData.getmBitmap() != null) {
				Bitmap bitmap = mRequestData.getmBitmap();
				ByteArrayOutputStream bao = new ByteArrayOutputStream();
				bitmap.compress(Bitmap.CompressFormat.JPEG, 100, bao);// 将bitmap转换为PNG
				entityWhole = new ByteArrayEntity(bao.toByteArray());
				httpPost.setEntity(entityWhole);
			}
			HttpResponse httpResponse = mHttpClient.execute(httpPost);
			int responseCode = httpResponse.getStatusLine().getStatusCode();
			if ((responseCode > 199) && (responseCode < 400)) {
				if (!mStopRunning && mRequestData.getSaveSession()) {
					CookieStore cookie = mHttpClient.getCookieStore();
					if (null != cookie) {
						saveCookie(cookie);
					}
				}
				if (mRequestData.getRequestType() == RequestAdapter.RequestType.eGeneral) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg("获取数据成功");
					msg.setData(EntityUtils.toByteArray(httpResponse
							.getEntity()));
					sendResponseMessage(msg);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileDown) {
					HttpEntity entity = httpResponse.getEntity();
					BufferedHttpEntity bufHttpEntity = new BufferedHttpEntity(
							entity);
					InputStream is = bufHttpEntity.getContent();
					downloadFile(is, responseCode);
				} else if (mRequestData.getRequestType() == RequestAdapter.RequestType.eFileUp) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
					msg.setData(EntityUtils.toByteArray(httpResponse
							.getEntity()));
					sendResponseMessage(msg);
				} else {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(responseCode);
					msg.setMsg(httpResponse.getStatusLine().getReasonPhrase());
					msg.setData(null);
					sendResponseMessage(msg);
				}

			} else {
				ResponseMessage msg = new ResponseMessage();
				msg.setCode(responseCode);
				msg.setMsg("请求错误");
				msg.setData(null);
				sendResponseMessage(msg);
			}
		} catch (Exception e) {
			ResponseMessage msg = new ResponseMessage();
			msg.setCode(-1);
			msg.setMsg(e.getMessage());
			msg.setData(null);
			sendResponseMessage(msg);
		}

	}

	private void downloadFile(InputStream is, int responseCode) {

		int fileSize = 0;
		try {
			fileSize = is.available();
		} catch (IOException e) {
			e.printStackTrace();
		}
		FileOutputStream out = null;
		int readCount = 0;
		try {
			Log.d("RequsetTask",
					"download File path is" + mRequestData.getLocalPath());
			out = new FileOutputStream(mRequestData.getLocalPath(), false);
			BufferedOutputStream bufferedOutputStream = new BufferedOutputStream(
					out);
			BufferedInputStream bufferedInputStream = new BufferedInputStream(
					is);
			byte[] buf = new byte[1024];
			int bytesRead = 0;
			while (bytesRead >= 0 && !mStopRunning) {
				long now = System.currentTimeMillis();
				try {
					bytesRead = bufferedInputStream.read(buf);
					if (bytesRead <= 0) {
						break;
					}
					bufferedOutputStream.write(buf, 0, bytesRead);
					readCount += bytesRead;
					ProgressMessage progressMsg = new ProgressMessage();
					progressMsg.setTotal(fileSize);
					progressMsg.setTransSize(readCount);
					sendProgressMessage(progressMsg);
					if (readCount == fileSize) {
						break;
					}
				} catch (IOException e) {
					ResponseMessage msg = new ResponseMessage();
					msg.setCode(-2);
					msg.setMsg(e.getMessage());
					msg.setData(null);
					sendResponseMessage(msg);
				}
			}

			try {
				is.close();
				bufferedInputStream.close();
				bufferedOutputStream.flush();
				bufferedOutputStream.close();
			} catch (IOException e) {
				ResponseMessage msg = new ResponseMessage();
				msg.setCode(-1);
				msg.setMsg(e.getMessage());
				msg.setData(null);
				sendResponseMessage(msg);
			}
		} catch (FileNotFoundException e) {
			ResponseMessage msg = new ResponseMessage();
			msg.setCode(-2);
			msg.setMsg(e.getMessage());
			msg.setData(null);
			sendResponseMessage(msg);
		}

		ResponseMessage msg = new ResponseMessage();
		msg.setCode(responseCode);
		msg.setMsg("文件下载成功");
		msg.setData(null);
		sendResponseMessage(msg);
	}

	private void sendProgressMessage(ProgressMessage msg) {
		Message hadnelMsg = mRevedHander.obtainMessage();
		Bundle bund = new Bundle();
		bund.putSerializable("adpater", mRequestData);
		bund.putSerializable("progressMsg", msg);
		hadnelMsg.setData(bund);
		hadnelMsg.sendToTarget();
	}

	private void sendResponseMessage(ResponseMessage msg) {
		Message hadnelMsg = mRevedHander.obtainMessage();
		Bundle bund = new Bundle();
		bund.putSerializable("adpater", mRequestData);
		bund.putSerializable("responseMsg", msg);

		hadnelMsg.setData(bund);
		hadnelMsg.sendToTarget();
	}

	private DefaultHttpClient getHttpClient(RequestAdapter data) {
		try {
			KeyStore trustStore = KeyStore.getInstance(KeyStore
					.getDefaultType());
			trustStore.load(null, null);
			SSLSocketFactory sf = new SSLSocketFactoryEx(trustStore);
			sf.setHostnameVerifier(SSLSocketFactory.ALLOW_ALL_HOSTNAME_VERIFIER);

			HttpParams httpParams = new BasicHttpParams();
			HttpProtocolParams.setVersion(httpParams, HttpVersion.HTTP_1_1);
			HttpProtocolParams.setUseExpectContinue(httpParams, true);

			SchemeRegistry schReg = new SchemeRegistry();
			schReg.register(new Scheme("http", PlainSocketFactory
					.getSocketFactory(), 80));
			schReg.register(new Scheme("https", sf, 443));

			ClientConnectionManager conManager = new ThreadSafeClientConnManager(
					httpParams, schReg);

			HttpConnectionParams.setConnectionTimeout(httpParams,
					data.getTimeOutLimit() * 1000);
			HttpConnectionParams.setSoTimeout(httpParams,
					data.getTimeOutLimit() * 1000); // 此处的单位是毫秒
			HttpConnectionParams.setSocketBufferSize(httpParams,
					mRevBufferSize * 2);
			DefaultHttpClient httpClient = new DefaultHttpClient(conManager,
					httpParams);
			return httpClient;
		} catch (Exception e) {
			return null;
		}
	}

	class SSLSocketFactoryEx extends SSLSocketFactory {

		SSLContext sslContext = SSLContext.getInstance("TLS");

		public SSLSocketFactoryEx(KeyStore truststore)
				throws NoSuchAlgorithmException, KeyManagementException,
				KeyStoreException, UnrecoverableKeyException {
			super(truststore);

			TrustManager tm = new X509TrustManager() {

				@Override
				public java.security.cert.X509Certificate[] getAcceptedIssuers() {
					return null;
				}

				@Override
				public void checkClientTrusted(
						java.security.cert.X509Certificate[] chain,
						String authType)
						throws java.security.cert.CertificateException {

				}

				@Override
				public void checkServerTrusted(
						java.security.cert.X509Certificate[] chain,
						String authType)
						throws java.security.cert.CertificateException {

				}
			};

			sslContext.init(null, new TrustManager[] { tm }, null);
		}

		@Override
		public Socket createSocket(Socket socket, String host, int port,
				boolean autoClose) throws IOException, UnknownHostException {
			return sslContext.getSocketFactory().createSocket(socket, host,
					port, autoClose);
		}

		@Override
		public Socket createSocket() throws IOException {
			return sslContext.getSocketFactory().createSocket();
		}
	}

	private static void saveCookie(CookieStore cookie) {

		if (cookie != null && cookie.getCookies() != null
				&& !cookie.getCookies().isEmpty()) {
			try {
				JSONArray array = new JSONArray();
				for (Cookie c : cookie.getCookies()) {
					
					JSONObject o = new JSONObject();
					o.put("name", c.getName());
					o.put("value", c.getValue());
					o.put("comment", c.getComment());
					o.put("commentURL", c.getCommentURL());
					o.put("domain", c.getDomain());
					o.put("path", c.getPath());
					o.put("ports", c.getPorts());
					o.put("version", c.getVersion());
					o.put("secure", c.isSecure());
					array.put(o);
				}
				String s = array.toString();
				// 把cookie持久化到文件
				File cookieFile = new File(LocalPath.intance().cacheBasePath
						+ "co");
				FileOutputStream fout = new FileOutputStream(cookieFile);
				byte[] bytes = s.getBytes();

				fout.write(bytes);
				fout.close();
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
		mCookieStore = cookie;
	}

	public static void setmCookieStore(CookieStore mCookieStore) {
		RequestTask.mCookieStore = mCookieStore;
	}

}
