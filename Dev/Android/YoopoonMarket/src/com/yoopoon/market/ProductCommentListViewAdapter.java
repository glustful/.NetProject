/** 
 * @ClassName: ProductCommentListViewAdapter 
 * @Description: 该适配器是对商品评价的数据和视图的绑定
 * @author: king
 * @date: 2015年9月11日 下午4:56:12  
 */
package com.yoopoon.market;

import java.util.ArrayList;

import javax.security.auth.PrivateCredentialPermission;

import org.json.JSONObject;

import com.nostra13.universalimageloader.core.ImageLoader;

import android.R.string;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class ProductCommentListViewAdapter extends BaseAdapter {
	private Context mContext;
	private ArrayList<JSONObject> datas;

	public ProductCommentListViewAdapter(Context context, ArrayList<JSONObject> arrayList) {
		mContext = context;
		datas = arrayList;
	}
	@Override
	public int getCount() {
		return datas.size();
	}
	@Override
	public Object getItem(int position) {
		return datas.get(position);
	}
	@Override
	public long getItemId(int position) {
		return position;
	}
	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		CommentViewHandler commentViewHandler;
		if (convertView != null) {
			commentViewHandler = (CommentViewHandler) convertView.getTag();
		} else {
			commentViewHandler = new CommentViewHandler();
			convertView = LayoutInflater.from(mContext).inflate(R.layout.item_product_comment, null);
			commentViewHandler.initViewHandler(convertView);
			//###############################################################################
			//                      如下的代码只做API出来前的测试用途
			//###############################################################################
			String url = "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg";
			commentViewHandler.userPhotoImageView.setTag(url);
			ImageLoader.getInstance().displayImage(url, commentViewHandler.userPhotoImageView,
					MyApplication.getOptions(), MyApplication.getLoadingListener());
			commentViewHandler.nickNameTextView.setText(datas.get(position).optString("nickName"));
			commentViewHandler.commentContentTextView.setText(datas.get(position).optString("comment"));
			//###############################################################################
			//                      如上的代码只做API出来前的测试用途
			//###############################################################################
			convertView.setTag(commentViewHandler);
		}
		return convertView;
	}

	class CommentViewHandler {
		private ImageView userPhotoImageView;
		private TextView nickNameTextView;
		private TextView commentContentTextView;

		void initViewHandler(View view) {
			userPhotoImageView = (ImageView) view.findViewById(R.id.img_comment_user_photo);
			nickNameTextView = (TextView) view.findViewById(R.id.tv_comment_user_nickname);
			commentContentTextView = (TextView) view.findViewById(R.id.tv_comment_content);
		}
	}

	public void refresh(ArrayList<JSONObject> jsonObjects) {
		datas.clear();
		if (jsonObjects != null) {
			datas.addAll(jsonObjects);
		}
		this.notifyDataSetChanged();
	}
}
