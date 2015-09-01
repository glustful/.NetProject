//
//  UITextFieldExtention.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/23.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit


extension UITextField{
    
    /**
    uitextfield左右摆动提示出错
    
    :param: times 摆动次数
    :param: delta 摆动的距离
    */
    func shake(var times: Int,var delta: CGFloat){
        
        UIView.animateWithDuration(0.1, animations: {
            
        self.transform = CGAffineTransformMakeTranslation(delta, 0)
            }, completion: {finished in
                if times == 0{
                    UIView.animateWithDuration(0.1, animations: {
                        self.transform = CGAffineTransformIdentity
                    })
                    return
                }
                delta = -delta
                self.shake(--times,delta: delta)
        })
       
    }
}