package com.yoopoon.common.base.photo;

import android.annotation.TargetApi;
import android.os.Build;
import android.view.View;


public class SDK16
{
    @TargetApi(Build.VERSION_CODES.JELLY_BEAN)
    public static void postOnAnimation(View view, Runnable r)
    {
        view.postOnAnimation(r);
    }
}
