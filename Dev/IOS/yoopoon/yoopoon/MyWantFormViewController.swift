//
//  MyWantFormViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class MyWantFormViewController: TextFieldViewController,UIPopoverPresentationControllerDelegate,DateChangeListener {
    var houseName:String?
    var houseType: String?
    var projectId: String?
    var projectName: String?
    var type:Int?
    private var postEntity = [String:String]()
    @IBOutlet weak var constraintScrollerWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintDataHeight: NSLayoutConstraint!
    @IBOutlet weak var uiDateView: UIView!
    @IBOutlet weak var uiCustomName: UITextField!
    @IBOutlet weak var uiCustomPhone: UITextField!
    @IBOutlet weak var uiPreDate: UITextField!
    @IBOutlet weak var uiDescript: UITextField!
    @IBOutlet weak var uiCommit: UIButton!
    @IBOutlet weak var uiHouseName: UITextField!
    @IBOutlet weak var uiHouseType: UITextField!
    @IBAction func commitAction(sender: UIButton) {
        if let field = self.currentField{
            field.resignFirstResponder()
        }
        if uiCustomName.text == nil || uiCustomName.text == ""{
            uiCustomName.shake(5, delta: 3)
            return
        }
        if !RegexHelper(clientNameRegx).match(uiCustomName.text!){
            TipTools().showToast("提示", message: "客户姓名必须是汉字组成", duration: 2)
            return
        }
        if uiCustomPhone.text == nil || uiCustomPhone.text == ""{
            uiCustomPhone.shake(5, delta: 3)
            return
        }
        if !RegexHelper(phoneRegex).match(uiCustomPhone.text!){
            TipTools().showToast("提示", message: "客户电话格式不对", duration: 2)
            return
        }
        
        if type == 0{
            if uiPreDate.text == nil || uiPreDate.text == ""{
                uiPreDate.shake(5, delta: 3)
                return
            }
        self.postEntity.updateValue(uiPreDate.text!, forKey: "Appointmenttime")
        }
        self.postEntity.updateValue(uiCustomName.text ?? "", forKey: "Clientname")
        self.postEntity.updateValue(uiCustomPhone.text ?? "", forKey: "Phone")
        self.postEntity.updateValue(uiDescript.text ?? "", forKey: "Note")
        post()
        
    }
    
    /**
    提交请示
    */
    private func post(){
        let url = type == 0 ? urlBrokerLeadClientAdd : urlBrokerRECClientAdd
        RequestAdapter()
        .setUrl(url)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .setParameters(self.postEntity)
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            
            if let status = json["Status"].bool{
                if status{
                    self.navigationController!.popViewControllerAnimated(true)
                    return
                }
            }
            TipTools().showToast("提示", message: "请求失败,请重试", duration: 3)
            }, faild: {error in
               
                TipTools().showToast("提示", message: "请求失败,请重试", duration: 3)
        })
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        constraintScrollerWidth.constant = screenBounds.width
        uiCommit.layer.cornerRadius = 10
        uiHouseName.text = houseName
        uiHouseType.text = houseType
        if type == 0{
            self.navigationItem.title = "带客"
            initLeadClient()
        }else{
           self.navigationItem.title = "推荐"
            initRECClient()
            CommentTools.removeAllViews(uiDateView)
            constraintDataHeight.constant = 0
        }
       
    }
    
    func adaptivePresentationStyleForPresentationController(controller: UIPresentationController) -> UIModalPresentationStyle {
        return UIModalPresentationStyle.None
    }
    
    func dateChange(date: NSDate) {
        let format = NSDateFormatter()
        format.dateFormat = "yyyy/MM/dd"
        let result = format.stringFromDate(date)
        self.uiPreDate.text = result
    }

    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        if let identifier = segue.identifier{
            if identifier == PopoverSegueIdentifier.chooseDateIdentifier{
                if let controller = segue.destinationViewController as? ChooseDateViewController{
                    if let hpp = controller.presentationController{
                        hpp.delegate = self
                    }
                    controller.delegate = self
                }
            }
        }
    }
    
    private func initLeadClient(){
        let userId = User.share.id ?? 0
        print("id=\(User.share.id)")
        self.postEntity.updateValue("\(userId)", forKey: "AddUser")
        self.postEntity.updateValue("\(userId)", forKey: "Broker")
        self.postEntity.updateValue(self.projectName ?? "", forKey: "Projectname")
        self.postEntity.updateValue(self.projectId ?? "", forKey: "Projectid")
        self.postEntity.updateValue("\(userId)", forKey: "ClientInfo")
        self.postEntity.updateValue("", forKey: "Brokername")
        self.postEntity.updateValue("", forKey: "Appointmenttime")
        self.postEntity.updateValue(self.houseName ?? "", forKey: "Houses")
        self.postEntity.updateValue(self.houseType ?? "", forKey: "HouseType")
        self.postEntity.updateValue("", forKey: "Clientname")
        self.postEntity.updateValue("", forKey: "Phone")
        self.postEntity.updateValue("", forKey: "Note")
        self.postEntity.updateValue("0", forKey: "Stats")
    }
    

    private func initRECClient(){
        let userId = User.share.id ?? 0
        self.postEntity.updateValue("\(userId)", forKey: "AddUser")
        self.postEntity.updateValue("\(userId)", forKey: "Broker")
        self.postEntity.updateValue(self.projectName ?? "", forKey: "Projectname")
        self.postEntity.updateValue(self.projectId ?? "", forKey: "Projectid")
        self.postEntity.updateValue("\(userId)", forKey: "ClientInfo")
        self.postEntity.updateValue("", forKey: "Brokername")
        self.postEntity.updateValue("", forKey: "Brokerlevel")
        self.postEntity.updateValue(self.houseName ?? "", forKey: "Houses")
        self.postEntity.updateValue(self.houseType ?? "", forKey: "HouseType")
        self.postEntity.updateValue("", forKey: "Clientname")
        self.postEntity.updateValue("", forKey: "Phone")
        self.postEntity.updateValue("", forKey: "Note")
        self.postEntity.updateValue("", forKey: "Qq")
        self.postEntity.updateValue("", forKey: "Type")
    }



}
