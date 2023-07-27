using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestImageThing : MonoBehaviour
{
    public Image Image;
    public string apiURL;
   public void GenImage()
    {
        GetImage.Instance.StartImageDownload(apiURL, Image);
    }
}
