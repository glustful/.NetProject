//
//  TipTools.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/20.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
class TipTools: NSObject {
    
    /**
    显示提示框，在设定的秒数后消失
    
    - parameter title:    标题
    - parameter message:  内容
    - parameter duration: 显示时间，eg. 3 (3秒)
    */
    func showToast(title:String,message:String,duration:Double){
        let alert = UIAlertController(title: title, message: message, preferredStyle: UIAlertControllerStyle.Alert)
       CommentTools.getCurrentController()!.presentViewController(alert, animated: true, completion: nil)
       
        NSTimer.scheduledTimerWithTimeInterval(duration, target: self, selector: "cancelToast:", userInfo: alert, repeats: true)
      
    }
    
    /**
    取消提示框
    
    - parameter alert: <#alert description#>
    */
    func cancelToast(alert: NSTimer){
        if alert.userInfo != nil{
            if alert.userInfo is UIAlertController{
                
                (alert.userInfo as! UIAlertController).dismissViewControllerAnimated(true, completion: nil)
            }
        }
       
    }
}