package com.yoopoon.home.ui.home;

import java.util.ArrayList;

import org.json.JSONObject;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.yoopoon.home.R;

/**
 * @ClassName: FramHouseConditionAdapter
 * @Description: 房源页顶端条件选择
 * @author: 徐阳会
 * @date: 2015年7月7日 下午4:20:28
 */
public class FramHouseConditionAdapter extends BaseAdapter {
	Context mContext;
	ArrayList<JSONObject> datas;

	/**
	 * @Title:FramHouseConditionAdapter
	 * @Description:构造方法
	 */
	public FramHouseConditionAdapter(Context context) {
		mContext = context;
		datas = new ArrayList<JSONObject>();
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
		ViewHandler viewHandler;
		if (convertView == null) {
			convertView = LayoutInflater.from(mContext).inflate(R.layout.home_fram_house_getcondition_listview_item,
					null);
			// LinearLayout listViewLinearLayout=(LinearLayout)
			// convertView.findViewById(R.id.house_condition_linearlayout);
			viewHandler = new ViewHandler();
			viewHandler.init(convertView);
			convertView.setTag(viewHandler);
		} else {
			viewHandler = (ViewHandler) convertView.getTag();
		}
		final JSONObject item = datas.get(position);
		viewHandler.houseConditionTextView.setText(item.optString("TypeName"));
		viewHandler.houseConditionTextView.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
			}
		});
		return convertView;
	}

	/**
	 * @ClassName: ViewHandler
	 * @Description: 展示条件调价下拉列表的TextView项目handler
	 * @author: 徐阳会
	 * @date: 2015年7月7日 下午4:31:48
	 */
	private class ViewHandler {
		private TextView houseConditionTextView;

		void init(View root) {
			houseConditionTextView = (TextView) root.findViewById(R.id.house_condition_textview);
		}
	}

	/**
	 * @Title: refresh
	 * @Description: 数据刷新
	 * @param mJsonObjects ArrayList<JSONObject>类型，传入的楼盘类型JSONObject数组
	 */
	public void refresh(ArrayList<JSONObject> mJsonObjects) {
		datas.clear();
		datas.addAll(mJsonObjects);
		this.notifyDataSetChanged();
	}
}
