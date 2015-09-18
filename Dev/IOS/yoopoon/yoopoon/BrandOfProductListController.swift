//
//  BrandOfProductListController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class BrandOfProductListController: SuperViewController,UITableViewDataSource,UITableViewDelegate {
    var brandId: String?
    var brandEntity: BrandEntity?
    var productList = [ProductEntity]()
    @IBOutlet weak var constraintTopViewWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintTopViewHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintBottomViewHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleHeight: NSLayoutConstraint!
    
    @IBOutlet weak var uiMiddleView: UIView!
    @IBOutlet weak var uiImageView: UIImageView!
    @IBOutlet weak var uiLabelTitle: UILabel!
    @IBOutlet weak var uiLabelAdTitle: UILabel!
    @IBOutlet weak var uiLabelStatic: UILabel!
    @IBOutlet weak var uiCallPhone: UIButton!
    @IBOutlet weak var uiLeftTableView: UITableView!
    @IBOutlet weak var uiRightTableView: UITableView!
    @IBAction func callPhoneAction(sender: UIButton) {
        CommentTools.dialPhonNumber(brandEntity?.productParamater?["来电咨询"] ?? "")
    }
    @IBAction func leadClientAction(sender: UIButton) {
        User.share.fromType = HouseFromType.leadOrRec
        let tab = self.navigationController!.viewControllers.first as! TabBarViewController
        tab.selectedIndex = 1
        
        self.navigationController!.popToViewController(tab, animated: true)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "品牌详情"
        self.constraintTopViewWidth.constant = screenBounds.width
        self.constraintBottomViewHeight.constant = 0
        uiCallPhone.layer.cornerRadius = 10
        uiLabelStatic.text = "楼盘\n详情"
        if !User.share.isLogin || !(User.share.isBroker ?? false){
            CommentTools.removeAllViews(uiMiddleView)
            constraintMiddleHeight.constant = 0
        }
        if brandEntity == nil{
            requestBrand()
        }else{
            initBrand()
        }
        
        self.uiLeftTableView.delegate = self
        self.uiLeftTableView.dataSource = self
        self.uiRightTableView.delegate = self
        self.uiRightTableView.dataSource = self
        self.uiLeftTableView.backgroundColor = appBackground
        self.uiRightTableView.backgroundColor = appBackground
        self.uiLeftTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        self.uiRightTableView.separatorStyle = UITableViewCellSeparatorStyle.None
        requestProductsByBrandId()
    }
    
    private func requestBrand(){
        RequestAdapter()
            .setEncoding(.URL)
            .setRequestMethod(.GET)
            .setUrl(urlBrandGetBrandById)
            .addParameter("BrandId", value: brandId ?? "")
            .request({json in
                if json["Bname"] != nil{
                    self.brandEntity = BrandEntity()
                    self.brandEntity!.generateSelf(json)
                    self.initBrand()
                }
                }, faild: {error in
                   
            })
    }
    
    private func initBrand(){
        let imgUrl = brandEntity?.productParamater?["图片banner"]
        
        self.uiImageView.load(imgHost + (imgUrl ?? ""), placeholder: UIImage(named: placeHoder))
        self.uiLabelTitle.text = brandEntity!.Bname
        self.uiLabelAdTitle.text = brandEntity!.subTitle
    }
    
    /**
    请示品牌下的把有商品信息
    */
    private func requestProductsByBrandId(){
        RequestAdapter()
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .setUrl(urlProductGetProductsByBrand)
        .addParameter("BrandId", value: brandId ?? "")
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            if json["productList"] != nil{
                self.showDataSource(json["productList"])
            }
            }, faild: {error in
                TipTools().showToast("提示", message: "\(error!.description)", duration: 2)
        })
    }
    
    private func showDataSource(json: JSON){
        for i in 0..<json.count{
            let entity = ProductEntity()
            entity.generateSelf(json[i])
            self.productList.append(entity)
        }
        var count = self.productList.count % 2 == 0 ? self.productList.count / 2 : self.productList.count / 2 + 1
        self.constraintBottomViewHeight.constant = computerContentHeight()//CGFloat(count) * screenBounds.height/2
        self.uiLeftTableView.reloadData()
        self.uiRightTableView.reloadData()
    }
    
    func tableView(tableView: UITableView, numberOfRowsInSection section: Int) -> Int{
        if tableView === self.uiLeftTableView{
            return self.productList.count % 2 == 0 ? self.productList.count / 2 : self.productList.count / 2 + 1
        }
        return self.productList.count / 2
    }
    
    
    
    func tableView(tableView: UITableView, cellForRowAtIndexPath indexPath: NSIndexPath) -> UITableViewCell{
        let identifier = "productTableViewCell"
        var cell: ProductTableViewCell? = tableView.dequeueReusableCellWithIdentifier(identifier) as? ProductTableViewCell
        if cell == nil{
            cell = NSBundle.mainBundle().loadNibNamed("ProductTableViewCell", owner: nil, options: nil).last as? ProductTableViewCell
            cell!.selectionStyle = UITableViewCellSelectionStyle.None
        }
        if tableView === self.uiLeftTableView{
            cell!.initLayout(self.productList[indexPath.row * 2])
        }else{
            cell!.initLayout(self.productList[indexPath.row * 2 + 1])
        }
        return cell!
    }
    
    func tableView(tableView: UITableView, heightForRowAtIndexPath indexPath: NSIndexPath) -> CGFloat{
        var row = 0
        if tableView === self.uiLeftTableView{
            row = indexPath.row * 2
        }else{
            row = indexPath.row * 2 + 1
        }
        let entity = self.productList[row]
        let nameSize = CommentTools.computerContentSize(entity.productName ?? "", fontSize: 17, widgetWidth: tableView.frame.size.width - 10)
        let subTitleSize = CommentTools.computerContentSize(entity.subTitle ?? "", fontSize: 14, widgetWidth: tableView.frame.size.width - 10)
        let priceSize = CommentTools.computerContentSize("均价\(entity.price ?? 0)\(unitMeter)起", fontSize: 15, widgetWidth: tableView.frame.size.width-10)
        let totalHeight = nameSize.height + subTitleSize.height + priceSize.height
        
        let height = (tableView.frame.size.width - 10 - 38) / (353/546)
       
        return 40 + height  + totalHeight + 22
    }
    
    func tableView(tableView: UITableView, didSelectRowAtIndexPath indexPath: NSIndexPath){
        performSegueWithIdentifier("productDetail", sender: tableView)
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        let identifier = segue.identifier ?? ""
        switch identifier{
        case "productDetail":
            if sender is UITableView{
                dealWithProductDetailSegue(segue,sender: sender)
            }
        default:
            break
        }
    }
    
    private func dealWithProductDetailSegue(segue: UIStoryboardSegue,sender: AnyObject?){
        let tableView = sender as! UITableView
        let row = tableView.indexPathForSelectedRow!.row ?? 0
        if segue.destinationViewController is ProductDetailViewController{
            let controller = segue.destinationViewController as! ProductDetailViewController
            if tableView === self.uiLeftTableView{
                controller.productId = "\(self.productList[row * 2].id ?? 0)"
            }else{
                controller.productId = "\(self.productList[row * 2 + 1].id ?? 0)"
            }
        }
        
    }
    
    /**
    计算滚动控件的高度
    
    - returns: <#return value description#>
    */
    private func computerContentHeight()->CGFloat{

        let height = (self.uiLeftTableView.frame.size.width - 10 - 38) / (353/546)
      
        var leftHeight: CGFloat = 0
        var rightHeight: CGFloat = 0
        for i in 0..<self.productList.count{
            let entity = self.productList[i]
            let nameSize = CommentTools.computerContentSize(entity.productName ?? "", fontSize: 17, widgetWidth: self.uiLeftTableView.frame.size.width-10)
            let subTitleSize = CommentTools.computerContentSize(entity.subTitle ?? "", fontSize: 14, widgetWidth: self.uiLeftTableView.frame.size.width-10)
            let priceSize = CommentTools.computerContentSize("均价\(entity.price ?? 0)\(unitMeter)起", fontSize: 15, widgetWidth: self.uiLeftTableView.frame.size.width-10)
            let totalHeight = nameSize.height + subTitleSize.height + priceSize.height
            if i % 2 == 0{
                leftHeight += totalHeight
              
            }else{
                rightHeight += totalHeight
            }
        }
        let count = self.productList.count % 2 == 0 ? self.productList.count / 2 : self.productList.count / 2 + 1
        return max(leftHeight,rightHeight) + (height + 62) * CGFloat(count)
    }
}
