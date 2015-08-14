//
//  TakeIntegrationViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/11.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TakeIntegrationViewController: SuperViewController {

    @IBOutlet weak var constraintScrollerWidth: NSLayoutConstraint!
    @IBOutlet weak var uiButton: UIButton!
    override func viewDidLoad() {
        super.viewDidLoad()

        self.navigationItem.title = "拿积分"
        constraintScrollerWidth.constant = screenBounds.width
        uiButton.layer.cornerRadius = 10
    }

   
}
