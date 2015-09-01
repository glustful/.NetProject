//
//  MeBottomView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/24.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class MeMiddleView: UIView {
    
    private var isBroker: Bool?
    @IBOutlet weak var ui1: UIVerticalButton!
    @IBOutlet weak var ui2: UIVerticalButton!
    @IBOutlet weak var ui3: UIVerticalButton!
    @IBOutlet weak var ui4: UIVerticalButton!
   // @IBOutlet weak var ui5: UIVerticalButton!
    @IBOutlet weak var uiImg1: UIImageView!
    @IBOutlet weak var uiLabel1: UILabel!
    @IBOutlet weak var uiImg2: UIImageView!
    @IBOutlet weak var uiLabel2: UILabel!
    @IBOutlet weak var uiImg3: UIImageView!
    @IBOutlet weak var uiLabel3: UILabel!
    @IBOutlet weak var uiImg4: UIImageView!
    @IBOutlet weak var uiLabel4: UILabel!
   // @IBOutlet weak var uiImg5: UIImageView!
    //@IBOutlet weak var uiLabel5: UILabel!
    
    @IBAction func settingAction(sender: UIVerticalButton) {
        var storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        if let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.settingIdentifier) as? SettingViewController{
            if let nav = CommentTools.getCurrentController()?.navigationController{
                nav.pushViewController(controller, animated: true)
            }
        }
    }
    
    @IBAction func buttonAction(sender: UIButton) {
        
        if let flag = self.isBroker{
            if !flag {
                if sender.tag == 0{
                    var storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
                    var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.personSettingIdentifier) as! UIViewController
                    CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
                }
                return
            }
            var storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
          
            switch sender.tag{
            case 0:
                if let tabbar = CommentTools.getCurrentController()?.navigationController?.viewControllers.first as? UITabBarController{
                    User.share.fromType = HouseFromType.leadOrRec
                tabbar.selectedIndex = 1
                }
                return
            case 1:
                
                var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.addAgentInentifier) as! AddAgentViewController
                controller.type = 0
                CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
            case 2:
                var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.addAgentInentifier) as! AddAgentViewController
                controller.type = 1
                CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
            case 3:
                
                var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.customListIdentifier) as! UIViewController
                CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
//            case 4:
//                identify = ControllerIdentifier.personSettingIdentifier
            default:
                break
            }
            
        }
        
    }
    func settingIcon(isBroker: Bool){
        self.isBroker = isBroker
        uiLabel1.textColor = UIColor.redColor()
        if isBroker{
            uiLabel2.textColor = UIColor.blackColor()
            uiImg1.image = UIImage(named: "b1")
            uiImg2.image = UIImage(named: "b2")
            uiImg3.image = UIImage(named: "b3")
            uiImg4.image = UIImage(named: "b4")
            //uiImg5.image = UIImage(named: "b5")
            
            uiLabel1.text = brokerTip1
            uiLabel2.text = brokerTip2
            uiLabel3.text = brokerTip3
            uiLabel4.text = brokerTip4
            //uiLabel5.text = brokerTip5
            
        }
        else{
            uiLabel2.textColor = UIColor.redColor()
            uiImg1.image = UIImage(named: "c1")
            uiImg2.image = UIImage(named: "c2")
            uiImg3.image = UIImage(named: "c3")
            uiImg4.image = UIImage(named: "c4")
            //uiImg5.image = UIImage(named: "c5")
            
            uiLabel1.text = commentUserTip1
            uiLabel2.text = commentUserTip2
            uiLabel3.text = commentUserTip3
            uiLabel4.text = commentUserTip4
           // uiLabel5.text = commentUserTip5
        }
    }
    
}
