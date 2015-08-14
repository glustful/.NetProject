//
//  URLConstant.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/16.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
 //api主机域名
let apiHost = "http://192.168.1.199:9010"
//let apiHost = "http://api.iyookee.cn"

//图片服务器
let imgHost = "http://img.yoopoon.com/"

//userController
let urlUserLogin = "/api/user/indexlogin"
let urlUserRegister = "/api/User/AddBroker"
let urlUserForgetPassword = "/api/User/ForgetPassword"
let urlUserChangeForPassword = "/api/User/ChangePassword"

//channelController
let urlChannelIndex = "/api/channel/index"
let urlChannelTitleImg = "/api/channel/GetTitleImg"
let urlChannelActiveTitleImg = "/api/channel/GetActiveTitleImg"

//brandControlller
let urlBrandGetAllBrand = "/api/brand/GetAllBrand"
let urlBrandGetOneBrand = "/api/Brand/GetOneBrand"
let urlBrandGetBrandDetial = "/api/Brand/GetBrandDetail"
let urlBrandGetBrandById = "/api/Brand/GetByBrandId"
let urlBrandSearchBrand = "/api/Brand/SearchBrand"

//productController
let urlProductGetSearchProduct = "/api/product/GetSearchProduct"
let urlProductGetProductsByBrand = "/api/Product/GetProductsByBrand"
let urlProductGetProductById = "/api/Product/GetProductById"

//resourceController
let urlRecourceUpload = "/api/resource/upload"

//brokerinfoController
let urlBrokerInfoGetBrokerDetails = "/api/BrokerInfo/GetBrokerDetails"
let urlBrokerInfoTopThree = "/api/BrokerInfo/OrderByBrokerTopThree"
let urlBrokerInfoGetBrokerByUserId = "/api/BrokerInfo/GetBrokerByUserId"
let urlBrokerInfoUpdateBroker = "/api/BrokerInfo/UpdateBroker"
let urlBrokerInfoGetBroker = "/api/BrokerInfo/GetBroker"

//clientinfoController
let urlClientInfoGetStatusByUserId = "/api/ClientInfo/GetStatusByUserId"

//conditonController
let urlConditionGetCondition = "/api/Condition/GetCondition"

//taskController
//method=get
//param: Id=0&page=1&pageSize=10&type=today
let urlTaskListMobile = "/api/Task/TaskListMobile"

//smsController
let urlSMSSend = "/api/SMS/SendSMS"
let urlSMSSendForChangePassword = "/api/SMS/SendSmsForbroker"

//partnerListControler
let urlPartnerListDetailed = "/api/PartnerList/PartnerListDetailed"
let urlPartnerListGetInviteForBroker = "/api/PartnerList/GetInviteForBroker"
let urlPartnerListAddPartnet = "/api/PartnerList/AddPartnerList"
let urlPartnerListSetPartner = "/api/PartnerList/SetPartner"

//bankCardController
let urlBankCardSearchAllBankByUser = "/api/BankCard/SearchAllBankByUser"
let urlBankCardDeleteCard = "/api/BankCard/DeleteBankCard"
let urlBankCardAddBankCard = "/api/BankCard/AddBankCard"

//bankController
let urlBankSearchAllBank = "/api/Bank/SearchAllBank"

//brokerLeadClientController
let urlBrokerLeadClientAdd = "/api/BrokerLeadClient/Add"

//BrokerRECClientController
let urlBrokerRECClientAdd = "/api/BrokerRECClient/Add"

//recommendAgentController
let urlRecommendAgentGetRecommendAgentListByUserId = "/api/RecommendAgent/GetRecommendAgentListByUserId"
