package com.yoopoon.home;

import java.util.HashMap;

import android.app.ProgressDialog;
import android.content.Context;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.TextView.OnEditorActionListener;

import com.yoopoon.common.base.utils.Utils;
import com.yoopoon.home.data.net.ProgressMessage;
import com.yoopoon.home.data.net.RequestAdapter;
import com.yoopoon.home.data.net.ResponseData;
import com.yoopoon.home.data.net.RequestAdapter.RequestMethod;

/**
 * 搜索数据用
 */

public class SearchFunction  {

    public String getSearchText() {
		return searchText;
	}

	public void setSearchText(String searchText) {
		this.searchText = searchText;
		this.text.setText(searchText);
	}

	String searchText;
    Button delText;
    EditText text;
    Context mContext;
    String url;
    View searchView;
    HashMap<String,String> paramMap;
    int pageCount = 1;
    protected ProgressDialog progressDialog;
    OnSearchCallBack listener;
    Button serachButton ;
   
    public SearchFunction(Context context,String url){
        mContext = context;
        paramMap = new HashMap<String, String>();
        progressDialog = new ProgressDialog(context);

        setProgressDialog();
        this.url = url;
       
        searchView  = LayoutInflater.from(context).inflate(R.layout.search_action_view,null);
        delText = (Button)searchView.findViewById(R.id.matter_search_del_view);
        text = (EditText)searchView.findViewById(R.id.matter_search_text);
        text.setImeActionLabel("搜索", EditorInfo.IME_ACTION_SEARCH);
        text.setImeOptions(EditorInfo.IME_ACTION_SEARCH);
        text.setSingleLine();
        text.setOnEditorActionListener(new OnEditorActionListener(){
			@Override
			public boolean onEditorAction(TextView v, int actionId,
					KeyEvent event) {
					switch(actionId){
						case  EditorInfo.IME_ACTION_SEARCH: //actionDone 事件时提交登录
							readySearch();
							break;
					}				
				return false;
			}});
         serachButton = (Button)searchView.findViewById(R.id.matter_search_button);
        
        serachButton.setOnClickListener(searchButtonClickListener);
        text.addTextChangedListener(searchWatcher);
        delText.setOnClickListener(delClickListener);
    }

    

	private void setProgressDialog(){
        progressDialog.setMessage("正在搜索，请稍后...");
        progressDialog.setTitle("搜索");
        progressDialog.setCanceledOnTouchOutside(false);
    }

    //搜索数据linstener
    View.OnClickListener searchButtonClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            if (searchText == null || searchText.equalsIgnoreCase("")){
                Toast.makeText(mContext, "请输入搜索的内容", 100).show();
                return;
            }
            Utils.hiddenSoftBorad(mContext);

           setPageCount(1);
           progressDialog.show();
            search();


        }
    };
    
    private void readySearch(){
    	 if (searchText == null || searchText.equalsIgnoreCase("")){
             Toast.makeText(mContext, "请输入搜索的内容", 100).show();
             return;
         }
         Utils.hiddenSoftBorad(mContext);

        setPageCount(1);
        progressDialog.show();
         search();

    }

    //搜索内容改变进行的操作
    TextWatcher searchWatcher = new TextWatcher() {
        @Override
        public void beforeTextChanged(CharSequence charSequence, int i, int i2, int i3) {

        }

        @Override
        public void onTextChanged(CharSequence charSequence, int i, int i2, int i3) {
            searchText = charSequence.toString();
            if (!searchText.trim().equalsIgnoreCase("")){
            	
                delText.setVisibility(View.VISIBLE);
                listener.textChange(true);
            }else{
                delText.setVisibility(View.GONE);
              
                
                listener.textChange(false);
            }
        }

        @Override
        public void afterTextChanged(Editable editable) {

        }
    };

    //删除搜索内容
    View.OnClickListener delClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View view) {
            text.setText("");
           // listener.deltext();
        }
    };
    //数据参数
    public void setParam(HashMap<String,String> paramMap){
    	this.paramMap.clear();
    	this.paramMap.putAll(paramMap);

    }
    
   

    public  void search(){
    	
        paramMap.put("condition",searchText);
    	
        new RequestAdapter(){

            @Override
            public void onReponse(com.yoopoon.home.data.net.ResponseData data) {

                    if (progressDialog.isShowing()){
                    progressDialog.dismiss();
                }
                    if(pageCount > 1){
                    	listener.addMore(data);
                    }else{
                    listener.search(data);
                    }
                pageCount ++;
                paramMap.put("page", pageCount+"");
            }

            @Override
            public void onProgress(ProgressMessage msg) {

            }
        }.setUrl(url)
                .addParam(paramMap)
                .setRequestMethod(RequestMethod.eGet)
                .notifyRequest();
    }

    //分页
    public void setPageCount(int count){
        pageCount = count;
        paramMap.put("page", pageCount+"");

    }

    public int getPageCount(){
        return pageCount;
    }

   
    
    public View getSearchView(){
        return searchView;
    }

  
    
    public interface OnSearchCallBack{
    	void search(ResponseData data);
    	void addMore(ResponseData data);
    	void textChange(Boolean isText);
    	void deltext();
    	void clearRefresh();
    }
    
    public void setSearchCallBack(OnSearchCallBack listener){
    	this.listener = listener;
    }

    //清空搜索数据
    public void clearSearch(){
        if (!text.getText().toString().trim().equalsIgnoreCase("")) {
            text.setText("");
            if(listener != null){
            	listener.clearRefresh();
            }
        }
    }

	public void setHint(String string) {
		text.setHint(string);
		
	}

	public void cleanFocus() {
		this.text.clearFocus();
		
	}

	public void descCount() {
		pageCount --;
        paramMap.put("page", pageCount+"");
		
	}

    //未完全退出后要清除原来的搜索字符

}
