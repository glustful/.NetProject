//
//  SettingViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class SettingViewController: SuperViewController {

    @IBOutlet weak var uiPerson: UIButton!
    @IBOutlet weak var uiSecuret: UIButton!
    @IBAction func loginOutAction(sender: AnyObject) {
        User.share.loginOut()
        var notifyCenter = NSNotificationCenter.defaultCenter()
        notifyCenter.postNotificationName("loginOut", object: nil)
        self.navigationController!.popViewControllerAnimated(true)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "设置中心"
        self.uiPerson.setBackgroundImage(UIImage(named: placeHoder), forState: UIControlState.Highlighted)
        self.uiSecuret.setBackgroundImage(UIImage(named: placeHoder), forState: UIControlState.Highlighted)
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    

}
