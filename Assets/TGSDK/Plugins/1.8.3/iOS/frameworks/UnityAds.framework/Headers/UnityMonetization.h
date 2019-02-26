#import "UMONPlacementContent.h"
#import "UMONRewardablePlacementContent.h"
#import "UMONShowAdPlacementContent.h"
#import "UMONPromoAdPlacementContent.h"
#import "UMONNativePromoAdapter.h"
#import "UnityMonetizationDelegate.h"
#import "UnityMonetizationPlacementContentState.h"

NS_ASSUME_NONNULL_BEGIN

@interface UnityMonetization : NSObject
+(void)setDelegate:(id <UnityMonetizationDelegate>)delegate;

+(nullable id <UnityMonetizationDelegate>)getDelegate;

+(BOOL)isReady:(NSString *)placementId;

+(nullable UMONPlacementContent *)getPlacementContent:(NSString *)placementId;
@end

NS_ASSUME_NONNULL_END
