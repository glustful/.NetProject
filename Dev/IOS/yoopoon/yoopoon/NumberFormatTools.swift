//
//  NumberFormatTools.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation

class NumberFormatTools: NSObject {
    
    /// double类型去掉未尾的零和点，返回对应的字符串
    class func clipEndZeros(source: Double)->String{
        var str: String = "\(source)"
        if str.componentsSeparatedByString(".").count == 2{
            while true{
                let end = str.endIndex
                let ch = str.removeAtIndex(end.advancedBy(-1))
                
                if ch == "0"{
                    continue
                }
                if ch == "."{
                    break
                }
                str.append(ch)
                break
            }
        }
        return str
    }
}
