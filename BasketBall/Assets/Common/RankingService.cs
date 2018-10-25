using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if GOOGLE_PLAY
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
public class RankingService : MonoBehaviour {
#if GOOGLE_PLAY
	void Start()
	{
	#if UNITY_ANDROID
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);

		PlayGamesPlatform.DebugLogEnabled = true;

		PlayGamesPlatform.Activate();
	#elif UNITY_IOS
		GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
	#endif
	}
	
	public void SignIn()
	{
		Social.localUser.Authenticate((bool success) =>
			{
				if (success)
				{
					// to do ...
					// 로그인 성공 처리
				}
				else
				{
					// to do ...
					// 로그인 실패 처리
				}
			});
	}

	public void SignOut()
	{
	#if UNITY_ANDROID
		PlayGamesPlatform.Instance.SignOut();
	#endif
	}

	public void UnlockAchievement(int score)
	{
		if (score == 0) {
			return;
		}

		string achivement = "";
		switch(score)
		{
		case 1:
			achivement = GPGSIds.achievement_stone;
			break;
		case 2:
			achivement = GPGSIds.achievement_microorganism;
			break;
		case 3:
			achivement = GPGSIds.achievement_bug;
			break;
		case 4:
			achivement = GPGSIds.achievement_dog;
			break;
		case 5:
			achivement = GPGSIds.achievement_monkey;
			break;
		case 6:
			achivement = GPGSIds.achievement_human;
			break;
		case 7:
			achivement = GPGSIds.achievement_iq_200;
			break;
		case 8:
			achivement = GPGSIds.achievement_progamer;
			break;
		case 9:
			achivement = GPGSIds.achievement_super_human;
			break;
		case 10:
			achivement = GPGSIds.achievement_god;
			break;
		default:
			achivement = GPGSIds.achievement_stone;
			break;
		}

	#if UNITY_ANDROID
		PlayGamesPlatform.Instance.ReportProgress(achivement, 100f, null);
	#elif UNITY_IOS
		Social.ReportProgress("Score_100", 100f, null);
	#endif
	}

	public void ShowAchievementUI()
	{
		// Sign In 이 되어있지 않은 상태라면
		// Sign In 후 업적 UI 표시 요청할 것
		if (Social.localUser.authenticated == false) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					// Sign In 성공
					// 바로 업적 UI 표시 요청
					Social.ShowAchievementsUI ();
					return;
				} else {
					// Sign In 실패 처리
					return;
				}
			});
		} else {
			Social.ShowAchievementsUI ();
		}
	}

	public void ReportScore(int score)
	{
	#if UNITY_ANDROID

		PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_world_ranking, (bool success) =>
			{
				if (success)
				{
					// Report 성공
					// 그에 따른 처리
				}
				else
				{
					// Report 실패
					// 그에 따른 처리
				}
			});

	#elif UNITY_IOS

		Social.ReportScore(score, "Leaderboard_ID", (bool success) =>
		{
		if (success)
		{
		// Report 성공
		// 그에 따른 처리
		}
		else
		{
		// Report 실패
		// 그에 따른 처리
		}
		});

	#endif
	}

	public void ShowLeaderboardUI()
	{
		// Sign In 이 되어있지 않은 상태라면
		// Sign In 후 리더보드 UI 표시 요청할 것
		if (Social.localUser.authenticated == false) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					// Sign In 성공
					// 바로 리더보드 UI 표시 요청
					Social.ShowLeaderboardUI ();
					return;
				} else {
					// Sign In 실패 
					// 그에 따른 처리
					return;
				}
			});
		} else {
		#if UNITY_ANDROID
			PlayGamesPlatform.Instance.ShowLeaderboardUI ();
		#elif UNITY_IOS
			GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
		#endif
		}
	}
#endif
}
