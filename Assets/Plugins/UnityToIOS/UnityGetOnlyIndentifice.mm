//
//  UnityGetOnlyIndentifice.m
//  Unity-iPhone
//
//  Created by 曾飞 on 2019/2/26.
//

#import "UnityGetOnlyIndentifice.h"
#import "KeyChainStore.h"
#import <AdSupport/AdSupport.h>
@implementation UnityGetOnlyIndentifice

+ (NSString *)getUUIDByKeyChain{
    // 这个key的前缀最好是你的BundleID
    NSString*strUUID = (NSString*)[KeyChainStore load:@"com.Company.ProductName"];
    //首次执行该方法时，uuid为空
    if([strUUID isEqualToString:@""]|| !strUUID)
    {
        // 获取UUID 这个是要引入<AdSupport/AdSupport.h>的
        strUUID = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
        
        if(strUUID.length ==0 || [strUUID isEqualToString:@"00000000-0000-0000-0000-000000000000"])
        {
            //生成一个uuid的方法
            CFUUIDRef uuidRef= CFUUIDCreate(kCFAllocatorDefault);
            strUUID = (NSString*)CFBridgingRelease(CFUUIDCreateString(kCFAllocatorDefault,uuidRef));
            CFRelease(uuidRef);
        }
        
        //将该uuid保存到keychain
        [KeyChainStore save:@"com.Company.ProductName" data:strUUID];
    }
    return strUUID;

}
@end
#if defined(__cplusplus)
extern "C" {
#endif
    char * MakeStringCopy (const char  *string)
    {
        if (NULL == string) {
            return NULL;
        }
        char *res = (char *)malloc(strlen(string) + 1);
        strcpy(res, string);
        return res;
    }
    //导出接口供unity使用
    char * IOS_GetUUID(){
        const char *uuid = [[UnityGetOnlyIndentifice getUUIDByKeyChain] UTF8String];
        return MakeStringCopy(uuid);
    }
    
    
#if defined(__cplusplus)
}
#endif
