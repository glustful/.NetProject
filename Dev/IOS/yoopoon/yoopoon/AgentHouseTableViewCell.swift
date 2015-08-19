//
//  AgentHouseTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AgentHouseTableViewCell: UITableViewCell {
    private var entity: BrandAgentEntity?
    @IBOutlet weak var uiBrandImg: UIImageView!
    @IBOutlet weak var uiAdContent: UILabel!
//    @IBOutlet weak var uiAdTitle: UILabel!
//    @IBOutlet weak var uiHouseType: UILabel!
//    @IBOutlet weak var uiBrandPrice: UILabel!
    @IBOutlet weak var uiCommiton: UILabel!
    @IBOutlet weak var uiText: UILabel!
    
    @IBAction func bringCustomAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.myWantFormIdentifier) as! MyWantFormViewController
        println(entity?.brandName)
        controller.houseName = entity?.brandName
        controller.houseType = entity?.houseType
        controller.projectId = entity?.productId
        controller.projectName = entity?.productName
        controller.type = 0 //带客
        CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)
    }
    
    @IBAction func recommendAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.myWantFormIdentifier) as! MyWantFormViewController
        controller.houseName = entity?.brandName
        controller.houseType = entity?.houseType
        controller.projectId = entity?.productId
        controller.projectName = entity?.productName
        controller.type = 1 //推荐
        CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)
    }
    
    @IBAction func GrabRedEnvelopeAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.grabRedEnvelopeIdentifier) as! GrabRedEnvelopeViewController
               CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)
    }
    
    @IBAction func takeIntegrationAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.takeIntegrationIdentifier) as! TakeIntegrationViewController
        CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)
    }
    
    /**
    数据模型初始化布局
    
    :param: json <#json description#>
    
    :returns: <#return value description#>
    */
    func initLayout(brand: BrandAgentEntity){
        self.entity = brand
        if let img = brand.imgUrl{
            uiBrandImg.loadImageFromURLString(img, placeholderImage: UIImage(named: placeHoder))
        }
        if let adContent = brand.adContent{
            uiAdContent.text = adContent
        }
        var text = " [\(JSONTools.optStringFromOption(brand.brandName))]"
        
        text += " 户型：\(JSONTools.optStringFromOption(brand.houseType))"
        
        text += " 价格：\(JSONTools.optStringFromOption(brand.price))"
        uiText.text = text
        uiCommiton.text = "最高佣金：\(JSONTools.optStringFromOption(brand.commition))/套"
        
        
    }
    
}
