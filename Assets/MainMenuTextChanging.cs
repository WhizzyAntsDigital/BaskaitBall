using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuTextChanging : MonoBehaviour
{
    [SerializeField] List<StringSpecs> specs;
    [SerializeField] TextMeshProUGUI textDisplay;

    private float timer = 0f;
    private int listIndex = 0;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if(timer >= specs[listIndex].textDuration)
        {
            textDisplay.text = specs[listIndex].textToShow;
            listIndex++;
            if(listIndex >= specs.Count)
            {
                listIndex = 0;
            }
            timer = 0f;
        }
    }
}
