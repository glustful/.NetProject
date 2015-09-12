//
//  RegexHelper.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/5.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
struct RegexHelper {
    let regex: NSRegularExpression?
    init(_ pattern: String) {
        var error: NSError?
        regex = NSRegularExpression(pattern: pattern,
            options: .CaseInsensitive,
            error: &error)
    }
    func match(input: String) -> Bool {
        if let matches = regex?.matchesInString(input,
            options: nil,
            range: NSMakeRange(0, count(input))) {
                return matches.count > 0
        } else {
            return false
        }
    }
}

let brokerNickNameRegex: String = "^[\\u4e00-\\u9fa5a-zA-Z0-9_]+$"
let brokerNameRegex: String = "^[\\u4e00-\\u9fa5]{2,5}$"
let identifyCardRegex: String = "^(^[1-9]\\d{7}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}$)|(^[1-9]\\d{5}[1-9]\\d{3}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])((\\d{4})|\\d{3}[Xx])$)$"
let emailRegex = "^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((.[a-zA-Z0-9_-]{2,3}){1,2})$"
let phoneRegex = "^(13[0-9]|14[0-9]|15[0-9]|18[0-9])\\d{8}$"
let clientNameRegx: String = "^[\\u4e00-\\u9fa5]+$"
let phoneRegx: String = "^1[3|4|5|7|8][0-9]\\d{8}$"