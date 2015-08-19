//
//  User.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/24.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class User: NSObject {
    var id:Int?
    var userName: String?
    var phone: String?
    var password: String?
    var isRemember: Bool = false
    var isBroker: Bool?
    var isLogin = false
    var fromType = HouseFromType.comment
    
    class var share: User{
       return Inner.instance
    }
    
    private struct Inner {
        static let instance = User()
    }
    
    override init() {
        super.init()
        var db = NSUserDefaults()
        var userId = db.objectForKey("userId") as? Int
        if userId == nil{
            return
        }
        self.id = userId
        if let name = db.objectForKey("userName") as? String{
            self.userName = name
        }
        if let name = db.objectForKey("userName") as? String{
            self.userName = name
        }
        if let passwd = db.objectForKey("userPassword") as? String{
            self.password = passwd
        }
        if let remem = db.objectForKey("isRemeber") as? Bool{
            self.isRemember = remem
        }
        if let broker = db.objectForKey("isBroker") as? Bool{
            self.isBroker = broker
        }
        if let phone = db.objectForKey("phone") as? String{
            self.phone = phone
        }
        self.isLogin = false
        self.isBroker = false
    }
    
    /**
    登陆成功保存信息到本地
    
    :param: json       <#json description#>
    :param: password   <#password description#>
    :param: isRemember <#isRemember description#>
    */
    func store(json: JSON,phone: String,password: String,isRemember: Bool){
        var object = json["Object"]
        var userDb = NSUserDefaults()
        if let id = object["Id"].int{
            userDb.setValue(id, forKey: "userId")
            self.id = id
        }else{
            return
        }
        if let name = object["UserName"].string{
            userDb.setValue(name, forKey: "userName")
            self.userName = name
        }
        else{
            return
        }
        userDb.setValue(phone, forKey: "phone")
        self.phone = phone
        userDb.setValue(password, forKey: "userPassword")
        self.password = password
        userDb.setValue(true, forKey: "isLogin")
        isLogin = true
        userDb.setValue(isRemember, forKey: "isRemeber")
        self.isRemember = isRemember
        if let roles = object["Roles"].array{
            if roles.count > 0{
                if let roleName = roles[0]["RoleName"].string{
                    if roleName == "broker"{
                        userDb.setValue(true, forKey: "isBroker")
                        self.isBroker = true
                        return
                    }
                }
            }
        }
        self.isBroker = false
       
    }
    
    /**
    判断是否自动登陆
    */
    func autoLogin(){
        var db = NSUserDefaults()
        var userId = db.objectForKey("userId") as? Int
        if userId == nil{
            return
        }
        var isRemember = db.objectForKey("isRemeber") as? Bool
        if (isRemember != nil && isRemember == true){
            var phone = db.objectForKey("phone") as? String
            var passwd = db.objectForKey("userPassword") as? String
            loginWithPhone(phone ?? "", password: passwd ?? "")
        }
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
            .request({(json:JSON) in
                if let status = json["Status"].bool{
                    if status{
                        self.store(json,phone: phone, password: passwd, isRemember: true)
                        
                    }
                }
                }, faild: {error in
                    println("登陆失败\(error!.description)")
                    //TipTools().showToast("登陆失败", message: "\(error?.description)", duration: 2)
            })
    }
    
    /**
    退出当前账号，清空用户名密码
    */
    func loginOut(){
        id = nil
        userName = nil
        phone = nil
        password = nil
        isRemember = false
        isBroker = false
        isLogin = false
        var userDB = NSUserDefaults()
       
        userDB.removePersistentDomainForName(NSBundle.mainBundle().bundleIdentifier!)
        
    }

}
