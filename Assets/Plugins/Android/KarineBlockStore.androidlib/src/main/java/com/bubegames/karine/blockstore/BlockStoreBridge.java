package com.bubegames.karine.blockstore;

import android.content.Context;
import com.google.android.gms.auth.blockstore.Blockstore;
import com.google.android.gms.auth.blockstore.BlockstoreClient;
import com.google.android.gms.auth.blockstore.DeleteBytesRequest;
import com.google.android.gms.auth.blockstore.RetrieveBytesRequest;
import com.google.android.gms.auth.blockstore.RetrieveBytesResponse;
import com.google.android.gms.auth.blockstore.StoreBytesData;
import com.google.android.gms.tasks.OnFailureListener;
import com.google.android.gms.tasks.OnSuccessListener;
import java.nio.charset.StandardCharsets;
import java.util.Collections;

// Karine misafir anahtarı: Google Block Store uygulama silinip yeniden kurulsa da
// (ve aynı Google hesabıyla açılan yeni telefonda) anahtarı geri verir.
// Sonuç Unity'ye Callback ile döner; C# tarafı ana iş parçacığına aktarır.
public final class BlockStoreBridge {
    public interface Callback { void done(String value); }

    public static void read(Context context, final String key, final Callback callback) {
        try {
            BlockstoreClient client = Blockstore.getClient(context);
            RetrieveBytesRequest request = new RetrieveBytesRequest.Builder().setKeys(Collections.singletonList(key)).build();
            client.retrieveBytes(request).addOnSuccessListener(new OnSuccessListener<RetrieveBytesResponse>() {
                @Override public void onSuccess(RetrieveBytesResponse response) {
                    RetrieveBytesResponse.BlockstoreData data = response.getBlockstoreDataMap().get(key);
                    callback.done(data == null ? null : new String(data.getBytes(), StandardCharsets.UTF_8));
                }
            }).addOnFailureListener(new OnFailureListener() {
                @Override public void onFailure(Exception e) { callback.done(null); }
            });
        } catch (Exception e) { callback.done(null); }
    }

    public static void write(Context context, String key, String value) {
        try {
            StoreBytesData data = new StoreBytesData.Builder()
                .setKey(key).setBytes(value.getBytes(StandardCharsets.UTF_8)).setShouldBackupToCloud(true).build();
            Blockstore.getClient(context).storeBytes(data);
        } catch (Exception ignored) {}
    }

    public static void delete(Context context, String key) {
        try {
            DeleteBytesRequest request = new DeleteBytesRequest.Builder().setKeys(Collections.singletonList(key)).build();
            Blockstore.getClient(context).deleteBytes(request);
        } catch (Exception ignored) {}
    }
}
