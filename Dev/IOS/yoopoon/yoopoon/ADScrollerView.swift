//
//  ADScrollerView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/14.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
///**
//pageControlShowStyle
//
//- UIPageControlShowStyleNone//default: <#UIPageControlShowStyleNone//default description#>
//- UIPageControlShowStyleLeft:          <#UIPageControlShowStyleLeft description#>
//- UIPageControlShowStyleCenter:        <#UIPageControlShowStyleCenter description#>
//- UIPageControlShowStyleRight:         <#UIPageControlShowStyleRight description#>
//*/
//enum UIPageControlShowStyle{
//    case UIPageControlShowStyleNone //default
//    case UIPageControlShowStyleLeft
//    case UIPageControlShowStyleCenter
//    case UIPageControlShowStyleRight
//}
///**
//广告标题显示的样式
//
//- AdTitleShowStyleNone:   <#AdTitleShowStyleNone description#>
//- AdTitleShowStyleLeft:   <#AdTitleShowStyleLeft description#>
//- AdTitleShowStyleCenter: <#AdTitleShowStyleCenter description#>
//- AdTitleShowStyleRight:  <#AdTitleShowStyleRight description#>
//*/
//enum ADTitleShowStyle{
//    case AdTitleShowStyleNone
//    case AdTitleShowStyleLeft
//    case AdTitleShowStyleCenter
//    case AdTitleShowStyleRight
//}
class ADScrollerView: UIView,UIScrollViewDelegate {
    
    //滑动定时器
    var moveTimer:NSTimer?
    //滑动控件和页面控制器
    var adScrollView: UIScrollView?
    var pageControl: UIPageControl?
    var centerAdLabel: UILabel?
    var leftImageView:UIImageView?
    var centerImageView:UIImageView?
    var rightImageView:UIImageView?
    //图片url和广告标题数组
    var imageLinkURL:[String]?{
       
        didSet{
            
            //self.imageLinkURL = newValue
            leftImageIndex = self.imageLinkURL!.count-1
            centerImageIndex = 0
            rightImageIndex = 1
            if self.imageLinkURL!.count==1{
                adScrollView!.scrollEnabled = false
                rightImageIndex = 0
            }
            //初始化图片控件
            leftImageView!.loadImageFromURLString(self.imageLinkURL![leftImageIndex], placeholderImage: self.placeHoldImage)
            
            centerImageView!.loadImageFromURLString(self.imageLinkURL![centerImageIndex], placeholderImage: self.placeHoldImage)
            rightImageView!.loadImageFromURLString(self.imageLinkURL![rightImageIndex], placeholderImage: self.placeHoldImage)
        }
        
    }
    var adTitleArray: [String]?{
        
        didSet{
            
            if adTitleShowStyle == .AdTitleShowStyleNone{
                return
            }
            //上面的灰色遮罩
            var vv = UIView(frame: CGRectMake(0, kADHeight!-30, kADWidth!, 30))//UIView(frame: cgre0, kADHeight! - 30, kADWidth!, 30)
            
            vv.backgroundColor = UIColor.blackColor()
            vv.alpha = 0.3;
            self.addSubview(vv)
            self.bringSubviewToFront(pageControl!)
            
            //上面的标题
            centerAdLabel = UILabel()
            centerAdLabel!.backgroundColor = UIColor.clearColor();
            centerAdLabel!.frame = CGRectMake(0, kADHeight! - 30, kADWidth!-20, 30);
            centerAdLabel!.textColor = UIColor.lightGrayColor();
            centerAdLabel!.font = UIFont.boldSystemFontOfSize(15);
            self.addSubview(centerAdLabel!)
            
            if adTitleShowStyle == .AdTitleShowStyleLeft
            {
                
                centerAdLabel!.textAlignment = NSTextAlignment.Left
            }
            else if adTitleShowStyle == .AdTitleShowStyleCenter
            {
                centerAdLabel!.textAlignment = NSTextAlignment.Center
            }
            else
            {
                centerAdLabel!.textAlignment = NSTextAlignment.Right
            }
            centerAdLabel!.text = self.adTitleArray![centerImageIndex];
        }
    }
    //显示样式
    var pageControlShowStyle: UIPageControlShowStyle = .UIPageControlShowStyleLeft{
        
        didSet{
            //self.pageControlShowStyle = newValue
            if self.pageControlShowStyle == .UIPageControlShowStyleNone || imageLinkURL!.count<=1{
                return;
            }
            pageControl = UIPageControl()
            pageControl!.numberOfPages = self.imageLinkURL!.count
            
            
            if (self.pageControlShowStyle == .UIPageControlShowStyleLeft)
            {
               
                pageControl!.frame = CGRectMake(0, kADHeight!-20, CGFloat(pageControl!.numberOfPages)*20, 20)
                
            }
            else if (self.pageControlShowStyle == .UIPageControlShowStyleCenter)
            {
                pageControl!.frame = CGRectMake(0.0, 0.0, 20.0*CGFloat(pageControl!.numberOfPages), 20.0);
                pageControl!.center = CGPointMake(kADWidth!/2.0, kADHeight! - 30);
            }
            else
            {
                
                pageControl!.frame = CGRectMake( (kADWidth!-20*CGFloat(pageControl!.numberOfPages))*1.0, kADHeight!-40, 20*CGFloat(self.pageControl!.numberOfPages), 20);
            }
            pageControl!.currentPage = 0;
            pageControl!.enabled = false;
            self.addSubview(pageControl!)
        }
    }
    var adTitleShowStyle: ADTitleShowStyle = .AdTitleShowStyleLeft
    //加载时的占位图片
    var placeHoldImage: UIImage?
    //设置是否需求定时滚动
    var isNeedCycleRoll: Bool = true{
       
        didSet{
            
            if !isNeedCycleRoll{
                moveTimer?.invalidate()
                moveTimer = nil
            }
        }
    }
    //设置滚动的时间，默认三秒
    var adMoveTime:Double = 3.0
    //三个图片控件对应的下标
    var leftImageIndex: Int = 0
    var centerImageIndex: Int = 0
    var rightImageIndex: Int = 0
    //用于确定滚动式由人导致的还是计时器到了,系统帮我们滚动的,true,则为系统滚动,false则为客户滚动(ps.在客户端中客户滚动一个广告后,这个广告的计时器要归0并重新计时)
    var isTimeUp = false;
    //常量，宽与高
    var kADWidth:CGFloat?;
    var kADHeight:CGFloat?;
    
    required init(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    override init(frame: CGRect) {
        super.init(frame: frame)
        
         //println("size=\(KFImageCacheManager.sharedInstance.diskCacheMaxAge)")
        adMoveTime = 2.0
        adScrollView = UIScrollView(frame: frame)
        kADWidth = adScrollView?.bounds.size.width
        kADHeight = adScrollView?.bounds.size.height
        adScrollView!.bounces = false
        adScrollView!.delegate = self;
        adScrollView!.pagingEnabled = true
        adScrollView!.showsHorizontalScrollIndicator = false
        adScrollView!.showsVerticalScrollIndicator = false
        adScrollView!.backgroundColor  = UIColor.whiteColor()
        adScrollView?.contentOffset = CGPointMake(kADWidth!, 0)
        adScrollView?.contentSize = CGSizeMake(kADWidth!*3, kADHeight!)
        //该句是否执行会影响pageControl的位置,如果该应用上面有导航栏,就是用该句,否则注释掉即可
        adScrollView!.contentInset = UIEdgeInsetsMake(0, 0, 0, 0);
        
        leftImageView = UIImageView(frame: CGRectMake(0, 0, kADWidth!, kADHeight!))
        adScrollView!.addSubview(leftImageView!)
        
        centerImageView = UIImageView(frame: CGRectMake(kADWidth!, 0, kADWidth!, kADHeight!))
        adScrollView!.addSubview(centerImageView!)
        
        rightImageView = UIImageView(frame: CGRectMake(kADWidth!*2, 0, kADWidth!, kADHeight!))
        adScrollView!.addSubview(rightImageView!)
        
       // isNeedCycleRoll = true
        self.addSubview(adScrollView!)
    }
    
    /**
    创建定时器
    */
    func setUpTime(){
        if isNeedCycleRoll && imageLinkURL!.count>=2{
            moveTimer = NSTimer.scheduledTimerWithTimeInterval(adMoveTime, target: self, selector: "animalMoveImage:", userInfo: nil, repeats: true)
            isTimeUp = false
        }
    }
    
    /**
    定时器到时间后滚动图片，
    
    :param: timer <#timer description#>
    */
    func animalMoveImage(timer: NSTimer){
        
        self.adScrollView!.setContentOffset(CGPointMake(kADWidth!*2.0, 0), animated: true)
        isTimeUp = true
        NSTimer.scheduledTimerWithTimeInterval(0.4, target: self, selector: "scrollViewDidEndDecelerating:", userInfo: nil, repeats: false)
    }
    
    
    /// 类方法
    class func adScrollViewWithFrame(frame:CGRect,imageLinkUrl:[String],pageControlShowStyle:UIPageControlShowStyle)->ADScrollerView?{
        if imageLinkUrl.count==0{
            return nil
        }
        var adView = ADScrollerView(frame: frame)
      
        adView.imageLinkURL = imageLinkUrl
        
        adView.pageControlShowStyle = pageControlShowStyle
        return adView
    }
    
    /// 类方法
    class func adScrollViewWithFrame(frame:CGRect,imageLinkUrl:[String],placeHoderImageName:String,pageControlShowStyle:UIPageControlShowStyle)->ADScrollerView?{
        if imageLinkUrl.count==0{
            return nil
        }
        var adView = adScrollViewWithFrame(frame, imageLinkUrl: imageLinkUrl, pageControlShowStyle: pageControlShowStyle)
        adView!.placeHoldImage = UIImage(named: placeHoderImageName)
        
        return adView
    }
    
    /// 类方法
    class func adScrollViewWithFrame(frame:CGRect,locaIimageLinkUrl imageLinkUrl:[String],pageControlShowStyle:UIPageControlShowStyle)->ADScrollerView?{
        if imageLinkUrl.count==0{
            return nil
        }
        // 加载本地方法未实现
//        var imagePath = [String]()
//        for path in imageLinkUrl{
//            var filePath = NSBundle.mainBundle().pathForResource(path, ofType: "jpg")
//            NSURL.fileURLWithPath(filePath!)
//        }
        var adView = adScrollViewWithFrame(frame, imageLinkUrl: imageLinkUrl, pageControlShowStyle: pageControlShowStyle)
        return adView
    }
    
    /**
    图片停止时，调用该函数使得滚动视图复用
    
    :param: scrollView <#scrollView description#>
    */
     func scrollViewDidEndDecelerating(scrollView: UIScrollView) {
        
        if adScrollView!.contentOffset.x == 0{
            centerImageIndex = centerImageIndex-1
            leftImageIndex = leftImageIndex-1
            rightImageIndex = rightImageIndex-1
            if leftImageIndex == -1{
                leftImageIndex = self.imageLinkURL!.count - 1
            }
            if centerImageIndex == -1{
                centerImageIndex = self.imageLinkURL!.count - 1
                
            }
            if(rightImageIndex == -1){
                rightImageIndex = self.imageLinkURL!.count - 1
            }
        }else if adScrollView?.contentOffset.x == kADWidth!*2{
            centerImageIndex = centerImageIndex + 1;
            leftImageIndex = leftImageIndex + 1;
            rightImageIndex = rightImageIndex + 1;
            
            if leftImageIndex == imageLinkURL!.count {
                leftImageIndex = 0;
            }
            if centerImageIndex == imageLinkURL!.count
            {
                centerImageIndex = 0;
            }
            if rightImageIndex == imageLinkURL!.count
            {
                rightImageIndex = 0;
            }

        }
        else{
            return
        }
        leftImageView!.loadImageFromURLString(self.imageLinkURL![leftImageIndex])//, placeholderImage: self.placeHoldImage, completion: nil)
        
        centerImageView!.loadImageFromURLString(self.imageLinkURL![centerImageIndex])//, placeholderImage: self.placeHoldImage)
        rightImageView!.loadImageFromURLString(self.imageLinkURL![rightImageIndex])//, placeholderImage: self.placeHoldImage)
        self.pageControl!.currentPage = centerImageIndex
        
        //有时候只有在有广告标签的时候才需要加载
        if adTitleArray != nil{
            if centerImageIndex <= adTitleArray!.count - 1{
                centerAdLabel?.text = self.adTitleArray![centerImageIndex]
            }
        }
       
       adScrollView?.contentOffset = CGPointMake(kADWidth!, 0)
        
        
        //手动控制图片滚动应该取消那个三秒的计时器
        if !isTimeUp{
            moveTimer!.fireDate = NSDate(timeIntervalSinceNow: adMoveTime)
        }
        isTimeUp = false
    }
    
    func scrollViewWillBeginDragging(scrollView: UIScrollView) {
        moveTimer!.invalidate()
        moveTimer = nil
    }
    
    func scrollViewDidEndDragging(scrollView: UIScrollView, willDecelerate decelerate: Bool) {
        self.setUpTime()
    }
    
    /**
    子视图添加到父视图或者离开父视图时调用
    
    :param: newSuperview <#newSuperview description#>
    */
   override func willMoveToSuperview(newSuperview: UIView?) {
        //if (newSuperview != nil){
            //self.moveTimer?.invalidate()
        //}else{
            setUpTime()
       // }
    }
}