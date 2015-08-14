//
//  SearchProductController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/14.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class SearchProductController: UIViewController {

    @IBAction func test() {
        println("test")
    }
 
    override func viewDidLoad() {
        super.viewDidLoad()
        println("serachcontroller=\(self.view.subviews.count)")
    }

   }