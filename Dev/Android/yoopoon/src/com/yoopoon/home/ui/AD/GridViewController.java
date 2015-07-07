package com.yoopoon.home.ui.AD;

import org.json.JSONArray;
import android.annotation.SuppressLint;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.GridView;
import android.widget.LinearLayout;
import com.yoopoon.home.R;

public abstract class GridViewController {
	protected View rootView;

	protected Context mContext;

	protected LayoutInflater inflater;
	protected GridView mGridView;
	
	

	public View getRootView() {
		if (rootView == null) {
			initView();
			
		}
		return rootView;
	}

	public GridViewController(Context context) {
		mContext = context;
		inflater = LayoutInflater.from(mContext);
	}

	public abstract void show(JSONArray urls) ;
	
	
	
	public void addHeadView(View headView){
		((LinearLayout)rootView).addView(headView, 0);
	}

	@SuppressLint("InflateParams") 
	private void initView() {

		rootView = inflater.inflate(R.layout.active_page_view, null);

		mGridView = (GridView) (rootView.findViewById(R.id.myGrid));
		
		mGridView.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				onGridItemClick(parent, view, position, id);
				
			}
		});

	}
	public abstract void onGridItemClick(AdapterView<?> parent, View view,
					int position, long id);
	
}
