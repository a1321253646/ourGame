#import "UPURTransactionDetails.h"
#import "UMONPlacementContent.h"
#import "UMONPromoAdPlacementContent.h"

typedef NS_ENUM(NSInteger, UMONNativePromoShowType) {
    kNativePromoShowTypePreview,
    kNativePromoShowTypeFull
};

@interface UMONNativePromoAdapter : NSObject
-(instancetype)initWithPromo:(UMONPromoAdPlacementContent *)promo;

-(void)promoDidShow;

-(void)promoDidShow:(UMONNativePromoShowType)showType;

-(void)promoDidPurchase:(UPURTransactionDetails *)details;

-(void)promoDidClose;

-(UMONPromoMetaData *)metadata;
@end