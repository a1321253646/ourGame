//
//  TGSDK.h
//  TGSDK
//
//  Created by SunHan on 9/7/15.
//  Copyright (c) 2015 SoulGame. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#define kTGSDKServiceResultErrorInfo @"kTGSDKServiceResultErrorInfo"
typedef void (^TGSDKServiceResultCallBack)(BOOL success, id _Nullable tag, NSDictionary* _Nullable result);

typedef enum {
    TGAdPlatformTG,
} TGAdPlatform;

typedef enum {
    TGAdTypeNone,
    TGAdTypeCP,
    TGAdType3rdCP,
    TGAdType3rdPop,
    TGAdType3rdVideo,
    TGADType3rdAward,
    TGAdType3rdNative,
    TGAdType3rdInteract,
    TGAdType3rdBanner
} TGAdType;

typedef enum {
    TGBannerNormal,
    TGBannerLarge,
    TGBannerMediumRectangle
} TGBannerType;

typedef enum {
    TGADIsOK,
    TGSDKNotInitialize,
    TGADNotPreload,
    TGADConfigNotFound,
    TGADIsABTest,
    TGADNotReady,
    TGADDevicePowerIsLow,
    TGADNoNetwork
} TGAdStatus;

typedef enum {
    TGNonPayingUser,
    TGSmallPaymentUser,
    TGMediumPaymentUser,
    TGLargePaymentUser
} TGPayingUser;

@protocol TGPreloadADDelegate <NSObject>
@optional
- (void) onPreloadSuccess:(NSString* _Nullable)result;

- (void) onPreloadFailed:(NSString* _Nullable)result WithError:(NSError* _Nullable) error;

- (void) onCPADLoaded:(NSString* _Nonnull) result __attribute__((deprecated));

- (void) onVideoADLoaded:(NSString* _Nonnull) result __attribute__((deprecated));

- (void) onAwardVideoLoaded:(NSString* _Nonnull) result;

- (void) onInterstitialLoaded:(NSString* _Nonnull) result;

- (void) onInterstitialVideoLoaded:(NSString* _Nonnull) result;

@end

@protocol TGADDelegate <NSObject>
@optional
- (void) onShowSuccess:(NSString* _Nonnull)result __attribute__((deprecated));

- (void) onShowFailed:(NSString* _Nonnull)result WithError:(NSError* _Nullable)error __attribute__((deprecated));

- (void) onADComplete:(NSString* _Nonnull)result __attribute__((deprecated));

- (void) onADClick:(NSString* _Nonnull)result __attribute__((deprecated));

- (void) onADClose:(NSString* _Nonnull)result __attribute__((deprecated));

- (void) onShow:(NSString* _Nonnull)scene Success:(NSString* _Nonnull)result;

- (void) onShow:(NSString* _Nonnull)scene Failed:(NSString* _Nonnull)result Error:(NSError* _Nullable)error;

- (void) onAD:(NSString* _Nonnull)scene Click:(NSString* _Nonnull)result;

- (void) onAD:(NSString* _Nonnull)scene Close:(NSString* _Nonnull)result Award:(BOOL)award;

@end

@protocol TGRewardVideoADDelegate <TGADDelegate>
@optional
- (void) onADAwardSuccess:(NSString* _Nonnull)result __attribute__((deprecated));

- (void) onADAwardFailed:(NSString* _Nonnull)result WithError:(NSError* _Nullable)error __attribute__((deprecated));

@end

@interface TGSDK : NSObject

@property (strong, nonatomic) NSString* _Nonnull appID;
@property (strong, nonatomic) NSString* _Nonnull publisherID;
@property (strong, nonatomic) NSString* _Nonnull channelID;
@property (strong, nonatomic, readonly) NSString* _Nonnull udid;
@property (strong, nonatomic, readonly) NSString* _Nullable tgid;
@property (strong, nonatomic, readonly) NSString* _Nullable userRegisterDate;
@property (nonatomic, readonly) BOOL debugEnv;
@property (nonatomic) BOOL enableLog;

+(TGSDK* _Nonnull)sharedInstance;

+ (NSString* _Nonnull) SDKVersion;

+ (BOOL) checkSDKVersion:(NSString* _Nonnull)version;

//初始化函数
+ (void) setDebugModel:(BOOL)debug;

+ (void) enableTestServer;

+ (void) initialize:(NSString* _Nonnull)appid
          channelID:(NSString* _Nonnull)channelid
           callback:(TGSDKServiceResultCallBack _Nullable)cb;

+ (void) initialize:(NSString* _Nonnull)appid
           callback:(TGSDKServiceResultCallBack _Nullable)cb;

+ (void) initialize:(TGSDKServiceResultCallBack _Nullable)cb;

+(void)setSDKConfig:(NSString* _Nullable)val forKey:(NSString* _Nonnull)key;
+(NSString* _Nullable)getSDKConfig:(NSString* _Nonnull)key;

//第三方绑定
//就是登录和注册的合体版本
+(void)userPartnerBind:(NSString* _Nonnull)puid
               partner:(NSString* _Nonnull)partner
                   tag:(id _Nullable)tag
              callBack:(TGSDKServiceResultCallBack _Nullable)cb;

/**************************   广告相关  ******************************/

/*游戏在启动、登陆完成后，调用预加载接口进行广告的预加载*/
+(int) isWIFI;
+(void) preloadAd:(id<TGPreloadADDelegate> _Nullable) delegate;
+(void) preloadAdOnlyWIFI:(id<TGPreloadADDelegate> _Nullable)delegate;

+(BOOL)couldShowAd:(NSString* _Nonnull)scene;
+(BOOL)couldShow:(NSString* _Nonnull)scene Ad:(NSString* _Nullable) sdk;
+(TGAdStatus) getAdStatus:(NSString* _Nonnull)scene;

/*当开始给用户显示广告的时候调用，返回值如果是NSString，则是预加载没有完成或者没有调用预加载，如果返回值是NSData，则是图片的数据。同时发送counter cp_adview*/
+(void)setADDelegate:(id<TGADDelegate> _Nullable)delegate;
+(void)setRewardVideoADDelegate:(id<TGRewardVideoADDelegate> _Nullable)delegate;
+(void)showAd: (NSString* _Nonnull)scene;
+(void)show:(NSString* _Nonnull) scene Ad:(NSString* _Nullable) sdk;
+(void)show:(NSString* _Nonnull) scene WithViewController:(UIViewController* _Nullable) view;
+(void)show:(NSString* _Nonnull) scene WithViewController:(UIViewController* _Nullable) view AndAd:(NSString* _Nullable) sdk;
+(void)reportAdRejected:(NSString* _Nonnull)sceneId;
+(void)showAdScene:(NSString* _Nonnull)scene;
/*banner类型广告专用*/
+(void)setBanner:(NSString* _Nonnull)scene Config:(TGBannerType)type x:(float)x y:(float)y width:(float)width height:(float)height Interval:(int)interval;
+(void)closeBanner:(NSString* _Nonnull)scene;

+(nullable id) parameterFromAdScene:(nonnull NSString*)scene WithKey:(nonnull NSString*)key;
+(nullable NSString*) getSceneNameById:(nonnull NSString*)scene;

/*自定义用户数据，主要用于S2S，将自定义用户数据发送给客户服务器用于标示用户*/
+(void)setCustomUserData:(NSString* _Nullable)userData;

/**************************   数据追踪  ******************************/
+ (void)sendCounter:(NSString* _Nonnull)name metaData:(NSDictionary* _Nullable)md;
+ (void)sendCounter:(NSString* _Nonnull)name metaDataJson:(NSString* _Nullable)mdJson;
+ (void)paymentCounter:(nonnull NSString*)productId
           WithMethod:(nullable NSString*)method
      AndTransactionId:(nullable NSString*)trans
           AndCurrency:(nullable NSString*)currency
              AndPrice:(float)price
           AndQuantity:(int)quantity
             AndAmount:(float)amount
        AndGoodsAmount:(int)goodsAmount;
+ (void)tagPayingUser:(TGPayingUser)user
         WithCurrency:(nullable NSString*)currency
    AndCurrentAmount:(float)currentAmount
       AndTotalAmount:(float)totalAmount;

/**************************   测试专用  ******************************/
+ (void)showTestView:(NSString* _Nonnull)scene;
+ (void)showTestView:(NSString * _Nonnull)scene WithViewController:(UIViewController* _Nullable) view;

/**************************  GDPR  *********************************/
+ (NSString* _Nonnull)getUserGDPRConsentStatus;
+ (void)setUserGDPRConsentStatus:(nonnull NSString*)status;
+ (NSString* _Nonnull)getIsAgeRestrictedUser;
+ (void)setIsAgeRestrictedUser:(NSString* _Nonnull)yesorno;
@end
