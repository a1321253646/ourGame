//
//  WindSDK.h
//  WindSDK
//
//  Created by happyelements on 2018/4/8.
//  Copyright © 2018 Codi. All rights reserved.
//

#import <UIKit/UIKit.h>

#if defined(__ARM_ARCH_7S__) && __ARM_ARCH_7S__
#error The Wind Ads SDK doesn't support linking with armv7s. Remove armv7s from "ARCHS" (Architectures) in your Build Settings.
#endif


// Header files.
#import <WindSDK/WindAds.h>
#import <WindSDK/WindAdOptions.h>
#import <WindSDK/WindRewardedVideoAd.h>
#import <WindSDK/WindRewardVideoAdAdapter.h>
#import <WindSDK/WindRewardVideoAdConnector.h>
#import <WindSDK/WindProtocol.h>
#import <WindSDK/WindRewardInfo.h>
#import <WindSDK/WindAdRequest.h>
#import <WindSDK/WindSplashAd.h>
#import <WindSDK/WindSplashAdAdapter.h>
#import <WindSDK/WindSplashAdConnector.h>
#import <WindSDK/WindDriftAd.h>
#import <WindSDK/WindDriftView.h>



