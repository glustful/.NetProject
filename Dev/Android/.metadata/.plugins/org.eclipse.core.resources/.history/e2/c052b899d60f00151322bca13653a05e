package com.miicaa.home.ui.home;

import java.util.ArrayList;

import android.content.Context;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.util.Log;
import android.view.ViewGroup;

public class HomeMainAdapter extends FragmentPagerAdapter{

	public static HomeMainAdapter instance;
	ArrayList<FragmentInfo> infos;
	FragmentManager fm;
	Context context;
	MenuClickLis listener;
	
	public HomeMainAdapter(FragmentManager fm,ArrayList<FragmentInfo> infos,Context context) {
		super(fm);
		instance = this;
		this.fm = fm;
	
		this.infos =  infos;
		this.context = context;
		this.listener = listener;
		
		// TODO Auto-generated constructor stub
	}

	public HomeMainAdapter getInstance(){
		return instance;
	}
	
	

	@Override
	public Object instantiateItem(ViewGroup container, int position) {
		return super.instantiateItem(container, position);
	}

	@Override
	public Fragment getItem(int arg0) {
		Log.d("TEST", "getItem() position:" + arg0);
		// TODO Auto-generated method stub
		FragmentInfo info = infos.get(arg0);
		Fragment f ;
//		f = Fragment.instantiate(context, FramMatterFragment.class.getName(),info.arg);
		/*
		 * 实例化每一个fragment
		 */
		f = Fragment.instantiate(context, info.class_.getName(), info.arg);
		Log.d("TEST", "getItem() info:" + info);
		Log.d("TEST", "getItem() info.class_.getName():" + info.class_.getName());
		return f;
	}

	@Override
	public int getCount() {
		// TODO Auto-generated method stub
		return infos.size();
	}

	public interface MenuClickLis{
		void click();
	}
	

}
