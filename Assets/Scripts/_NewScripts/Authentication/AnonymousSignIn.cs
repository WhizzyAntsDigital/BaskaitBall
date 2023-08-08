using GooglePlayGames;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.EventSystems;
public class AnonymousSignIn : MonoBehaviour
{
    public Action OnAnonymouslyLoggedIn;
    public Action OnAnonymouslyLogInFailed;
    public async void SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            OnAnonymouslyLoggedIn?.Invoke();
            Debug.Log("Sign in anonymously succeeded!");
        }
        catch (AuthenticationException ex)
        {
            OnAnonymouslyLogInFailed?.Invoke();
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            OnAnonymouslyLogInFailed?.Invoke();
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}