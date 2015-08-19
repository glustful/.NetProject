//
//  RegisterViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/5.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class RegisterViewController: TextFieldViewController {
    private var timeCount = 60
    private var brokerEntity = ["Username":"","Password":"","SecondPassword":"","Phone":"","MobileYzm":"","Hidm":"","inviteCode":""]
    @IBOutlet weak var uiPhone: UITextField!
    @IBOutlet weak var uiPasswd: UITextField!
    @IBOutlet weak var uiConfirePasswd: UITextField!
    @IBOutlet weak var uiValide: UITextField!
    @IBOutlet weak var uiSendSMS: UIButton!
    @IBOutlet weak var constraintViewWidth: NSLayoutConstraint!
    
    @IBAction func goLoginAction(sender: UIButton) {
        self.navigationController!.popViewControllerAnimated(true)
    }
    @IBAction func sendSMSAction(sender: UIButton) {
        if uiPhone.text == nil || uiPhone.text == ""{
            uiPhone.shake(5, delta: 5)
            return
        }
        if !RegexHelper(phoneRegex).match(uiPhone.text){
            TipTools().showToast("格式不符", message: "电话号码格式不对，重新输入", duration: 2)
            return
        }
        sendSMS()
    }
    
    @IBAction func registerAction(sender: UIButton) {
        if validate(){
            register()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "用户注册"
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
        if !RegexHelper(phoneRegex).match(uiPhone.text){
            TipTools().showToast("格式不符", message: "电话号码格式不对，重新输入", duration: 2)
            return false
        }
        self.brokerEntity.updateValue(uiPhone.text, forKey: "Phone")
        self.brokerEntity.updateValue(uiPhone.text, forKey: "Username")
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
        .setUrl(urlUserRegister)
        .setIsShowIndicator(true, currentView: self.view)
        .setParameters(brokerEntity)
        .request({json in
            println(json)
            if let status = json["Status"].bool{
                if status{
                    User.share.store(json, phone: self.uiPhone.text, password: self.uiPasswd.text, isRemember: true)
                    self.navigationController!.popToViewController(self.navigationController!.viewControllers.first as! UIViewController, animated: true)
                    return
                }
            }
            TipTools().showToast("错误", message: "注册失败，请重试", duration: 2)
            }, faild: {error in
                TipTools().showToast("错误", message: "注册失败，请重试", duration: 2)
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
        .setUrl(urlSMSSend)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .addParameter("Mobile", value: uiPhone.text)
        .addParameter("SmsType", value: SMSType.registerType)
        .request({json in
            if json["Message"].stringValue == "1"{
                if let hidm = json["Desstr"].string{
                self.brokerEntity.updateValue(hidm, forKey: "Hidm")
                }
            }
            }, faild: {error in
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
