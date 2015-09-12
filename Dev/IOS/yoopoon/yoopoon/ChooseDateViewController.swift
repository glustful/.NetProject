//
//  ChooseDateViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ChooseDateViewController: UIViewController {
    var delegate: DateChangeListener?
    @IBOutlet weak var uiDatePicker: UIDatePicker!
    @IBOutlet weak var uiSure: UIButton!
    @IBOutlet weak var uiCancel: UIButton!
    @IBAction func cancel(sender: UIButton) {
        self.dismissViewControllerAnimated(true, completion: nil)
    }
    @IBAction func sure(sender: UIButton) {
        if delegate != nil{
        delegate!.dateChange(uiDatePicker.date)
        }
        self.dismissViewControllerAnimated(true, completion: nil)
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        uiSure.layer.cornerRadius = 10
        uiCancel.layer.cornerRadius = 10
        var locale = NSLocale(localeIdentifier: "zh_CN")
        uiDatePicker.locale = locale
        uiDatePicker.timeZone = NSTimeZone(name: "GMT+0800")
        uiDatePicker.date = NSDate()
        uiDatePicker.datePickerMode = UIDatePickerMode.Date
        uiDatePicker.minimumDate = NSDate()
    }

    
}

protocol DateChangeListener{
    func dateChange(date:NSDate)
}
