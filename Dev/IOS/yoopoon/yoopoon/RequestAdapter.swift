//
//  RequestAdapter.swift
//  yoopoon
//
//  Created by yunjoy on 15/7/14.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

import Foundation
import Alamofire
import UIKit

class RequestAdapter:NSObject, NSURLConnectionDataDelegate{
    //私有变量
    private var method: Alamofire.Method = .GET
    private var URLString: URLStringConvertible = apiHost
    private var parameters: [String: AnyObject]?
    private var encoding: ParameterEncoding = .JSON
    private var cookie: String?
    private var isSave = false
    private var isShowIndicator = false
    private var view: UIView?
    private var dicatorView: UIActivityIndicatorView?
    private var uploadHeadPhotoCallback: ((success: Bool,strResult: NSData?)->())?
    private var uploadHeadPhotoResponseMsg: NSData?
    
    override init(){
        parameters = [String:AnyObject]()
        cookie = NSUserDefaults().objectForKey("cookie") as? String
        
    }
    
    func setIsShowIndicator(indicator: Bool,currentView:UIView)->Self{
        self.isShowIndicator = indicator
        self.view = currentView
        return self
    }
    
    func setRequestMethod(method:Alamofire.Method)->RequestAdapter{
        self.method = method
        return self
    }
    
    func setUrl(url:URLStringConvertible)->Self{
        self.URLString = "\(self.URLString)\(url)"
        return self
    }
    
    func setEncoding(encode:ParameterEncoding)->RequestAdapter{
        self.encoding = encode
        return self
    }
    
    func setParameters(params:[String:AnyObject])->RequestAdapter{
        self.parameters = params
        return self
    }
    
    func addParameter(key:String,value:AnyObject)->Self{
        self.parameters!.updateValue(value, forKey: key)
        return self
    }
    
    func setSaveSession(isSave: Bool)->Self{
        self.isSave = isSave
        return self
    }
    
    func request(finish:(json:JSON)->Void,faild:(error:NSError?)->Void){
        if self.isShowIndicator{
        self.showDicator()
        }
        let request:Request = Alamofire.request(method, URLString, parameters: parameters, encoding: encoding)
        if cookie != nil && !isSave{
          
           request.session.configuration.HTTPAdditionalHeaders!.updateValue(self.cookie!, forKey: "Cookie")
          
        }else{
           request.session.configuration.HTTPAdditionalHeaders!.updateValue("", forKey: "Cookie")
        }
        request.responseJSON{
            response in
            if self.isShowIndicator{
                self.dimisionDicator()
            }
            let cookies: String? = response.response?.allHeaderFields["Set-Cookie"] as? String
            if (self.isSave && cookies != nil){
                
                NSUserDefaults().setValue(cookies, forKey: "cookie")
            }
            if response.result.error != nil{
                faild(error: response.result.error)
            }else{
                let array = JSON(data: response.data!)
               
                finish(json: array)
            }

        }
        /*
        request.responseJSON{
            (request,response,json,error) in
            
            if self.isShowIndicator{
                self.dimisionDicator()
            }
            var cookies: String? = response?.allHeaderFields["Set-Cookie"] as? String
            if (self.isSave && cookies != nil){
                
                NSUserDefaults().setValue(cookies, forKey: "cookie")
            }
            if error != nil{
                faild(error: error)
            }else{
                var array = JSON(json!)
                finish(json: array)
            }
            
        }*/
    }
    
    private func showDicator(){
        dicatorView = UIActivityIndicatorView(activityIndicatorStyle: UIActivityIndicatorViewStyle.WhiteLarge)
        dicatorView!.color = UIColor.grayColor()
        dicatorView!.center = self.view!.center
        
        self.view!.addSubview(dicatorView!)
        dicatorView!.startAnimating()
        
    }
    
    private func dimisionDicator(){
        self.dicatorView!.stopAnimating()
        self.dicatorView!.removeFromSuperview()
    }
    

    
    func uploadHeadImg(image: UIImage,callBack: (sucess: Bool,result: NSData?)->()){
        uploadHeadPhotoCallback = callBack
        
        let formBoundary = "----WebKitFormBoundaryUn6BaRHhVPJWBScv"
        let request = NSMutableURLRequest(URL: NSURL(string: apiHost + urlRecourceUpload)!, cachePolicy: NSURLRequestCachePolicy.ReloadIgnoringLocalCacheData, timeoutInterval: 10)
        let startBoundary = "--" + formBoundary
        let endBoundary = startBoundary + "--"
        let data:NSData = UIImageJPEGRepresentation(image,1)!
        let body = NSMutableString()
        body.appendFormat(startBoundary + "\r\n")
        body.appendFormat("Content-Disposition: form-data; name=\"fileToUpload\"; filename=\"headPhoto.jpg\"\r\n")
        body.appendFormat("Content-Type: image/png\r\n\r\n")
        let requestData = NSMutableData()
        requestData.appendData(body.dataUsingEncoding(NSUTF8StringEncoding)!)
        requestData.appendData(data)
        let end = "\r\n\(endBoundary)"
        requestData.appendData(end.dataUsingEncoding(NSUTF8StringEncoding, allowLossyConversion: true)!)
        let content = "multipart/form-data; boundary=\(formBoundary)"
        request.setValue(content, forHTTPHeaderField: "Content-Type")
        request.setValue("\(requestData.length)", forHTTPHeaderField: "Content-Length")
        
        request.setValue(self.cookie, forHTTPHeaderField: "Cookie")
        request.HTTPBody = requestData
        request.HTTPMethod = "POST"
       
        _ = NSURLConnection(request: request, delegate: self)
        
    }
    
    func deleteBankCardById(id:Int,callBack: (sucess: Bool,result: NSData?)->()){
        uploadHeadPhotoCallback = callBack
        
        let request = NSMutableURLRequest(URL: NSURL(string: apiHost + urlBankCardDeleteCard)!, cachePolicy: NSURLRequestCachePolicy.ReloadIgnoringLocalCacheData, timeoutInterval: 10)
                let content = "application/json; charset=utf-8"
        request.setValue(content, forHTTPHeaderField: "Content-Type")
        request.setValue(self.cookie, forHTTPHeaderField: "Cookie")
        request.HTTPBody = "\(id)".dataUsingEncoding(NSUTF8StringEncoding, allowLossyConversion: false)
        request.HTTPMethod = "POST"
        
        _ = NSURLConnection(request: request, delegate: self)
        
    }

    
    func connection(connection: NSURLConnection, didReceiveResponse response: NSURLResponse){
        
        if uploadHeadPhotoCallback != nil{
            let rsp = response as! NSHTTPURLResponse
            if rsp.statusCode>=200 && rsp.statusCode<400{
                return
            }
            uploadHeadPhotoCallback!(success: false, strResult: nil)
            uploadHeadPhotoCallback = nil
        }
        
    }
    
    func connectionDidFinishLoading(connection: NSURLConnection){
        
        if uploadHeadPhotoCallback != nil{
            
            uploadHeadPhotoCallback!(success: true,strResult: uploadHeadPhotoResponseMsg)
            
        }
    }
    
    func connection(connection: NSURLConnection, didReceiveData data: NSData){
        
        uploadHeadPhotoResponseMsg = data
    }
    
    func connection(connection: NSURLConnection, didFailWithError error: NSError){
        if uploadHeadPhotoCallback != nil{
            uploadHeadPhotoCallback!(success: false,strResult: nil)
        }
    }
    
//    private func getRequest(method: Method, _ URLString: URLStringConvertible, parameters: [String: AnyObject]? = nil) -> NSMutableURLRequest {
//        let request = NSMutableURLRequest(URL: NSURL(string: URLString.URLString)!)
//        request.HTTPMethod = method.rawValue
//        if parameters != nil {
//            request.HTTPBody = NSJSONSerialization.dataWithJSONObject(parameters!, options: nil, error: nil)
//        }
//        request.setValue(API_UA, forHTTPHeaderField: "User-Agent")
//        request.setValue(HEADER_ACCEPT, forHTTPHeaderField: "Accept")
//        request.setValue("application/json", forHTTPHeaderField: "Content-Type")
//        
//        return request
//    }
//    
//    var request: Request!
//    if DeviceUtils.isIOS7() {
//    request =  mHttpManager.request(getRequest(method, URLString, parameters: parameters))
//    } else {
//    request = mHttpManager.request(method, URLString , parameters: parameters, encoding: ParameterEncoding.JSON)
//    }
    //request.responseJSON....
    
}
