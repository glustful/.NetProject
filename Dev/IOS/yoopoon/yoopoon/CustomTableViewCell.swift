//
//  CustomTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class CustomTableViewCell: UITableViewCell {

    @IBOutlet weak var uiPhone: UILabel!
    @IBOutlet weak var uiTitle: UILabel!
    @IBOutlet weak var uiSubTitle: UILabel!
    @IBOutlet weak var uiFooter: UIView!
    @IBOutlet weak var uiP1: UIImageView!
    @IBOutlet weak var uiP2: UIImageView!
    @IBOutlet weak var uiP3: UIImageView!
    @IBOutlet weak var uiP4: UIImageView!
    @IBOutlet weak var uiL1: UILabel!
    @IBOutlet weak var uiL2: UILabel!
    @IBOutlet weak var uiL3: UILabel!
    @IBOutlet weak var uiL4: UILabel!
    @IBOutlet weak var constraintMiddleToSuerBottom: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleToFooterBottom: NSLayoutConstraint!
    
    func showFooter(){
        uiFooter.hidden = false
        constraintMiddleToFooterBottom.priority = 998
        constraintMiddleToSuerBottom.priority = 750
    }
    
    func hiddenFooter(){
        uiFooter.hidden = true
        constraintMiddleToFooterBottom.priority = 750
        constraintMiddleToSuerBottom.priority = 997
    }
    
    func toggle(){
        
        if uiFooter.hidden{
            showFooter()
        }else{
           
            hiddenFooter()
            
        }
    }
    
    func initData(entity: CustomProgressEntity){
        
        uiPhone.text = entity.phone
        uiTitle.text = "\(entity.strType!) \(entity.clientName!)"
        uiSubTitle.text = "\(entity.houseType!) \(entity.houses!)"
        reset()
        let image = UIImage(named: "redCycle")
        let appRedBackground = UIColor.redColor()//UIColor(red: 241/255, green: 56/255, blue: 0, alpha: 1)
        if let status = entity.status{
        switch status{
        case CustomProgressStatus.first,CustomProgressStatus.first_copy:
            uiP1.image = image
            uiL1.textColor = appRedBackground
        case CustomProgressStatus.second:
            uiP2.image = image
            uiL2.textColor = appRedBackground
        case CustomProgressStatus.third:
            uiP3.image = image
            uiL3.textColor = appRedBackground
        case CustomProgressStatus.four:
            uiP4.image = image
            uiL4.textColor = appRedBackground
        default:
            break
            
        }
        }
    }
    
    private func reset(){
        uiP1.image = UIImage(named: "grayCycle")
        uiP2.image = UIImage(named: "grayCycle")
        uiP3.image = UIImage(named: "grayCycle")
        uiP4.image = UIImage(named: "grayCycle")
        uiL1.textColor = UIColor.grayColor()
        uiL2.textColor = UIColor.grayColor()
        uiL3.textColor = UIColor.grayColor()
        uiL4.textColor = UIColor.grayColor()
        
    }
}
