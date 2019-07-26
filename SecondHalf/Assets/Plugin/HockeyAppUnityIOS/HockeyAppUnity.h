#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C" {
#endif
  
  void UnitySendMessage(const char *obj, const char *method, const char *msg);
  
#ifdef __cplusplus
}
#endif

@interface HockeyAppUnity : NSObject

#pragma mark - Setup SDK

+ (void)startManagerWithIdentifier:(NSString *)appIdentifier
                         serverURL:(NSString *)serverURL
                          authType:(NSString *)authType
                            secret:(NSString *)secret
              updateManagerEnabled:(BOOL)updateManagerEnabled
                userMetricsEnabled:(BOOL)userMetricsEnabled
                   autoSendEnabled:(BOOL)autoSendEnabled;
+ (BOOL)handleOpenURL:(NSURL *)url
    sourceApplication:(NSString *)sourceApplication
           annotation:(id)annotation;

#pragma mark - SDK features

+ (void)checkForUpdate;
+ (NSString *)versionCode;
+ (NSString *)versionName;
+ (NSString *)bundleIdentifier;
+ (NSString *)sdkVersion;
+ (NSString *)sdkName;
+ (NSString *)crashReporterKey;

@end
