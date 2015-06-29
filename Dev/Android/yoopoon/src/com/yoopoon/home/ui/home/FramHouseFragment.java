package com.yoopoon.home.ui.home;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.EFragment;

import com.yoopoon.home.R;

@EFragment(R.layout.home_fram_house_fragment)
public class FramHouseFragment extends FramSuper{
	public static FramHouseFragment instance = null;


@AfterInject
void afterInject(){
	instance = this;
	
}




}
