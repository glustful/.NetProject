//
//  AddBankCardViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AddBankCardViewController: TextFieldViewController,UIPopoverPresentationControllerDelegate,BankProtocol {
    private var mBankEntity: BankEntity?
    private var timeCount = 60
   
    private var postEntity = ["Num":"","Type":"储蓄卡","Bank":"","Address":"","MobileYzm":"","Hidm":""]
    @IBOutlet weak var constraintViewWidth: NSLayoutConstraint!
    @IBOutlet weak var uiCardType: UISegmentedControl!
    @IBOutlet weak var uiCardNo: UITextField!
    @IBOutlet weak var uiBankName: UITextField!
    @IBOutlet weak var uiAddress: UITextView!
    @IBOutlet weak var uiValide: UITextField!
    @IBOutlet weak var uiSureButton: UIButton!
    @IBOutlet weak var uiValideButton: UIButton!
    @IBAction func sendSMSAction(sender: UIButton) {
        sendSMS()
    }
    @IBAction func sureAction(sender: UIButton){
        hidenKeyBoard()
        if validate(){
            post()
        }
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        constraintViewWidth.constant = screenBounds.width
        self.navigationItem.title = "添加银行卡"
        uiAddress.layer.borderWidth = 1
        uiAddress.layer.borderColor = UIColor.lightGrayColor().CGColor
        uiAddress.layer.cornerRadius = 5
        uiValideButton.layer.cornerRadius = 5
        uiSureButton.layer.cornerRadius = 10
    }

    override func hidenKeyBoard() {
        super.hidenKeyBoard()
        uiAddress.resignFirstResponder()
    }
    
    func adaptivePresentationStyleForPresentationController(controller: UIPresentationController) -> UIModalPresentationStyle {
        return UIModalPresentationStyle.None
    }
    
    //MARK: --选择银行的回调方法
    func callBack(entity:BankEntity){
        self.mBankEntity = entity
        self.uiBankName.text = entity.name ?? ""
    }
    
    //MARK: --验证参数信息与提交
    private func validate()->Bool{
        if self.uiCardNo.text == nil || uiCardNo.text == ""{
            uiCardNo.shake(5, delta: 5)
            return false
        }
        
            if ( count(self.uiCardNo.text) < 16 ||  count(self.uiCardNo.text) > 19) {
                TipTools().showToast("提示", message: "银行卡号长度必须在16到19之间", duration: 2)
                
                return false
            }
        
            //开头6位
            var strBin = [10,18,30,35,37,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,58,60,62,65,68,69,84,87,88,94,95,98,99]
        var flag = false
        for key in strBin{
            if self.uiCardNo.text.hasPrefix(String(key)){
                flag = true
                break

            }
        }
        if !flag{
            TipTools().showToast("提示", message: "银行卡号开头6位不符合规范", duration: 2)
            
            return false
        }
        if self.uiBankName.text == nil || uiBankName.text == ""{
            uiBankName.shake(5, delta: 5)
            return false
        }
        if self.uiAddress.text == nil || uiAddress.text == ""{
            TipTools().showToast("提示", message: "请输入开户银行地址", duration: 2)
            return false
        }
        if self.uiValide.text == nil || uiValide.text == ""{
            uiValide.shake(5, delta: 5)
            return false
        }
        postEntity.updateValue(uiCardNo.text, forKey: "Num")
        postEntity.updateValue("\(self.mBankEntity?.id ?? 0)", forKey: "Bank")
        postEntity.updateValue(uiAddress.text, forKey: "Address")
        postEntity.updateValue(uiValide.text, forKey: "MobileYzm")
        if uiCardType.selectedSegmentIndex == 0{
            postEntity.updateValue("储蓄卡", forKey: "Type")
        }else{
            postEntity.updateValue("信用卡", forKey: "Type")
        }
        

        return true
    }
    
    private func post(){
        
        RequestAdapter()
        .setUrl(urlBankCardAddBankCard)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .setParameters(self.postEntity)
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            println(json)
            if let status = json["Status"].bool{
                if status{
                    self.navigationController!.popViewControllerAnimated(true)
                    return
                }
            }
            TipTools().showToast("提示", message: "添加银行卡失败，请重试", duration: 3)
            }, faild: {error in
                TipTools().showToast("提示", message: "添加银行卡失败，请重试", duration: 3)
        })
    }
    
    /**
    发送验证码
    */
    private func sendSMS(){
        uiValideButton.enabled = false
        timeCount = 60
        NSTimer.scheduledTimerWithTimeInterval(1, target: self, selector: "changeTimeCount:", userInfo: nil, repeats: true)
        uiValideButton.backgroundColor = UIColor.grayColor()
        uiValideButton.setTitle("重新发送(\(timeCount))", forState: UIControlState.Normal)
        RequestAdapter()
            .setUrl(urlSMSSendForChangePassword)
            .setEncoding(.JSON)
            .setRequestMethod(.POST)
            .addParameter("SmsType", value: SMSType.addBankCardType)
            .request({json in
                if json["Message"].stringValue == "1"{
                    if let hidm = json["Desstr"].string{
                        self.postEntity.updateValue(hidm, forKey: "Hidm")
                    }
                }
                }, faild: {error in
            })
    }
    
    func changeTimeCount(time: NSTimer){
        if timeCount == 0{
            timeCount = 60
            uiValideButton.enabled = true
            uiValideButton.backgroundColor = appRedBackground
            uiValideButton.setTitle("获取验证码", forState: UIControlState.Normal)
            time.invalidate()
            return
        }
        timeCount--
        uiValideButton.setTitle("重新发送(\(timeCount))", forState: UIControlState.Normal)
    }

    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        if let identifier = segue.identifier{
            if identifier == PopoverSegueIdentifier.chooseBankIdentifier{
                if let cbv = segue.destinationViewController as? ChooseBankViewController{
                    if let hpp = cbv.popoverPresentationController{
                        hpp.delegate = self
                    }
                    cbv.delegate = self
                }
            }
        }
    }

}
