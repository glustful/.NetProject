package com.yoopoon.home.data.json;



import android.os.AsyncTask;

public class SerializerJSON extends AsyncTask<Void, Void, String> {

    public interface SerializeListener {
        public void onComplete(String serializeResult);
        public String onSerialize();
    }

    private final SerializeListener mParseListener;
    

    public SerializerJSON(SerializeListener serializeListener) {
        mParseListener = serializeListener;
        
    }

	@Override
    protected String doInBackground(Void... params) {
        

        return mParseListener.onSerialize();
    }

    @Override
    protected void onPostExecute(String parseResult) {
        mParseListener.onComplete(parseResult);
    }

   
}
