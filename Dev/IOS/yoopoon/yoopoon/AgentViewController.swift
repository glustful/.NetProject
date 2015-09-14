//
//  AgentViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AgentViewController: UIViewController, UITableViewDelegate,UITableViewDataSource {
    @IBOutlet weak var mTableView: UITableView!
    //判断是否去到登陆页回来的
    private var isGoLogin = false
    //滚动视图
    private var adScrollerView: ADView?
    //品牌数据源
    private var brandList = [BrandAgentEntity]()
    //英雄榜前三位数据模型
    private var horeDataSource: JSON?
    override func viewDidLoad() {
        super.viewDidLoad()
        mTableView.delegate = self
        mTableView.dataSource = self
        mTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        NSNotificationCenter.defaultCenter().addObserver(self, selector: "notifyLoginOut:", name: "loginOut", object: nil)
    }
    
    func notifyLoginOut(notify: NSNotification){
        self.isGoLogin = false
       
        
    }
    
    override func viewDidAppear(animated: Bool) {
        if !User.share.isLogin{
            if self.isGoLogin{
                self.tabBarController!.selectedIndex = 0
                isGoLogin = false
                return
            }
            self.isGoLogin = true
            
            var storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
            var controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.loginIdentifier) as! LoginViewController
            self.navigationController!.pushViewController(controller, animated: true)
            return
        }else if !(User.share.isBroker ?? false){
            println(self.tabBarController!.selectedIndex)
            self.tabBarController!.selectedIndex = 3
            TipTools().showToast("提示", message: "你还没有成为经纪人，完善资料成为经纪人", duration: 2)
            //return
        }
        else{
            if self.brandList.count == 0{
            requestADImg()
            requestOneBrand()
            requestHoreTopThree()
            }
        }

    }
    
    /**
    请求广告图片，
    */
    private func requestADImg(){
        RequestAdapter()
            .setUrl(urlChannelTitleImg)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .addParameter("ChannelName", value: "banner")
            .request({json in
                var imagesURL = [String]()
                for i in 0..<json.count{
                    imagesURL.append(imgHost+json[i]["TitleImg"].string!)
                    
                }
                //如果你的这个广告视图是添加到导航控制器子控制器的View上,请添加此句,否则可忽略此句
                self.automaticallyAdjustsScrollViewInsets = false;
                
                self.adScrollerView = ADView.adScrollViewWithFrame(CGRectMake(0, 0, UIScreen.mainScreen().bounds.width, UIScreen.mainScreen().bounds.height/5),imageLinkUrl:imagesURL,placeHoderImageName:"placeHoder.jpg" ,pageControlShowStyle:UIPageControlShowStyle.UIPageControlShowStyleCenter)!;
                self.mTableView.reloadData()
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    /**
     请求经纪人页品牌列表，默认10项
    */
    private func requestOneBrand(){
        RequestAdapter()
            .setUrl(urlBrandGetOneBrand)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                if let array = json["List"].array{
                    self.brandList.removeAll(keepCapacity: false)
                    for i in 0..<array.count{
                        var entity = BrandAgentEntity()
                        entity.initData(array[i])
                        self.brandList.append(entity)
                    }
                    self.mTableView.reloadData()
                }
                
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    /**
    获取英雄榜前三位信息
    */
    private func requestHoreTopThree(){
        RequestAdapter()
            .setUrl(urlBrokerInfoTopThree)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .request({json in
                if let array = json["List"].array{
                    if array.count > 0{
                    self.horeDataSource = json
                    self.mTableView.reloadData()
                    }
                }
                
                
                },
                faild: {error in println("\(error!.description)")})
    }

    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        
        return self.brandList.count + 5
    }

    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell {
        //let cell = tableView.dequeueReusableCellWithIdentifier("reuseIdentifier", forIndexPath: indexPath) as! UITableViewCell

        let adIdentity = "adTableviewcell"
        let commentFunctionIdentity = "commentFunction"
        let activeIdentity = "agentActive"
        let horeIdentity = "agentHore"
        let titleIdentity = "agentTitle"
        let houseIdentity = "agentHouse"
       if indexPath.row == 0{
            var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier(adIdentity) as? UITableViewCell
            if cell == nil{
                cell = UITableViewCell(style: UITableViewCellStyle.Default, reuseIdentifier: adIdentity)
                cell!.selectionStyle = UITableViewCellSelectionStyle.None
            }
            if self.adScrollerView != nil{
                cell!.contentView.addSubview(adScrollerView!)
            }
            return cell!
       }
       else if indexPath.row == 1{
        var cell:AgentCommentFunctionViewTableViewCell? = tableView.dequeueReusableCellWithIdentifier(commentFunctionIdentity) as? AgentCommentFunctionViewTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("AgentCommentFunctionViewTableViewCell", owner: self, options: nil).last as? AgentCommentFunctionViewTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        
        return cell!
       }else if indexPath.row == 2{
        var cell:AgentRichTableViewCell? = tableView.dequeueReusableCellWithIdentifier(activeIdentity) as? AgentRichTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("AgentRichTableViewCell", owner: self, options: nil).last as? AgentRichTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        
        return cell!
       }else if indexPath.row == 3{
        var cell:AgentHoreTableViewCell? = tableView.dequeueReusableCellWithIdentifier(horeIdentity) as? AgentHoreTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("AgentHoreTableViewCell", owner: self, options: nil).last as? AgentHoreTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        if horeDataSource != nil{
            cell!.initLayout(horeDataSource!)
        }else{
            cell!.removeChild()
        }
        return cell!
        }
       else if indexPath.row == 4{
        var cell:AgentTitleTableViewCell? = tableView.dequeueReusableCellWithIdentifier(titleIdentity) as? AgentTitleTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("AgentTitleTableViewCell", owner: self, options: nil).last as? AgentTitleTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        
        return cell!
        }
        var cell:AgentHouseTableViewCell? = tableView.dequeueReusableCellWithIdentifier(houseIdentity) as? AgentHouseTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("AgentHouseTableViewCell", owner: self, options: nil).last as? AgentHouseTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        cell!.initLayout(self.brandList[indexPath.row-5])
        return cell!
    }
    
    /**
    设置tableviewcell的高度
    
    :param: tableView <#tableView description#>
    :param: indexPath <#indexPath description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        if indexPath.row == 0{
            return UIScreen.mainScreen().bounds.height/5
        }else if indexPath.row == 1{
            return screenBounds.height/7
        }else if indexPath.row == 2{

            return screenBounds.width / 4 + 50
        }else if indexPath.row == 3{
            if self.horeDataSource == nil{
                return 45
            }
            if let img = UIImage(named: "no1"){
                return img.size.height + 93
            }
            return screenBounds.height/4
          
        }
        else if indexPath.row == 4{
            return 40
        }
        var height = screenBounds.width/(727/280)
        return height + 10
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        if indexPath.row < 5{
            return
        }
        var storyBoard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        if let controller = storyboard?.instantiateViewControllerWithIdentifier(ControllerIdentifier.brandDetailIdentifier) as? BrandDetialController{
            controller.brandId = self.brandList[indexPath.row-5].brandId ?? ""
            self.navigationController!.pushViewController(controller, animated: true)
        }
    }
    
    
}
