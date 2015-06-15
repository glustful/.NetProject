package com.miicaa.home.ui.home;

import java.util.ArrayList;

import org.androidannotations.annotations.AfterInject;
import org.androidannotations.annotations.AfterViews;
import org.androidannotations.annotations.EFragment;
import org.androidannotations.annotations.ViewById;

import android.content.ContentValues;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.RelativeLayout;

import com.miicaa.common.base.OnEachRow;
import com.miicaa.home.R;
import com.miicaa.home.data.business.account.AccountInfo;
import com.miicaa.home.data.business.org.UserInfo;
import com.miicaa.home.data.business.org.UserInfoSql;
import com.miicaa.home.ui.contactList.ContactData;
import com.miicaa.home.ui.contactList.ContactList;
//@EFragment(R.layout.home_fram_contact_fragment)
@EFragment(R.layout.home_fram_contact_fragment)
public class FramContactFragment extends Fragment{
	public static FramContactFragment instance = null;
	
	private static String TAG = "FramContactFragment";
@ViewById(R.id.homeMatterTopLayout)
RelativeLayout topLayout;
@ViewById(R.id.homeMatterProgressBar)
ProgressBar mProgressBar;

ContactList contactList;


@AfterInject
void afterInject(){
	instance = this;
}
@AfterViews
 void createListView() {
	Log.d(TAG, "afterView is running!");
	createList();
}

public void createList(){
	ArrayList<ContactData> contactDataArrayList = new ArrayList<ContactData>();
    contactList = new ContactList(topLayout.getContext(), contactDataArrayList,"newList",null,ContactList.MUTILSELECT);
    addNewView();
}

public void refreshList(){
	getAllUserInOrg();
}

void addNewView(){
	topLayout.removeAllViews();
	topLayout.addView(contactList.getRootView());
}


// 从数据库中查找本单位的所有联系人
 private void getAllUserInOrg() {
	 new MyContactListTask()
	 .execute(1);
}
 
 
 
   @Override
public void onResume() {
	   getAllUserInOrg();
	super.onResume();
}



class MyContactListTask extends AsyncTask<Integer, Long, ArrayList<ContactData>>{

	@Override
	protected ArrayList<ContactData> doInBackground(Integer... params) {
		  ArrayList<ContactData> list = new ArrayList<ContactData>();
		    UserInfo.usersInOrg(AccountInfo.instance().getLastOrgInfo(), new OnEachRow() {
		        @SuppressWarnings("unchecked")
				@Override
		        public void eachRow(ContentValues row, Object cbData) {
		        	
		            UserInfo user = UserInfoSql.fromRow(row);
		            ContactData contactData = new ContactData();
		            contactData.setName(user.getName());
		            contactData.setUserCode(user.getCode());
		            contactData.setDataType("person");
		            contactData.setQuanPing(user.getNamePY());
		            contactData.setQuanPingFirst(user.getNameFPY());
		            contactData.setUid(user.getId());
		            ((ArrayList<ContactData>) cbData).add(contactData);
		        }
		    }, list);
		    return  list;
	}

	@Override
	protected void onPreExecute() {
		mProgressBar.setVisibility(View.VISIBLE);
		super.onPreExecute();
	}

	@Override
	protected void onPostExecute(ArrayList<ContactData> result) {
		Log.d(TAG, "ContactData size:"+result.size());
		contactList.refreshList(result);
		if(result.size() > 0)
		mProgressBar.setVisibility(View.GONE);
		super.onPostExecute(result);
	}

	@Override
	protected void onProgressUpdate(Long... values) {
		super.onProgressUpdate(values);
	}

	@Override
	protected void onCancelled(ArrayList<ContactData> result) {
		super.onCancelled(result);
	}
	   
   }




	
}
