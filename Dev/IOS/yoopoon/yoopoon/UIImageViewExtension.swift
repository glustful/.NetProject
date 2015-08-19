//
//  UIImageViewExtension.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

// MARK: - 图片异步加载＋缓存
extension UIImageView{
    func loadImageFromURLString(url:URLLiteralConvertible,placeholderImage: UIImage?){
        self.load(url, placeholder: placeholderImage)
    }
    
    func loadImageFromURLString(url:URLLiteralConvertible){
        self.load(url)
    }
}
