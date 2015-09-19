//
//  MeTopView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/23.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class MeTopView: UIView {

    @IBOutlet weak var uiBrokerView: UIView!
    @IBOutlet weak var uiUserName: UILabel!
    @IBOutlet weak var uiLevel: UILabel!
    @IBOutlet weak var uiOrder: UILabel!
    @IBOutlet weak var uiPartner: UIButton!
    @IBOutlet weak var uiReference: UIButton!
    @IBOutlet weak var uiClient: UIButton!
    @IBOutlet weak var uiMoney: UILabel!
    @IBOutlet weak var uiTakeMoney: UIButton!
    @IBOutlet weak var uiHeadPhoto: UIImageView!
    
    @IBOutlet weak var uiCommentUserView: UIView!
    @IBOutlet weak var uiCommentUserName: UILabel!
    
    @IBAction func headPhotoAction(sender: UIButton) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.personSettingIdentifier) as! PersonSettingViewController
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
    }
    @IBAction func partnetAction(sender: AnyObject) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.addAgentInentifier) as! AddAgentViewController
        controller.type = 0
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
        
       
    }
    @IBAction func referAction(sender: UIButton) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.addAgentInentifier) as! AddAgentViewController
        controller.type = 1
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
    }
    @IBAction func coutomAction(sender: UIButton) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
               let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.customListIdentifier) 
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
    }
   
    @IBAction func takeMoneyAction(sender: UIButton) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.takeMoneyIdentifier) 
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
    }
    
    override func willMoveToSuperview(newSuperview: UIView?) {
        uiTakeMoney.layer.cornerRadius = 8
        uiHeadPhoto.layer.cornerRadius = uiHeadPhoto.frame.size.width/2
       
    }
    
    func initLayout(json: JSON,isBroker: Bool = false){
        self.uiCommentUserView.hidden = isBroker
        self.uiBrokerView.hidden = !isBroker
        
        if !isBroker{
            if let name = json["Nickname"].string{
            self.uiCommentUserName.text = name
            }
            if let photo = json["Headphoto"].string{
                
                    uiHeadPhoto.load(imgHost + photo, placeholder: UIImage(named: "default_head_img"))
            }else{
                uiHeadPhoto.image = UIImage(named: "default_head_img")
            }
            return
        }
        
        if let name = json["Name"].string{
            uiUserName.text = name
        }
        if let level = json["levelStr"].string{
            uiLevel.text = level
        }
        if let order = json["orderStr"].string{
            uiOrder.text = "总排名：\(order)"
        }
        if let partner = json["partnerCount"].int{
            uiPartner.setTitle("合伙人：\(partner)", forState: UIControlState.Normal)
        }
        if let refer = json["refereeCount"].int{
            uiReference.setTitle("/ 推荐：\(refer)", forState: UIControlState.Normal)
        }
        if let photo = json["photo"].string{
            uiHeadPhoto.load(imgHost + photo, placeholder: UIImage(named: "default_head_img"))
        }
        else{
            uiHeadPhoto.image = UIImage(named: "default_head_img")
        }
    }
    
    func initClientInfo(json: JSON){
        if let count = json["totalCount"].int{
           uiClient.setTitle("/ 客户：\(count)", forState: UIControlState.Normal)
        }
       
        
    }

}
