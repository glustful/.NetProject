//
//  PersonSettingViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class PersonSettingViewController: SuperViewController,UITextFieldDelegate,UINavigationControllerDelegate,UIImagePickerControllerDelegate {
    private var currentField: UITextField?
    private var postFormData: JSON?
    @IBOutlet weak var constraintHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintWidth: NSLayoutConstraint!
    
    @IBOutlet weak var uiFormView: UIView!
    @IBOutlet weak var uiScrollForm: UIScrollView!
    //MARK: -FORM TEXTFIELD
    @IBOutlet weak var uiAliase: UITextField!
    @IBOutlet weak var uiName: UITextField!
    @IBOutlet weak var uiSex: UISegmentedControl!
    @IBOutlet weak var uiIdentifyCard: UITextField!
    @IBOutlet weak var uiEmail: UITextField!
    @IBOutlet weak var uiWX: UITextField!
    @IBOutlet weak var uiValidate: UITextField!
    @IBOutlet weak var uiPhone: UILabel!
    @IBOutlet weak var uiHeadPhoto: UIImageView!
    @IBOutlet weak var uiStore: UIButton!
    
    @IBAction func pickHeadPhotoAction(sender: UIButton) {
        var picker = UIImagePickerController()
        picker.sourceType = UIImagePickerControllerSourceType.PhotoLibrary
        picker.allowsEditing = false
        picker.delegate = self
        self.presentViewController(picker, animated: true, completion: nil)
    }
    @IBAction func storeAction(sender: UIButton) {
        if validate(){
        post()
        }
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "个人信息设置"
        self.uiHeadPhoto.layer.cornerRadius = self.uiHeadPhoto.frame.width/2
        constraintWidth.constant = screenBounds.width
        constraintHeight.constant = 17 * 40 + 100 + 40 + 60
        var tap = UITapGestureRecognizer(target: self, action: "hidenKeyBoard")
        tap.numberOfTouchesRequired = 1
        self.view.addGestureRecognizer(tap)
        NSNotificationCenter.defaultCenter().addObserver(self, selector: "dealWithKeyboardHeight:", name: UIKeyboardWillShowNotification, object: nil)
        requestBrokerInfo()
    }
    
    /**
    获取经纪人信息
    */
    private func requestBrokerInfo(){
        RequestAdapter()
        .setUrl(urlBrokerInfoGetBrokerByUserId)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .setIsShowIndicator(true, currentView: self.view)
        .addParameter("userId", value: User.share.id ?? 0)
        .request({json in
            self.postFormData = json
            self.showBrokerInfo(json)
            }, faild: {error in
                TipTools().showToast("出错了", message: "\(error!.description)", duration: 1)
        })
    }
    
    /**
    初始化数据
    
    :param: json <#json description#>
    */
    private func showBrokerInfo(json: JSON){
        if let value = json["Nickname"].string{
            self.uiAliase.text = value
        }
        if let value = json["Realname"].string{
            self.uiName.text = value
        }
        if let value = json["Sfz"].string{
            self.uiIdentifyCard.text = value
        }
        if let value = json["Sexy"].string{
            if value == "先生"{
                self.uiSex.selectedSegmentIndex = 0
            }else{
                self.uiSex.selectedSegmentIndex = 1
            }
            
        }
        if let value = json["Phone"].string{
            self.uiPhone.text = value
        }
        if let value = json["WeiXinNumber"].string{
            self.uiWX.text = value
        }
        if let value = json["Email"].string{
            self.uiEmail.text = value
        }
        if let value = json["Headphoto"].string{
            self.uiHeadPhoto.load(imgHost + value, placeholder: UIImage(named: placeHoder))
        }
    }
    
    /**
    验证表单数据
    
    :returns: <#return value description#>
    */
    private func validate()->Bool{
        if !RegexHelper(brokerNickNameRegex).match(self.uiAliase.text){
            self.uiScrollForm.setContentOffset(CGPointZero, animated: true)
            TipTools().showToast("格式不符", message: "昵称只能是汉字字母下划线组成", duration: 2)
            return false
        }
        if self.uiName.text == nil || self.uiName.text == ""{
            self.uiScrollForm.setContentOffset(CGPointZero, animated: true)
            self.uiName.shake(5, delta: 5)
            return false
        }
        if !RegexHelper(brokerNameRegex).match(self.uiName.text){
            self.uiScrollForm.setContentOffset(CGPointZero, animated: true)
            TipTools().showToast("格式不符", message: "姓名只能是2-5个汉字组成", duration: 2)
            return false
        }
        if self.uiIdentifyCard.text == nil || self.uiIdentifyCard.text == ""{
            self.uiIdentifyCard.shake(5, delta: 5)
            return false
        }
        if !RegexHelper(identifyCardRegex).match(self.uiIdentifyCard.text){
            
            TipTools().showToast("格式不符", message: "身份证号码不对", duration: 2)
            return false
        }
        if self.uiEmail.text == nil || self.uiEmail.text == ""{
            self.uiEmail.shake(5, delta: 5)
            return false
        }
        if !RegexHelper(emailRegex).match(self.uiEmail.text){
            
            TipTools().showToast("格式不符", message: "邮箱格式不对", duration: 2)
            return false
        }
        return true
    }
    
    /**
    提交表单数据
    */
    private func post(){
        self.postFormData!["Nickname"].string = self.uiAliase.text
        self.postFormData!["Realname"].string = self.uiName.text
        self.postFormData!["Sfz"].string = self.uiIdentifyCard.text
        self.postFormData!["Sexy"].string = self.uiSex.selectedSegmentIndex == 0 ? "先生" : "女士"
        self.postFormData!["Phone"].string = self.uiPhone.text
    
        self.postFormData!["WeiXinNumber"].string = self.uiWX.text
        self.postFormData!["Email"].string = self.uiEmail.text
        //
        self.postFormData!["code"].string = self.uiValidate.text
        
        RequestAdapter()
        .setUrl(urlBrokerInfoUpdateBroker)
        .setEncoding(.JSON)
        .setRequestMethod(.POST)
        .setIsShowIndicator(true, currentView: self.view)
        .setParameters(CommentTools.jsonToDic(self.postFormData!))
        .request({json in
            if let status = json["Status"].bool{
                if status{
                    
                    self.navigationController!.popToViewController(self.navigationController!.viewControllers.first as! UIViewController, animated: true)
                    return
                }
            }
            TipTools().showToast("提示", message: "保存失败", duration: 2)
                }, faild: {error in
                    println("\(error!.description)")
                TipTools().showToast("提示", message: "保存失败", duration: 2)
            })
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
    
    /**
    获取软键盘的高度
    
    :param: notify <#notify description#>
    */
    func dealWithKeyboardHeight(notify: NSNotification){
        //获取键盘的高度
        var userInfo = notify.userInfo
        var value = userInfo![UIKeyboardFrameEndUserInfoKey] as? NSValue
        
        var keyboardRect = value!.CGRectValue()
        keyboardHeight = keyboardRect.size.height
       NSNotificationCenter.defaultCenter().removeObserver(self)
    }
    
    //MARK: -图片选择代理方法
    func imagePickerController(picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [NSObject : AnyObject]){
        //获得原始的图片
        if let image = info["UIImagePickerControllerOriginalImage"] as? UIImage{
            var progress = UIActivityIndicatorView(activityIndicatorStyle: UIActivityIndicatorViewStyle.WhiteLarge)
            progress.color = UIColor.greenColor()
            progress.center = self.uiHeadPhoto.center
            self.uiScrollForm.addSubview(progress)
            progress.startAnimating()
            RequestAdapter()
            .uploadHeadImg(image,callBack: {(success,result) in
                progress.stopAnimating()
                progress.removeFromSuperview()
                if let data = result{
                    var json = JSON(data:data)
                    if let status = json["Status"].bool{
                        if status{
                            self.uiHeadPhoto.image = image
                            self.postFormData!["Headphoto"] = json["Msg"]
                            return
                        }
                    }
                }
                })
        }else{
            TipTools().showToast("提示", message: "图片选择失败", duration: 2)
        }
        
        picker.dismissViewControllerAnimated(true, completion: nil)
    }
    
    func imagePickerControllerDidCancel(picker: UIImagePickerController){
        picker.dismissViewControllerAnimated(true, completion: nil)
    }
    
    //MARK: -UITEXTFIELD代理方法
    func textFieldDidBeginEditing(textField: UITextField) {
        self.currentField = textField
        var frame = textField.frame
        var offset = frame.origin.y + self.uiFormView.frame.origin.y + 40 - self.uiScrollForm.contentOffset.y - (self.view.frame.size.height - keyboardHeight)//键盘高度258
        var animationDuration: NSTimeInterval = 0.30
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
