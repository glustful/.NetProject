//
//  AddAgentViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/6.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AddAgentViewController: SuperViewController,UIPopoverPresentationControllerDelegate {
    private var partnerList = [JSON]()
    private var inviteList = [JSON]()
    var type:Int = 0
    @IBOutlet weak var uiTableView: UITableView!
    @IBOutlet weak var uiFooterLabel: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()
        self.uiTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        if type == 0{
            self.uiFooterLabel.text = "添加合伙人"
        self.navigationItem.title = "我的合伙人"
            requestPartnerList()
            requestInviteList()
        }else{
            self.navigationItem.title = "推荐经纪人"
            self.uiFooterLabel.text = "添加经纪人"
            requestRecommendAgentList()
        }
        
        
    }
    
    /**
    获取合伙人的数据
    */
    private func requestPartnerList(){
        RequestAdapter()
        .setUrl(urlPartnerListDetailed)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .setIsShowIndicator(true, currentView: self.view)
        .addParameter("userId", value: User.share.id ?? 0)
        .request({json in
            if let list = json["partnerList"].array{
                for i in 0..<list.count{
                    self.partnerList.append(list[i])
                }
                self.uiTableView.reloadData()
            }else{
                TipTools().showToast("提示", message: "数据请求出错", duration: 2)
            }
            }, faild: {error in
                TipTools().showToast("提示", message: "数据请求出错", duration: 2)
        })
    }
    
    /**
    获取受邀请的数据
    */
    private func requestInviteList(){
        self.inviteList.removeAll(keepCapacity: false)
        self.uiTableView.reloadData()
        RequestAdapter()
            .setUrl(urlPartnerListGetInviteForBroker)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .addParameter("brokerId", value: User.share.id ?? 0)
            .request({json in
                
                if let list = json["list"].array{
                    for i in 0..<list.count{
                        self.inviteList.append(list[i])
                    }
                    self.uiTableView.reloadData()
                }
                }, faild: {error in
                    //TipTools().showToast("提示", message: "数据请求出错", duration: 2)
            })
    }
    
    /**
    获取推荐经纪人数据
    */
    private func requestRecommendAgentList(){
        RequestAdapter()
            .setUrl(urlRecommendAgentGetRecommendAgentListByUserId)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .setIsShowIndicator(true, currentView: self.view)
            .addParameter("userId", value: User.share.id ?? 0)
            .request({json in
                
                    for i in 0..<json.count{
                        self.partnerList.append(json[i])
                    }
                    self.uiTableView.reloadData()
                
                }, faild: {error in
                    TipTools().showToast("提示", message: "数据请求出错", duration: 2)
            })
    }


    
    //MARK: UITABLEVIEW DELEGATE METHOD
    func numberOfSectionsInTableView(tableView: UITableView) -> Int{
        if type == 0{
        return 2
        }else{
            return 1
        }
    }
    
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        if type == 0{
        if section == 0{
            return self.partnerList.count
        }
        return self.inviteList.count
        }else{
            return self.partnerList.count
        }
    }
    
    func tableView(tableView: UITableView, viewForHeaderInSection section: Int) -> UIView?{
        if type != 0{
            return nil
        }
        if section == 1{
            let title = UIInsetLabel()
            
            title.text = "收到的邀请"
            title.textColor = appRedBackground
            
            return title
        }
        let title = UIInsetLabel()
        title.frame = CGRectMake(0, 0, tableView.frame.size.width, 50)
        title.text = "合伙人"
        title.textColor = appRedBackground
        return title
    }
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        
        let identifier = "partnerCell"
        var cell = tableView.dequeueReusableCellWithIdentifier(identifier) as? PartnerTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("PartnerTableViewCell", owner: nil, options: nil).last as? PartnerTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        if indexPath.section == 1{
            cell!.initData(self.inviteList[indexPath.row], type: 1)
            cell!.refreshFunc = requestInviteList
        }else{
            if self.type == 0{
                cell!.initData(self.partnerList[indexPath.row],type: 0)
            }else{
                cell!.initData(self.partnerList[indexPath.row],type: 2)
            }
        
        }
        return cell!
        
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        if indexPath.section == 0{
            if type == 0{
                performSegueWithIdentifier(SegueIdentifier.partnerDetailIdentifier, sender: indexPath)
            }
        }
        
        
    }
    
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        return 80
        
        
    }


    //MARK: DELEGATE方法
    func adaptivePresentationStyleForPresentationController(controller: UIPresentationController, traitCollection: UITraitCollection) -> UIModalPresentationStyle {
        return UIModalPresentationStyle.None
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        
        if let identifier = segue.identifier{
            switch identifier{
            case PopoverSegueIdentifier.addAgentIdentifier:
                
                if let hpp = segue.destinationViewController as? AddAgentDetailViewController{
                    hpp.type = self.type
                    if let ppc = hpp.popoverPresentationController{
                        ppc.delegate = self
                    }
                    
                }
            case SegueIdentifier.partnerDetailIdentifier:
                if let pvc = segue.destinationViewController as? PartnerDetailViewController{
                    if let indexPath = sender as? NSIndexPath{
                        pvc.id = self.partnerList[indexPath.row]["PartnerId"].stringValue
                    }
                    //
                }
            default:
                break
            }
        }
    }
}
