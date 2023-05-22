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
    [field: SerializeField] private Image skinIcon;
    [field: SerializeField] private Button actionButton;
    private int currentSkin = 0;

    [field: Header("Swipe Input")]
    [field: SerializeField] private GameObject shopPanel;
    [field: SerializeField] private float minSwipeDistance = 50f;
    [field: SerializeField] private float swipeThreshold = 1f;
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    private void Start()
    {
        for (int i = 0; i < infoOnSkins.Count; i++)
        {
            if (SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] == true)
            {
                infoOnSkins[i].isEquipped = true;
                skinIcon.sprite = infoOnSkins[i].skinIcon;
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
        skinIcon.sprite = infoOnSkins[currentSkin].skinIcon;
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
            if (infoOnSkins[currentSkin].skinPrice <= UserDataHandler.instance.ReturnSavedValues().amountOfCurrency)
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
        CurrencyManager.instance.AdjustCurrency(-infoOnSkins[currentSkin].skinPrice);
        UpdateActionButton();
    }
}
