using UnityEngine;
using UnityEngine.UI;

public class UIFadeIn : MonoBehaviour
{
    private float fadeInDuration = 0.2f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        SetUIOpacity(0f);
    }

    private void OnEnable()
    {
        StartFadeIn();
    }

    private void Update()
    {
        if (isFading)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(timer / fadeInDuration);

            SetUIOpacity(alpha);

            if (alpha >= 0.96f)
            {
                isFading = false;
            }
        }
    }

    public void StartFadeIn()
    {
        timer = 0f;
        isFading = true;
    }

    private void SetUIOpacity(float alpha)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = alpha;
        }
        else
        {
            Image image = GetComponent<Image>();
            if (image != null)
            {
                Color color = image.color;
                color.a = alpha;
                image.color = color;
            }
            else
            {
                Text text = GetComponent<Text>();
                if (text != null)
                {
                    Color color = text.color;
                    color.a = alpha;
                    text.color = color;
                }
            }
        }
    }
}
