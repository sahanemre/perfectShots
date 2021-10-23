using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private AdsDm dm;

    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardBasedVideoAd rewardBasedVideo;
    private RewardedAd rewardedAd;
    bool isRewarded;

    public static AdManager instance;

    GameManager gameManager;

    Action onLevelComplete;
    private bool isShowing;
    private bool adCloseFlag;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {
        dm = new AdsDm();
        dm.InitAdsDm();

        MobileAds.Initialize(InitializationStatus => { });
        //Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;


        //RewardBasedVideoAd is a singleton, so handlers should only be registered onceü
        this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
        this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;


        this.RequestRewardedAd();

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += this.HandleRewardedAdLoaded;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += this.HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += this.HandleRewardedAdClosed;

        this.RequestRewardBasedVideo();
        this.RequestBanner();
    }

    private void Update()
    {
        if (isRewarded)
        {
            isRewarded = false;
            //FindObjectOfType<ShopManager>().UnlockCarReward();
            //give reward
        }

        //if (this.adCloseFlag == true)
        //{
        //    this.adCloseFlag = false;
        //    StartCoroutine(CallGoOnCoroutine());
        //}
    }


    private void RequestBanner()
    {
        this.bannerAd = new BannerView(dm.GetBannerAdId(), AdSize.Banner, AdPosition.Bottom);
        this.bannerAd.LoadAd(CreateRequest());
    }

    private void RequestInterstitial()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }


        //Create an interstitial
        this.interstitialAd = new InterstitialAd(dm.GetInterstitialAdId());


        //Load an interstitial ad
        this.interstitialAd.LoadAd(this.CreateRequest());
    }

    private void RequestRewardedAd()
    {
        this.rewardedAd = new RewardedAd(dm.GetRewardedAdId());

        this.rewardedAd.LoadAd(this.CreateRequest());
    }

    public void ShowInterstitial()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstital Ad is not ready yet");
        }
    }

    public void RequestRewardBasedVideo()
    {
        this.rewardBasedVideo.LoadAd(this.CreateRequest(),dm.GetRewardedAdId());
    }

    public void ShowRewardBasedVideo()
    {
        if (this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
        else
        {
            Debug.Log("reward ad is not ready yet");
        }
    }

    public void ShowRewarded(Action onComplete)
    {
        this.ShowAd(onComplete);

    }

    private void ShowAd(Action onComplete = null)
    {
        if (this.rewardedAd.IsLoaded())
        {
            onLevelComplete = onComplete;
            isShowing = true;
            adCloseFlag = false;
            this.rewardedAd.Show();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
    private AdRequest CreateRequest()
    {
        return new AdRequest.Builder().Build();
    }


    private void HandleRewardBasedVideoClosed(object sender, EventArgs e)
    {
        this.RequestRewardBasedVideo();
    }

    private void HandleRewardBasedVideoRewarded(object sender, Reward e)
    {
        isRewarded = true;
    }


    //HandleRewarded
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //isShowing = false;
        //CallGoOnAction();
        RequestRewardedAd();
        //MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        isRewarded = true;
    }

    public void CallGoOnAction()
    {
        adCloseFlag = true;
    }
    private IEnumerator CallGoOnCoroutine()
    {
        yield return new WaitForEndOfFrame();
        onLevelComplete?.Invoke();
        onLevelComplete = null;
    }
}
