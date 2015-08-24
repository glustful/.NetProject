//
//  AgentHoreTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AgentHoreTableViewCell: UITableViewCell {
    @IBOutlet weak var uiBottomView: UIView!
    @IBOutlet weak var uiNo1: UIView!
    @IBOutlet weak var uiNo2: UIView!
    @IBOutlet weak var uiNo3: UIView!
    @IBOutlet weak var uiNo1Name: UILabel!
    @IBOutlet weak var uiNo1Price: UILabel!
    @IBOutlet weak var uiNo2Name: UILabel!
    @IBOutlet weak var uiNo2Price: UILabel!
    @IBOutlet weak var uiNo3Name: UILabel!
    @IBOutlet weak var uiNo3Price: UILabel!
    
    func removeChild(){
        
        if uiBottomView != nil{
        uiBottomView.removeFromSuperview()
        }
    }
    
    func initLayout(json: JSON){
        if let array = json["List"].array{
            
            if array.count == 1{
                uiNo2.hidden = true
                uiNo3.hidden = true
            }else if array.count == 2{
                uiNo3.hidden = true
            }
            
            for i in 0..<array.count{
                var obj = array[i]
                if i == 0{
                    self.uiNo1Name.text = obj["Brokername"].string
                    if let amount = obj["Amount"].double{
                        self.uiNo1Price.text = "总佣金：\(NumberFormatTools.clipEndZeros(amount))"
                    }
                }else if i == 1{
                    self.uiNo2Name.text = obj["Brokername"].string
                    if let amount = obj["Amount"].double{
                        self.uiNo2Price.text = "总佣金：\(NumberFormatTools.clipEndZeros(amount))"
                    }
                    
                }
                else if i == 2{
                    self.uiNo3Name.text = obj["Brokername"].string
                    if let amount = obj["Amount"].double{
                        self.uiNo3Price.text = "总佣金：\(NumberFormatTools.clipEndZeros(amount))"
                    }
                    
                }
            }
        }
    }
}
