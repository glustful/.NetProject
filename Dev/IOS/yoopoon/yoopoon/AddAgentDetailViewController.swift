//
//  AddAgentDetailViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/6.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AddAgentDetailViewController: UIViewController {
    var type:Int = 0
    
    @IBOutlet weak var uiButton: UIButton!
    @IBOutlet weak var uiPhone: UITextField!
    @IBAction func pleaseAction(sender: UIButton) {
        if uiPhone.text == nil || uiPhone.text == ""{
            uiPhone.shake(5, delta: 5)
            return
        }
        if !RegexHelper(phoneRegex).match(uiPhone.text!){
            TipTools().showToast("格式不符", message: "电话号码格式不对，重新输入", duration: 2)
            return
        }
        print("phone=\(uiPhone.text)")
        sender.enabled = false
        if type == 0{
        requestAddPartner()
        }else{
            sendSMS()
        }
    }
    @IBAction func endInput(sender: UITextField) {
        sender.resignFirstResponder()
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        let gap = UITapGestureRecognizer(target: self, action: "hiddenKeyBoard")
        gap.numberOfTapsRequired = 1
        self.view.addGestureRecognizer(gap)
        
    }
    
    /**
    隐藏软键盘
    */
    func hiddenKeyBoard(){
        self.uiPhone.resignFirstResponder()
    }

    private func requestAddPartner(){
        RequestAdapter()
        .setUrl(urlPartnerListAddPartnet)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .setIsShowIndicator(true, currentView: self.view)
        .addParameter("Id", value: "0")
        .addParameter("Broker", value: "")
        .addParameter("PartnerId", value: "0")
        .addParameter("userId", value: "0")
        .addParameter("BrokerId", value: "0")
        .addParameter("Phone", value: uiPhone.text!)
        .request({json in
            self.uiButton.enabled = true
            if let status = json["Status"].bool{
                if status{
                    TipTools().showToast("提示", message: json["Msg"].stringValue, duration: 3)
                    NSTimer.scheduledTimerWithTimeInterval(2, target: self, selector: "cancel:", userInfo: nil, repeats: false)
                    return
                }
            }
            TipTools().showToast("提示", message: "请求出错，请重试", duration: 2)
            }, faild: {error in
                self.uiButton.enabled = true
                TipTools().showToast("提示", message: "\(error!.description)", duration: 2)
        })
    }
    
    private func sendSMS(){
        RequestAdapter()
            .setUrl(urlSMSSend)
            .setEncoding(.JSON)
            .setRequestMethod(.POST)
            .setIsShowIndicator(true, currentView: self.view)
            
            .addParameter("SmsType", value: SMSType.referAgentType)
           
            .addParameter("Mobile", value: uiPhone.text!)
            .request({json in
                self.uiButton.enabled = true
              
                        TipTools().showToast("提示", message: "邀请已发送", duration: 3)
                        NSTimer.scheduledTimerWithTimeInterval(2, target: self, selector: "cancel:", userInfo: nil, repeats: false)
                        
                }, faild: {error in
                    self.uiButton.enabled = true
                    TipTools().showToast("提示", message: "\(error!.description)", duration: 2)
            })
    }

    
    func cancel(timer: NSTimer){
        self.dismissViewControllerAnimated(true, completion: nil)
    }

//    override var preferredContentSize: CGSize{
//        get{
//            return CGSizeMake(screenBounds.width*2/3, 180)
//        }
//        set{
//            super.preferredContentSize = newValue
//        }
//    }


}
