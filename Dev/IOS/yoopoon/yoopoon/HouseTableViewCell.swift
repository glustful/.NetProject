//
//  HouseTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/18.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class HouseTableViewCell: UITableViewCell {
    private var entity: ProductEntity?
    @IBOutlet weak var mFooterView: UIView!
    @IBOutlet weak var mImageView: UIImageView!
    @IBOutlet weak var mTitle: UILabel!
    @IBOutlet weak var mPrice: UILabel!
    @IBOutlet weak var mDescript: UILabel!
    @IBOutlet weak var mAdTitle: UILabel!
    @IBOutlet weak var uiLeadClientButton: UIButton!
    @IBOutlet weak var uiRecommendButton: UIButton!
    @IBOutlet weak var uiRedButton: UIButton!
    @IBAction func bringCustomAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.myWantFormIdentifier) as! MyWantFormViewController
        
        controller.houseName = entity?.productName
        controller.houseType = entity?.type
        var id = entity?.id ?? 0
        controller.projectId = "\(id)"
        controller.projectName = entity?.productName
        controller.type = 0 //带客
        CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)
    }
    
    @IBAction func recommendAction(sender: UIButton) {
        var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.myWantFormIdentifier) as! MyWantFormViewController
        controller.houseName = entity?.productName
        controller.houseType = entity?.type
        var id = entity?.id ?? 0
        controller.projectId = "\(id)"
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
    使用实体类初始化布局
    
    :returns: <#return value description#>
    */
    func initLayout(product: ProductEntity){
        initButton()
        self.entity = product
        mImageView.loadImageFromURLString(product.productImg ?? "", placeholderImage: UIImage(named: placeHoder))
        mTitle.text = product.productName
        mPrice.text = "\(product.price ?? 0)" + unitMeter
        mAdTitle.text = product.advertisement
        mDescript.text = "\(product.type!)/\(product.acreage!)m²/在售\(product.stockRule!)套"
    }
    
    /**
    初始化底部条
    
    :returns: <#return value description#>
    */
    private func initButton(){
        var redColor = UIColor(red: 1, green: 48/255, blue: 0, alpha: 1)
        var grayColor = UIColor(red: 102/255, green: 102/255, blue: 102/255, alpha: 1)
        if User.share.fromType == HouseFromType.leadOrRec{
            uiRecommendButton.layer.cornerRadius = 5
            uiLeadClientButton.layer.cornerRadius = 5
            uiRecommendButton.backgroundColor = appRedBackground
            uiLeadClientButton.backgroundColor = appRedBackground
            uiRecommendButton.setTitleColor(UIColor.whiteColor(), forState: UIControlState.Normal)
            uiLeadClientButton.setTitleColor(UIColor.whiteColor(), forState: UIControlState.Normal)
            uiRedButton.setTitleColor(redColor, forState: UIControlState.Normal)
        }else{
            uiRecommendButton.layer.cornerRadius = 0
            uiLeadClientButton.layer.cornerRadius = 0
            uiRecommendButton.backgroundColor = UIColor.whiteColor()
            uiLeadClientButton.backgroundColor = UIColor.whiteColor()
            uiRecommendButton.setTitleColor(redColor, forState: UIControlState.Normal)
            uiLeadClientButton.setTitleColor(redColor, forState: UIControlState.Normal)
            uiRedButton.setTitleColor(grayColor, forState: UIControlState.Normal)
        }
    }
    
}
