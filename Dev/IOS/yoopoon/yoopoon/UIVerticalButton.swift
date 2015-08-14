//
//  UIVerticalButton.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/24.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit
/// 文本在下，图片在上
class UIVerticalButton: UIButton {

    override func layoutSubviews() {
        super.layoutSubviews()
        var secHeight = self.frame.size.height/8
        // Center image
        var imgFrame = self.imageView!.frame
        imgFrame.origin.x = (self.frame.width - 3 * secHeight) / 2
        imgFrame.origin.y = secHeight
        imgFrame.size.width = 3 * secHeight
        imgFrame.size.height = 3 * secHeight
        self.imageView!.frame = imgFrame
        
        
        //Center text
        var newFrame = self.titleLabel!.frame;
        newFrame.origin.x = 0;
        newFrame.origin.y = self.imageView!.frame.size.height + CGFloat(2 * secHeight)
        newFrame.size.width = self.frame.size.width
        self.titleLabel!.adjustsFontSizeToFitWidth = true
        self.titleLabel!.frame = newFrame;
        self.titleLabel!.textAlignment = NSTextAlignment.Center
    }

}
