//
//  BrandAgentEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class BrandAgentEntity: NSObject {
    var imgUrl: String?
    var adContent: String?
    var brandName: String?
    var commition: String?
    var houseType: String?
    var price: String?
    var brandId: String?
    var productId: String?
    var productName: String?
    
    /**
    初始化数据模型
    
    :param: json <#json description#>
    
    :returns: <#return value description#>
    */
    func initData(json: JSON){
        self.brandId = json["BrandId"].string
        self.brandName = json["BrandName"].string
        if let bimg = json["Bimg"].string{
           self.imgUrl = imgHost + bimg
        }
        
        self.productId = json["ProductId"].string
        self.productName = json["Productname"].string
        self.houseType = json["HouseType"].string
        self.price = json["Price"].string
        self.commition = json["Commition"].string
        self.adContent = json["SubTitle"].string
    }
}
