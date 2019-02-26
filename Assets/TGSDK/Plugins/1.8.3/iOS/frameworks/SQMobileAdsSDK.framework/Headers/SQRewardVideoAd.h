//
//  SQRewardVideoAd.h
//  SQMobileAds-iOS
//
//  Created by ht on 2017/7/28.
//  Copyright © 2017年 ht. All rights reserved.
//

#import "SQRewardVideoAdDelegate.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

NS_ASSUME_NONNULL_BEGIN

@interface SQRewardVideoAd : NSObject

/// Delegate to receive video notifications.
@property (nonatomic, weak, nullable) id<SQRewardVideoAdDelegate> delegate;

/// Return the shared SQRewardVideoAd instance.
+ (SQRewardVideoAd *)sharedInstance;

/// Initiates the request to fetch the reward video ad.
/// The magicKey is used for fetching an ad and must not be nil.
- (void)loadWithMagicKey:(NSString *)magicKey;

/// Indicates if the receiver is ready to presented video.
- (BOOL)isReady;

/// Presents the reward video ad with the provided view controller.
- (void)presentFromRootViewController:(UIViewController *)viewController;

@end

NS_ASSUME_NONNULL_END
