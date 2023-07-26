using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [field: Header("Shop Manager")]
    [field: SerializeField] private List<SkinsInfo> infoOnSkins;
    [field: SerializeField] private Button actionButton;
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private AudioClip clip;
    private int currentSkin = 0;

    [field: Header("Swipe Input")]
    [field: SerializeField] private GameObject shopPanel;
    [field: SerializeField] private float minSwipeDistance = 50f;
    [field: SerializeField] private float swipeThreshold = 1f;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    public List<Vector3> scalesOfSkins;

    bool transitioning = false;

    private void Start()
    {
        for (int i = 0; i < infoOnSkins.Count; i++)
        {
            if (SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] == true)
            {
                infoOnSkins[i].isEquipped = true;
                infoOnSkins[i].skinObject.SetActive(true);
                currentSkin = i;
                actionButton.gameObject.SetActive(false);
            }
            else
            {
                infoOnSkins[i].isEquipped = false;
            }
        }
        if(!String.IsNullOrEmpty(UserDataHandler.instance.ReturnSavedValues().userName))
        {
            for (int i = 0; i< infoOnSkins.Count; i++)
            {
                infoOnSkins[i].isOwned = SkinsOwnershipDataHandler.instance.ReturnSavedValues().isOwned[i];
            }
        }

        for(int i = 0; i < infoOnSkins.Count; i++)
        {
            scalesOfSkins.Add(infoOnSkins[i].skinObject.transform.localScale);
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0 && shopPanel.activeInHierarchy)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                CheckSwipe();
            }
        }
    }

    private void CheckSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) >= minSwipeDistance)
        {
            float deltaX = fingerUpPosition.x - fingerDownPosition.x;
            float deltaY = fingerUpPosition.y - fingerDownPosition.y;

            if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) * swipeThreshold)
            {
                if (deltaX > 0)
                {
                    //Right
                    ScrollThroughSkins(false);
                }
                else
                {
                    //Left
                    ScrollThroughSkins(true);
                }
            }
        }
    }

    private void ScrollThroughSkins(bool idkman)
    {
        int previousSkin = currentSkin;
        if (idkman == true)
        {
            currentSkin++;
            if (currentSkin > infoOnSkins.Count-1)
            {
                currentSkin = 0;
            }
        }
        else if (idkman == false)
        {
            currentSkin--;
            if (currentSkin < 0)
            {
                currentSkin = infoOnSkins.Count-1;
            }
        }
        //infoOnSkins[previousSkin].skinObject.SetActive(false);
        //infoOnSkins[currentSkin].skinObject.SetActive(true);
        //UpdateActionButton();
        ScrollingEffect(previousSkin);
    }

    private void ScrollingEffect(int previousSkin)
    {
        audioSource.clip = clip;
        audioSource.Play();
        float transitionDuration = 0.1f;
        float targetScaleOfPreviousObject = 0.2f;
        Vector3 initialScaleOfPreviousObject = scalesOfSkins[previousSkin];
        float initialScaleOfCurrentObject = 0f;
        Vector3 targetScaleOfCurrentObject = scalesOfSkins[currentSkin];
        StartCoroutine(ShrinkMenuItem(previousSkin, targetScaleOfPreviousObject, transitionDuration, initialScaleOfPreviousObject, currentSkin, targetScaleOfCurrentObject, initialScaleOfCurrentObject));
    }

    private IEnumerator ShrinkMenuItem(int indexOfShrinkingObject, float targetScale, float transitionDuration, Vector3 startScale, int indexOfGrowingObject, Vector3 growingTargetScale, float growingStartScale)
    {
        transitioning = true;

        float elapsedTime = 0.0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float scale = Mathf.Lerp(startScale.x, targetScale, t);
            infoOnSkins[indexOfShrinkingObject].skinObject.transform.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        infoOnSkins[indexOfShrinkingObject].skinObject.transform.localScale = new Vector3(targetScale, targetScale, targetScale);
        transitioning = false;
        infoOnSkins[indexOfShrinkingObject].skinObject.SetActive(false);
        infoOnSkins[indexOfGrowingObject].skinObject.SetActive(true);
        StartCoroutine(GrowMenuItem(indexOfGrowingObject, growingTargetScale, transitionDuration, growingStartScale));
    }

    private IEnumerator GrowMenuItem(int indexOfGrowingObject, Vector3 targetScale, float transitionDuration, float startScale)
    {
        transitioning = true;

        float elapsedTime = 0.0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            float scale = Mathf.Lerp(startScale, targetScale.x, t);
            infoOnSkins[indexOfGrowingObject].skinObject.transform.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        infoOnSkins[indexOfGrowingObject].skinObject.transform.localScale = targetScale;
        transitioning = false;
        UpdateActionButton();
    }

    public void UpdateActionButton()
    {
        if (infoOnSkins[currentSkin].isOwned == true)
        {
            actionButton.gameObject.SetActive(false);
            for(int i = 0; i < infoOnSkins.Count; i++)
            {
                infoOnSkins[i].isEquipped = false;
                SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] = false;
                if(i == currentSkin)
                {
                    infoOnSkins[i].isEquipped = true;
                    SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] = true;
                }
            }
            SkinsOwnershipDataHandler.instance.SaveSkinData();
        }

        else if(infoOnSkins[currentSkin].isOwned == false)
        {
            actionButton.gameObject.SetActive(true);
            if (infoOnSkins[currentSkin].skinPrice <= CurrencyDataHandler.instance.ReturnSavedValues().amountOfCoins)
            {
                actionButton.interactable = true;
            }
            else
            {
                actionButton.interactable = false;
            }
            actionButton.GetComponentInChildren<TextMeshProUGUI>().text = "$" + infoOnSkins[currentSkin].skinPrice;
        }
    }

    public void UponPurchase()
    {
        infoOnSkins[currentSkin].isOwned = true;
        SkinsOwnershipDataHandler.instance.ReturnSavedValues().isOwned[currentSkin] = true;
        SkinsOwnershipDataHandler.instance.SaveSkinData();
        CurrencyManager.instance.AdjustCoins(-infoOnSkins[currentSkin].skinPrice);
        UpdateActionButton();
    }
}
