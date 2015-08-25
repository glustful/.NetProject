//
//  PopoverTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/30.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class PopoverTableViewCell: UIView {
    var id:String?
    var name: String?
    var type: HouseConditionFilterType?
    var level: Int?
    @IBOutlet weak var uiButtonTitle: UIButton!
    var titleLabel: String?{
        didSet{
            //self.uiButtonTitle.setBackgroundImage(UIImage(named: placeHoder), forState: UIControlState.Selected)
            self.uiButtonTitle.setTitle(titleLabel, forState: UIControlState.Normal)
        }
    }
    
    var requestFunction: ((String,Int)->())?
    var funcDealWithResult: ((PopoverTableViewCell)->())?
    
    @IBAction func itemOnclick(sender: UIButton) {
       // sender.selected = true
        if let type = self.type{
            switch type{
            case .CITY:
                if level == 3{
                    if funcDealWithResult != nil{
                        funcDealWithResult!(self)
                    }
                    return
                }
                if requestFunction != nil && id != nil{
                    requestFunction!(id!,level ?? 0)
                }
            case .TYPE:
                if funcDealWithResult != nil{
                    funcDealWithResult!(self)
                }
            case .PRICE:
                if funcDealWithResult != nil{
                    funcDealWithResult!(self)
                }
            default:
                break
            }
        }
    }
    
    
    
}
