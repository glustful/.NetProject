//
//  ExceptionHandler.m
//  yoopoon
//
//  Created by yunjoy on 15/8/12.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

#import "ExceptionHandler.h"

@implementation ExceptionHandler
volatile void exceptionHandler(NSException *exception) {
    NSLog(@"exception");
    // 异常的堆栈信息
    NSArray *stackArray = [exception callStackSymbols];
    // 出现异常的原因
    NSString *reason = [exception reason];
    // 异常名称
    NSString *name = [exception name];
    NSString *exceptionInfo = [NSString stringWithFormat:@"Exception reason：%@\nException name：%@\nException stack：%@",name, reason, stackArray];
    NSLog(@"%@", exceptionInfo);
    
    //保存到本地  --  当然你可以在下次启动的时候，上传这个log
    [exceptionInfo writeToFile:[NSString stringWithFormat:@"%@/Documents/error.log",NSHomeDirectory()]  atomically:YES encoding:NSUTF8StringEncoding error:nil];
    
}

NSUncaughtExceptionHandler *exceptionHandlerPtr = (NSUncaughtExceptionHandler *)&exceptionHandler;

@end
