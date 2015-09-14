//
//  ProductEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/18.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ProductEntity: NSObject {
    var productImg: String?
    var id: Int?
    var subTitle: String?
    var type: String?
    var stockRule: Int?
    var advertisement: String?
    var price: Double?
    var productName: String?
    var acreage: String?
    var phone: String?
    private var _productImg: String?{
        didSet{
            self.productImg = imgHost + _productImg!
        }
    }
    
    /**
    解析json为自身对象
    
    :param: json <#json description#>
    
    :returns: <#return value description#>
    */
    func generateSelf(json: JSON)->Self{
        self._productImg = json["Productimg"].string
        self.id = json["Id"].number?.integerValue
        self.subTitle = json["SubTitle"].string
        self.type = json["Type"].string
        self.stockRule = json["StockRule"].number?.integerValue
        self.advertisement = json["Advertisement"].string
        self.price = json["Price"].double
        self.productName = json["Productname"].string
        self.acreage = json["Acreage"].string
        self.phone = JSONTools.optStringFromOption(json["Phone"].string)
        return self
    }
}
