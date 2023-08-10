using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [field: Header("Shop Manager")]
    [field: SerializeField] private List<SkinsInfo> infoOnSkins;
    [field: SerializeField] private TextMeshProUGUI nameOfBall;
    [field: SerializeField] private Button actionButton;
    [field: SerializeField] private AudioSource audioSource;
    [field: SerializeField] private AudioClip clip;
    private int currentSkin = 0;

    [field: Header("Action Button Sprites")]
    [field: SerializeField] private Sprite purchaseSkin;
    [field: SerializeField] private Sprite equipSkin;
    [field: SerializeField] private Sprite equippedSkin;
    [field: SerializeField] private GameObject purchasableText;
    [field: SerializeField] private GameObject purchasedText;

    [field: Header("IAP Stuff")]
    [field: SerializeField] private string mainPanelTag;
    [field: SerializeField] private string bottomPanelTag;
    [field: SerializeField] private GameObject coinsIAPPanel;
    [field: SerializeField] private GameObject gemsIAPPanel;
    [field: SerializeField] private GameObject circle1;
    [field: SerializeField] private GameObject circle2;
    [field: SerializeField] private Vector2 smallerCircleScale;
    [field: SerializeField] private List<CodelessIAPButton> iapButtons;
    [field: SerializeField] private List<TextMeshProUGUI> iapTexts;

    [field: Header("Swipe Input")]
    [field: SerializeField] private GameObject shopPanel;
    [field: SerializeField] private float minSwipeDistance = 50f;
    [field: SerializeField] private float swipeThreshold = 1f;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    int mainPOC;

    public List<Vector3> scalesOfSkins;

    bool transitioning = false;

    private void Start()
    {
        for (int i = 0; i < infoOnSkins.Count; i++)
        {
            Debug.Log(i);
            if (SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] == true)
            {
                infoOnSkins[i].isEquipped = true;
                infoOnSkins[i].skinObject.SetActive(true);
                currentSkin = i;
                nameOfBall.text = infoOnSkins[currentSkin].skinName;
                UpdateActionButton();
            }
            else
            {
                infoOnSkins[i].isEquipped = false;
            }
        }
        if (MiscellaneousDataHandler.instance.ReturnSavedValues().hasPlayedTutorial)
        {
            for (int i = 0; i < infoOnSkins.Count; i++)
            {
                infoOnSkins[i].isOwned = SkinsOwnershipDataHandler.instance.ReturnSavedValues().isOwned[i];
            }
        }

        for (int i = 0; i < infoOnSkins.Count; i++)
        {
            scalesOfSkins.Add(infoOnSkins[i].skinObject.transform.localScale);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" && Input.touchCount > 0 && shopPanel.activeInHierarchy)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
                CheckUITouch(touch.position, ref mainPOC);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                CheckSwipe(mainPOC);
            }
        }
    }

    private void CheckSwipe(int poc)
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
                    switch (poc)
                    {
                        case 1: ScrollThroughSkins(false); break;
                        case 2: ScrollThroughIAP(false); break;
                        default: HelperClass.DebugWarning("Wrong Input????"); break;
                    }
                }
                else
                {
                    //Left
                    switch (poc)
                    {
                        case 1: ScrollThroughSkins(true); break;
                        case 2: ScrollThroughIAP(true); break;
                        default: HelperClass.DebugWarning("Wrong Input????"); break;
                    }
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
            if (currentSkin > infoOnSkins.Count - 1)
            {
                currentSkin = 0;
            }
        }
        else if (idkman == false)
        {
            currentSkin--;
            if (currentSkin < 0)
            {
                currentSkin = infoOnSkins.Count - 1;
            }
        }
        ScrollingEffect(previousSkin);
    }

    private void ScrollThroughIAP(bool idkman)
    {
        if (coinsIAPPanel.activeInHierarchy)
        {
            coinsIAPPanel.SetActive(false);
            gemsIAPPanel.SetActive(true);
            circle1.GetComponent<RectTransform>().localScale = smallerCircleScale;
            circle2.GetComponent<RectTransform>().localScale = Vector2.one;
        }
        else
        {
            coinsIAPPanel.SetActive(true);
            gemsIAPPanel.SetActive(false);
            circle2.GetComponent<RectTransform>().localScale = smallerCircleScale;
            circle1.GetComponent<RectTransform>().localScale = Vector2.one;
        }
    }


    private void ScrollingEffect(int previousSkin)
    {
        audioSource.clip = clip;
        nameOfBall.text = infoOnSkins[currentSkin].skinName;
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
            purchasedText.SetActive(true);
            purchasableText.SetActive(false);
            if (infoOnSkins[currentSkin].isEquipped == true)
            {
                actionButton.gameObject.GetComponent<Image>().sprite = equippedSkin;
                actionButton.interactable = false;
                purchasedText.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
            }
            else
            {
                actionButton.gameObject.GetComponent<Image>().sprite = equipSkin;
                actionButton.interactable = true;
                purchasedText.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
            }
        }

        if (infoOnSkins[currentSkin].isOwned == false)
        {
            purchasableText.SetActive(true);
            purchasedText.SetActive(false);
            actionButton.gameObject.GetComponent<Image>().sprite = purchaseSkin;
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

    private void EquipSkin()
    {
        if (infoOnSkins[currentSkin].isOwned && !infoOnSkins[currentSkin].isEquipped)
        {
            actionButton.gameObject.GetComponent<Image>().sprite = equippedSkin;
            for (int i = 0; i < infoOnSkins.Count; i++)
            {
                infoOnSkins[i].isEquipped = false;
                SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] = false;
                if (i == currentSkin)
                {
                    infoOnSkins[i].isEquipped = true;
                    SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] = true;
                }
            }
            SkinsOwnershipDataHandler.instance.SaveSkinData();
        }
    }

    public void UponPurchase()
    {
        if (infoOnSkins[currentSkin].isOwned == false)
        {
            infoOnSkins[currentSkin].isOwned = true;
            SkinsOwnershipDataHandler.instance.ReturnSavedValues().isOwned[currentSkin] = true;
            SkinsOwnershipDataHandler.instance.SaveSkinData();

            switch (infoOnSkins[currentSkin].isGems)
            {
                case true:
                    CurrencyManager.instance.AdjustGems(-infoOnSkins[currentSkin].skinPrice);
                    break;
                case false:
                    CurrencyManager.instance.AdjustCoins(-infoOnSkins[currentSkin].skinPrice);
                    break;
            }
        }
        else
        {
            EquipSkin();
        }
        UpdateActionButton();
    }
    private void CheckUITouch(Vector2 position, ref int poc)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(mainPanelTag))
            {
                poc = 1;
                break;
            }
            else if (result.gameObject.CompareTag(bottomPanelTag))
            {
                poc = 2;
                break;
            }
        }
    }

    public void OnPurchaseOfCoins(int amount)
    {
        CurrencyManager.instance.AdjustCoins(amount);
    }
    public void OnPurchaseOfGems(int amount)
    {
        CurrencyManager.instance.AdjustGems(amount);
    }

    public void AssignPrices()
    {
        for (int i = 0; i < iapTexts.Count; i++)
        {
            iapTexts[i].text = CodelessIAPStoreListener.Instance.StoreController.products.WithID(iapButtons[i].productId).metadata.localizedPriceString;
        }
    }
}
