using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;
using System;

public class AdMobManager : MonoBehaviour
{
    private string appID = "";
    private string revardedVideoID = "";

    private BannerView bannerView;
    private RewardBasedVideoAd rewardBasedVideo;


    public void InitAdmob(string property)
    {


        appID = "ca-app-pub-6267489793748314~3374953289";
        revardedVideoID = "ca-app-pub-6267489793748314/2214695100"; //no test

        MobileAds.Initialize(appID);

        if (!property.Equals("noads"))
        {
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

    }

    public void RequestBanner()
    {
        AdRequest request;
            request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }


    public void RequestRewardBasedVideo()
    {
        AdRequest request;
            request = new AdRequest.Builder().Build();
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
        RequestRewardBasedVideo();
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