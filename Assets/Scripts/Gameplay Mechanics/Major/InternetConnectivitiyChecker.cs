using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InternetConnectivitiyChecker : MonoBehaviour
{
    [field: Header("Internet Connection Checking")]
    [field: SerializeField] private float internetConnectionCheckInterval = 3f;
    [field: SerializeField] private bool shouldCheckForInternetConnection = true;
    public Action IsConnectedToInternet, IsDisconnectedFromInternet;
    private bool whenConnectedActionsCarriedOut = false;
    private bool whenDisconnectedActionsCarriedOut = false;

    private void Start()
    {
        StartCoroutine(CheckInternetConnection());
    }
    private IEnumerator CheckInternetConnection()
    {
        while (shouldCheckForInternetConnection)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (!whenDisconnectedActionsCarriedOut)
                {
                    IsDisconnectedFromInternet?.Invoke();
                    whenDisconnectedActionsCarriedOut = true;
                    whenConnectedActionsCarriedOut = false;
                }
            }
            else
            {
                if (!whenConnectedActionsCarriedOut)
                {
                    IsConnectedToInternet?.Invoke();
                    whenConnectedActionsCarriedOut = true;
                    whenDisconnectedActionsCarriedOut = false;
                }
            }
            yield return new WaitForSeconds(internetConnectionCheckInterval);
        }
    }

}
