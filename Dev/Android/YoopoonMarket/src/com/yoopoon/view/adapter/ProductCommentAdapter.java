package com.yoopoon.view.adapter;

import java.util.ArrayList;

import org.json.JSONObject;

import com.nostra13.universalimageloader.core.ImageLoader;
import com.yoopoon.market.MyApplication;
import com.yoopoon.market.R;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;
import android.widget.TextView;

public class ProductCommentAdapter extends BaseAdapter {
	private ArrayList<JSONObject> datas;
	private Context mContext;

	public ProductCommentAdapter(Context context, ArrayList<JSONObject> arrayList) {
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
			convertView.setTag(commentViewHandler);
		}
		//String imageUrl = datas.get(position).optString("UserImg");
		String imageUrlString = "http://img.iyookee.cn/20150825/20150825_105153_938_32.jpg";
		commentViewHandler.userPhotoImageView.setTag(imageUrlString);
		ImageLoader.getInstance().displayImage(imageUrlString, commentViewHandler.userPhotoImageView,
				MyApplication.getOptions(), MyApplication.getLoadingListener());
		commentViewHandler.userNameTextView.setText(datas.get(position).optString("UserName"));
		commentViewHandler.commentContentTextView.setText(datas.get(position).optString("Content"));
		return convertView;
	}

	class CommentViewHandler {
		private ImageView userPhotoImageView;
		private TextView userNameTextView;
		private TextView commentContentTextView;

		void initViewHandler(View view) {
			userPhotoImageView = (ImageView) view.findViewById(R.id.img_comment_user_photo);
			userNameTextView = (TextView) view.findViewById(R.id.tv_comment_user_nickname);
			commentContentTextView = (TextView) view.findViewById(R.id.tv_comment_content);
		}
	}

	public void refresh(ArrayList<JSONObject> arrayList) {
		if (arrayList != null) {
			datas.addAll(arrayList);
		}
		this.notifyDataSetChanged();
	}
}
