//
//  BrandDetialController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/17.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import UIKit

class BrandDetialController: SuperViewController,UIWebViewDelegate {
   
    var brandId: String?
    var brandEntity: BrandEntity?
    @IBOutlet weak var uiImageView: UIImageView!
    @IBOutlet weak var uiLabelTitle: UILabel!
    @IBOutlet weak var uiLabelAdTitle: UILabel!
    @IBOutlet weak var uiCallPhone: UIButton!
    @IBOutlet weak var uiHouseType: UIButton!
    //@IBOutlet weak var uiDescript: UIInsetLabel!
    @IBOutlet weak var uiWebContent: UIWebView!
    @IBOutlet weak var topWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintBottomViewHeight: NSLayoutConstraint!
    @IBAction func CallPhoneAction(sender: UIButton) {
        CommentTools.dialPhonNumber(brandEntity?.productParamater?["来电咨询"] ?? "")
    }
    
    override func viewDidLoad() {
        
         self.automaticallyAdjustsScrollViewInsets = false;
        navigationItem.title = "楼盘详情"
        if let backItem = self.navigationController?.navigationBar.topItem?.backBarButtonItem{
            backItem.title = "返回"
        }
        self.topWidth.constant = screenBounds.width
        //设置button的圆角
        self.uiCallPhone.layer.cornerRadius = 10
        self.uiHouseType.layer.cornerRadius = 10
        self.uiWebContent.delegate = self
        if self.brandEntity == nil{
            requestBrandDetail()
        }else{
            initBrand()
        }
        
        
        //requestBrandDetail()
    }
    
    private func initBrand(){
        //uilabel加载html
        var data = brandEntity?.content?.dataUsingEncoding(NSUnicodeStringEncoding, allowLossyConversion: true)
        var html = NSAttributedString(data: data!, options: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType], documentAttributes: nil, error: nil)
        //self.uiDescript.sizeToFit()
        
        // self.uiDescript.attributedText = html
        if let content = brandEntity?.content{
            uiWebContent.loadHTMLString(content, baseURL: nil)
        }
        
        var imgUrl = brandEntity?.productParamater?["图片banner"]
        
        self.uiImageView.loadImageFromURLString(imgHost + (imgUrl ?? ""), placeholderImage: UIImage(named: placeHoder))
        self.uiLabelTitle.text = brandEntity!.Bname
        self.uiLabelAdTitle.text = brandEntity!.subTitle
    }
    
    private func requestBrandDetail(){
        RequestAdapter()
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .addParameter("brandId", value: self.brandId!)
        .setUrl(urlBrandGetBrandDetial)
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            self.brandEntity = BrandEntity()
            self.brandEntity!.generateSelf(json)
            self.initBrand()
            }, faild: {error in
                TipTools().showToast("提示", message: "\(error!.description)", duration: 2)
        })
    }
    
    override func viewWillDisappear(animated: Bool) {
       
        (self.navigationController!.view.subviews.last as! SearchProductView).hidden = false
    }
    
    override func viewWillAppear(animated: Bool) {
        (self.navigationController!.view.subviews.last as! SearchProductView).hidden = true
    }
    
    func webViewDidFinishLoad(webView: UIWebView){
        var size = webView.sizeThatFits(CGSizeZero)
       //webView.frame.size.height = size.height
        constraintBottomViewHeight.constant = size.height
    }
    
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        if segue.destinationViewController is BrandOfProductListController{
            var controller = segue.destinationViewController as! BrandOfProductListController
            controller.brandId = self.brandId
            controller.brandEntity = self.brandEntity
        }
    }
}
