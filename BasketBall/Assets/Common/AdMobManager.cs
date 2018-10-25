﻿#define ADMOB
using UnityEngine;
using System;
#if ADMOB
using GoogleMobileAds.Api;
#endif

//배너 
//ca-app-pub-1808701058779760/9805259218
//풀스크린
//ca-app-pub-1808701058779760/4932206483
//리워드
//ca-app-pub-1808701058779760/6721174143

public class AdMobManager : MonoBehaviour
{
	public string android_banner_id = "ca-app-pub-1808701058779760/9805259218";
	public string ios_banner_id;

	public string android_interstitial_id = "ca-app-pub-1808701058779760/4932206483";
	public string ios_interstitial_id;

	public string android_reward_id = "ca-app-pub-1808701058779760/6721174143";
	#if ADMOB
	private BannerView bannerView;
	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardAd;
	#endif
	public void Start()
	{
		RequestBannerAd();
		RequestInterstitialAd();

		ShowBannerAd();
	}

	public void RequestBannerAd()
	{
		#if ADMOB
		string adUnitId = string.Empty;

		#if UNITY_ANDROID
		adUnitId = android_banner_id;
		#elif UNITY_IOS
		adUnitId = ios_bannerAdUnitId;
		#endif

		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		AdRequest request = new AdRequest.Builder().Build();

		bannerView.LoadAd(request);
		#endif
	}

	private void RequestInterstitialAd()
	{
		#if ADMOB
		string adUnitId = string.Empty;

		#if UNITY_ANDROID
		adUnitId = android_interstitial_id;
		#elif UNITY_IOS
		adUnitId = ios_interstitialAdUnitId;
		#endif

		interstitialAd = new InterstitialAd(adUnitId);
		AdRequest request = new AdRequest.Builder().Build();

		interstitialAd.LoadAd(request);

		interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
		#endif
	}

	private void RequestRewardAd()
	{
	}

	public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
	{
		#if ADMOB
		print("HandleOnInterstitialAdClosed event received.");

		interstitialAd.Destroy();

		RequestInterstitialAd();
		#endif
	}

	public void ShowBannerAd()
	{
		#if ADMOB
		bannerView.Show();
		#endif
	}

	public void ShowInterstitialAd()
	{
		#if ADMOB
		if (!interstitialAd.IsLoaded())
		{
			RequestInterstitialAd();
			return;
		}

		interstitialAd.Show();
		#endif
	}

}