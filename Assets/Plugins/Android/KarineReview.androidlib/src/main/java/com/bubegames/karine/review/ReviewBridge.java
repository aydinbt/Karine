package com.bubegames.karine.review;

import android.app.Activity;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.android.play.core.review.ReviewInfo;
import com.google.android.play.core.review.ReviewManager;
import com.google.android.play.core.review.ReviewManagerFactory;

// Google Play oyun içi değerlendirme. Google kendi kotasını uygular; pencere
// çıkmayabilir ve oyun bunun sonucunu bilmez (bilmemelidir).
public final class ReviewBridge {
    public static void request(final Activity activity) {
        try {
            final ReviewManager manager = ReviewManagerFactory.create(activity);
            manager.requestReviewFlow().addOnCompleteListener(new OnCompleteListener<ReviewInfo>() {
                @Override public void onComplete(Task<ReviewInfo> task) {
                    if (task.isSuccessful()) manager.launchReviewFlow(activity, task.getResult());
                }
            });
        } catch (Exception ignored) {}
    }
}
