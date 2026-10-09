using System;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Bube {
// Unity IAP (5.x) gerçeklemesi: iki tek seferlik ürün, "Reklamları kaldır" ve "Kalıcı öncelik".
// Mağazaya bağlanınca ürünler ve geçmiş satın almalar çekilir; sahip olunan ürün kendiliğinden
// geri gelir (Google Play). iOS'ta oyuncu Ayarlar › Oyun › "Satın alımları geri yükle" ile ister
// (Apple 3.1.1). Ürün kimlikleri iki mağazada da aynı: `Store.NoAdsProduct`, `Store.PriorityProduct`.
public sealed class UnityStore : IStore {
 static readonly string[] Products = { Store.NoAdsProduct, Store.PriorityProduct };
 StoreController controller;
 bool ready;
 Action<string> pending, restoring;

 [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
 static void Install() {
  if (!Application.isMobilePlatform) return;
  var store = new UnityStore();
  Store.Provider = store;
  store.Start();
 }

 async void Start() {
  try { if (UnityServices.State == ServicesInitializationState.Uninitialized) await UnityServices.InitializeAsync(); }
  catch (Exception e) { Debug.LogWarning("Unity Services could not start: " + e.Message); }
  controller = UnityIAPServices.StoreController();
  controller.OnProductsFetched += _ => { ready = true; controller.FetchPurchases(); };
  controller.OnProductsFetchFailed += f => Debug.LogWarning("IAP products failed: " + f.FailureReason);
  controller.OnPurchasesFetched += orders => {
   foreach (var o in orders.ConfirmedOrders) GrantAll(o);
   foreach (var o in orders.PendingOrders) { GrantAll(o); controller.ConfirmPurchase(o); }
   Restored();
  };
  controller.OnPurchasesFetchFailed += _ => Restored();
  controller.OnPurchasePending += order => { GrantAll(order); controller.ConfirmPurchase(order); Finish(null); };
  controller.OnPurchaseFailed += order => Finish(order.FailureReason == PurchaseFailureReason.UserCancelled ? "store.error.cancelled" : "store.error.failed");
  controller.OnPurchaseDeferred += _ => Finish("store.error.failed");
  try {
   await controller.Connect();
   var list = new List<ProductDefinition>();
   foreach (var id in Products) list.Add(new ProductDefinition(id, ProductType.NonConsumable));
   controller.FetchProducts(list);
  } catch (Exception e) { Debug.LogWarning("IAP connect failed: " + e.Message); }
 }

 public bool Ready => ready;
 public string Price(string product) => controller?.GetProductById(product)?.metadata.localizedPriceString;

 public void Buy(string product, Action<string> done) {
  if (!Ready) { done("store.error.unavailable"); return; }
  if (Store.Owned(product)) { done(null); return; }
  pending = done;
  controller.PurchaseProduct(product);
 }

 public void Restore(Action<string> done) {
  if (!Ready) { done("store.error.unavailable"); return; }
  // Geri yükleme bitince satın almalar yeniden çekilir; sonuç ürünler verildikten sonra söylenir.
  controller.RestoreTransactions((ok, _) => {
   if (!ok) { done("store.error.failed"); return; }
   restoring = done; controller.FetchPurchases();
  });
 }

 static void GrantAll(Order order) {
  foreach (var item in order.CartOrdered.Items()) {
   var id = item.Product?.definition.id;
   if (Array.IndexOf(Products, id) >= 0) Store.Grant(id);
  }
 }
 void Restored() { var done = restoring; restoring = null; done?.Invoke(AdGateway.AdsRemoved || Priority.Owned ? null : "store.notice.nothing"); }
 void Finish(string error) { var done = pending; pending = null; done?.Invoke(error); }
}
}
