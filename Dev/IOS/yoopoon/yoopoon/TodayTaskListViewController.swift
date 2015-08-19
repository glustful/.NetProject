//
//  TodayTaskListViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/4.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class TodayTaskListViewController: SuperViewController,UITableViewDataSource,UITableViewDelegate {
    private let cellIdentifier = "taskListCell"
    private var page = 1
    private var mTaskList = [TaskEntity]()
    @IBOutlet weak var uiTableView: UITableView!
    @IBOutlet weak var uiTaskCount: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "我的任务"
        self.uiTableView.delegate = self
        self.uiTableView.dataSource = self
//        self.uiTableView.addFooterWithCallback({
//            self.requestTodayTaskList()
//        })
        self.uiTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        
        requestTodayTaskList()
    }
    
    private func requestTodayTaskList(){
        RequestAdapter()
        .setUrl(urlTaskListMobile)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .addParameter("page", value: "\(page)")
        .addParameter("pageSize", value: "10")
        .addParameter("type", value: "today")
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            self.uiTableView.footerEndRefreshing()
            if let count = json["totalCount"].int{
                self.uiTaskCount.text = "今日任务(\(count))"
            }
            if json["list"] != nil{
                self.showDataSource(json["list"])
            }
            }, faild: {error in
                TipTools().showToast("出错了", message: "\(error!.description)", duration: 1)
        })
    }
    
    private func showDataSource(json:JSON){
        if json.count == 0{
            TipTools().showToast("提示", message: "数据已经加载完成", duration: 1)
            return
        }
        page++
        for i in 0..<json.count{
            var entity = TaskEntity()
            entity.generateSelf(json[i])
            self.mTaskList.append(entity)
           
        }
        self.uiTableView.reloadData()
    }
    
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.mTaskList.count
    }
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        var cell: TaskListTableViewCell? = tableView.dequeueReusableCellWithIdentifier(cellIdentifier) as? TaskListTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("TaskListTableViewCell", owner: nil, options: nil).last as? TaskListTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        cell!.taskName = self.mTaskList[indexPath.row].taskName
        var name = self.mTaskList[indexPath.row].awardName ?? ""
        var value = self.mTaskList[indexPath.row].awardValue ?? ""
        cell!.awardMsg = "\(name):\(value)"
        return cell!
    }
    
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
//        var taskName = self.mTaskList[indexPath.row].taskName
//        var name = self.mTaskList[indexPath.row].awardName ?? ""
//        var value = self.mTaskList[indexPath.row].awardValue ?? ""
//        var awardMsg = "\(name):\(value)"
//        var h1 = CommentTools.computerContentSize(taskName ?? "", fontSize: 15, widgetWidth: tableView.frame.size.width).height
//        var h2 = CommentTools.computerContentSize(awardMsg, fontSize: 17, widgetWidth: tableView.frame.size.width).height
        
        return 70//h1 + h2 + 13 + 20
    }
}
