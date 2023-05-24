using UnityEngine;
using System.Threading.Tasks;

public class UnlockAnimation : MonoBehaviour
{
    public Transform lockIcon;
    public Transform unlockedIcon;
    public float shakeDuration = 0.5f;
    public float shakeIntensity = 0.1f;
    public TournamentModesUIManager tournamentModesUIManager;

    private Vector3 initialLockIconPosition;

    private void Start()
    {
        initialLockIconPosition = lockIcon.localPosition;
    }

    public void UnlockIcon()
    {
        StartCoroutine(AnimateUnlock());
    }

    private System.Collections.IEnumerator AnimateUnlock()
    {
        float shakeTimer = 0f;
        while (shakeTimer < shakeDuration)
        {
            shakeTimer += Time.deltaTime;
            float progress = shakeTimer / shakeDuration;
            float shakeOffsetX = Mathf.Sin(progress * Mathf.PI * 20f) * shakeIntensity;

            lockIcon.localPosition = initialLockIconPosition + new Vector3(shakeOffsetX, 0f, 0f);

            yield return null;
        }

        lockIcon.gameObject.SetActive(false);
        unlockedIcon.gameObject.SetActive(true);
        WaitBeforeRefreshing();
    }
    private async void WaitBeforeRefreshing()
    {
        await Task.Delay(1000);
        tournamentModesUIManager.AssignPrices();
    }
}
