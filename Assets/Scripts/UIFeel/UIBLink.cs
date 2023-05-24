using UnityEngine;
using UnityEngine.UI;

public class UIBLink : MonoBehaviour
{
    public float blinkInterval = 0.5f;

    private Image uiImage;
    private Text uiText;
    private bool isVisible = true;
    private float timer = 0f;

    private void Start()
    {
        uiImage = GetComponent<Image>();
        uiText = GetComponent<Text>();

        SetUIVisibility(true);
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime; 

        if (timer >= blinkInterval)
        {
            timer -= blinkInterval;
            isVisible = !isVisible;

            SetUIVisibility(isVisible);
        }
    }

    private void SetUIVisibility(bool visible)
    {
        if (uiImage != null)
            uiImage.enabled = visible;

        if (uiText != null)
            uiText.enabled = visible;
    }
}
