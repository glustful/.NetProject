//
//  BankEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class BankEntity: NSObject {
    var id: Int?
    var name: String?
    
    func generateSelf(json: JSON){
        self.id = json["Id"].int
        self.name = json["Codeid"].string
        
    }
}
