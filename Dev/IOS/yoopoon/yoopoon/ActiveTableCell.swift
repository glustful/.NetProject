//
//  ActiveTableCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/16.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
class ActiveTableCell: UITableViewCell{
    private var phone: String?
    @IBOutlet weak var mImage: UIImageView!
    @IBOutlet weak var mAdContent: UILabel!
    
    @IBOutlet weak var uiText: UILabel!
    @IBOutlet weak var mTitle: UILabel!
//    @IBOutlet weak var mArea: UILabel!
//    @IBOutlet weak var mAddress: UILabel!
//    @IBOutlet weak var mCity: UILabel!
    
    @IBOutlet weak var mPrefencetia: UIButton!
    @IBOutlet weak var mPhone: UIButton!
    
    @IBAction func dialPhoneAction(sender: UIButton) {
        
        if let phone = self.phone{
            if phone != ""{
            CommentTools.dialPhonNumber(phone)
                return
            }
        }
            TipTools().showToast("提示", message: "电话号码未知", duration: 1)
        
        
    }
    /**
    使用model填充view
    
    :param: brand 产品实体类
    //        cell!.textTitle = brandList[indexPath.row].Bname
    //        var phone = brandList[indexPath.row].productParamater?["来电咨询"]
    //        cell!.textPhone = "来电咨询：\(phone!)"
    //        var preferential = brandList[indexPath.row].productParamater?["最高优惠"]
    //        cell!.textPreferential = "最高优惠：\(preferential!)"
    //        cell!.imageUrl = brandList[indexPath.row].Bimg
    :returns: <#return value description#>
    */
    func initLayout(brand:BrandEntity){
      
        mImage.loadImageFromURLString(brand.Bimg!, placeholderImage: UIImage(named: placeHoder))
        mTitle.backgroundColor = appBackground
        if let title = brand.subTitle{
        mTitle.text = "  \(title)"
        }
        
        
        //uilabel加载html
        if let adTitle = brand.adTitle{
           
        var data = adTitle.dataUsingEncoding(NSUnicodeStringEncoding, allowLossyConversion: true)
            var html = NSAttributedString(data: data!, options: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType,NSFontAttributeName:UIColor.whiteColor()], documentAttributes: nil, error: nil)
       mAdContent.textColor = UIColor.whiteColor()
        mAdContent.attributedText = html
        }
        var text = " ["
        if let city = brand.productParamater?["所属城市"]{
            text += city
            //mCity.text = "[\(city)]"
        }
        text += "] "
        if let name = brand.Bname{
            text += name
            //mAddress.text = brand.Bname
        }
        text += " "
        if let area = brand.productParamater?["占地面积"]{
            text += area
           // mArea.text = area
        }
        uiText.text = text
        
        if let phone = brand.productParamater?["来电咨询"]{
        self.phone = phone
        mPhone.setTitle("来电咨询：\(phone)", forState: UIControlState.Normal)
        }
        let prefencetia = brand.productParamater?["最高优惠"]
        mPrefencetia.setTitle("最高优惠：\(prefencetia!)", forState: UIControlState.Normal)
       
    }
}