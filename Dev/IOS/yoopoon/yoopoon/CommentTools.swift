//
//  CommentTools.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/28.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class CommentTools: NSObject {
    
    /// 调用打电话功能
    class func dialPhonNumber(phoneNumber: String){
        //第一种方式，在webview里打开
       // var phoneUrl = NSURL(string: "tel:\(phoneNumber)")
        
       // var  phoneCallWebView = UIWebView(frame: CGRectZero)
       // phoneCallWebView.loadRequest(NSURLRequest(URL: phoneUrl!))
        
        //第二种，通用型，结束后停留在打电话界面
        UIApplication.sharedApplication().openURL(NSURL(string: "tel://\(phoneNumber)")!)
        //第三种，不能上appstore，调用私有api
        //UIApplication.sharedApplication().openURL(NSURL(string: "telprompt://\(phoneNumber)")!)
    }
    
    /// 移除指定元素下所有子元素
    class func removeAllViews(parentView: UIView){
        for view in parentView.subviews{
            if let child = view as? UIView{
                child.removeFromSuperview()
            }
        }
    }
    
    //根据传入的字符串，字体大小，显示宽度计算实际大小
    class func computerContentSize(content: String,fontSize: CGFloat,widgetWidth:CGFloat=screenBounds.width)->CGSize{
        
        let tmpContent: NSString = content
        
        let titleSize = tmpContent.boundingRectWithSize(CGSizeMake(widgetWidth, CGFloat(MAXFLOAT)), options: [NSStringDrawingOptions.UsesLineFragmentOrigin, NSStringDrawingOptions.UsesFontLeading], attributes: [NSFontAttributeName: UIFont.systemFontOfSize(fontSize)], context: nil).size
        return titleSize
        
    }
    
    
    //获取当前显示的controller
    class func getCurrentController()->UIViewController?{
        
        return ((UIApplication.sharedApplication().delegate as? AppDelegate)?.window?.rootViewController as? UINavigationController)?.visibleViewController
    }
    
    //JSON 到字典的转换
    class func jsonToDic(json: JSON)->[String:AnyObject]{
        var dic = [String:AnyObject]()
        for (key, subJson): (String, JSON) in json {
            
            dic.updateValue(subJson.stringValue, forKey: key)
        }
        return dic
    }
}
