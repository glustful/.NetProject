//
//  SuperViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class SuperViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        self.automaticallyAdjustsScrollViewInsets = false;
        if let navBackItem = self.navigationController?.navigationBar.topItem{
            var backItem = UIBarButtonItem()
            backItem.title = "返回"
            
            navBackItem.backBarButtonItem = backItem
        }
    }
    
    override func viewWillAppear(animated: Bool) {
        (self.navigationController!.view.subviews.last as! SearchProductView).hidden = true
    }

}
