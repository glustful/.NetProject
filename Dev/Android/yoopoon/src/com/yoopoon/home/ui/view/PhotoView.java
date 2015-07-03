package com.yoopoon.home.ui.view;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.InputStream;

import com.yoopoon.common.base.photo.IPhotoView;
import com.yoopoon.common.base.photo.PhotoViewAttacher;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.RectF;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.util.AttributeSet;
import android.widget.ImageView;


public class PhotoView extends ImageView implements IPhotoView
{
    private final PhotoViewAttacher mAttacher;
    private ScaleType mPendingScaleType;

    public PhotoView(Context context)
    {
        this(context, null);
    }

    public PhotoView(Context context, AttributeSet attr)
    {
        this(context, attr, 0);
    }

    public PhotoView(Context context, AttributeSet attr, int defStyle)
    {
        super(context, attr, defStyle);
        super.setScaleType(ScaleType.MATRIX);
        mAttacher = new PhotoViewAttacher(this);

        if (null != mPendingScaleType) {
            setScaleType(mPendingScaleType);
            mPendingScaleType = null;
        }
    }

    @Override
    public boolean canZoom()
    {
        return mAttacher.canZoom();
    }

    @Override
    public RectF getDisplayRect() {
        return mAttacher.getDisplayRect();
    }

    @Override
    public float getMinScale() {
        return mAttacher.getMinScale();
    }

    @Override
    public float getMidScale() {
        return mAttacher.getMidScale();
    }

    @Override
    public float getMaxScale() {
        return mAttacher.getMaxScale();
    }

    @Override
    public float getScale() {
        return mAttacher.getScale();
    }

    @Override
    public ScaleType getScaleType() {
        return mAttacher.getScaleType();
    }

    @Override
    public void setAllowParentInterceptOnEdge(boolean allow) {
        mAttacher.setAllowParentInterceptOnEdge(allow);
    }

    @Override
    public void setMinScale(float minScale) {
        mAttacher.setMinScale(minScale);
    }

    public void setMinzoom()
    {
        mAttacher.setMinzoom();
    }

    @Override
    public void setMidScale(float midScale) {
        mAttacher.setMidScale(midScale);
    }

    @Override
    public void setMaxScale(float maxScale)
    {
        mAttacher.setMaxScale(maxScale);
    }

    @Override
    // setImageBitmap calls through to this method
    public void setImageDrawable(Drawable drawable) {
        super.setImageDrawable(drawable);
        if (null != mAttacher) {
            mAttacher.update();
        }
    }

    @Override
    public void setImageResource(int resId) {
        super.setImageResource(resId);
        if (null != mAttacher) {
            mAttacher.update();
        }
    }

    @Override
    public void setImageURI(Uri uri)
    {
        super.setImageURI(uri);
        if (null != mAttacher)
        {
            mAttacher.update();
        }
    }

    public void setImagePath(String path)
    {


        File file = new File(path);
        Bitmap bmp = null;
        if(file.length()>1024*1024)
        {
            BitmapFactory.Options options=new BitmapFactory.Options();
            options.inSampleSize=10;//图片缩放比例
            bmp = BitmapFactory.decodeFile(path, options);
        }
        else
        {
            bmp = BitmapFactory.decodeFile(path);
        }
        setImageBitmap(bmp);
        if (null != mAttacher)
        {
            mAttacher.update();
        }
    }

    public static byte[] readStream(InputStream in) throws Exception
    {
        byte[] buffer  =new byte[1024];
        int len  =-1;
        ByteArrayOutputStream outStream = new ByteArrayOutputStream();

        while((len=in.read(buffer))!=-1){
            outStream.write(buffer, 0, len);
        }
        byte[] data  =outStream.toByteArray();
        outStream.close();
        in.close();
        return data;
    }

    @Override
    public void setOnMatrixChangeListener(PhotoViewAttacher.OnMatrixChangedListener listener) {
        mAttacher.setOnMatrixChangeListener(listener);
    }

    @Override
    public void setOnLongClickListener(OnLongClickListener l) {
        mAttacher.setOnLongClickListener(l);
    }

    @Override
    public void setOnPhotoTapListener(PhotoViewAttacher.OnPhotoTapListener listener) {
        mAttacher.setOnPhotoTapListener(listener);
    }

    @Override
    public void setOnViewTapListener(PhotoViewAttacher.OnViewTapListener listener) {
        mAttacher.setOnViewTapListener(listener);
    }

    @Override
    public void setScaleType(ScaleType scaleType) {
        if (null != mAttacher) {
            mAttacher.setScaleType(scaleType);
        } else {
            mPendingScaleType = scaleType;
        }
    }

    @Override
    public void setZoomable(boolean zoomable) {
        mAttacher.setZoomable(zoomable);
    }

    @Override
    public void zoomTo(float scale, float focalX, float focalY) {
        mAttacher.zoomTo(scale, focalX, focalY);
    }

    @Override
    protected void onDetachedFromWindow() {
        mAttacher.cleanup();
        super.onDetachedFromWindow();
    }
}
