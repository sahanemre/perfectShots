using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsDm
{
    public bool isTesting = false;
    private IAdsPlatformInfo platformInfo;

    public void InitAdsDm()
    {
#if UNITY_IPHONE
            platformInfo = new AdsIosInfo();
#else
        platformInfo = new AdsAndroidInfo();
#endif

        var appId = platformInfo.AppId;
        MobileAds.SetiOSAppPauseOnBackground(true);
    }

    public string GetRewardedAdId()
    {
        if (isTesting)
        {
           return platformInfo.TestAdIdRewarded;
        }
        else
        {
           return platformInfo.AdIdRewarded;
        }
    }

    public string GetInterstitialAdId()
    {
        if (isTesting)
        {
            return platformInfo.TestAdIdInterstitial;
        }
        else
        {
            return platformInfo.AdIdInterstitial;
        }
    }

    public string GetBannerAdId()
    {
        if (isTesting)
        {
            return platformInfo.TestAdIdBanner;
        }
        else
        {
            return platformInfo.AdIdBanner;
        }
    }
}
