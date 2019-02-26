//
//  BaiduMobAdRewardVideoDelegate.h
//  BaiduMobAdSDK
//
//  Created by Yang,Dingjia on 2018/7/3.
//  Copyright © 2018年 Baidu Inc. All rights reserved.
//


#import <Foundation/Foundation.h>
#import "BaiduMobAdCommonConfig.h"

@class BaiduMobAdRewardVideo;

@protocol BaiduMobAdRewardVideoDelegate <NSObject>
@optional
#pragma mark - 视频缓存delegate
/**
 *  视频预加载缓存成功
 */
- (void)videoPreloadSuccess:(BaiduMobAdRewardVideo *)video;

/**
 *  视频预加载缓存失败
 */
- (void)videoPreloadFail:(BaiduMobAdRewardVideo *)video withError:(BaiduMobFailReason)reason;

#pragma mark - 视频播放delegate

/**
 *  广告展示失败
 */
- (void)videoFailPresentScreen:(BaiduMobAdRewardVideo *)video withError:(BaiduMobFailReason)reason;

/**
 *  广告完成播放
 */
- (void)videoDidFinishPlayingMedia:(BaiduMobAdRewardVideo *)video;

/**
 *  用户点击关闭/跳过
 @param progress 当前播放进度 单位百分比 （注意浮点数）
 */
- (void)userDidSkipPlayingMedia:(BaiduMobAdRewardVideo *)video withPlayingProgress:(CGFloat)progress;

/**
 *  用户点击下载/查看详情
 @param progress 当前播放进度 单位百分比
 */
- (void)userDidClickLP:(BaiduMobAdRewardVideo *)video withPlayingProgress:(CGFloat)progress;

@end

