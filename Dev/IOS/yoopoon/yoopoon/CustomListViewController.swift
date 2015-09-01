//
//  CustomListViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/7.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class CustomListViewController: SuperViewController,UITableViewDataSource,UITableViewDelegate {
    private var index: NSIndexPath?
    private var page: Int = 1
    private var customProgressList = [CustomProgressEntity]()
    @IBOutlet weak var uiTableView: UITableView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "我的客户"
        self.uiTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        self.uiTableView.addFooterWithCallback({
            self.requestBrokerClientInfo()
        })
        requestBrokerClientInfo()
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
            .addParameter("page", value: "\(page)")
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                self.uiTableView.footerEndRefreshing()
                if let list = json["List"].array{
                    if list.count > 0{
                        self.index = nil
                        self.page++
                    }else{
                        TipTools().showToast("提示", message: "数据已加载完成", duration: 1)
                    }
                    for i in 0..<list.count{
                        var entity = CustomProgressEntity()
                        entity.generateSelf(list[i])
                        self.customProgressList.append(entity)
                    }
                    self.uiTableView.reloadData()
                }else{
                    TipTools().showToast("提示", message: "获取客户失败", duration: 1)
                }
                }, faild: {error in
                    self.uiTableView.footerEndRefreshing()
                    TipTools().showToast("提示", message: "\(error?.description)", duration: 1)
            })
    }

    
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.customProgressList.count
    }
    
    
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        var identifier = "customCell"
        var cell = tableView.dequeueReusableCellWithIdentifier(identifier) as? CustomTableViewCell
        if (cell == nil) {
            cell = NSBundle.mainBundle().loadNibNamed("CustomTableViewCell", owner: nil, options: nil).last as? CustomTableViewCell
            
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        cell!.backgroundColor = appBackground
        cell!.initData(self.customProgressList[indexPath.row])
        if index != nil{
            
            if index == indexPath{
               // println("toggle=\(self.tableView(tableView, heightForRowAtIndexPath: indexPath))")
                
                cell!.toggle()
                return cell!;
                
            }
            
            
            
        }
        cell!.hiddenFooter()
        
        return cell!;
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        
        
        if index != nil && index == indexPath{
            
            index = nil
            
        }else{
            index = indexPath
        }
        
        self.uiTableView.reloadData()
        
        
        
    }
    
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        
        if index != nil && index!.row == indexPath.row{
            
            return 290
        }else {
            return 130
        }
        
    }
}
