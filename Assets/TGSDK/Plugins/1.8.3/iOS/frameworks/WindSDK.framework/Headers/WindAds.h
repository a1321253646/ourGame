//
//  WindAds.h
//  WindSDK
//
//  Created by happyelements on 2018/4/8.
//  Copyright © 2018 Codi. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <WindSDK/WindAdOptions.h>

typedef NS_ENUM(NSUInteger, WindLogLevel){
    WindLogLevelError=8,
    WindLogLevelWarning=6,
    WindLogLevelInformation=4,
    WindLogLevelDebug=2
};


typedef void(^WindAdDebugCallBack)(NSString * _Nullable msg, WindLogLevel level);

//NS_ASSUME_NONNULL_BEGIN

@interface WindAds : NSObject

@property (nonatomic,strong) WindAdOptions *adOptions;

+ (instancetype)sharedAds;

// Initialize Wind Ads SDK
+ (void) startWithOptions:(WindAdOptions *)options;

/**
 *   DeBug开关显示
 *
 *  @param enable true 开启debug，false 关闭debug
 */
- (void)setDebugEnable:(BOOL)enable;


/**
 *   自定义debug 内容回调显示
 *
 *  @param callBack debugBlock，若不设置则在Xcode debug中显示，
 */
- (void)setDebugCallBack:(WindAdDebugCallBack _Nullable )callBack;



@end

//NS_ASSUME_NONNULL_END
