//
//  ConfigConstant.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/16.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit
//异步加载时图片的占位符
let placeHoder = "placeHoder.jpg"
//默认头像图片
let defaultHeadPhoto = "default_head_img"
//屏幕的尺寸
let screenBounds = UIScreen.mainScreen().bounds
//软键盘高度
var keyboardHeight:CGFloat = 258.0

struct ControllerIdentifier {
    static let productDetailIdentifier = "productDetailController"
    static let brandOfProductListIdentifier = "brandOfProductListController"
    static let settingIdentifier = "settingController"
    static let personSettingIdentifier = "personSettingController"
    static let addAgentInentifier = "addAgentControler"
    static let customListIdentifier = "customListControler"
    static let takeMoneyIdentifier = "takeMoneyController"
    static let myWantFormIdentifier = "myWantFormController"
    static let grabRedEnvelopeIdentifier = "grabRedEnvelopeController"
    static let takeIntegrationIdentifier = "takeIntegrationController"
    static let loginIdentifier = "loginController"
    static let brandDetailIdentifier = "brandDetailController"
}

struct SMSType {
    static let registerType = "0"
    static let changePasswordType = "1"
    static let forgetPasswordType = "2"
    static let addBankCardType = "3"
    static let referAgentType = "6"
}

struct SegueIdentifier {
    static let partnerDetailIdentifier = "partnetDetailSegue"
}

struct PopoverSegueIdentifier {
    static let addAgentIdentifier = "addAgentPopover"
    static let chooseBankIdentifier = "chooseBankPopover"
    static let chooseDateIdentifier = "chooseDatePopover"
}

struct CustomProgressStatus {
    static let first = "预约中"
    static let second = "等待上访"
    static let third = "洽谈中"
    static let four = "洽谈成功"
}

struct HouseFromType {
    static let comment = 0
    static let leadOrRec = 1
}
