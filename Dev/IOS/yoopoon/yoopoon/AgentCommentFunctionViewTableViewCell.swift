//
//  agentCommentFunctionViewTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AgentCommentFunctionViewTableViewCell: UITableViewCell {

    @IBAction func leadClientAction(sender: UIVerticalButton) {
        if let tabbar = CommentTools.getCurrentController()?.navigationController?.viewControllers.first as? UITabBarController{
            User.share.fromType = HouseFromType.leadOrRec
            tabbar.selectedIndex = 1
        }
    }
    @IBAction func myPackageAction(sender: UIVerticalButton) {
        let storyboard = UIStoryboard(name: "Me", bundle: NSBundle.mainBundle())
        let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.takeMoneyIdentifier) 
        CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
    }
    @IBAction func newBabyCourseAction(sender: UIVerticalButton) {
        TipTools().showToast("提示", message: "请关注我们的官网主页", duration: 2)
    }
    @IBAction func offenQuestionAction(sender: UIVerticalButton) {
         TipTools().showToast("提示", message: "请关注我们的官网主页", duration: 2)
    }
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }
    
}
