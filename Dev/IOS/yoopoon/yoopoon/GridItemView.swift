//
//  GridItemView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/15.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
class GridItemView: UIView{
    //私有变量，控件
    private var labelTitle: UILabel?
    private var labelDescript: UILabel?
    private var imageView: CycleImageView?
    
//    //属性监察器
//    var title:String = ""{
//        didSet{
//            labelTitle!.text = title
//        }
//    }
//    var descript: String = ""{
//        didSet{
//            labelDescript!.text = descript
//        }
//    }
//    var image: UIImage?{
//        didSet{
//            imageView!.image = image
//            println("\(image)")
//        }
//    }
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        labelTitle = UILabel(frame: CGRectMake(0, 2, frame.width, 15))
        labelTitle!.textAlignment = NSTextAlignment.Center
        labelTitle!.font = UIFont(name: "Helvetica", size: 15)
        labelTitle!.adjustsFontSizeToFitWidth = true
        labelTitle!.numberOfLines = 0
        self.addSubview(labelTitle!)
        
        labelDescript = UILabel(frame: CGRectMake(0, 18, frame.width, 15))
        labelDescript!.textAlignment = NSTextAlignment.Center
        labelDescript!.font = UIFont(name: "Helvetica", size: 12)
        labelDescript!.textColor = UIColor.grayColor()
        labelDescript!.numberOfLines = 0
        labelDescript!.adjustsFontSizeToFitWidth = true
        self.addSubview(labelDescript!)
        
        let width = min(frame.width-8, frame.height-41)
        var marginTop = max(41, frame.height-width)
        marginTop -= (marginTop-41)/2
        imageView = CycleImageView(frame: CGRectMake((frame.width-width)/2, marginTop, width, width))
        self.addSubview(imageView!)
    }

    required init?(coder aDecoder: NSCoder) {
        fatalError("init(coder:) has not been implemented")
    }
    
    func initLayout(json:JSON){
        self.labelTitle!.text = JSONTools.optStringFromOption(json["Title"].string)
        self.labelDescript!.text = JSONTools.optStringFromOption(json["AdSubTitle"].string)
        self.imageView!.loadImageFromURLString(imgHost + JSONTools.optStringFromOption(json["TitleImg"].string), placeholderImage: UIImage(named: placeHoder))
    }
}
