//
//  LoginViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/22.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class LoginViewController: SuperViewController {
    @IBOutlet weak var uiRememberMe: UIButton!
    @IBOutlet weak var uiPhone: UITextField!
    @IBOutlet weak var uiPassword: UITextField!
    @IBOutlet weak var uiRegister: UIButton!
    
    @IBAction func rememberMe(sender: UIButton) {
        sender.selected = !sender.selected
    }
    
    
    @IBAction func login(sender: UIButton) {
        if self.uiPhone.text == ""{
        self.uiPhone.shake(5,delta: 5)
            return
        }
        if self.uiPassword.text == ""{
            self.uiPassword.shake(5, delta: 5)
            return
        }
        loginWithPhone(self.uiPhone.text,password: self.uiPassword.text)
    }
    
    /**
    用户登陆
    
    :param: phone  手机号码
    :param: passwd 密码
    */
    private func loginWithPhone(phone:String,password passwd: String){
        RequestAdapter()
        .setUrl(urlUserLogin)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .setSaveSession(true)
        .addParameter("UserName", value: phone)
        .addParameter("Password", value: passwd)
        .setIsShowIndicator(true, currentView: self.view)
        .request({(json:JSON) in
            if let status = json["Status"].bool{
                if status{
                    User.share.store(json,phone: phone, password: passwd, isRemember: self.uiRememberMe.selected)
                    self.navigationController?.popViewControllerAnimated(true)
                }else{
                   TipTools().showToast("登陆失败", message: json["Msg"].string ?? "失败", duration: 2)
                }
            }else{
                TipTools().showToast("登陆失败", message: "\(json)", duration: 2)
            }
            }, faild: {error in
                TipTools().showToast("登陆失败", message: "\(error?.description)", duration: 2)
        })
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "用户登陆"
        self.uiRememberMe.setImage(UIImage(named: "check"), forState: UIControlState.Normal)
        self.uiRememberMe.setImage(UIImage(named: "checked"), forState: UIControlState.Selected)
        initData()
    }
    
    /**
    初始化用户名和密码
    
    :returns: <#return value description#>
    */
    func initData(){
        var user = User.share
        self.uiPhone.text = user.phone
        self.uiPassword.text = user.password
        self.uiRememberMe.selected = user.isRemember
    }
    
   }
