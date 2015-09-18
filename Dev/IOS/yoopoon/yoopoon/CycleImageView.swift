//
//  CycleImageView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/15.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
class CycleImageView: UIImageView{
    override init(frame: CGRect) {
        super.init(frame: frame)
        self.layer.cornerRadius = frame.size.height/2
        self.layer.masksToBounds = false
        self.contentMode = UIViewContentMode.ScaleAspectFill
        self.clipsToBounds = true
        self.layer.shadowColor = UIColor.whiteColor().CGColor
        self.layer.shadowOffset = CGSizeMake(4.0, 4.0)
        self.layer.shadowOpacity = 0.5
        self.layer.shadowRadius = 2.0
        self.layer.borderColor = UIColor.whiteColor().CGColor
        self.layer.borderWidth = 2.0
    }

    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
}
