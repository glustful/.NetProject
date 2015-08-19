//
//  AgentTitleTableViewCell.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/27.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import UIKit

class AgentTitleTableViewCell: UITableViewCell {

    @IBOutlet weak var uiSuperView: UIView!
    @IBOutlet weak var uiTitle: UILabel!
    @IBOutlet weak var constraintLeftSpace: NSLayoutConstraint!
    @IBOutlet weak var constraintViewLeftSpace: NSLayoutConstraint!
    var title: String?{
        didSet{
            self.uiTitle.text = title
        }
    }
    
    var leftSpace: CGFloat?{
        didSet{
            self.constraintLeftSpace.constant = leftSpace!
        }
    }
    
    var viewLeftSpace: CGFloat?{
        didSet{
            self.constraintViewLeftSpace.constant = viewLeftSpace!
        }
    }
    
    var bgColor: UIColor?{
        didSet{
            self.uiSuperView.backgroundColor = bgColor
        }
    }
    
    var titleColor: UIColor?{
        didSet{
            self.uiTitle.textColor = titleColor
        }
    }
}
