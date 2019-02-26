//
//  WindDriftAd.h
//  WindSDK
//
//  Created by happyelements on 2018/10/19.
//  Copyright © 2018 Codi. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol WindDriftAdDelegate <NSObject>

@optional


/**
 浮标广告加载成功

 @param placementId 广告位ID
 */
- (void)onDriftAdDidRecieved:(NSString *)placementId;


/**
 浮标广告加载失败

 @param placementId 广告位ID
 @param error 错误信息
 */
- (void)onDriftAdDidFailToLoad:(NSString *)placementId error:(NSError *)error;



/**
 浮标广告开始展现

 @param placementId 广告位ID
 */
- (void)onDriftAdDidPresent:(NSString *)placementId;


/**
 浮标广告展示页面关闭

 @param placementId 广告位ID
 */
- (void)onDriftAdDidClose:(NSString *)placementId;


/**
 浮标广告view被关闭

 @param placementId 广告位ID
 */
- (void)onDriftAdDidDriftViewClose:(NSString *)placementId;


@end

@interface WindDriftAd : NSObject


- (instancetype)initWithPlacementId:(NSString *)placementId
              presentViewController:(UIViewController *)viewController
                           delegate:(id<WindDriftAdDelegate>)delegate;


- (void)loadAdData;


- (BOOL)isReady;

/**
 获取浮标的icon

 @return icon本地路径(图片类型：PNG/JPG/JPEG/GIF)
 */
- (NSString *)getDriftAdIconPath;


/**
 点击浮标icon
 */
- (void)setClickHandle;

@end


