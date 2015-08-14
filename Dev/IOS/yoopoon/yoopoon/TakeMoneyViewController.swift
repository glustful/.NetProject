//
//  TakeMoneyViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TakeMoneyViewController: SuperViewController,UIAlertViewDelegate {
    private var currentClickItem: JSON?
    @IBOutlet weak var uiMoney: UILabel!
    @IBOutlet weak var uiTakeMoney: UIButton!
    @IBOutlet weak var uiMiddleContent: UIView!
    @IBOutlet weak var constraintTopWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleHeight: NSLayoutConstraint!
    @IBAction func takeMoneyAction(sender: UIButton) {
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        self.constraintTopWidth.constant = screenBounds.width
        self.uiTakeMoney.layer.cornerRadius = 5
        self.navigationItem.title = "我的钱包"
        
    }
    
    override func viewWillAppear(animated: Bool) {
        super.viewWillAppear(true)
        requestMyBankCard()
    }
    
    /**
    请求银行卡列表
    */
    private func requestMyBankCard(){
        RequestAdapter()
            .setUrl(urlBankCardSearchAllBankByUser)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                if let money = json["AmountMoney"].double{
                    self.uiMoney.text = "\(money)"
                }
                if let list = json["List"].array{
                    CommentTools.removeAllViews(self.uiMiddleContent)
                    
                    self.setContraint( CGFloat(list.count * 125))
                    for i in 0..<list.count{
                        self.addSubView(list[i],index: i)
                    }
                }
                }, faild: {error in
                    
            })
    }
    
    private func setContraint(height: CGFloat){
        self.constraintMiddleHeight.constant = height
        self.uiMiddleContent.frame.size.height = height
    }
    
    /**
    添加银行卡布局
    
    :param: json  <#json description#>
    :param: index <#index description#>
    */
    private func addSubView(json: JSON, index: Int){
        
        var view = NSBundle.mainBundle().loadNibNamed("BankCardView", owner: nil, options: nil).last as? BankCardView
        
        view!.frame = CGRectMake(0, CGFloat((125) * index), self.uiMiddleContent.frame.size.width, 120)
        
        view!.layer.cornerRadius = 5
        view!.initData(json)
        view!.deleteCallBack = deleteCard
        
        self.uiMiddleContent.addSubview(view!)
    }
    
    /**
    删除指定银行卡
    
    :param: json <#json description#>
    */
    private func deleteCard(json: JSON){
        self.currentClickItem = json
        var alert = UIAlertView(title: "提示", message: "确认要删除该银行卡吗？", delegate: self, cancelButtonTitle: "取消", otherButtonTitles: "确定")
        
        alert.show()
        
    }
    
    /**
    二次确认删除
    */
    private func deleteSecondCard(){
        if self.currentClickItem == nil{
            return
        }
        var id = self.currentClickItem!["Id"].int ?? 0
        
        RequestAdapter()
            .deleteBankCardById(id, callBack: {(result,data) in
                
                if !result{
                    TipTools().showToast("提示", message: "删除失败，请重试", duration: 2)
                    return
                }
                if let tmp = data{
                    var json = JSON(data:tmp)
                    if let status = json["Status"].bool{
                        if status{
                            TipTools().showToast("提示", message: json["Msg"].stringValue, duration: 2)
                            self.requestMyBankCard()
                            return
                        }else{
                            TipTools().showToast("提示", message: json["Msg"].stringValue, duration: 2)
                            return
                        }
                    }
                }
                TipTools().showToast("提示", message: "删除失败，请重试", duration: 2)
                
            })
        
    }
    
    func alertView(alertView: UIAlertView, clickedButtonAtIndex buttonIndex: Int){
        if buttonIndex == 1{
            deleteSecondCard()
        }
    }
    
}
