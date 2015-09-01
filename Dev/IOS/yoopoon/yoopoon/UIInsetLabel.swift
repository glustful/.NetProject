//
//  UIInsetLabel.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class UIInsetLabel: UILabel {
    
    override func drawTextInRect(rect: CGRect) {
        super.drawTextInRect(UIEdgeInsetsInsetRect(rect, UIEdgeInsetsMake(5, 15, 0, 15)))
        
    }

}
