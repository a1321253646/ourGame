//
//  yomobads.h
//  yomobads
//
//  Created by 徐恺 on 2018/12/18.
//  Copyright © 2018 徐恺. All rights reserved.
//

#import <UIKit/UIKit.h>

FOUNDATION_EXPORT NSString * _Nonnull yomobadsVersion;

@protocol yomobadsRewardDelegate <NSObject>
@optional
- (void)rewardDidLoad;
- (void)rewardLoadFailed:(NSString *)error;
- (void)rewardDidShow;
- (void)rewardShowFailed:(NSString *)error;
- (void)rewardDidClick;
- (void)rewardDidClosed;
@end

@protocol yomobadsInterstitialDelegate <NSObject>
@optional
- (void)interstitialDidLoad;
- (void)interstitialLoadFailed:(NSString *)error;
- (void)interstitialDidClick;
- (void)interstitialDidClosed;
@end

@protocol yomobadsBannerDelegate <NSObject>
@optional
- (void)bannerDidLoad;
- (void)bannerLoadFailed:(NSString *)error;
- (void)bannerDidClick;
- (void)bannerDidClosed;
@end

@interface yomobads : NSObject

+ (yomobads *)initialize:(NSString *)appId userId:(NSString *)tgId;
+ (yomobads *)sharedInstance;
+ (void)setDebugModel:(BOOL)debug;

//Reward广告
- (void)preloadRewardWithDelegate:(NSObject<yomobadsRewardDelegate> * _Nullable)delegate;
- (BOOL)couldShowReward;
- (void)showReward:(UIViewController *)currentVC;

//Banner广告
- (void)setBannerInterval:(CGFloat)interval;
- (void)showBanner:(UIViewController *)currentVC WithFrame:(CGRect)rect Delegate:(NSObject<yomobadsBannerDelegate> * _Nullable)delegate;
- (void)closeBanner;

//Interstitial广告
- (void)preloadInterstitial:(UIViewController *)currentVC WithDelegate:(NSObject<yomobadsInterstitialDelegate> * _Nullable)delegate;
- (BOOL)couldShowInterstitial;
- (void)showInterstitial:(UIViewController *)currentVC;

@end
