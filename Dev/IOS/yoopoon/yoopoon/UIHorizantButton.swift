//
//  UIHorizantButton.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/28.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class UIHorizantButton: UIButton {

    override func layoutSubviews() {
        super.layoutSubviews()
        
        //Center text
        var newFrame = self.titleLabel!.frame;
        newFrame.origin.x = (self.frame.size.width - newFrame.size.width - self.imageView!.frame.size.width)/2
        self.titleLabel!.frame = newFrame;
        self.titleLabel!.adjustsFontSizeToFitWidth = true
        
       // self.titleLabel!.textAlignment = NSTextAlignment.Center
       
        
        // Center image
        var imgFrame = self.imageView!.frame
        imgFrame.origin.x = self.titleLabel!.frame.origin.x + self.titleLabel!.frame.size.width
       
        self.imageView!.frame = imgFrame
        
    
    }

}
