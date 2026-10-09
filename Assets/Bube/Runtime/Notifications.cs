using System;
using UnityEngine;

namespace Bube {

// Yerel bildirim dikişi. Sunucu yok, bildirim cihazın kendi takvimine kurulur:
// oyuncu arkaya atınca kurulur, geri gelince silinir. Gerçeklemesi `Bube.Notify`
// derlemesinde (com.unity.mobile.notifications); paket yoksa hiçbir şey olmaz.
public interface INotifier {
 void RequestPermission(Action<bool> done);
 void Schedule(int id, string title, string body, TimeSpan after);
 void CancelAll();
}

public sealed class NoNotifier : INotifier {
 public void RequestPermission(Action<bool> done) => done(false);
 public void Schedule(int id, string title, string body, TimeSpan after) {}
 public void CancelAll() {}
}

public static class Notifications {
 const string EnabledKey = "karine.notify.enabled", AskedKey = "karine.notify.asked";
 public static INotifier Provider = new NoNotifier();
 // Oyuncu bir kez izin verdiyse açık; ayarlardan kapatabilir. Cihaz ayarıdır, buluta gitmez.
 public static bool Enabled { get => PlayerPrefs.GetInt(EnabledKey, 0) == 1; set { PlayerPrefs.SetInt(EnabledKey, value ? 1 : 0); PlayerPrefs.Save(); } }
 public static bool Asked { get => PlayerPrefs.GetInt(AskedKey, 0) == 1; set { PlayerPrefs.SetInt(AskedKey, value ? 1 : 0); PlayerPrefs.Save(); } }
 public static void Ask(Action<bool> done = null) {
  Asked = true;
  Provider.RequestPermission(granted => { Enabled = granted; done?.Invoke(granted); });
 }
}
}
