using System;
using System.Collections;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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
        if("https://api.waifu.pics/sfw/waifu" == url)
        {
            string pattern = "\"url\":\"(.*?)\"";
            Match match = Regex.Match(url, pattern);
            if (match.Success)
            {
                string link = match.Groups[1].Value;
                url = link;
                print(link + " Original: " + url);
            }
            else
            {
                Debug.Log("No link found in the input string.");
            }
        }
        StartCoroutine(DownloadImage(url, imageTarget));
    }

    private IEnumerator DownloadImage(string URL, Image imageTarget)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error while fetching image: " + www.error);
            }
            else
            {
                print(www.downloadHandler.text);
                if ("https://api.waifu.pics/sfw/waifu" == URL || "https://api.waifu.pics/nsfw/waifu" == URL)
                {
                    string pattern = "\"url\":\"(.*?)\"";
                    Match match = Regex.Match(www.downloadHandler.text, pattern);
                    if (match.Success)
                    {
                        string link = match.Groups[1].Value;
                        StartCoroutine(DownloadImage(link, imageTarget));
                    }
                    else
                    {
                        Debug.Log("No link found in the input string.");
                    }
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);

                    imageTarget.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }
    }
}
