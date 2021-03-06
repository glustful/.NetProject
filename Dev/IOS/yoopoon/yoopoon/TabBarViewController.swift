//
//  TabBarViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/31.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TabBarViewController: UITabBarController ,UITabBarControllerDelegate{
    
    override func awakeFromNib() {
        super.awakeFromNib()
        self.delegate = self
        //定义背景色
        tabBar.barTintColor = UIColor(red: 241.0/255.0, green: 56.0/255.0, blue: 0.0, alpha: 0.0)
        //定义选中时的背景色
        // tabBar.tintColor = UIColor(red: 250.0/255.0, green: 243.0/255.0, blue: 0.0, alpha: 1.0)
        let selectedColor = UIColor(red: 250.0/255.0, green: 243.0/255.0, blue: 0.0, alpha: 1.0)
        var item = tabBar.items![0] 
        item.image = UIImage(named: "tabactive")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.selectedImage = UIImage(named: "tabactive1")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:UIColor.whiteColor()], forState: UIControlState.Normal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:selectedColor], forState: UIControlState.Selected)
        
        item = tabBar.items![1] 
        
        item.image = UIImage(named: "tabhouse")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.selectedImage = UIImage(named: "tabhouse1")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:UIColor.whiteColor()], forState: UIControlState.Normal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:selectedColor], forState: UIControlState.Selected)
        
        item = tabBar.items![2] 
        item.image = UIImage(named: "tabagent")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.selectedImage = UIImage(named: "tabagent1")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:UIColor.whiteColor()], forState: UIControlState.Normal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:selectedColor], forState: UIControlState.Selected)
        
        item = tabBar.items![3] 
        item.image = UIImage(named: "tabme")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.selectedImage = UIImage(named: "tabme1")?.imageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:UIColor.whiteColor()], forState: UIControlState.Normal)
        item.setTitleTextAttributes([NSForegroundColorAttributeName:selectedColor], forState: UIControlState.Selected)
        if let navBackItem = self.navigationController?.navigationBar.topItem{
            let backItem = UIBarButtonItem()
            backItem.title = "返回"
            
            navBackItem.backBarButtonItem = backItem
        }
    }
    
    func tabBarController(tabBarController: UITabBarController, didSelectViewController viewController: UIViewController){
        for view in tabBarController.view.subviews{
            if view is UITableView{
                (view as! UITableView).removeFromSuperview()
                return
            }
        }
    }
    
    override func viewWillAppear(animated: Bool) {
        
        (self.navigationController!.view.subviews.last as! SearchProductView).hidden = false
        if selectedViewController != nil{
        self.selectedViewController!.viewWillAppear(true)
        }
//        if self.selectedIndex == 3{
//            self.selectedViewController!.viewWillAppear(true)
//        }
//        else if self.selectedIndex == 2{
//            self.selectedViewController!.viewWillAppear(true)
//        }

    }
    
        

    
}
