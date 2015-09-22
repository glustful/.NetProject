//
//  ActiveADView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/20.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ActiveADView: UIView {

    var json:JSON?{
        didSet{
            let rows = json!.count%3==0 ? json!.count/3 : json!.count+1
            for index in 0..<json!.count{
               
                let gridView = GridItemView(frame: CGRectMake(CGFloat(index%3)*CGFloat(self.bounds.width/3), CGFloat(index/3) * self.bounds.height/CGFloat(rows), bounds.width/3, bounds.height/CGFloat(rows)))
                gridView.initLayout(json![index])
                gridView.backgroundColor = UIColor.whiteColor()
                gridView.layer.borderWidth = 0.5
                gridView.layer.borderColor = appBackground.CGColor
                self.addSubview(gridView)
            }
        }
    }

}
