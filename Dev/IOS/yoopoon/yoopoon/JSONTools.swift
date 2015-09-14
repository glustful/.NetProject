//
//  JSONTools.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/16.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
class JSONTools{
    class func optStringFromOption(source:String?)->String{
        if let result = source{
            return source!
        }else{
            return ""
        }
    }
}