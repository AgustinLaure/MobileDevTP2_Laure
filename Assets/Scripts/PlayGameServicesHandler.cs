using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class PlayGameServicesHandler
{
    public bool IsSignedIn { get; private set; }

    private const float maxProgress = 100f;

    public PlayGameServicesHandler()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = Debug.isDebugBuild;
        PlayGamesPlatform.Activate();
        SignIn();
#endif
    }

    public void SignIn()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(status =>
        {
            IsSignedIn = status == SignInStatus.Success;

            if (!IsSignedIn)
            {
                Debug.Log("Failed to authenticate");
            }
        });
#endif
    }

    public void UnlockAchievement(string achievementId)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportProgress(achievementId, maxProgress, success =>
        {
            if (!success)
            {
                Debug.Log("Failed to unlock achievement");
            }
        });
#endif
    }

    public void IncrementAchievement(string achievementId, int amount)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.IncrementAchievement(achievementId, amount, success =>
        {
            if (!success)
            {
                Debug.Log("Failed to increment achievement");
            }
        });
#endif
    }

    public void ShowAchievements()
    {
#if UNITY_ANDROID
        if (!IsSignedIn)
        {
            SignInManually();
            return;
        }

        PlayGamesPlatform.Instance.ShowAchievementsUI();
#endif
    }

    public void SignInManually()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ManuallyAuthenticate(status =>
        {
            IsSignedIn = status == SignInStatus.Success;
            Debug.Log("GPGS manual login: " + status);

            if (!IsSignedIn)
            {
                Debug.LogWarning("Failed to manually authenticate");
            }
        });
#endif
    }

    public void IncrementScore(string leaderboardId, long score)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportScore(score, leaderboardId, success =>
        {
            if (!success)
            {
                Debug.LogWarning("Failed to report score");
            }
        });
#endif
    }

    public void ShowLeaderboard(string leaderboardId)
    {
#if UNITY_ANDROID
        if (!IsSignedIn)
        {
            SignInManually();
            return;
        }

        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
#endif
    }
}