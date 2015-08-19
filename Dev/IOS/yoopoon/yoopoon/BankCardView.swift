//
//  BankCardView.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class BankCardView: UIView {
    var deleteCallBack: ((json: JSON)->())?
    private var json: JSON?
    @IBOutlet weak var uiBankName: UILabel!
    @IBOutlet weak var uiBankNo: UILabel!
    @IBAction func deleteAction(sender: UIButton) {
        if deleteCallBack != nil && json != nil{
            deleteCallBack!(json:json!)
        }
    }
    
    func initData(json: JSON){
        self.json = json
        uiBankName.text = json["bankName"].stringValue
        var num = json["Num"].stringValue
        uiBankNo.text = "**** **** **** \(num)"
    }
}
