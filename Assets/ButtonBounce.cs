using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float scaleFactor = 1.2f;
    private float animationDuration = 0.1f;
    private float pressedOpacity = 0f;
    private float normalOpacity = 1f;

    private Vector3 initialScale;
    private Image buttonImage;
    private bool isPressed = false;

    private void Start()
    {
        initialScale = transform.localScale;

        buttonImage = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isPressed = true;

            StartCoroutine(BounceScaleAnimation(initialScale * scaleFactor, pressedOpacity));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isPressed)
        {
            isPressed = false;

            StartCoroutine(BounceScaleAnimation(initialScale, normalOpacity));
        }
    }

    private System.Collections.IEnumerator BounceScaleAnimation(Vector3 targetScale, float targetOpacity)
    {
        float timeElapsed = 0f;
        Vector3 startScale = transform.localScale;
        float startOpacity = buttonImage.color.a;

        while (timeElapsed < animationDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / animationDuration);

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            Color color = buttonImage.color;
            color.a = Mathf.Lerp(startOpacity, targetOpacity, t);
            buttonImage.color = color;

            yield return null;
        }

        transform.localScale = targetScale;
        Color finalColor = buttonImage.color;
        finalColor.a = targetOpacity;
        buttonImage.color = finalColor;
    }
}
