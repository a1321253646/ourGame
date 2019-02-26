//
//  WindRewardedVideoAd.h
//  WindSDK
//
//  Created by happyelements on 2018/4/8.
//  Copyright © 2018 Codi. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@class WindAdRequest;
@class WindRewardInfo;

NS_ASSUME_NONNULL_BEGIN

@protocol WindRewardedVideoAdDelegate<NSObject>

/**
 激励视频广告物料加载成功

 @param placementId 广告位Id
 */
-(void)onVideoAdLoadSuccess:(NSString * _Nullable)placementId;


/**
 激励视频广告开始播放

 @param placementId 广告位Id
 */
-(void)onVideoAdPlayStart:(NSString * _Nullable)placementId;



/**
 激励视频广告发生点击

 @param placementId 广告位Id
 */
-(void)onVideoAdClicked:(NSString * _Nullable)placementId;


/**
 激励视频广告关闭

 @param info WindRewardInfo里面包含一次广告关闭中的是否完整观看等参数
 @param placementId 广告位Id
 */
- (void)onVideoAdClosedWithInfo:(WindRewardInfo * _Nullable)info placementId:(NSString * _Nullable)placementId;


/**
 激励视频广告发生错误

 @param error 发生错误时会有相应的code和message
 @param placementId 广告位Id
 */
-(void)onVideoError:(NSError *)error placementId:(NSString * _Nullable)placementId;


/**
 激励视频广告调用播放时发生错误
 
 @param error 发生错误时会有相应的code和message
 @param placementId 广告位Id
 */
-(void)onVideoAdPlayError:(NSError *)error placementId:(NSString * _Nullable)placementId;


@end



@interface WindRewardedVideoAd : NSObject

@property (nonatomic,weak) id<WindRewardedVideoAdDelegate> delegate;



+ (instancetype)sharedInstance;

- (BOOL)isReady:(NSString *)placementId;

- (void)loadRequest:(WindAdRequest *)request withPlacementId:(NSString * _Nullable)placementId;

- (BOOL)playAd:(UIViewController *)controller withPlacementId:(NSString * _Nullable)placementId options:(NSDictionary * _Nullable)options error:( NSError *__autoreleasing _Nullable *_Nullable)error;


@end

NS_ASSUME_NONNULL_END
