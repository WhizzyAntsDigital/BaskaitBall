using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
//using Apple.GameKit;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AppleAuthentication : MonoBehaviour
{
    //string Signature;
    //string TeamPlayerID;
    //string Salt;
    //string PublicKeyUrl;
    //string Timestamp;

    //public Action OnAppleLoginComplete;

    //// Start is called before the first frame update
    ////async void Start()
    ////{
    //    //await Login();
    ////}

    //public async Task Login()
    //{
    //    if (!GKLocalPlayer.Local.IsAuthenticated)
    //    {
    //        // Perform the authentication.
    //        var player = await GKLocalPlayer.Authenticate();
    //        Debug.Log($"GameKit Authentication: player {player}");
    //        // Grab the display name.
    //        var localPlayer = GKLocalPlayer.Local;
    //        Debug.Log($"Local Player: {localPlayer.DisplayName}");

    //        // Fetch the items.
    //        var fetchItemsResponse = await GKLocalPlayer.Local.FetchItems();

    //        Signature = Convert.ToBase64String(fetchItemsResponse.GetSignature());
    //        TeamPlayerID = localPlayer.TeamPlayerId;
    //        Debug.Log($"Team Player ID: {TeamPlayerID}");

    //        Salt = Convert.ToBase64String(fetchItemsResponse.GetSalt());
    //        PublicKeyUrl = fetchItemsResponse.PublicKeyUrl;
    //        Timestamp = fetchItemsResponse.Timestamp.ToString();

    //        Debug.Log($"GameKit Authentication: signature => {Signature}");
    //        Debug.Log($"GameKit Authentication: publickeyurl => {PublicKeyUrl}");
    //        Debug.Log($"GameKit Authentication: salt => {Salt}");
    //        Debug.Log($"GameKit Authentication: Timestamp => {Timestamp}");
    //    }
    //    else
    //    {
    //        Debug.Log("AppleGameCenter player already logged in.");
    //    }
    //}

    //async Task SignInWithAppleGameCenterAsync(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
    //{
    //    try
    //    {
    //        await AuthenticationService.Instance.SignInWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
    //        Debug.Log("SignIn is successful.");
    //    }
    //    catch (AuthenticationException ex)
    //    {
    //        // Compare error code to AuthenticationErrorCodes
    //        // Notify the player with the proper error message
    //        Debug.LogException(ex);
    //    }
    //    catch (RequestFailedException ex)
    //    {
    //        // Compare error code to CommonErrorCodes
    //        // Notify the player with the proper error message
    //        Debug.LogException(ex);
    //    }
    //}

    //async Task LinkWithAppleGameCenterAsync(string signature, string teamPlayerId, string publicKeyURL, string salt, ulong timestamp)
    //{
    //    try
    //    {
    //        await AuthenticationService.Instance.LinkWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp);
    //        Debug.Log("Link is successful.");
    //    }
    //    catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
    //    {
    //        // Prompt the player with an error message.
    //        Debug.LogError("This user is already linked with another account. Log in instead.");
    //    }
    //    catch (AuthenticationException ex)
    //    {
    //        // Compare error code to AuthenticationErrorCodes
    //        // Notify the player with the proper error message
    //        Debug.LogException(ex);
    //    }
    //    catch (RequestFailedException ex)
    //    {
    //        // Compare error code to CommonErrorCodes
    //        // Notify the player with the proper error message
    //        Debug.LogException(ex);
    //    }
    //}

    //public async void StartSignInWithApple()
    //{
    //    await Login();
    //}
}
