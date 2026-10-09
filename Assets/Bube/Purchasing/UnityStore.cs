using System;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Bube {
// Unity IAP (4.x) gerçeklemesi: tek ürün, tek seferlik "Reklamları kaldır".
// Mağaza açılınca makbuzu olan ürün kendiliğinden geri gelir (Google Play);
// iOS'ta oyuncu Ayarlar › Oyun › "Satın alımları geri yükle" ile ister (Apple 3.1.1).
// Ürün kimliği iki mağazada da aynı: `Store.NoAdsProduct`.
public sealed class UnityStore : IStore, IDetailedStoreListener {
 IStoreController controller;
 IExtensionProvider extensions;
 Action<string> pending;

 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static async void Install() {
  if (!Application.isMobilePlatform) return;
  var store = new UnityStore();
  Store.Provider = store;
  try { if (UnityServices.State == ServicesInitializationState.Uninitialized) await UnityServices.InitializeAsync(); }
  catch (Exception e) { Debug.LogWarning("Unity Services could not start: " + e.Message); }
  var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
  builder.AddProduct(Store.NoAdsProduct, ProductType.NonConsumable);
  UnityPurchasing.Initialize(store, builder);
 }

 public bool Ready => controller != null;
 public string NoAdsPrice => controller?.products.WithID(Store.NoAdsProduct)?.metadata.localizedPriceString;

 public void BuyNoAds(Action<string> done) {
  if (!Ready) { done("store.error.unavailable"); return; }
  if (AdGateway.AdsRemoved) { done(null); return; }
  pending = done;
  controller.InitiatePurchase(Store.NoAdsProduct);
 }

 public void Restore(Action<string> done) {
  if (!Ready) { done("store.error.unavailable"); return; }
  Action<bool,string> finished = (ok, _) => done(ok ? (AdGateway.AdsRemoved ? null : "store.notice.nothing") : "store.error.failed");
#if UNITY_IOS
  extensions.GetExtension<IAppleExtensions>().RestoreTransactions(finished);
#else
  extensions.GetExtension<IGooglePlayStoreExtensions>().RestoreTransactions(finished);
#endif
 }

 public void OnInitialized(IStoreController c, IExtensionProvider e) {
  controller = c; extensions = e;
  var product = c.products.WithID(Store.NoAdsProduct);
  if (product != null && product.hasReceipt) AdGateway.SetAdsRemoved(true);
 }
 public void OnInitializeFailed(InitializationFailureReason error) => Debug.LogWarning("IAP init failed: " + error);
 public void OnInitializeFailed(InitializationFailureReason error, string message) => Debug.LogWarning("IAP init failed: " + error + " " + message);

 public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
  if (args.purchasedProduct.definition.id == Store.NoAdsProduct) AdGateway.SetAdsRemoved(true);
  Finish(null);
  return PurchaseProcessingResult.Complete;
 }
 public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) =>
  Finish(reason == PurchaseFailureReason.UserCancelled ? "store.error.cancelled" : "store.error.failed");
 public void OnPurchaseFailed(Product product, PurchaseFailureDescription description) =>
  Finish(description.reason == PurchaseFailureReason.UserCancelled ? "store.error.cancelled" : "store.error.failed");

 void Finish(string error) { var done = pending; pending = null; done?.Invoke(error); }
}
}
