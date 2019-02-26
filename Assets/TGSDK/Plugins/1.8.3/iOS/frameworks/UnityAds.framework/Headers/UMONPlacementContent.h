#import "UnityMonetizationPlacementContentState.h"

NS_ASSUME_NONNULL_BEGIN

@interface UMONPlacementContent : NSObject
-(instancetype)initWithPlacementId:(NSString *)placementId withParams:(NSDictionary *)params;

@property(nonatomic, readonly, getter=isConsumed) BOOL consumed;
@property(nonatomic, readonly, getter=isIgnored) BOOL ignored;
@property(nonatomic, readonly, getter=isReady) BOOL ready;
@property(nonatomic, readonly) NSString *type;
@property(retain, nonatomic, readonly) NSString *placementId;
@property(nonatomic) UnityMonetizationPlacementContentState state;
@property(nonatomic) NSDictionary *userInfo;

-(void)consume;

-(void)ignore;

-(void)sendCustomEvent:(NSDictionary *)eventData;
@end

NS_ASSUME_NONNULL_END
