NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, UnityMonetizationPlacementContentState) {
    kPlacementContentStateUndecided,
    kPlacementContentStateConsumed,
    kPlacementContentStateIgnored,
    kPlacementContentStateWaiting,
    kPlacementContentStateReady
};

NSString *NSStringFromPlacementContentState(UnityMonetizationPlacementContentState);

UnityMonetizationPlacementContentState PlacementContentStateFromNSString(NSString *);

NS_ASSUME_NONNULL_END
