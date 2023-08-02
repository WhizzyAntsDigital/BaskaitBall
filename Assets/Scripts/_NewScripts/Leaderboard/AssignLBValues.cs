using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssignLBValues : MonoBehaviour
{
    [field: Header("Leaderboard Content Values For Prefab")]
    [field: SerializeField] private TextMeshProUGUI usernameText;
    [field: SerializeField] private TextMeshProUGUI scoreText;
    [field: SerializeField] private TextMeshProUGUI positionText;
    [field: SerializeField] private Image profilePicture;
    [field: SerializeField] private Image badgeHolder;
    [field: SerializeField] private Image mainBG;

    [field: Header("Default Values")]
    [field: SerializeField] private Sprite defaultPFP;
    [field: SerializeField] private Sprite goldBadge;
    [field: SerializeField] private Sprite silverBadge;
    [field: SerializeField] private Sprite bronzeBadge;
    [field: SerializeField] private Sprite firstPlaceBG;

    Texture2D img;

    public void AssignValues(string username, int  score, int position, Sprite pfp, bool isPLayer)
    {
        usernameText.text = username;
        scoreText.text = score.ToString();
        positionText.text = position.ToString();

        if(pfp == null)
        {
            if(!isPLayer)
            {
                GetImage.Instance.StartImageDownload(profilePicture);
            }
            else
            {
                string id = Social.localUser.id;
                string name = Social.localUser.userName;
                StartCoroutine(KeepCheckingAvatar());
            }
        }
        else
        {
            profilePicture.sprite = pfp;
        }

        switch (position)
        {
            case 1:
                badgeHolder.sprite = goldBadge; mainBG.sprite = firstPlaceBG; break;
            case 2:
                badgeHolder.sprite = silverBadge; break;
            case 3: 
                badgeHolder.sprite = bronzeBadge; break;
            default: 
                badgeHolder.gameObject.SetActive(false); positionText.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(positionText.gameObject.GetComponent<RectTransform>().sizeDelta.x * 1.5f, positionText.gameObject.GetComponent<RectTransform>().sizeDelta.y * 1.5f); break;
        }
    }
    private IEnumerator KeepCheckingAvatar()
    {
        float secondsOfTrying = 20;
        float secondsPerAttempt = 0.2f;
        while (secondsOfTrying > 0)
        {
            if (Social.localUser.image != null)
            {
                profilePicture.sprite = Sprite.Create(Social.localUser.image, new Rect(0, 0, Social.localUser.image.width, Social.localUser.image.height), new Vector2(0.5f, 0.5f));
                break;
            }

            secondsOfTrying -= secondsPerAttempt;
            yield return new WaitForSeconds(secondsPerAttempt);
        }
    }
}
