//
//  TaskListTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TaskListTableViewCell: UITableViewCell {

    var taskName: String?{
        didSet{
            self.uiTaskName.text = taskName
        }
    }
    var awardMsg: String?{
        didSet{
            self.uiawardMsg.text = awardMsg
        }
    }
    
    @IBOutlet weak var uiTaskName: UILabel!
    @IBOutlet weak var uiawardMsg: UILabel!
//    @IBOutlet weak var uiGoFinish: UIButton!{
//        didSet{
//            uiGoFinish.layer.cornerRadius = 5
//            uiGoFinish.layer.borderWidth = 1
//            uiGoFinish.layer.borderColor = appRedBackground.CGColor
//        }
//    }
    
  
}
