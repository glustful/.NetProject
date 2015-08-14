//
//  ExceptionHandler.h
//  yoopoon
//
//  Created by yunjoy on 15/8/12.
//  Copyright (c) 2015年 yunjoy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface ExceptionHandler : NSObject
volatile void exceptionHandler(NSException *exception);
extern NSUncaughtExceptionHandler *exceptionHandlerPtr;
@end
