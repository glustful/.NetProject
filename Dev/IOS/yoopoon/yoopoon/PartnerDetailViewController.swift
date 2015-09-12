//
//  PartnerDetailViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/8/12.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class PartnerDetailViewController: SuperViewController {
    var id:String?
    @IBOutlet weak var uiHeadPhoto: UIImageView!
    @IBOutlet weak var uiName: UILabel!
    @IBOutlet weak var uiPhone: UILabel!
    @IBOutlet weak var uiRegisterDate: UILabel!
    override func viewDidLoad() {
        super.viewDidLoad()
        uiHeadPhoto.layer.cornerRadius = uiHeadPhoto.frame.size.height/2
        self.navigationItem.title = "合伙人详情"
        requestBrokerById()
    }

    /**
    请示合伙人的详细信息
    */
    private func requestBrokerById(){
        RequestAdapter()
        .setUrl(urlBrokerInfoGetBroker)
        .setEncoding(.URL)
        .setRequestMethod(.GET)
        .addParameter("Id", value: self.id ?? "")
        .setIsShowIndicator(true, currentView: self.view)
        .request({json in
            
                self.initData(json["List"])
            
            }, faild: {error in
                TipTools().showToast("提示", message: "请示错误，请重试", duration: 3)
        })
    }
    
    /**
    初始化数据
    
    :param: json <#json description#>
    
    :returns: <#return value description#>
    */
    private func initData(json: JSON){
        if let name = json["Brokername"].string{
            self.uiName.text = name
        }
        if let name = json["Phone"].string{
            self.uiPhone.text = name
        }
        if let name = json["Headphoto"].string{
            self.uiHeadPhoto.load(imgHost + name, placeholder: UIImage(named: defaultHeadPhoto))
        }
        if let name = json["rgtime"].string{
            self.uiRegisterDate.text = name
        }
    }
}
