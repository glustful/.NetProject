package com.yoopoon.home.data.net;

import java.io.Serializable;

public class ProgressMessage implements Serializable
{
    private static final long serialVersionUID = -7060210544600464481L;
    private int mTotal = 0;
    private int mTransSize = 0;

    final protected void setTotal(int total)
    {
        mTotal = total;
    }

    final protected void setTransSize(int size)
    {
        mTransSize = size;
    }

    final public int getTotal()
    {
        return mTotal;
    }

    final public int getTransSize()
    {
        return mTransSize;
    }

    final public float getProgress()
    {
        if(mTotal == 0)
        {
            return 0.0f;
        }
        return ((float)mTransSize)/((float)mTotal);
    }
}
