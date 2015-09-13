package com.yoopoon.market.utils;

import android.os.AsyncTask;

public class ParserJSON extends AsyncTask<Void, Void, Object> {

    public interface ParseListener {
        public void onComplete(Object parseResult);
        public Object onParse();
    }

    private final ParseListener mParseListener;
  

    public ParserJSON(ParseListener parseListener) {
        mParseListener = parseListener;
        
    }

    @Override
    protected Object doInBackground(Void... params) {
        

        return mParseListener.onParse();
    }

    @Override
    protected void onPostExecute(Object parseResult) {
        mParseListener.onComplete(parseResult);
    }

}
