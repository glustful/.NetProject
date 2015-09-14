//
//  PartnerTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class PartnerTableViewCell: UITableViewCell {
    private var type:Int = 0
    private var json:JSON?
    var refreshFunc: (()->())?
    @IBOutlet weak var uiHeadPhoto: UIImageView!
    @IBOutlet weak var uiName: UILabel!
    @IBOutlet weak var uiPhone: UILabel!
    @IBOutlet weak var uiView: UIView!
    @IBOutlet weak var uiAgree: UIButton!
    @IBOutlet weak var uiRefuse: UIButton!
    
    @IBAction func agreeAction(sender: UIButton) {
        if self.type != 1{
            return
        }
        refresh("1")
    }
    @IBAction func refuseAction(sender: UIButton) {
        if self.type != 1{
            return
        }
        refresh("-1")
    }
    override func layoutSubviews() {
        
        uiHeadPhoto.layer.cornerRadius = 29.5
    }
    
    
    /**
    初始化数据
    
    :param: json <#json description#>
    :param: type 0为合伙人，1为我接收到的邀请，2我的推荐
    
    :returns: <#return value description#>
    */
    func initData(json: JSON,type:Int){
        self.type = type
        self.json = json
        switch type{
        case 0:
            self.uiView.hidden = true
            if let name = json["Name"].string{
                uiName.text = name
            }
            if let name = json["Phone"].string{
                uiPhone.text = name
            }
            if let name = json["Headphoto"].string{
                uiHeadPhoto.load(imgHost + name, placeholder: UIImage(named: defaultHeadPhoto))
            }
            
        case 1:
            
            if let name = json["BrokerName"].string{
                uiName.text = name
            }
            
        case 2:
            
            if let name = json["Brokername"].string{
                uiName.text = name
            }
            if let name = json["Agentlevel"].string{
                uiAgree.setTitle("等级：\(name)", forState: UIControlState.Normal)
            }
            if let name = json["Phone"].string{
                uiRefuse.setTitle(name, forState: UIControlState.Normal)
            }
            
        default:
            break
        }
    }
    
    private func refresh(type:String){
        RequestAdapter()
        .setUrl(urlPartnerListSetPartner)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .addParameter("status", value: type)
        .addParameter("id", value: self.json!["Id"].stringValue)
        .setIsShowIndicator(true, currentView: CommentTools.getCurrentController()!.view)
        .request({json in
            if let status = json["Status"].bool{
                if status{
                    if self.refreshFunc != nil{
                        self.refreshFunc!()
                        return
                    }
                }
            }
            TipTools().showToast("提示", message: json["Msg"].stringValue, duration: 2)
        }, faild: { error in
            TipTools().showToast("提示", message: "请示出错，请重试", duration: 2)
        })
    }
}
