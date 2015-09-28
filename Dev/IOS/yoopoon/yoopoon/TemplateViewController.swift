//
//  TemplateViewController.swift
//  yoopoon
//
//  Created by yunjoy on 15/9/25.
//  Copyright © 2015年 yunjoy. All rights reserved.
//

import UIKit

class TemplateViewController: SuperViewController {
    private let question = "<h2>1 无法注册</h2><p>用户只有在注册后才能使用优客惠提供的优质服务，因此在点击个人中心后会提示进行登陆，如果没有账户可以选择注册。注册需要提供自己的手机号码，作为以后的登陆账号。如果出现无法注册，请检查个人手机的网络链接是否正常。验证码接收由于通讯运营商网络影响，会在2分钟接收到，如果一直无法接收到验证码，请在首页点击关于，找到我们的联系方式，电话或者邮件联系我们帮您处理。</p><h2>2 登陆故障</h2><p>如果出现无法登陆，请您核对好自己的账户名和账户密码，如果忘记了密码时可以使用注册时提供的手机号码进行密码重置和修改。确认账号和密码无误，依然无法登陆时，请检查您手机的网络链接是否正常，如果一直出现无法链接，请联系我们的客户帮助您处理。于此同时提醒您，勿将密码和账号泄露给他人，造成财务损失和信息泄露。</p><h2>3 无法打开应用程序或者出现闪退和程序崩溃</h2><p>请检查您手机或者其他移动设备的网络链接，以及内存，存储剩余，如果网络正常，内存，存储使用正常依然无法使用，请联系我们的客服帮助您处理问题，我们会安排技术人员帮助您检查问题处理故障，给您造成的麻烦敬请谅解。同时提醒您，不要在不明应用商店或者网站下载本APP，本APP已在百度应用市场，360应用市场，豌豆荚等发布，您可以直接请往如上提示的市场下载，不明市场下载APP由安全风险。</p><h2>4 无法充值或者体现</h2><p>请您确认充值的银行账户能正常使用，同时确认网络链接正常，同时提醒您勿在陌生Wifi网络中进行银行账户操作，如果账户以及网络都正常，但是依然不能正常充值和体现，请您联系我们的客服，我们会在最短的时间内帮助您处理故障。<p><h2>5 找不到客服信息</h2><p>我们的客服信息在首页的关于菜单中，您可以找到我们的客服联系电话和邮件联系方式，如果您有任何疑问以及故障，都可以联系我们，我们很高兴为您提供服务。</p>"
    private let about = "<h2>公司名称：</h2><h3>云南优朋信息科技有限公司</h3><h2>公司地址：</h2><h3>云南省昆明市盘龙区金尚俊园A座2601</h3><h2>公司简介：</h2><p>云南优朋信息科技有限公司成立于2013年，公司位于昆明市盘龙区金尚国际写字楼，是一家专业为云南省中小企业提供B2B电子商务平台及企业信息化服务的公司。云南优朋现有产品有优客惠，优购物，云交易、云商平台网、他知等产品，并自主开发以互联网电子商务为主的各类互联网营销软件、热门游戏等，如微信营销、HTM5小游戏等。同时公司根据中小企业的市场化及发展需求定制开发各类产品及软件应用。</p><h2>联系电话：</h2><h3>4006060176</h3><h2>联系邮箱：</h2><h3>1395612622@qq.com</h3>"
    var type:Int = 0
    @IBOutlet weak var uiTemplateText: UITextView!
    override func viewDidLoad() {
        super.viewDidLoad()
        if type == 0{
            self.navigationItem.title = "常见问题"
            let data = question.dataUsingEncoding(NSUnicodeStringEncoding, allowLossyConversion: true)
            let html = try? NSAttributedString(data: data!, options: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType,NSFontAttributeName:UIColor.whiteColor()], documentAttributes: nil)
            
           // let html = NSAttributedString(string: question, attributes: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType])
            self.uiTemplateText.attributedText = html
            
        }else{
            self.navigationItem.title = "关于我们"
            let data = about.dataUsingEncoding(NSUnicodeStringEncoding, allowLossyConversion: true)
            let html = try? NSAttributedString(data: data!, options: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType,NSFontAttributeName:UIColor.whiteColor()], documentAttributes: nil)
            
            // let html = NSAttributedString(string: question, attributes: [NSDocumentTypeDocumentAttribute:NSHTMLTextDocumentType])
            self.uiTemplateText.attributedText = html

        }
        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
