#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <StoreKit/StoreKit.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

// Karine iOS köprüsü: reklam takip izni (ATT) ve oyun içi puan isteme.
typedef void (*KarineTrackingCallback)(int status);

extern "C" {
// ATT: 0 belirlenmedi, 1 kısıtlı, 2 reddedildi, 3 izin verildi. iOS 14 öncesi izin sayılır.
void KarineRequestTracking(KarineTrackingCallback callback) {
    if (@available(iOS 14, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            dispatch_async(dispatch_get_main_queue(), ^{ callback((int)status); });
        }];
    } else {
        callback(3);
    }
}

// Apple kendi sınırını uygular (yılda en çok 3 kez); pencere çıkmayabilir.
void KarineRequestReview() {
    if (@available(iOS 14, *)) {
        for (UIScene* scene in UIApplication.sharedApplication.connectedScenes) {
            if ([scene isKindOfClass:[UIWindowScene class]] && scene.activationState == UISceneActivationStateForegroundActive) {
                [SKStoreReviewController requestReviewInScene:(UIWindowScene*)scene];
                return;
            }
        }
    }
    [SKStoreReviewController requestReview];
}
}
