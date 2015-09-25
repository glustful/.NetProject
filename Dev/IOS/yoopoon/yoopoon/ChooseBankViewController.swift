//
//  ChooseBankViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/10.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ChooseBankViewController: UIViewController,UITableViewDataSource,UITableViewDelegate{
    var delegate: BankProtocol?
    private var dataSource = [BankEntity]()
    @IBOutlet weak var uiTableView: UITableView!
    override func viewDidLoad() {
        super.viewDidLoad()
        requestBankList()
      
    }
    
    private func requestBankList(){
        RequestAdapter()
        .setUrl(urlBankSearchAllBank)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            if let list = json["List"].array{
                for i in 0..<list.count{
                    let entity = BankEntity()
                    entity.generateSelf(list[i])
                    self.dataSource.append(entity)
                }
                self.uiTableView.reloadData()
            }
            }, faild: {error in
                TipTools().showToast("提示", message: "获取数据失败，请重试", duration: 2)
        })
    }

    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        return self.dataSource.count
    }
    
    
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let identifier = "identifier"
        var cell = tableView.dequeueReusableCellWithIdentifier(identifier) as UITableViewCell?
        if cell == nil{
            cell = UITableViewCell(style: UITableViewCellStyle.Subtitle, reuseIdentifier: identifier)
        }
        cell!.textLabel!.text  = self.dataSource[indexPath.row].name
        return cell!
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        if let callback = self.delegate{
            callback.callBack(self.dataSource[indexPath.row])
        }
        self.dismissViewControllerAnimated(true, completion: nil)
    }
}


