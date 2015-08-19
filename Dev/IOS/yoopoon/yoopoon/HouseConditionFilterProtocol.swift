//
//  HouseConditionFilterProtocol.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/31.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation

protocol HouseConditionFilterProtocol{
    //MARK: 点击事件回调函数
    func dealWithCitySelected(cell: PopoverTableViewCell)
    func dealWithTypeSelected(cell: PopoverTableViewCell)
    func dealWithPriceSelected(cell: PopoverTableViewCell)
}