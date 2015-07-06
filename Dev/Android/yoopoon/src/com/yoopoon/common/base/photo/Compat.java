package com.yoopoon.common.base.photo;

import android.os.Build;
import android.view.View;

/**
 * Created by Administrator on 13-12-10.
 */
public class Compat
{
    private static final int SIXTY_FPS_INTERVAL = 1000 / 60;
    public static void postOnAnimation(View view, Runnable runnable)
    {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN)
        {
            SDK16.postOnAnimation(view, runnable);
        }
        else
        {
            view.postDelayed(runnable, SIXTY_FPS_INTERVAL);
        }
    }
}
