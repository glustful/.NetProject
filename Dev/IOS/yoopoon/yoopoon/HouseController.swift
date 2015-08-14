//
//  HouseController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class HouseController: UIViewController, UITableViewDataSource,UITableViewDelegate,UIPopoverPresentationControllerDelegate,HouseConditionFilterProtocol {
    
    //请求产品列表参数
    private var parameter = [String:String]()
    private var totalCount = 0
    //请求产品列表
    private var productList = [ProductEntity]()
    private var isBroker = false
    
    @IBOutlet weak var uiAreaButton: UIHorizantButton!
    @IBOutlet weak var uiTypeButton: UIHorizantButton!
    @IBOutlet weak var uiPriceButton: UIHorizantButton!
    @IBOutlet weak var mTableView: UITableView!
    @IBAction func resetAction(sender: UIButton) {
        HouseConditionFilter.share.reset()
        self.uiAreaButton.setTitle("区域", forState: UIControlState.Normal)
        self.uiTypeButton.setTitle("类型", forState: UIControlState.Normal)
        self.uiPriceButton.setTitle("价格", forState: UIControlState.Normal)
        
        initParam()
        requestProductList()
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        
        isBroker = User.share.isBroker ?? false
        self.view.backgroundColor = appBackground
        mTableView.delegate = self
        mTableView.dataSource = self
        mTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        initParam()
        setupRefresh()
        //requestADImg()
        //requestProductList()
        
        
    }
    
    override func viewWillAppear(animated: Bool) {
        isBroker = User.share.isBroker ?? false
        if self.productList.count == 0{
            requestADImg()
            requestProductList()
        }else{
            self.mTableView.reloadData()
        }
    }
    
    override func viewWillDisappear(animated: Bool) {
        User.share.fromType = HouseFromType.comment
    }
    
    //初始化请求参数
    private func initParam(){
        var filter = HouseConditionFilter.share
        parameter.updateValue(filter.areaName, forKey: "AreaName")
        parameter.updateValue("true", forKey: "IsDescending")
        parameter.updateValue("OrderByAddtime", forKey: "OrderBy")
        parameter.updateValue("\(filter.page)", forKey: "Page")
        parameter.updateValue("\(filter.pageCount)", forKey: "PageCount")
        parameter.updateValue(filter.priceBegin, forKey: "PriceBegin")
        parameter.updateValue(filter.priceEnd, forKey: "PriceEnd")
        parameter.updateValue(filter.typeId, forKey: "TypeId")
    }
    
    /**
    请求产品列表
    */
    private func requestProductList(){
        RequestAdapter()
            .setUrl(urlProductGetSearchProduct)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .setParameters(parameter)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                if let count = json["TotalCount"].int{
                    self.totalCount = count
                }
                if json["List"] != nil{
                    self.showDataSource(json["List"])
                }
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    private func showDataSource(json:JSON){
        if HouseConditionFilter.share.page == 1{
            productList.removeAll(keepCapacity: false)
        }
        self.mTableView.footerEndRefreshing()
        if json.count == 0{
            TipTools().showToast("提示", message: "数据已经加载完成", duration: 2.0)
            //return
        }else{
            HouseConditionFilter.share.page++
        }
        for i in 0..<json.count{
            var entity = ProductEntity()
            entity.generateSelf(json[i])
            productList.append(entity)
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
                
                var adView:ADView = ADView.adScrollViewWithFrame(CGRectMake(0, 0, UIScreen.mainScreen().bounds.width, UIScreen.mainScreen().bounds.height/5),imageLinkUrl:imagesURL,placeHoderImageName:"placeHoder.jpg" ,pageControlShowStyle:UIPageControlShowStyle.UIPageControlShowStyleCenter)!;
                self.mTableView.tableHeaderView = adView
                
                },
                faild: {error in println("\(error!.description)")})
    }
    
    /**
    添加尾部视图
    */
    func setupRefresh(){
        
        self.mTableView.addFooterWithCallback({
            self.initParam()
            self.requestProductList()
        })
        
    }
    
    /**
    设置tableviewcell的个数
    
    :param: tableView <#tableView description#>
    :param: section   <#section description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.productList.count + 1
    }
    
    
    /**
    返回tableviewcell
    
    :param: tableView <#tableView description#>
    :param: indexPath <#indexPath description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let identify = "tableviewcell"
        let titleIdentity = "agentTitle"
        if indexPath.row == 0{
            var cell:AgentTitleTableViewCell? = tableView.dequeueReusableCellWithIdentifier(titleIdentity) as? AgentTitleTableViewCell
            if cell == nil{
                cell = NSBundle.mainBundle().loadNibNamed("AgentTitleTableViewCell", owner: self, options: nil).last as? AgentTitleTableViewCell
                cell!.selectionStyle = UITableViewCellSelectionStyle.None
                cell!.viewLeftSpace = 0
                cell!.leftSpace = 20
                cell!.bgColor = appBackground
                cell!.titleColor = UIColor.darkGrayColor()
                
            }
            cell!.title = "共\(self.totalCount)个楼盘"
            return cell!
        }
        var cell: HouseTableViewCell? = tableView.dequeueReusableCellWithIdentifier(identify) as? HouseTableViewCell
        
        if cell == nil { // no value
            //cell = ActiveTableCell(style: UITableViewCellStyle.RawValue, reuseIdentifier: identify)
            cell = NSBundle.mainBundle().loadNibNamed("HouseTableViewCell", owner: self, options: nil).last as? HouseTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
            
        }
        if !isBroker{
            cell!.mFooterView.hidden = true
            
            for constraint in cell!.contentView.constraints(){
                if constraint is TopWithSuerViewBottomMargin{
                    if let cons = constraint as? TopWithSuerViewBottomMargin{
                        cons.priority = 1000
                    }
                }else if constraint is TopWithBottomViewVerticalSpace{
                    if let cons = constraint as? TopWithBottomViewVerticalSpace{
                        cons.priority = 998
                    }
                }
                
                
            }
        }
        cell!.initLayout(productList[indexPath.row-1])
        
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
            return 50
        }
        if isBroker {
            return screenBounds.width/(727/325) + 5
        }
        return screenBounds.width/(727/248) + 5
    }
    
    // Called after the user changes the selection.
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        if indexPath.row > 0{
            var storyBoard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
            if let controller = storyBoard.instantiateViewControllerWithIdentifier(ControllerIdentifier.productDetailIdentifier) as? ProductDetailViewController{
                if let id = self.productList[indexPath.row-1].id{
                controller.productId = "\(id)"
                }
                self.navigationController!.pushViewController(controller, animated: true)
            }
        }
    }
    
    //MARK: DELEGATE方法
    func adaptivePresentationStyleForPresentationController(controller: UIPresentationController!, traitCollection: UITraitCollection!) -> UIModalPresentationStyle {
        return UIModalPresentationStyle.None
    }
    
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        if let identifier = segue.identifier{
            switch identifier{
            case HouseConditionSegueIdentifier.cityIdentifier:
                dealWithPopover(segue, filterType: .CITY)
            case HouseConditionSegueIdentifier.typeIdentifier:
                dealWithPopover(segue, filterType: .TYPE)
            case HouseConditionSegueIdentifier.priceIdentifier:
                dealWithPopover(segue, filterType: .PRICE)
            case "productDetail":
                if let controller = segue.destinationViewController as? ProductDetailViewController{
                    controller.productId = "\(self.productList[self.mTableView.indexPathForSelectedRow()!.row-1].id)"
                }
            default:
                break
            }
        }
        
    }
    
    private func dealWithPopover(segue: UIStoryboardSegue,filterType: HouseConditionFilterType){
        if let hpp = segue.destinationViewController as? HouseConditionFilterViewController{
            if let ppc = hpp.popoverPresentationController{
                ppc.delegate = self
            }
            hpp.delegate = self
            hpp.filterType = filterType
        }
    }
    
    //MARK: 点击事件回调函数
    func dealWithCitySelected(cell: PopoverTableViewCell){
        if let price = cell.name{
            self.uiAreaButton.setTitle(price, forState: UIControlState.Normal)
        }
        HouseConditionFilter.share.areaName = cell.name ?? ""
        HouseConditionFilter.share.page = 1
        initParam()
        requestProductList()
    }
    func dealWithTypeSelected(cell: PopoverTableViewCell){
        if let price = cell.name{
            self.uiTypeButton.setTitle(price, forState: UIControlState.Normal)
        }
        HouseConditionFilter.share.typeId = cell.id ?? ""
        HouseConditionFilter.share.page = 1
        initParam()
        requestProductList()
    }
    func dealWithPriceSelected(cell: PopoverTableViewCell){
        if let price = cell.name{
            self.uiPriceButton.setTitle(price, forState: UIControlState.Normal)
        }
        if let id = cell.id{
            var filter = HouseConditionFilter.share
            switch id{
            case "0":
                filter.priceBegin = "0"
                filter.priceEnd = "4000"
            case "1","2","3","4","5","6":
                if let name = cell.name{
                    var bettwen = name.componentsSeparatedByString("-")
                    if bettwen.count == 2{
                        filter.priceBegin = bettwen[0]
                        filter.priceEnd = bettwen[1]
                        break
                    }
                }
                filter.priceBegin = ""
                filter.priceEnd = ""
                
            case "7":
                filter.priceBegin = "10000"
                filter.priceEnd = "50000"
            default:
                filter.priceBegin = ""
                filter.priceEnd = ""
            }
            HouseConditionFilter.share.page = 1
            initParam()
            requestProductList()
        }
    }
    
}