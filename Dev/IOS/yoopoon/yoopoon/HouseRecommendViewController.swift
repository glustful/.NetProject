//
//  HouseRecommendViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/11.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class HouseRecommendViewController: SuperViewController {
    //请求产品列表参数
    private var paramter = [String:String]()
    //请求产品列表
    private var brandList = [BrandEntity]()
    @IBOutlet weak var mTableView: UITableView!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "推荐楼盘"
        mTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        initParam()
        setupRefresh()
        requestBrandList()
    }
    //初始化请求参数
    private func initParam(){
        paramter.updateValue("房地产", forKey: "className")
        paramter.updateValue("1", forKey: "page")
        paramter.updateValue("6", forKey: "pageSize")
        paramter.updateValue("type", forKey: "all")
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
        return self.brandList.count
    }
    
    
    /**
    返回tableviewcell
    
    :param: tableView <#tableView description#>
    :param: indexPath <#indexPath description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let identify = "tableviewcell"
        
            var cell: ActiveTableCell? = tableView.dequeueReusableCellWithIdentifier(identify) as? ActiveTableCell
            
            if cell == nil { // no value
                
                cell = NSBundle.mainBundle().loadNibNamed("ActiveTableCell", owner: self, options: nil).last as? ActiveTableCell
                cell!.selectionStyle = UITableViewCellSelectionStyle.None
                
            }
            cell!.initLayout(brandList[indexPath.row])
            
            return cell!
        
        
    }
    
    /**
    设置tableviewcell的高度
    
    :param: tableView <#tableView description#>
    :param: indexPath <#indexPath description#>
    
    :returns: <#return value description#>
    */
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        
        var height = screenBounds.width/(729/390)
        
        return height + 15
    }
    
    // Called after the user changes the selection.
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        dealWithBrandDetailSegue(indexPath)
    }
    
    
    private func dealWithBrandDetailSegue(indexPath: NSIndexPath){
        var segue = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        var controller = segue.instantiateViewControllerWithIdentifier(ControllerIdentifier.brandDetailIdentifier) as! BrandDetialController
        controller.brandId = String(self.brandList[indexPath.row].id)
        controller.brandEntity = self.brandList[indexPath.row]
        self.navigationController!.pushViewController(controller, animated: true)
    }


    
}
