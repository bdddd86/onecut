#define ADMOB
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
	private string android_banner_test = "ca-app-pub-3940256099942544/6300978111";
	private string android_banner_id = "ca-app-pub-1808701058779760/9805259218";
	private string ios_banner_id;

	private string android_interstitial_id = "ca-app-pub-1808701058779760/4932206483";
	private string ios_interstitial_id;

	private string android_reward_id = "ca-app-pub-1808701058779760/6721174143";
	#if ADMOB
	private BannerView bannerView;
	private InterstitialAd interstitialAd;
	private RewardBasedVideoAd rewardAd;
	#endif
	public void Start()
	{
		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize("ca-app-pub-1808701058779760~1000824040");
		Debug.Log ("### Initialize");
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

		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
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

		AdRequest request = new AdRequest.Builder().AddTestDevice("60AD33567E81B9E5A1E848B1A8553993").Build();

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
		AdRequest request = new AdRequest.Builder().AddTestDevice("60AD33567E81B9E5A1E848B1A8553993").Build();

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
		Debug.Log("HandleOnInterstitialAdClosed event received.");

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

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("HandleFailedToReceiveAd event received with message: "
			+ args.Message);

		if (args.Message.CompareTo ("No fill") == 0) {
			//this.RequestBannerAd ();
		}
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		Debug.Log("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		Debug.Log("HandleAdClosed event received");
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLeavingApplication event received");
	}
}