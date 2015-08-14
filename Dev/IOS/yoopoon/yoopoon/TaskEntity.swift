//
//  TaskEntity.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TaskEntity: NSObject {
   //"Taskname":"新手任务测试","Name":"新手任务测试","awardname":"积分奖励","awardvalue":"100","Endtime":"\/Date(1451491200000)\/","Adduser":4,"Id":4
    var taskName: String?
    var name: String?
    var awardName: String?
    var awardValue: String?
    var addUser: Int?
    var id: Int?
    
    func generateSelf(json: JSON){
        if let Taskname = json["Taskname"].string{
            self.taskName = Taskname
        }
        if let Name = json["Name"].string{
            self.name = Name
        }
        if let aName = json["awardname"].string{
            self.awardName = aName
        }
        if let aValue = json["awardvalue"].string{
            self.awardValue = aValue
        }
        if let addUser = json["Adduser"].int{
            self.addUser = addUser
        }
        if let Id = json["Id"].int{
            self.id = Id
        }
    }
}
