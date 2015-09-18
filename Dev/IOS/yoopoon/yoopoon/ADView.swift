//
//  ADView.swift
//  yoopoon
//  不重用view，重用出现图片切换时抖动现象，以后解决
//  Created by yunjoy on 15/7/15.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//
import Foundation
import UIKit
/**
pageControlShowStyle

- UIPageControlShowStyleNone//default: <#UIPageControlShowStyleNone//default description#>
- UIPageControlShowStyleLeft:          <#UIPageControlShowStyleLeft description#>
- UIPageControlShowStyleCenter:        <#UIPageControlShowStyleCenter description#>
- UIPageControlShowStyleRight:         <#UIPageControlShowStyleRight description#>
*/
enum UIPageControlShowStyle{
    case UIPageControlShowStyleNone //default
    case UIPageControlShowStyleLeft
    case UIPageControlShowStyleCenter
    case UIPageControlShowStyleRight
}
/**
广告标题显示的样式

- AdTitleShowStyleNone:   <#AdTitleShowStyleNone description#>
- AdTitleShowStyleLeft:   <#AdTitleShowStyleLeft description#>
- AdTitleShowStyleCenter: <#AdTitleShowStyleCenter description#>
- AdTitleShowStyleRight:  <#AdTitleShowStyleRight description#>
*/
enum ADTitleShowStyle{
    case AdTitleShowStyleNone
    case AdTitleShowStyleLeft
    case AdTitleShowStyleCenter
    case AdTitleShowStyleRight
}
class ADView: UIView,UIScrollViewDelegate {
    
    //滑动定时器
    var moveTimer:NSTimer?
    //滑动控件和页面控制器
    var adScrollView: UIScrollView?
    var pageControl: UIPageControl?
   
    //图片url数组
    var imageLinkURL:[String]?{
        
        didSet{
            adScrollView!.contentSize = CGSizeMake(kADWidth!*CGFloat(self.imageLinkURL!.count+2), kADHeight!)
            var index:CGFloat = 1.0
            for url in self.imageLinkURL!{
                let imageView = UIImageView(frame: CGRectMake(kADWidth!*index, 0, kADWidth!, kADHeight!))
                index = index + CGFloat(1)
                imageView.loadImageFromURLString(url,placeholderImage:placeHoldImage)
                adScrollView!.addSubview(imageView)
            }
            if self.imageLinkURL?.count>1{
            var imageView = UIImageView(frame: CGRectMake(0, 0, kADWidth!, kADHeight!))
            
            imageView.loadImageFromURLString((self.imageLinkURL?.last)!,placeholderImage:placeHoldImage)
            adScrollView!.addSubview(imageView)
            
            imageView = UIImageView(frame: CGRectMake(kADWidth!*index, 0, kADWidth!, kADHeight!))
            
            imageView.loadImageFromURLString((self.imageLinkURL?.first as String?)!,placeholderImage:placeHoldImage)
            adScrollView!.addSubview(imageView)
            }
            self.addSubview(adScrollView!)
            
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
  
    //用于确定滚动式由人导致的还是计时器到了,系统帮我们滚动的,true,则为系统滚动,false则为客户滚动(ps.在客户端中客户滚动一个广告后,这个广告的计时器要归0并重新计时)
    var isTimeUp = false;
    //常量，宽与高
    var kADWidth:CGFloat?;
    var kADHeight:CGFloat?;
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
    }
    override init(frame: CGRect) {
        super.init(frame: frame)
       
        adMoveTime = 3.0
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
        
        //该句是否执行会影响pageControl的位置,如果该应用上面有导航栏,就是用该句,否则注释掉即可
        adScrollView!.contentInset = UIEdgeInsetsMake(0, 0, 0, 0);
        
        // isNeedCycleRoll = true
        
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
    
    - parameter timer: <#timer description#>
    */
    func animalMoveImage(timer: NSTimer){

        self.adScrollView!.setContentOffset(CGPointMake(kADWidth!+CGFloat(self.adScrollView!.contentOffset.x), 0), animated: true)
        isTimeUp = true
        NSTimer.scheduledTimerWithTimeInterval(0.4, target: self, selector: "scrollViewDidEndDecelerating:", userInfo: nil, repeats: false)
    }
    
    
    
    /// 类方法
    class func adScrollViewWithFrame(frame:CGRect,imageLinkUrl:[String],placeHoderImageName:String,pageControlShowStyle:UIPageControlShowStyle)->ADView?{
        if imageLinkUrl.count==0{
            return nil
        }
        let adView = ADView(frame: frame)
        
         adView.placeHoldImage = UIImage(named: placeHoderImageName)
        adView.imageLinkURL = imageLinkUrl
       adView.pageControlShowStyle = pageControlShowStyle
        
        return adView
    }
    
    /**
    图片停止时，调用该函数使得滚动视图复用
    
    - parameter scrollView: <#scrollView description#>
    */
    func scrollViewDidEndDecelerating(scrollView: UIScrollView) {
        
        let number: NSNumber = NSNumber(float: Float(adScrollView!.contentOffset.x / kADWidth!))
       
        if adScrollView!.contentOffset.x == 0{
            pageControl!.currentPage = self.imageLinkURL!.count-1
            adScrollView!.contentOffset = CGPointMake(kADWidth!*CGFloat(self.imageLinkURL!.count), 0)
        }else if adScrollView!.contentOffset.x == CGFloat(self.imageLinkURL!.count+1)*self.kADWidth!{
            pageControl!.currentPage = 0
            adScrollView!.contentOffset = CGPointMake(kADWidth!, 0)
        }else{
            pageControl!.currentPage = number.integerValue-1
        }
       
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
    
    - parameter newSuperview: <#newSuperview description#>
    */
    override func willMoveToSuperview(newSuperview: UIView?) {
        
        setUpTime()
       
    }
}