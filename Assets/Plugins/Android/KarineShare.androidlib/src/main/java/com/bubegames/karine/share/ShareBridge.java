package com.bubegames.karine.share;

import android.app.Activity;
import android.content.Intent;
import android.net.Uri;
import androidx.core.content.FileProvider;
import java.io.File;

// Kapanış kartını sistemin paylaşma penceresine verir.
public final class ShareBridge {
    public static void share(final Activity activity, final String path, final String text) {
        activity.runOnUiThread(new Runnable() {
            @Override public void run() {
                Uri uri = FileProvider.getUriForFile(activity, activity.getPackageName() + ".karineshare", new File(path));
                Intent send = new Intent(Intent.ACTION_SEND);
                send.setType("image/png");
                send.putExtra(Intent.EXTRA_STREAM, uri);
                send.putExtra(Intent.EXTRA_TEXT, text);
                send.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
                activity.startActivity(Intent.createChooser(send, null));
            }
        });
    }
}
