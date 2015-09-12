//
//  CustomProgressEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class CustomProgressEntity: NSObject {
   
    var clientName:String?
    var houseType:String?
    var houses:String?
    var status:String?
    var phone:String?
    var id:String?
    var strType:String?
    
    func generateSelf(json:JSON){
       
        clientName = json["Clientname"].stringValue
        houseType = json["Housetype"].stringValue
        houses = json["Houses"].stringValue
        status = json["Status"].stringValue
        phone = json["Phone"].stringValue
        id = json["Id"].stringValue
        strType = json["StrType"].stringValue
       
    }
}
