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

// Kapanış kartı: sistemin paylaşma penceresi. iPad'de pencere ekranın ortasından açılır.
void KarineShare(const char* path, const char* text) {
    NSString* file = [NSString stringWithUTF8String:path];
    NSString* caption = [NSString stringWithUTF8String:text];
    NSMutableArray* items = [NSMutableArray arrayWithObject:caption];
    UIImage* image = [UIImage imageWithContentsOfFile:file];
    if (image) [items insertObject:image atIndex:0];
    UIViewController* top = UIApplication.sharedApplication.keyWindow.rootViewController;
    while (top.presentedViewController) top = top.presentedViewController;
    UIActivityViewController* sheet = [[UIActivityViewController alloc] initWithActivityItems:items applicationActivities:nil];
    if (sheet.popoverPresentationController) {
        sheet.popoverPresentationController.sourceView = top.view;
        sheet.popoverPresentationController.sourceRect = CGRectMake(CGRectGetMidX(top.view.bounds), CGRectGetMidY(top.view.bounds), 0, 0);
        sheet.popoverPresentationController.permittedArrowDirections = 0;
    }
    [top presentViewController:sheet animated:YES completion:nil];
}
}
