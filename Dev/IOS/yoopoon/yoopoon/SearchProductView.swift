//
//  SearchProductView.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/23.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class SearchProductView: UIView ,UITableViewDataSource,UITableViewDelegate{
    //请求产品列表参数
    private var paramter = [String:String]()
    //请求产品列表
    private var brandList = [BrandEntity]()
    private var mTableView: UITableView!
    @IBOutlet weak var uiSearchText: UITextField!
    @IBAction func searchAction(sender: UIButton) {
        uiSearchText.resignFirstResponder()
        if let controller = CommentTools.getCurrentController() as? TabBarViewController{
            let y = controller.navigationController!.navigationBar.frame.size.height + UIApplication.sharedApplication().statusBarFrame.height
            let height = controller.view.frame.height - controller.tabBar.frame.size.height - y
            mTableView = UITableView(frame: CGRectMake(0, y, controller.view.frame.size.width, height))
           
            mTableView.backgroundColor = appBackground
            mTableView.delegate = self
            mTableView.dataSource = self
            mTableView.separatorStyle = UITableViewCellSeparatorStyle.None
            controller.view.addSubview(mTableView)
            initParam()
            setupRefresh()
            requestBrandList()
        }
    }
   
    
    @IBAction func editDidEnd(sender: UITextField) {
        sender.resignFirstResponder()
    }
  
    //初始化请求参数
    private func initParam(){
        self.brandList.removeAll(keepCapacity: false)
        self.mTableView.reloadData()
        paramter.updateValue("10", forKey: "PageCount")
        paramter.updateValue("1", forKey: "page")
        paramter.updateValue(uiSearchText.text, forKey: "condition")
        paramter.updateValue("房地产", forKey: "className")
    }
    
    /**
    请求产品列表
    */
    private func requestBrandList(){
        RequestAdapter()
            .setUrl(urlBrandSearchBrand)
            .setRequestMethod(.GET)
            .setEncoding(.URL)
            .setParameters(paramter)
            .setIsShowIndicator(true, currentView: CommentTools.getCurrentController()!.view)
            .request({json in
                
                self.showDataSource(json["List"])
                
                },
                faild: {error in print("\(error!.description)")})
    }
    
    /**
    uitableview加载数据源
    
    - parameter json: <#json description#>
    */
    private func showDataSource(json:JSON){
        self.mTableView.footerEndRefreshing()
        if json.count == 0{
            TipTools().showToast("提示", message: "数据已经加载完成", duration: 2.0)
            return
        }
        
        paramter.updateValue(String(Int(paramter["page"]!)!+1), forKey: "page")
        for i in 0..<json.count{
            let entity = BrandEntity()
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
    
    - parameter tableView: <#tableView description#>
    - parameter section:   <#section description#>
    
    - returns: <#return value description#>
    */
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.brandList.count
    }
    
    
    /**
    返回tableviewcell
    
    - parameter tableView: <#tableView description#>
    - parameter indexPath: <#indexPath description#>
    
    - returns: <#return value description#>
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
    
    - parameter tableView: <#tableView description#>
    - parameter indexPath: <#indexPath description#>
    
    - returns: <#return value description#>
    */
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        let height = screenBounds.width/(729/390)
        
        return height + 15
    }
    
    // Called after the user changes the selection.
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        dealWithBrandDetailSegue(indexPath)
    }
    
    
    private func dealWithBrandDetailSegue(indexPath: NSIndexPath){
        let segue = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        let controller = segue.instantiateViewControllerWithIdentifier(ControllerIdentifier.brandDetailIdentifier) as! BrandDetialController
        controller.brandId = String(self.brandList[indexPath.row].id)
        controller.brandEntity = self.brandList[indexPath.row]
        CommentTools.getCurrentController()!.navigationController!.pushViewController(controller, animated: true)
    }

}
