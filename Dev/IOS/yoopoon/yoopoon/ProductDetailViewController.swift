//
//  ProductDetailViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/29.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class ProductDetailViewController: SuperViewController {
    var productId: String?
    private var phone:String?
    private var productName: String?
    private var houseType: String?
    @IBOutlet weak var constraintTopViewWidth: NSLayoutConstraint!
    @IBOutlet weak var constraintMiddleViewHeight: NSLayoutConstraint!
    @IBOutlet weak var constraintBottomViewHeight: NSLayoutConstraint!
    @IBOutlet weak var uiLabelStatic: UILabel!
    @IBOutlet weak var uiTitle: UILabel!
    @IBOutlet weak var uiPrice: UILabel!
    @IBOutlet weak var uiAdTitle: UILabel!
    @IBOutlet weak var uiProductImg: UIImageView!
    @IBOutlet weak var uiPicDetail: UIImageView!
    @IBOutlet weak var uiCallPhone: UIButton!
    @IBOutlet weak var uiFooterView: UIView!
    @IBAction func skipHouseAction(sender: UIButton) {
        let storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.myWantFormIdentifier) as! MyWantFormViewController
        
        controller.houseName = productName ?? ""
        controller.houseType = houseType ?? ""
        let id = productId ?? ""
        controller.projectId = id
        controller.projectName = productName ?? ""
        
        controller.type = sender.tag //带客
        
        CommentTools.getCurrentController()?.navigationController?.pushViewController(controller, animated: true)

    }
    @IBAction func callPhoneAction(sender: UIButton) {
        if let phone = self.phone{
            CommentTools.dialPhonNumber(phone)
        }else{
            TipTools().showToast("提示", message: "电话号码未知", duration: 2)
        }
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        self.navigationItem.title = "户型详情"
        constraintTopViewWidth.constant = screenBounds.width
        if !(User.share.isBroker ?? false){
            self.uiFooterView.removeFromSuperview()
        }
        self.requestProductDetail()
      
    }
    
    
    /**
    发起请求，获取产品详情
    */
    private func requestProductDetail(){
        RequestAdapter()
        .setUrl(urlProductGetProductById)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .addParameter("productId", value: self.productId ?? "")
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            
            if json.count > 0{
                self.showDataSource(json)
            }
            }, faild: {error in
                TipTools().showToast("提示", message: "\(error!.description)", duration: 2)
        })
    }
    
    private func showDataSource(json: JSON){
        if let Productname = json["Productname"].string{
            self.productName = Productname
            self.uiTitle.text = Productname
        }
        
        if let list = json["ParameterValue"].array{
            for i in 0..<list.count{
                if let string = list[i]["ParameterString"].string{
                    if string == "户型"{
                    self.houseType = list[i]["Value"].stringValue
                        break
                    }
                }
            }
        }
        
        if let phone = json["Phone"].string{
            self.phone = phone
            self.uiCallPhone.setTitle("咨询热线：\(phone)", forState: UIControlState.Normal)
        }
        
        let price = NumberFormatTools.clipEndZeros(json["Price"].double ?? 0)
        self.uiPrice.text = "均价\(price)\(unitMeter)起"
        if let SubTitle = json["SubTitle"].string{
            self.uiAdTitle.text = SubTitle
        }
        
        if let productImg = json["ProductDetailImg"].string{
           // println()
            self.uiProductImg.load(imgHost + productImg, placeholder: UIImage(named: placeHoder), completionHandler: {(_,image,_,_) in
                if let img = image{
                    if img.size.width < screenBounds.width{
                    self.constraintMiddleViewHeight.constant = img.size.height
                    }else{
                        self.constraintMiddleViewHeight.constant = screenBounds.width * img.size.height / img.size.width
                    }
                }
            })
        }
        if let productDetailImg = json["Productimg1"].string{
            self.uiPicDetail.load(imgHost + productDetailImg, placeholder: UIImage(named: placeHoder), completionHandler: {(_,image,_,_) in
                if let img = image{
                   
                    if img.size.width < screenBounds.width{
                        self.constraintBottomViewHeight.constant = img.size.height
                    }else{
                        self.constraintBottomViewHeight.constant = screenBounds.width * img.size.height / img.size.width
                    }
                }
            })
        }
        
        if let content = json["Content"].string{
            //uilabel加载html
            let data = content.dataUsingEncoding(NSUnicodeStringEncoding, allowLossyConversion: true)
            let html = try? NSAttributedString(data: data!, options: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType], documentAttributes: nil)
            self.uiLabelStatic.sizeToFit()
            
            self.uiLabelStatic.attributedText = html
        }

    }

    }
