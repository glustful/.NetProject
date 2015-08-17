//
//  ProductTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ProductTableViewCell: UITableViewCell {
    private var entity: ProductEntity?
    @IBOutlet weak var uiTitle: UILabel!
    @IBOutlet weak var uiDescript: UILabel!
    @IBOutlet weak var uiPrice: UILabel!
    @IBOutlet weak var uiImageView: UIImageView!
    
    @IBAction func callPhoneAction(sender: UIButton) {
        if entity == nil || entity!.phone! != ""{
            
                CommentTools.dialPhonNumber(entity!.phone!)
        }else{
            TipTools().showToast("提示", message: "电话号码未知", duration: 2)
        }
    }
    func initLayout(entity: ProductEntity){
        self.entity = entity
        self.uiTitle.text = entity.productName
        self.uiDescript.text = entity.subTitle
        self.uiPrice.text = "均价\(entity.price ?? 0)\(unitMeter)起"
        self.uiImageView.load(entity.productImg!, placeholder: UIImage(named: placeHoder))
    }
}
