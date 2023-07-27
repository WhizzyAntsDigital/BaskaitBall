using Unity.Services.Authentication;
using UnityEngine;
using System;
[RequireComponent(typeof(AnonymousSignIn), typeof(GPManager))]
public class AuthenticatorManager : MonoBehaviour
{
    public static AuthenticatorManager Instance { get; private set; }
    private AnonymousSignIn anonymousSignIn;
    private GPManager googleSignInManager;
    private String userId;
    public Action OnLoggedInCompleted;
    private void Awake()
    {
        Instance = this;
        print("called authentication");
        anonymousSignIn = GetComponent<AnonymousSignIn>();
        googleSignInManager = GetComponent<GPManager>();
        anonymousSignIn.OnAnonymouslyLoggedIn += OnLoggedIn;
        googleSignInManager.OnGoogleLoginComplete += OnLoggedIn;
    }
    private void OnLoggedIn()
    {
        userId = AuthenticationService.Instance.PlayerId;
        OnLoggedInCompleted?.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        anonymousSignIn.Init();
        anonymousSignIn.SignInAnonymouslyAsync();
#elif UNITY_ANDROID
        googleSignInManager.Init();
       googleSignInManager.LoginGooglePlayGames();
#endif
    }
    public string GetUserID()
    {
        return userId;
    }
}