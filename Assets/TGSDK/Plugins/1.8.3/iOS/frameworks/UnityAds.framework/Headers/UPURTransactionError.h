
NS_ASSUME_NONNULL_BEGIN

typedef NS_ENUM(NSInteger, UPURTransactionError) {
    kUPURTransactionErrorNotSupported,
    kUPURTransactionErrorItemUnavailable,
    kUPURTransactionErrorUserCancelled,
    kUPURTransactionErrorNetworkError,
    kUPURTransactionErrorServerError,
    kUPURTransactionErrorUnknownError
};
NSString *NSStringFromUPURTransactionError(UPURTransactionError);

NS_ASSUME_NONNULL_END
