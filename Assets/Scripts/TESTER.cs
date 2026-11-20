using UnityEngine;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class TESTER : MonoBehaviour
{
#if UNITY_ANDROID
    private bool isSignedIn = false;

    private void Awake()
    {
        Debug.Log(GPGSIds.achievement_test);
    }

    private void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    void ProcessAuthentication(SignInStatus status)
    {
        isSignedIn = status == SignInStatus.Success;
        Debug.Log("GPGS login: " + status);

        if (isSignedIn)
        {
            SendFirstAchievement();
        }
        // Si falló, mostrá un botón "Iniciar sesión" que llame a SignInManually()
    }

    public void SendFirstAchievement()
    {
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_test, 5, success =>
        {
            Debug.Log("IncrementAchievement: " + success);
        });
    }

    public void SignInManually()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }
#endif
}
