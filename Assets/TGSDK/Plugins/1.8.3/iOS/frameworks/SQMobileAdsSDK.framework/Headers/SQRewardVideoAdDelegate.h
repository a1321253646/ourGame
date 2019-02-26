//
//  SQRewardVideoAdDelegate.h
//  SQMobileAds-iOS
//
//  Created by ht on 2017/7/28.
//  Copyright © 2017年 ht. All rights reserved.
//

#import <Foundation/Foundation.h>

@class SQRewardVideoAd;
@protocol SQRewardVideoAdDelegate <NSObject>

@required

// Tells the delegate that the reward video ad has rewarded the user.
- (void)rewardVideoAd:(SQRewardVideoAd *)rewardVideoAd
       rewardCurrency:(NSString *)rewardCurrency
         rewardAmount:(int)rewardAmount;

@optional

// Tells the delegate that the reward video ad data was received.
// If you want to play video, please based on the method of isReady.
- (void)rewardVideoAdDidReceiveAd:(SQRewardVideoAd *)rewardVideoAd;

// Tells the delegate that the reward video ad failed to load.
- (void)rewardVideoAd:(SQRewardVideoAd *)rewardVideoAd didFailToLoadWithError:(NSError *)error;

// Tells the delegate that the reward video ad started playing.
- (void)rewardVideoAdDidStartPlaying:(SQRewardVideoAd *)rewardVideoAd;

// Tells the delegate that the reward video ad closed.
- (void)rewardVideoAdDidClose:(SQRewardVideoAd *)rewardVideoAd;

// Tells the delegate that the reward video ad will leave tha application.
- (void)rewardVideoAdWillLeaveApplication:(SQRewardVideoAd *)rewardVideoAd;

@end
