//
//  KeyChainStore.h
//  Unity-iPhone
//
//  Created by 曾飞 on 2019/2/26.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface KeyChainStore : NSObject
+ (void)save:(NSString*)service data:(id)data; // 保存
+ (id)load:(NSString*)service;                 // 获取
+ (void)deleteKeyData:(NSString*)service;      // 删除
@end

NS_ASSUME_NONNULL_END
