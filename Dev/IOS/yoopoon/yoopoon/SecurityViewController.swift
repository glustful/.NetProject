//
//  SecurityViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class SecurityViewController: TextFieldViewController {

    private var timeCount = 60
    private var brokerEntity = ["OldPassword":"","Password":"","SecondPassword":"","MobileYzm":"","Hidm":""]
    @IBOutlet weak var uiPhone: UITextField!
    @IBOutlet weak var uiPasswd: UITextField!
    @IBOutlet weak var uiConfirePasswd: UITextField!
    @IBOutlet weak var uiValide: UITextField!
    @IBOutlet weak var uiSendSMS: UIButton!
    @IBOutlet weak var constraintViewWidth: NSLayoutConstraint!
    
    @IBAction func sendSMSAction(sender: UIButton) {
        if User.share.phone == nil{
            TipTools().showToast("提示", message: "电话号码没发现，请完成个人资料", duration: 2)
            return
        }
        sendSMS()
    }
    
    @IBAction func registerAction(sender: UIButton) {
        hidenKeyBoard()
        if validate(){
            register()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "安全设置"
        self.constraintViewWidth.constant = screenBounds.width
    }
    
    //验证输入信息是否正确
    private func validate()->Bool{
        if uiValide.text == nil || uiValide.text == ""{
            uiValide.shake(5, delta: 5)
            return false
        }
        self.brokerEntity.updateValue(uiValide.text, forKey: "MobileYzm")
        if uiPhone.text == nil || uiPhone.text == ""{
            uiPhone.shake(5, delta: 5)
            return false
        }
        
        self.brokerEntity.updateValue(uiPhone.text, forKey: "OldPassword")
        
        if uiPasswd.text == nil || uiPasswd.text == ""{
            uiPasswd.shake(5, delta: 5)
            return false
        }
        
        if uiConfirePasswd.text == nil || uiConfirePasswd.text == "" || uiConfirePasswd.text != uiPasswd.text{
            TipTools().showToast("提示", message: "两次输入的密码不相同，重新输入", duration: 2)
            return false
        }
        self.brokerEntity.updateValue(uiPasswd.text, forKey: "Password")
        self.brokerEntity.updateValue(uiConfirePasswd.text, forKey: "SecondPassword")
        return true
    }
    
    //提交注册信息
    private func register(){
        RequestAdapter()
            .setEncoding(.JSON)
            .setRequestMethod(.POST)
            .setUrl(urlUserChangeForPassword)
            .setIsShowIndicator(true, currentView: self.view)
            .setParameters(brokerEntity)
            .request({json in
                
                if let status = json["Status"].bool{
                    if status{
                        
                        self.navigationController!.popViewControllerAnimated(true)
                        return
                    }
                }
                TipTools().showToast("错误", message: "修改密码失败，请重试", duration: 2)
                }, faild: {error in
                    TipTools().showToast("错误", message: "修改密码失败，请重试", duration: 2)
            })
    }
    
    /**
    发送验证码
    */
    private func sendSMS(){
        uiSendSMS.enabled = false
        timeCount = 60
        NSTimer.scheduledTimerWithTimeInterval(1, target: self, selector: "changeTimeCount:", userInfo: nil, repeats: true)
        uiSendSMS.backgroundColor = UIColor.grayColor()
        uiSendSMS.setTitle("重新发送(\(timeCount))", forState: UIControlState.Normal)
        RequestAdapter()
            .setUrl(urlSMSSendForChangePassword)
            .setEncoding(.JSON)
            .setRequestMethod(.POST)
            .addParameter("smstype", value: SMSType.changePasswordType)
            .request({json in
                
                if json["Message"].stringValue == "1"{
                    if let hidm = json["Desstr"].string{
                        self.brokerEntity.updateValue(hidm, forKey: "Hidm")
                    }
                }
                }, faild: {error in
                    print("\(error!.description)")
            })
    }
    
    func changeTimeCount(time: NSTimer){
        if timeCount == 0{
            uiSendSMS.enabled = true
            uiSendSMS.backgroundColor = appRedBackground
            uiSendSMS.setTitle("获取验证码", forState: UIControlState.Normal)
            time.invalidate()
            return
        }
        timeCount--
        uiSendSMS.setTitle("重新发送(\(timeCount))", forState: UIControlState.Normal)
    }
}
