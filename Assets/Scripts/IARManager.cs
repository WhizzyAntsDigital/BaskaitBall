using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Review;
public class IARManager : MonoBehaviour
{
    public static IARManager instance;
    public ReviewManager _reviewManager;
    public PlayReviewInfo _playReviewInfo;
    void Awake()
    {
        instance = this;
    }
    public void TryLoadAndShowReviewRequest()
    {
        StartCoroutine(RequestReviews());
    }
    IEnumerator RequestReviews()
    {
        _reviewManager = new ReviewManager();
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        else
        {
        }
        _playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        else
        {
            UserDataHandler.instance.UpdateReviewRequested();
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
}
