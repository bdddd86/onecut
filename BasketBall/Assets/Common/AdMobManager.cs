using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
	public string android_banner_id;
	public string ios_banner_id;

	public string android_interstitial_id;
	public string ios_interstitial_id;

	public string android_reward_id;

	private BannerView bannerView;
	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardAd;

	public void Start()
	{
		RequestBannerAd();
		RequestInterstitialAd();

		ShowBannerAd();
	}

	public void RequestBannerAd()
	{
		string adUnitId = string.Empty;

		#if UNITY_ANDROID
		adUnitId = android_banner_id;
		#elif UNITY_IOS
		adUnitId = ios_bannerAdUnitId;
		#endif

		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
		AdRequest request = new AdRequest.Builder().Build();

		bannerView.LoadAd(request);
	}

	private void RequestInterstitialAd()
	{
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
	}

	private void RequestRewardAd()
	{
	}

	public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
	{
		print("HandleOnInterstitialAdClosed event received.");

		interstitialAd.Destroy();

		RequestInterstitialAd();
	}

	public void ShowBannerAd()
	{
		bannerView.Show();
	}

	public void ShowInterstitialAd()
	{
		if (!interstitialAd.IsLoaded())
		{
			RequestInterstitialAd();
			return;
		}

		interstitialAd.Show();
	}

}