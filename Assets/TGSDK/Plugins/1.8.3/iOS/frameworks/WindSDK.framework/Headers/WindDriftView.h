//
//  WindDriftView.h
//  WindSDK
//
//  Created by happyelements on 2018/10/19.
//  Copyright © 2018 Codi. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol WindDriftAdDelegate;

@interface WindDriftView : UIView

+ (instancetype)adViewWithFrame:(CGRect)frame
                    placementId:(NSString *)placementId
          presentViewController:(UIViewController *)viewController
                       delegate:(id<WindDriftAdDelegate>)delegate;
@end
