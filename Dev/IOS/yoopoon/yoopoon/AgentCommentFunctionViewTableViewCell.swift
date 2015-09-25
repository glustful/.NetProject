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
        let storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        if let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.templateIdentifier) as? TemplateViewController{
            controller.type = 0
            CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
        }
    }
    @IBAction func offenQuestionAction(sender: UIVerticalButton) {
        let storyboard = UIStoryboard(name: "Main", bundle: NSBundle.mainBundle())
        if let controller = storyboard.instantiateViewControllerWithIdentifier(ControllerIdentifier.templateIdentifier) as? TemplateViewController{
            controller.type = 1
            CommentTools.getCurrentController()?.navigationController!.pushViewController(controller, animated: true)
        }
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
