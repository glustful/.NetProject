//
//  HousePriceViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class HouseConditionFilterViewController: UIViewController{
    var filterType: HouseConditionFilterType?
    var delegate: HouseConditionFilterProtocol?
    private var priceArray:[String:String]?
    private var cityOrType: HouseConditionFilter?
    private var leftHeight: CGFloat = 0
    private var middleHeight: CGFloat = 0
    private var rightHeight: CGFloat = 0
    @IBOutlet weak var constraintLeftWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintLeftHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintRightWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintLeftToMiddleSpace: NSLayoutConstraint!
    @IBOutlet weak var uiLeftTableView: UIView!
    @IBOutlet weak var uiMiddleTableView: UIView!
    @IBOutlet weak var uiRightTableView: UIView!
    @IBOutlet weak var uiScroll: UIScrollView!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        constraintLeftWidth.constant = self.preferredContentSize.width-20
        constraintLeftHeight.constant = leftHeight
       
        self.cityOrType = HouseConditionFilter.share
        initLayout()
        
    }
    
    private func requestData(parentId:String,level: Int){
        var cityEntity: HouseConditionDataSource?
        switch level{
        case 0:
            if HouseConditionFilter.share.city.count > 0{
                self.reloadData()
                return
            }
            
        case 1:
            for data in self.cityOrType!.city{
                if data.id == parentId.toInt(){
                    if data.children != nil{
                        self.reloadDataOfLevelOne(parentId)
                        return
                    }else{
                        cityEntity = data
                    }
                }
            }
            
        case 2:
            for data in self.cityOrType!.city{
                if data.children != nil{
                    for child in data.children!{
                        if child.id == parentId.toInt(){
                            if child.children != nil{
                                self.reloadDataOfLevelTwo(parentId)
                                return
                            }
                            else{
                                cityEntity = child
                            }
                        }
                    }
                }
            }
            
        default:
            return
        }
        
        RequestAdapter()
            .setUrl(urlConditionGetCondition)
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .addParameter("parentId", value: parentId)
            .setIsShowIndicator(true, currentView: self.view)
            .request({json in
                
                if level == 0{
                    self.cityOrType!.city = self.cityOrType!.initData(json)
                    self.reloadData()
                }
                else if cityEntity != nil{
                    cityEntity!.children = self.cityOrType!.initData(json)
                    if level == 1{
                        self.reloadDataOfLevelOne(parentId)
                    }else if level == 2{
                        self.reloadDataOfLevelTwo(parentId)
                    }
                }
                }, faild: {error in
                    println("error=\(error!.description)")
            })
        
    }
    
    /**
    根据filterType初始化界面
    
    :returns: <#return value description#>
    */
    private func initLayout(){
        if let type = filterType{
            switch type{
            case .CITY:
                requestData("0",level: 0)
            case .PRICE:
                
                initPrice()
            case .TYPE:
                requestData("0",level: 0)
            default:
                break
            }
        }
    }
    
    /**
    加载城市与房源类型
    */
    private func reloadData(){
        CommentTools.removeAllViews(self.uiLeftTableView)
        CommentTools.removeAllViews(self.uiMiddleTableView)
        CommentTools.removeAllViews(self.uiRightTableView)
        constraintMiddleWidth.constant = 0
        constraintRightWidth.constant = 0
        
        if let type = self.filterType{
            switch type{
            case .CITY:
                if self.cityOrType != nil{
                    self.constraintLeftHeight.constant = CGFloat(self.cityOrType!.city.count * 40)
                    leftHeight = self.constraintLeftHeight.constant
                    for i in 0..<self.cityOrType!.city.count{
                        var cell = self.addSubView(self.uiLeftTableView,title: self.cityOrType!.city[i].name ?? "", index: i,id: "\(self.cityOrType!.city[i].id ?? -1)",type: .CITY,level: 1)
                        if cell != nil{
                            cell!.requestFunction = requestData
                        }
                    }
                }
                
            case .TYPE:
                if self.cityOrType != nil && self.cityOrType!.city.count>0{
                    if let type = self.cityOrType!.city[0].typeList{
                        self.constraintLeftHeight.constant = CGFloat(type.count * 40)
                        
                        for i in 0..<type.count{
                            var cell = self.addSubView(self.uiLeftTableView,title: type[i]["name"] ?? "", index: i,id: type[i]["id"] ?? "", type: .TYPE)
                            if cell != nil{
                                cell!.funcDealWithResult = dealWithTypeSelected
                            }
                            
                        }
                    }
                }
                
            default:
                break
            }
            
        }
        
    }
    
    private func reloadDataOfLevelOne(parentId: String){
        CommentTools.removeAllViews(self.uiMiddleTableView)
        CommentTools.removeAllViews(self.uiRightTableView)
        constraintRightWidth.constant = 0
        if self.cityOrType != nil{
            self.constraintMiddleWidth.constant = self.preferredContentSize.width
            self.constraintLeftToMiddleSpace.constant = 20
            self.uiScroll.setContentOffset(CGPointMake(self.preferredContentSize.width, 0), animated: true)
            for data in self.cityOrType!.city{
                if data.id == parentId.toInt(){
                    if data.children != nil{
                        middleHeight = CGFloat(data.children!.count * 40)
                        constraintLeftHeight.constant = max(leftHeight,middleHeight)
                        for i in 0..<data.children!.count{
                            var cell = self.addSubView(self.uiMiddleTableView,title: data.children?[i].name ?? "", index: i,id: "\(data.children?[i].id ?? -1)",type: .CITY,level: 2)
                            if cell != nil{
                                cell!.requestFunction = requestData
                            }
                        }
                    }
                }
            }

            
        }

    }
    
    private func reloadDataOfLevelTwo(parentId: String){
        CommentTools.removeAllViews(self.uiRightTableView)
        if self.cityOrType != nil{
            for data in self.cityOrType!.city{
                constraintRightWidth.constant = self.preferredContentSize.width
                self.uiScroll.setContentOffset(CGPointMake(self.preferredContentSize.width * 2, 0), animated: true)
                if data.children != nil{
                    for child in data.children!{
                        if child.id == parentId.toInt(){
                            if child.children != nil{
                                rightHeight = CGFloat(child.children!.count * 40)
                                constraintLeftHeight.constant = max(leftHeight,middleHeight,rightHeight)
                                for i in 0..<child.children!.count{
                                    var cell = self.addSubView(self.uiRightTableView,title: child.children![i].name ?? "", index: i,id: "\(child.children![i].id ?? -1)",type: .CITY,level: 3)
                                    if cell != nil{
                                        cell!.requestFunction = requestData
                                        cell!.funcDealWithResult = dealWithCitySelected
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }

            
        }

    }
    
    private func initPrice(){
        if let dictionary = NSBundle.mainBundle().infoDictionary{
            self.priceArray = dictionary["PriceDictionary"] as? [String:String]
            
            if self.priceArray != nil{
                self.constraintLeftHeight.constant = CGFloat(self.priceArray!.count * 40)
                for i in 0..<self.priceArray!.count{
                    var cell = self.addSubView(self.uiLeftTableView,title: self.priceArray?["\(i)"] ?? "", index: i,id: "\(i)",type: .PRICE)
                    if cell != nil{
                    cell!.funcDealWithResult = dealWithPriceSelected
                    }
                }
            }
            
        }
    }
    
    /**
    添加选项
    */
    private func addSubView(parentView: UIView,title: String,index: Int,id: String,type: HouseConditionFilterType,level: Int = 0)->PopoverTableViewCell?{
        
        if let cell = NSBundle.mainBundle().loadNibNamed("PopoverTableViewCell", owner: nil, options: nil).last as? PopoverTableViewCell{
            cell.frame = CGRectMake(10, CGFloat(index * 40), self.preferredContentSize.width-20, 40)
            cell.titleLabel = title
            cell.id = id
            cell.name = title
            cell.type = type
            cell.level = level
        
            parentView.addSubview(cell)
            return cell
        }
        return nil
    }
    
    //MARK: 点击事件回调函数
    private func dealWithCitySelected(cell: PopoverTableViewCell){
        if delegate != nil{
            delegate!.dealWithCitySelected(cell)
        }
        self.dismissViewControllerAnimated(true, completion: nil)
    }
    
    private func dealWithTypeSelected(cell: PopoverTableViewCell){
        if delegate != nil{
            delegate!.dealWithTypeSelected(cell)
        }
        self.dismissViewControllerAnimated(true, completion: nil)
    }
    
    private func dealWithPriceSelected(cell: PopoverTableViewCell){
        if delegate != nil{
            delegate!.dealWithPriceSelected(cell)
        }
        self.dismissViewControllerAnimated(true, completion: nil)
    }
   
    
    override var preferredContentSize: CGSize{
        get{
            return CGSizeMake(screenBounds.width - 30, screenBounds.height*2/3)
        }
        set{
            super.preferredContentSize = newValue
        }
    }
}
