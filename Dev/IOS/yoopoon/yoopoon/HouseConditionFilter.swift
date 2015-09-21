//
//  HouseConditionFilter.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/30.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class HouseConditionFilter: NSObject {
    var city = [HouseConditionDataSource]()
    var areaName = ""
    var page = 1
    var pageCount = 10
    var priceBegin = ""
    var priceEnd = ""
    var typeId = ""
    class var share: HouseConditionFilter{
        get{
            return Inner.instance
        }
    }
    
    private struct Inner{
        static let instance = HouseConditionFilter()
    }
    
    func reset(){
        areaName = ""
        page = 1
        priceBegin = ""
        priceEnd = ""
        typeId = ""
    }
    
    /**
    初始化列表
    
    - parameter json: <#json description#>
    
    - returns: <#return value description#>
    */
    func initData(json: JSON)->[HouseConditionDataSource]{
        var city = [HouseConditionDataSource]()
        var typeList = [[String:String]]()
        if let type = json["TypeList"].array{
            for i in 0..<type.count{
                var dic = [String:String]()
                dic.updateValue(type[i]["TypeName"].string ?? "", forKey: "name")
                let id = type[i]["TypeId"].int ?? -1
                dic.updateValue("\(id)", forKey: "id")
                typeList.append(dic)
            }
        }
        city.removeAll(keepCapacity: false)
        if let area = json["AreaList"].array{
            for i in 0..<area.count{
                let entity = HouseConditionDataSource()
                entity.id = area[i]["Id"].int
                entity.name = area[i]["AreaName"].string
                entity.typeList = typeList
                city.append(entity)
            }
        }
        return city
    }
}

enum HouseConditionFilterType {
    case CITY
    case TYPE
    case PRICE
}

struct HouseConditionSegueIdentifier{
    static let cityIdentifier = "houseCityPopover"
    static let typeIdentifier = "houseTypePopover"
    static let priceIdentifier = "housePricePopover"
}

class HouseConditionDataSource {
    var id:Int?
    var name: String?
    var children: [HouseConditionDataSource]?
    var typeList: [[String:String]]?
}
