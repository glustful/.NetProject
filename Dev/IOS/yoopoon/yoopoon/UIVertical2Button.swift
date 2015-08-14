//
//  UIVertical2Button.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

/// 文本在上，图片在下
class UIVertical2Button: UIButton {

    override func layoutSubviews() {
        super.layoutSubviews()
        //var secHeight = self.frame.size.height/8
        //Center text
        
        var newFrame = self.titleLabel!.frame;
        newFrame.origin.x = 0;
        newFrame.origin.y = 5
        newFrame.size.width = self.frame.size.width;
        newFrame.size.height = 25
        self.titleLabel!.frame = newFrame;
        self.titleLabel!.textAlignment = NSTextAlignment.Center
        
        // Center image
        var imgFrame = self.imageView!.frame
        imgFrame.origin.y = self.titleLabel!.frame.size.height + 5
        imgFrame.origin.x = (self.frame.width - (self.frame.size.height - imgFrame.origin.y - 5))/2
        imgFrame.size.width = self.frame.size.height - imgFrame.origin.y - 5
        imgFrame.size.height = self.frame.size.height - imgFrame.origin.y - 5
        //self.imageView!.center.x = self.center.x
        self.imageView!.frame = imgFrame
        

    }


}
