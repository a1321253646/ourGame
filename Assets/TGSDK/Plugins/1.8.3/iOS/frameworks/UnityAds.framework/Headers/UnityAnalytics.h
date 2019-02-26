#import "UnityAnalyticsAcquisitionType.h"

NS_ASSUME_NONNULL_BEGIN

@interface UnityAnalytics : NSObject

+(void)onItemAcquired:(NSString *)transactionId itemId:(NSString *)itemId transactionContext:(NSString *)transactionContext level:(NSString *)level itemType:(NSString *)itemType amount:(float)amount balance:(float)balance acquisitionType:(UnityAnalyticsAcquisitionType)acquisitionType;

+(void)onItemSpent:(NSString *)transactionId itemId:(NSString *)itemId transactionContext:(NSString *)transactionContext level:(NSString *)level itemType:(NSString *)itemType amount:(float)amount balance:(float)balance acquisitionType:(UnityAnalyticsAcquisitionType)acquisitionType;

+(void)onLevelFail:(int)levelIndex;

+(void)onLevelUp:(int)theNewLevelIndex;

+(void)onAdComplete:(NSString *)placementId network:(NSString *)network rewarded:(BOOL)rewarded;

+(void)onIapTransaction:(NSString *)productId amount:(float)amount currency:(NSString *)currency transactionId:(long)transactionId isIapService:(BOOL)isIapService isPromo:(BOOL)isPromo receipt:(NSString *)receipt;

+(void)onEvent:(NSDictionary<NSString *, NSObject *> *)jsonObject;

@end

NS_ASSUME_NONNULL_END
