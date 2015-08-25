//
//  HomeNavigationController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/13.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class HomeNavigationController: UINavigationController {
 
    override func viewDidLoad() {
        super.viewDidLoad()
       
        
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    override func shouldAutorotate() -> Bool {
        return true
    }
    
    override func supportedInterfaceOrientations() -> Int {
        return UIInterfaceOrientationMask.Portrait.rawValue.hashValue | UIInterfaceOrientationMask.PortraitUpsideDown.rawValue.hashValue
    }
}