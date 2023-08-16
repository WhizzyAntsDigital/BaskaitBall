using Unity.Services.Authentication;
using UnityEngine;
using System;
using Unity.Services.Core;
using System.Threading.Tasks;

[RequireComponent(typeof(AnonymousSignIn), typeof(AppleAuthentication))]
public class AuthenticatorManager : MonoBehaviour
{

    [SerializeField] private GameObject noInternetPanel;

    public static AuthenticatorManager Instance { get; private set; }
    private AnonymousSignIn anonymousSignIn;
    private AppleAuthentication appleSignInManager;
    public Action OnLoggedInCompleted;
    private void Awake()
    {
        Instance = this;
        HelperClass.DebugMessage("called authentication");
        anonymousSignIn = GetComponent<AnonymousSignIn>();
        appleSignInManager = GetComponent<AppleAuthentication>();
        anonymousSignIn.OnAnonymouslyLoggedIn += OnLoggedIn;
        //appleSignInManager.OnAppleLoginComplete += OnLoggedIn;

        anonymousSignIn.OnAnonymouslyLogInFailed += OnInternetConnectionFailed;
        //appleSignInManager.OnAppleLoginComplete += OnInternetConnectionFailed;

        UnityServices.InitializeAsync();
    }
    private void OnLoggedIn()
    {
        OnLoggedInCompleted?.Invoke();
    }
    void Start()
    {
        noInternetPanel.SetActive(false);
        if (!AuthenticationService.Instance.IsSignedIn)
        {
//#if UNITY_EDITOR
            anonymousSignIn.SignInAnonymouslyAsync();
//#elif UNITY_IOS
        //appleSignInManager.StartSignInWithApple();
//#endif
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

    private void OnInternetConnectionFailed()
    {
        Time.timeScale = 0f;
        noInternetPanel?.SetActive(true);
    }
}