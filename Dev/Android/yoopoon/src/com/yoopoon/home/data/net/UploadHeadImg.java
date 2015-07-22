package com.yoopoon.home.data.net;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.List;
import org.apache.http.client.CookieStore;
import org.apache.http.cookie.Cookie;
import org.apache.http.impl.client.BasicCookieStore;
import org.apache.http.impl.cookie.BasicClientCookie;
import org.apache.http.util.EncodingUtils;
import org.json.JSONArray;
import org.json.JSONObject;
import com.yoopoon.home.data.storage.LocalPath;

public class UploadHeadImg {
	/**
	 * @param path 上传路径 * @param file 上传文件
	 */

	static String TAG = "UploadHeadImg";

	public String post(String path, File file, OnProgressListener listen, OnCompleteListener complete) {
		BufferedReader reader = null;
		OutputStream outStream = null;
		HttpURLConnection conn = null;
		try {

			final String BOUNDARY = "----WebKitFormBoundaryUn6BaRHhVPJWBScv"; // 数据分隔线
			final String endline = "--" + BOUNDARY + "--";// 数据结束标志

			int fileDataLength = 0;

			// 得到文件类型数据的总长度

			StringBuilder fileExplain = new StringBuilder();
			fileExplain.append("--");
			fileExplain.append(BOUNDARY);
			fileExplain.append("\r\n");
			fileExplain.append("Content-Disposition: form-data;name=\"fileToUpload\";filename=\"" + file.getName()
					+ "\"\r\n");
			fileExplain.append("Content-Type: image/*\r\n\r\n");

			fileDataLength += fileExplain.length();

			fileDataLength += file.length();
			fileDataLength += "\r\n".getBytes().length;

			// 计算传输给服务器的实体数据总长度
			int dataLength = fileDataLength + endline.getBytes().length;

			URL url = new URL(path);

			conn = (HttpURLConnection) url.openConnection();
			conn.setDoOutput(true);
			conn.setDoInput(true);
			conn.setUseCaches(false);
			conn.setConnectTimeout(10000); // 连接超时为10秒
			conn.setRequestMethod("POST");

			conn.setRequestProperty("Cookie", setCookie());

			conn.setRequestProperty("Content-Type", "multipart/form-data; boundary=" + BOUNDARY);
			conn.setRequestProperty("Content-Length", dataLength + "");
			outStream = conn.getOutputStream();

			long total = 0;

			StringBuilder fileEntity = new StringBuilder();
			fileEntity.append("--");
			fileEntity.append(BOUNDARY);
			fileEntity.append("\r\n");
			fileEntity.append("Content-Disposition: form-data;name=\"fileToUpload\";filename=\"");
			outStream.write(fileEntity.toString().getBytes());
			outStream.write(file.getName().getBytes(Charset.forName("UTF-8")));

			String temp = "\"\r\n" + "Content-Type: image/*\r\n\r\n";
			outStream.write(temp.getBytes());
			FileInputStream fis = new FileInputStream(file);
			if (fis != null) {

				byte[] buffer = new byte[1024 * 200];

				int len = 0;

				while ((len = fis.read(buffer, 0, 1024 * 200)) != -1) {
					total += len;
					if (listen != null) {
						listen.onProgress((int) (total / fileDataLength * 100));
					}
					outStream.write(buffer, 0, len);

				}

				fis.close();
			}
			outStream.write("\r\n".getBytes());

			// 下面发送数据结束标志，表示数据已经结束
			outStream.write(endline.getBytes());
			outStream.flush();

			reader = new BufferedReader(new InputStreamReader(conn.getInputStream(), "UTF-8"));

			String line = reader.readLine();

			if (line != null && complete != null)
				complete.onSuccess(line);

		} catch (Exception e) {
			e.printStackTrace();
			if (complete != null) {
				complete.onFailed();
			}
		} finally {

			try {
				if (outStream != null)
					outStream.close();
				if (reader != null)
					reader.close();
				if (conn != null)
					conn.disconnect();
			} catch (IOException e) {

				e.printStackTrace();

			}

		}

		return null;
	}

	private static String setCookie() throws IOException {
		CookieStore mCookie = RequstManager.getInstance().getCookie();
		if (mCookie == null) {
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
							String str = "Cookie: ";
							str = "";
							BasicClientCookie basicClientCookie = new BasicClientCookie(name, value);
							str += name + "=" + value + ";";
							basicClientCookie.setComment(o.optString("comment"));
							basicClientCookie.setDomain(o.optString("domain"));
							basicClientCookie.setPath(o.optString("path"));
							basicClientCookie.setVersion(o.optInt("version"));
							basicClientCookie.setSecure(o.optBoolean("secure"));
							basicCookieStore.addCookie(basicClientCookie);
							str += "comment=" + o.optString("comment") + ";";
							str += "domain=" + o.optString("domain") + ";";
							str += "path=" + o.optString("path") + ";";
							str += "version=" + o.optInt("version") + ";";
							str += "secure=" + o.optBoolean("secure");// + "\r\n";
							return str;
							// outStream.write(str.getBytes());

						}

					}

					RequstManager.getInstance().setCookie(basicCookieStore);
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		} else {
			List<Cookie> cookies = mCookie.getCookies();
			for (Cookie cok : cookies) {
				String str = "" + cok.getName() + "=" + cok.getValue() + ";";
				str += "comment=" + cok.getComment() + ";";
				str += "domain=" + cok.getDomain() + ";";
				str += "path=" + cok.getPath() + ";";
				str += "version=" + cok.getVersion() + ";";
				str += "secure=" + cok.isSecure();// + "\r\n";
				return str;
				// outStream.write(str.getBytes());

			}
		}
		return null;
	}

	public interface OnProgressListener {
		void onProgress(int progress);
	}

	public interface OnCompleteListener {
		void onSuccess(String json);

		void onFailed();
	}

}
