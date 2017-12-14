using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class SocialFeatures : MonoBehaviour
{

    void Start()
    {
        // Authenticate and register a ProcessAuthentication callback
        // This call needs to be made before we can proceed to other calls in the Social API
        
		#if UNITY_ANDROID

		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
		#endif
		Social.localUser.Authenticate (ProcessAuthentication);
    }

    // This function gets called when Authenticate completes
    // Note that if the operation is successful, Social.localUser will contain data from the server. 
    void ProcessAuthentication(bool success)
    {
        if (success) DontDestroyOnLoad(gameObject);
    }

    public static void ShowLeaderboard()
    {
        if (Social.localUser.authenticated) Social.ShowLeaderboardUI();
    }

    public static void ReportLeaderboard(long score, string leaderboard)
    {
        if (Social.localUser.authenticated) Social.ReportScore(score, leaderboard, success => { });
    }

}
// Copy and pasted from https://blog.beanpolelabs.com/how-to-add-google-play-games-and-game-center-to-your-unity-game-3edd5666f144