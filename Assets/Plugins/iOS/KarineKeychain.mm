#import <Foundation/Foundation.h>
#import <Security/Security.h>

// Karine misafir anahtarı: Keychain kaydı uygulama silinince silinmez; aynı
// iPhone'da yeniden kurulumda aynı misafir döner. iCloud Keychain'e gitmez.
static NSMutableDictionary* KarineQuery(const char* key) {
    return [@{ (__bridge id)kSecClass: (__bridge id)kSecClassGenericPassword,
               (__bridge id)kSecAttrService: @"com.bubegames.karine",
               (__bridge id)kSecAttrAccount: [NSString stringWithUTF8String:key] } mutableCopy];
}

extern "C" {
const char* KarineKeychainRead(const char* key) {
    NSMutableDictionary* query = KarineQuery(key);
    query[(__bridge id)kSecReturnData] = @YES;
    query[(__bridge id)kSecMatchLimit] = (__bridge id)kSecMatchLimitOne;
    CFTypeRef result = NULL;
    if (SecItemCopyMatching((__bridge CFDictionaryRef)query, &result) != errSecSuccess || result == NULL) return NULL;
    NSData* data = (__bridge_transfer NSData*)result;
    NSString* text = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    return text == nil ? NULL : strdup([text UTF8String]); // Unity serbest bırakır.
}

void KarineKeychainWrite(const char* key, const char* value) {
    NSMutableDictionary* query = KarineQuery(key);
    SecItemDelete((__bridge CFDictionaryRef)query);
    query[(__bridge id)kSecValueData] = [[NSString stringWithUTF8String:value] dataUsingEncoding:NSUTF8StringEncoding];
    query[(__bridge id)kSecAttrAccessible] = (__bridge id)kSecAttrAccessibleAfterFirstUnlockThisDeviceOnly;
    SecItemAdd((__bridge CFDictionaryRef)query, NULL);
}

void KarineKeychainDelete(const char* key) {
    SecItemDelete((__bridge CFDictionaryRef)KarineQuery(key));
}
}
