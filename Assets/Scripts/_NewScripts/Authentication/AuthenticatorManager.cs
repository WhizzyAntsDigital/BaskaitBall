using Unity.Services.Authentication;
using UnityEngine;
using System;
using Unity.Services.Core;
using System.Threading.Tasks;

[RequireComponent(typeof(AnonymousSignIn), typeof(GPManager))]
public class AuthenticatorManager : MonoBehaviour
{
    public static AuthenticatorManager Instance { get; private set; }
    private AnonymousSignIn anonymousSignIn;
    private GPManager googleSignInManager;
    public Action OnLoggedInCompleted;
    private void Awake()
    {
        Instance = this;
        HelperClass.DebugMessage("called authentication");
        anonymousSignIn = GetComponent<AnonymousSignIn>();
        googleSignInManager = GetComponent<GPManager>();
        anonymousSignIn.OnAnonymouslyLoggedIn += OnLoggedIn;
        googleSignInManager.OnGoogleLoginComplete += OnLoggedIn;
        UnityServices.InitializeAsync();
    }
    private void OnLoggedIn()
    {
        OnLoggedInCompleted?.Invoke();
    }
    void Start()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
#if UNITY_EDITOR
            anonymousSignIn.SignInAnonymouslyAsync();
#elif UNITY_ANDROID
        googleSignInManager.Init();
       googleSignInManager.LoginGooglePlayGames();
#endif
        }
        else
        {
            Invoke(HelperClass.GetActionName(InvokeOnCompleteStuff), 1f);
        }
    }
    private void InvokeOnCompleteStuff()
    {
        OnLoggedInCompleted?.Invoke();
    }
}