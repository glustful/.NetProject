//
//  TextFieldViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/6.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TextFieldViewController: SuperViewController {
    var currentField: UITextField?
    override func viewDidLoad() {
        super.viewDidLoad()
        let tap = UITapGestureRecognizer(target: self, action: "hidenKeyBoard")
        tap.numberOfTouchesRequired = 1
        self.view.addGestureRecognizer(tap)
    }
    
    /**
    隐藏软键盘
    */
    func hidenKeyBoard(){
        if let field = self.currentField{
            self.view.frame = CGRectMake(0, 0, self.view.frame.size.width, self.view.frame.size.height)
            
            field.resignFirstResponder()
        }
    }

    //MARK: -UITEXTFIELD代理方法
    func textFieldDidBeginEditing(textField: UITextField) {
        self.currentField = textField
        //var frame = textField.frame
        let frame = textField.convertRect(textField.bounds, toView: nil)
        
        var offset = frame.origin.y  + 40 - (self.view.frame.size.height - keyboardHeight)//键盘高度258
        let animationDuration: NSTimeInterval = 0.30
        UIView.beginAnimations("ResizeForKeyboard", context: nil)
        
        UIView.setAnimationDuration(animationDuration)
        offset += 64
        //将视图的Y坐标向上移动offset个单位，以使下面腾出地方用于软键盘的显示
        if(offset > 0){
            self.view.frame = CGRectMake(0.0, -offset, self.view.frame.size.width, self.view.frame.size.height)
        }
        UIView.commitAnimations()
    }
    
    func textFieldDidEndEditing(textField: UITextField){
        self.view.frame = CGRectMake(0, 0, self.view.frame.size.width, self.view.frame.size.height)
    }
    
    func textFieldShouldReturn(textField: UITextField) -> Bool{
        
        textField.resignFirstResponder()
        return true
    }


}
