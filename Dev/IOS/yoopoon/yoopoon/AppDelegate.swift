//
//  AppDelegate.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate {

    var window: UIWindow?
    /// 自定义uitabbar的背景颜色
    var tabbarController: UITabBarController?

    func application(application: UIApplication, didFinishLaunchingWithOptions launchOptions: [NSObject: AnyObject]?) -> Bool {
        // Override point for customization after application launch.
        hiddenNavigationBar()
        initTabBar()
       
        NSSetUncaughtExceptionHandler(exceptionHandlerPtr)
        return true
    }
    

    func applicationWillResignActive(application: UIApplication) {
        // Sent when the application is about to move from active to inactive state. This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) or when the user quits the application and it begins the transition to the background state.
        // Use this method to pause ongoing tasks, disable timers, and throttle down OpenGL ES frame rates. Games should use this method to pause the game.
    }

    func applicationDidEnterBackground(application: UIApplication) {
        // Use this method to release shared resources, save user data, invalidate timers, and store enough application state information to restore your application to its current state in case it is terminated later.
        // If your application supports background execution, this method is called instead of applicationWillTerminate: when the user quits.
    }

    func applicationWillEnterForeground(application: UIApplication) {
        // Called as part of the transition from the background to the inactive state; here you can undo many of the changes made on entering the background.
    }

    func applicationDidBecomeActive(application: UIApplication) {
        // Restart any tasks that were paused (or not yet started) while the application was inactive. If the application was previously in the background, optionally refresh the user interface.
    }

    func applicationWillTerminate(application: UIApplication) {
        // Called when the application is about to terminate. Save data if appropriate. See also applicationDidEnterBackground:.
    }
    
    /**
    隐藏导航栏，导航栏为rootViewController
    */
    func hiddenNavigationBar(){
       // (window!.rootViewController as! UINavigationController).navigationBar.hidden = true
        var navi = (window!.rootViewController as! UINavigationController)
        
        var searchFrame = CGRectMake(0, UIApplication.sharedApplication().statusBarFrame.height, UIScreen.mainScreen().bounds.width, navi.navigationBar.frame.height)
       var searchView =
       NSBundle.mainBundle().loadNibNamed("SearchProductView", owner: nil, options: nil).last as! SearchProductView
       // var searchViewController = SearchProductController(nibName: "SearchProductView", bundle: nil)
        searchView.frame = searchFrame
        searchView.backgroundColor = appRedBackground
        
        navi.view.addSubview(searchView)
        //navi.navigationItem.backBarButtonItem = UIBarButtonItem(barButtonSystemItem: UIBarButtonSystemItem.Done, target: nil, action: nil)
    }

    /**
    自定义uitabbar样式
    
    :returns: <#return value description#>
    */
    func initTabBar(){
        
               
       //定义背景色
        //tabbarController!.tabBar.barTintColor = UIColor(red: 241.0/255.0, green: 56.0/255.0, blue: 0.0, alpha: 0.0)
        //定义选中时的背景色
        //tabbarController!.tabBar.tintColor = UIColor(red: 250.0/255.0, green: 243.0/255.0, blue: 0.0, alpha: 1.0)
       
    }
    

}

