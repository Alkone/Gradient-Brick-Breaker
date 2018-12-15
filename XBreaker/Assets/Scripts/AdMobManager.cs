using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;
using System;

public class AdMobManager : MonoBehaviour
{
    private string appID = "";
    private string bannerID = "";
    private string revardedVideoID = "";
    private string testDevice = "";
    private string testDevice2 = "";
    public bool testMode;
    public bool revarvedVideoIsLoaded;

    private BannerView bannerView;
    private RewardBasedVideoAd rewardBasedVideo;

    // Use this for initialization
    void Start()
    {
        initAdmob();
    }


    void initAdmob()
    {

#if UNITY_IOS
                 appID="";
                 bannerID="";
                 videoID="";

#elif UNITY_ANDROID

        appID = "ca-app-pub-6267489793748314~3374953289";
        bannerID = "ca-app-pub-3940256099942544/6300978111";
        revardedVideoID = "ca-app-pub-3940256099942544/5224354917";
        testDevice = "328A49BA78BFB651575852BEDF075723";
        testDevice2 = "518C032D113D8EF54BC0D4728F79920A";

#endif

        MobileAds.Initialize(appID);


        bannerView = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Bottom);
        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;



        rewardBasedVideo = RewardBasedVideoAd.Instance;
        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        RequestRewardBasedVideo();

    }

    public void RequestBanner()
    {
        AdRequest request;
        if (testMode)
        {
            request = new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice(testDevice2)
            .Build();
        }
        else
        {
            request = new AdRequest.Builder()
            .Build();
        }
        bannerView.LoadAd(request);
    }

    public void DestroyBanner()
    {
        bannerView.Destroy();
    }


    public void RequestRewardBasedVideo()
    {
        AdRequest request;
        if (testMode)
        {
            request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice(testDevice2)
            .Build();
        }
        else
        {
            request = new AdRequest.Builder()
            .Build();
        }
        rewardBasedVideo.LoadAd(request, revardedVideoID);
    }

    public void UserOptToWatchAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
    }

    public bool GetRewardBasedVideoIsLoaded()
    {
        return rewardBasedVideo.IsLoaded();
    }

    //Banner

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }


    //Revarded video

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameManager.instance.StartGame("continue");
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
}