//
//  ActiveController.swift
//  yoopoon
//  活动页的控制器
//  Created by 郭俊军 on 15/7/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class ActiveController: UIViewController,UITableViewDataSource,UITableViewDelegate {
    
    //请求产品列表参数
    private var paramter = [String:String]()
    //请求产品列表
    private var brandList = [BrandEntity]()
    //滚动视图
    private var adScrollerView: ADView?
    private var activeADView: ActiveADView?
    
    @IBOutlet weak var mTableView: UITableView!
    override func viewDidLoad() {
        super.viewDidLoad()
        NSThread.sleepForTimeInterval(2.0)
        self.view.backgroundColor = appBackground
        mTableView.delegate = self
        mTableView.dataSource = self
        mTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        initParam()
        setupRefresh()
        
       
    }
    
    override func viewWillAppear(animated: Bool) {
        if self.brandList.count == 0{
            requestADImg()
            requestBrandList()
            requestActiveAD()
            User.share.autoLogin()
        }
    }
    
    //初始化请求参数
    private func initParam(){
        paramter.updateValue("房地产", forKey: "className")
        paramter.updateValue("1", forKey: "page")
        paramter.updateValue("6", forKey: "pageSize")
        paramter.updateValue("type", forKey: "all")
    }
    
    /**
    请求活动页的相关活动简介
    */
    private func requestActiveAD(){
        RequestAdapter()
            .setUrl(urlChannelActiveTitleImg)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .addParameter("channelName", value: "活动")
            .request({json in
                var height = (json.count % 3 == 0) ? (json.count / 3) : (json.count / 3 + 1)
                self.activeADView = ActiveADView(frame: CGRectMake(0, 0, self.mTableView!.bounds.width, screenBounds.height * 0.2 * CGFloat(height)))
                self.activeADView!.json = json
                self.activeADView!.backgroundColor = UIColor.redColor()
                self.mTableView.reloadData()
                
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    /**
    请求产品列表
    */
    private func requestBrandList(){
        RequestAdapter()
            .setUrl(urlBrandGetAllBrand)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .setParameters(paramter)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                self.showDataSource(json["List"])
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    /**
    uitableview加载数据源
    
    :param: json <#json description#>
    */
    private func showDataSource(json:JSON){
        self.mTableView.footerEndRefreshing()
        if json.count == 0{
            TipTools().showToast("提示", message: "数据已经加载完成", duration: 2.0)
            return
        }
        
        paramter.updateValue(String(paramter["page"]!.toInt()!+1), forKey: "page")
        for i in 0..<json.count{
            var entity = BrandEntity()
            entity.generateSelf(json[i])
            brandList.append(entity)
        }
        mTableView.reloadData()
        
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
    添加尾部视图
    */
    func setupRefresh(){
        
        self.mTableView.addFooterWithCallback({
            self.requestBrandList()
        })
        
    }
    
    /**
    设置tableviewcell的个数
    
    :param: tableView <#tableView description#>
    :param: section   <#section description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.brandList.count + 2
    }
    
    
    /**
    返回tableviewcell
    
    :param: tableView <#tableView description#>
    :param: indexPath <#indexPath description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let identify = "tableviewcell"
        let adIdentity = "adTableviewcell"
        let activeIdentity = "activeTableViewCell"
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
        }else if indexPath.row == 1{
            var cell:UITableViewCell? = tableView.dequeueReusableCellWithIdentifier(activeIdentity) as? UITableViewCell
            if cell == nil{
                cell = UITableViewCell(style: UITableViewCellStyle.Default, reuseIdentifier: adIdentity)
                cell!.selectionStyle = UITableViewCellSelectionStyle.None
            }
            if self.activeADView != nil{
                cell!.contentView.addSubview(activeADView!)
            }
            return cell!
        }
        else {
        var cell: ActiveTableCell? = tableView.dequeueReusableCellWithIdentifier(identify) as? ActiveTableCell
        
        if cell == nil { // no value
           
            cell = NSBundle.mainBundle().loadNibNamed("ActiveTableCell", owner: self, options: nil).last as? ActiveTableCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
            
        }
        cell!.initLayout(brandList[indexPath.row-2])
        
        return cell!
    }
    
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
            return activeADView?.bounds.height ?? 0//UIScreen.mainScreen().bounds.height/3
        }
        //729:390
        var height = screenBounds.width/(729/390)
        
        return height + 10 + 5
    }
    
    // Called after the user changes the selection.
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        if indexPath.row > 1{
        self.performSegueWithIdentifier("brandDetail", sender: self)
        }
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        let identifier = segue.identifier ?? ""
        switch identifier{
        case "brandDetail" :
                if segue.destinationViewController is BrandDetialController{
                   dealWithBrandDetailSegue(segue)
            }
        default: break
        }
    }
    
    private func dealWithBrandDetailSegue(segue: UIStoryboardSegue){
        var controller = segue.destinationViewController as! BrandDetialController
        controller.brandId = String(self.brandList[self.mTableView.indexPathForSelectedRow()!.row-2].id)
        controller.brandEntity = self.brandList[self.mTableView.indexPathForSelectedRow()!.row-2]
    }
    

}