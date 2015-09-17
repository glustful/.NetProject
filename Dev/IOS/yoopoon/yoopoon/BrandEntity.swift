//
//  BrandEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/16.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
class BrandEntity{
    var id:Int = 0
    var Bimg: String?
    var Bname: String?
    var subTitle: String?
    var productParamater: [String:String]?
    var content: String?
    var adTitle: String?
    private var _Bimg: String?{
        didSet{
            if _Bimg != nil{
            self.Bimg = imgHost + _Bimg!
            }
        }
    }
    init(){
        productParamater = [String:String]()
    }
    
    /**
    解析json封装成对象
    
    - parameter json: <#json description#>
    
    - returns: <#return value description#>
    */
    func generateSelf(json:JSON)->Self{
        self.id = json["Id"].number!.integerValue
        self._Bimg = json["Bimg"].string
        self.Bname = json["Bname"].string
        self.subTitle = json["SubTitle"].string
        self.content = json["Content"].string
        self.adTitle = json["AdTitle"].string
        var product = json["ProductParamater"]
       
        if product.count == 0{
            product = json["Parameters"]
        }
        
        self.productParamater!.updateValue(JSONTools.optStringFromOption(product["来电咨询"].string), forKey: "来电咨询")
        self.productParamater!.updateValue(JSONTools.optStringFromOption(product["最高优惠"].string), forKey: "最高优惠")
        self.productParamater!.updateValue(JSONTools.optStringFromOption(product["占地面积"].string), forKey: "占地面积")
        self.productParamater!.updateValue(JSONTools.optStringFromOption(product["图片banner"].string), forKey: "图片banner")
        self.productParamater!.updateValue(JSONTools.optStringFromOption(product["所属城市"].string), forKey: "所属城市")
        return self
    }
}