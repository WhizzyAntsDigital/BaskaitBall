using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkinsInfo
{
    public int skinID;
    public Sprite skinIcon;
    public Material skinMaterial;
    public int skinPrice;
    public bool isOwned = false;
    public bool isEquipped = false;
}
