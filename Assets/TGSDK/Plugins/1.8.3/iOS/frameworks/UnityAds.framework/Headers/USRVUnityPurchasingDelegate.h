#import "UPURProduct.h"
#import "UPURTransactionDetails.h"
#import "UPURTransactionError.h"

NS_ASSUME_NONNULL_BEGIN

typedef void (^UnityPurchasingLoadProductsCompletionHandler)(NSArray<UPURProduct*>*);
typedef void (^UnityPurchasingTransactionCompletionHandler)(UPURTransactionDetails*);
typedef void (^UnityPurchasingTransactionErrorHandler)(UPURTransactionError, NSException*);

@protocol USRVUnityPurchasingDelegate
-(void)loadProducts:(UnityPurchasingLoadProductsCompletionHandler)completionHandler;
-(void)purchaseProduct:(NSString *)productId
     completionHandler:(UnityPurchasingTransactionCompletionHandler)completionHandler
          errorHandler:(UnityPurchasingTransactionErrorHandler)errorHandler
              userInfo:(nullable NSDictionary *)extras;
@end

NS_ASSUME_NONNULL_END
