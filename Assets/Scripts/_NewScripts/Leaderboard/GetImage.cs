using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{
    public static GetImage Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void StartImageDownload(string url, Image imageTarget)
    {
        StartCoroutine(DownloadImage(url, imageTarget));
    }

    private IEnumerator DownloadImage(string URL, Image imageTarget)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL))
        {
            // Wait for the API response
            yield return www.SendWebRequest();

            // Check for errors
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while fetching image: " + www.error);
            }
            else
            {
                // Convert the API response to a Texture2D
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Assign the texture to the RawImage UI element
                imageTarget.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); ;
            }
        }
    }
}
