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
    
    :param: title    标题
    :param: message  内容
    :param: duration 显示时间，eg. 3 (3秒)
    */
    func showToast(title:String,message:String,duration:Double){
        var toast = UIAlertView(title: title, message: message, delegate: nil, cancelButtonTitle: nil)
        NSTimer.scheduledTimerWithTimeInterval(duration, target: self, selector: "cancelToast:", userInfo: toast, repeats: true)
        toast.show()
    }
    
    /**
    取消提示框
    
    :param: alert <#alert description#>
    */
    func cancelToast(alert: NSTimer){
        if alert.userInfo != nil{
            if alert.userInfo is UIAlertView{
                (alert.userInfo as! UIAlertView).dismissWithClickedButtonIndex(0, animated: true)
            }
        }
       
    }
}