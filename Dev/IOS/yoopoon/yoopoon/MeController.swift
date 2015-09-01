//
//  MeController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class MeController: UIViewController {
    //判断是否去到登陆页回来的
    private var isGoLogin = false
    //控件绑定
    @IBOutlet weak var uiTopView: UIView!
    @IBOutlet weak var uiMiddleView: UIView!
    
    //控件约束绑定
    @IBOutlet weak var constraintTopViewWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintTopViewHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleViewHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintBottomViewHeight: NSLayoutConstraint!
    
    //top middle bottom布局实例
    private var mTopView: MeTopView?
    private var mMiddleView: MeMiddleView?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.constraintTopViewWidth.constant = screenBounds.width
        self.constraintTopViewHeight.constant = 150
        NSNotificationCenter.defaultCenter().addObserver(self, selector: "notifyLoginOut:", name: "loginOut", object: nil)
        var img = UIImage(named: "b1")
        
        constraintMiddleViewHeight.constant = img!.size.height*2 + 30 + 40
        
        initUI()
    }
    
    func notifyLoginOut(notify: NSNotification){
        self.isGoLogin = false
       self.tabBarController!.selectedIndex = 0
        
    }
    
    override func viewWillAppear(animated: Bool) {
       
        if !User.share.isLogin{
            if self.isGoLogin{
                self.tabBarController!.selectedIndex = 0
                isGoLogin = false
                return
            }
            self.isGoLogin = true
            
            self.performSegueWithIdentifier("loginSegue", sender: nil)
        }else{
           
            if User.share.isBroker ?? false{
                requestBrokerDetails()
                requestBrokerClientInfo()
            }
            else{
                requestUserInfo()
            }
        }
    }
    
    /**
    初始化ui
    
    :returns: <#return value description#>
    */
    private func initUI(){
        mTopView = NSBundle.mainBundle().loadNibNamed("MeTopView", owner: nil, options: nil).last as? MeTopView
        mTopView!.frame = CGRectMake(0, 0, uiTopView.frame.width, uiTopView.frame.height)
        uiTopView.addSubview(mTopView!)
        
        mMiddleView = NSBundle.mainBundle().loadNibNamed("MeMiddleView", owner: nil, options: nil).last as? MeMiddleView
        mMiddleView!.frame = CGRectMake(0, 5, uiMiddleView.frame.width, uiMiddleView.frame.height-5)
        uiMiddleView.addSubview(mMiddleView!)
    }
    
    /**
    不是经纪人，获取对应信息
    */
    private func requestUserInfo(){
        RequestAdapter()
            .setUrl(urlBrokerInfoGetBrokerByUserId)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .addParameter("userId", value: User.share.id ?? 0)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                self.mMiddleView!.settingIcon(false)
                self.mTopView!.initLayout(json,isBroker: false)
                
                }, faild: {error in
                    TipTools().showToast("提示", message: "\(error?.description)", duration: 1)
            })
    }
    
    /*
    * 网络请示块开始
    */
    /**
    经济人详细信息获取
    */
    private func requestBrokerDetails(){
        RequestAdapter()
        .setUrl(urlBrokerInfoGetBrokerDetails)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
                self.mMiddleView!.settingIcon(true)
                self.mTopView!.initLayout(json,isBroker: true)
            
            }, faild: {error in
                TipTools().showToast("提示", message: "\(error?.description)", duration: 1)
        })
    }
    
    /**
    *  经济人对应客户信息获取
    */
    private func requestBrokerClientInfo(){
        RequestAdapter()
            .setUrl(urlClientInfoGetStatusByUserId)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .addParameter("pageSize", value: "10")
            .addParameter("page", value: "1")
            .request({json in
                if let list = json["List"].array{
                    self.mTopView!.initClientInfo(json)
                }else{
                    //TipTools().showToast("提示", message: "\(json.string)", duration: 1)
                }
                }, faild: {error in
                    //TipTools().showToast("提示", message: "\(error?.description)", duration: 1)
            })
    }

    //网络请求块结束
}