package com.yoopoon.home.data.net;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.security.KeyManagementException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.UnrecoverableKeyException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.locks.ReentrantLock;

import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import org.apache.http.HttpResponse;
import org.apache.http.HttpVersion;
import org.apache.http.NameValuePair;
import org.apache.http.client.CookieStore;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.cookie.Cookie;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.BasicCookieStore;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.impl.cookie.BasicClientCookie;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpConnectionParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.util.EncodingUtils;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONObject;

import com.yoopoon.home.data.net.RequestAdapter.RequestContentType;
import com.yoopoon.home.data.storage.LocalPath;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;

public class RequstManager {
	private static RequstManager mManager = null;
	private static final int mRevBufferSize = 1024 * 2;
	private Boolean mStopRunning = false;
	private ReentrantLock mLock;
	private static CookieStore mCookieStore;
	private BlockingQueue<Runnable> mBlockingQueue;
	private ThreadPoolExecutor mThreadPoolExecutor;
	private HashMap<String, RequestTask> mTaskQueue;

	public static RequstManager getInstance() {
		if (mManager == null) {
			synchronized (RequstManager.class) {
				if (mManager == null) {
					mManager = new RequstManager();
					mManager.init();
				}
			}
		}
		return mManager;
	}

	private RequstManager() {

	}

	private void init() {
		mLock = new ReentrantLock();
		clearCookie();

		int threadPoolSize = Runtime.getRuntime().availableProcessors() * 2 + 2;

		mBlockingQueue = new LinkedBlockingQueue<Runnable>();
		mThreadPoolExecutor = new HttpThreadPoolExecutor(threadPoolSize,
				threadPoolSize * 2, 30, TimeUnit.SECONDS, mBlockingQueue);// 将网络请求全部加入线程池
		mTaskQueue = new HashMap<String, RequestTask>();
	}

	public void setCookie(CookieStore cookie) {
		mCookieStore = cookie;
	}

	public CookieStore getCookie() {
		return mCookieStore;
	}

	public void clearCookie() {
		mCookieStore = null;
	}

	private void execute(RequestAdapter adpater) {
		RequestTask task = new RequestTask(adpater, revedHander);
		adpater.setId(task.getId());
		mThreadPoolExecutor.execute(task);
		mTaskQueue.put(adpater.getId(), task);// 这里执行
	}

	protected boolean sendRequest(RequestAdapter adpater) {
		if (adpater.checkParameter()) {
			if (adpater.getRequestMode() == RequestAdapter.RequestMode.eSync) {
				exeCuteSync(adpater);
			} else {
				execute(adpater);
				return true;
			}
		}

		return false;
	}

	private void exeCuteSync(RequestAdapter mRequestData) {
		DefaultHttpClient mHttpClient;
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
		// urlString = mRequestData.getHost() + mRequestData.getUrl();
		if (mRequestData.getCallMethod() == RequestAdapter.CallMethod.eUnity) {
			urlString = mRequestData.getUnityUrl();
			mRequestData.addParam("url", mRequestData.getUrl());
		} else {
			urlString = mRequestData.getHost() + mRequestData.getUrl();
		}
		requestSyncPostMode(urlString, mRequestData, mHttpClient);
	}

	public boolean cancelRequest(String id) {
		boolean rc = false;
		if (mLock.tryLock()) {
			RequestTask task = mTaskQueue.get(id);
			if (null != task) {
				task.cancel();
				mTaskQueue.remove(id);
				rc = true;
			}
			mLock.unlock();
		}

		return rc;
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

	private void requestSyncPostMode(String urlstring,
			RequestAdapter mRequestData, DefaultHttpClient mHttpClient) {
		Map<String, String> params = mRequestData.getParams();
		HttpPost httpPost = new HttpPost(urlstring);
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
				if(mRequestData.getContentType()==RequestContentType.eGeneral){
					httpPost.setEntity(new UrlEncodedFormEntity(reqPar, "UTF-8"));
				}else{
					httpPost.setEntity(new StringEntity(mRequestData.getJSON(), "UTF-8"));
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
	                   
	                    msg.setData(EntityUtils.toByteArray(httpResponse.getEntity()));
	                    mRequestData.handleResponeMsg(msg);
	                }
	            } else {
	                ResponseMessage msg = new ResponseMessage();
	                msg.setCode(responseCode);
	                msg.setMsg("请求错误");
	                msg.setData(null);
	                mRequestData.handleResponeMsg(msg);
	            }
	        } catch (Exception e) {
	            ResponseMessage msg = new ResponseMessage();
	            msg.setCode(-1);
	            msg.setMsg(e.getMessage());
	            msg.setData(null);
	            mRequestData.handleResponeMsg(msg);
	        }
		
	}
	
	private static void saveCookie(CookieStore cookie) {

		if (cookie != null && cookie.getCookies() != null
				&& !cookie.getCookies().isEmpty()) {
			try {
				JSONArray array = new JSONArray();
				for (Cookie c : cookie.getCookies()) {
					System.out.println("cookie=" + c);
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
				// 把cookie持久化到文件�?
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

	private Handler revedHander = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			Bundle bundle = msg.getData();
			ProgressMessage progressMessage = (ProgressMessage) bundle
					.getSerializable("progressMsg");
			ResponseMessage responseMessage = (ResponseMessage) bundle
					.getSerializable("responseMsg");
			RequestAdapter adpater = (RequestAdapter) bundle
					.getSerializable("adpater");
			if (adpater == null) {
				return;
			}

			if (responseMessage != null)// 线程执行成功
			{
				// System.out.println("resmsg="+new
				// String(responseMessage.getData()));
				adpater.handleResponeMsg(responseMessage);
			} else if (progressMessage != null)// 正在执行
			{
				adpater.handleProgresMsg(progressMessage);
			}
		}
	};

	public class HttpThreadPoolExecutor extends ThreadPoolExecutor {
		public HttpThreadPoolExecutor(int corePoolSize, int maximumPoolSize,
				long keepAliveTime, TimeUnit unit,
				BlockingQueue<Runnable> workQueue) {
			super(corePoolSize, maximumPoolSize, keepAliveTime, unit, workQueue);
		}

		@Override
		public void beforeExecute(Thread t, Runnable r) {
			super.beforeExecute(t, r);
		}

		@Override
		public void afterExecute(Runnable r, Throwable t) {

			mLock.lock();

			RequestTask taskItem = (RequestTask) r;
			String id = taskItem.getId();
			mTaskQueue.remove(id);
			mLock.unlock();
			super.afterExecute(r, t);
		}
	}
}
