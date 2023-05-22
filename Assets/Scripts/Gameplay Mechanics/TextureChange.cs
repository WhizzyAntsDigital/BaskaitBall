using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChange : MonoBehaviour
{
    [field: Header("Texture Changer")]
    [field: SerializeField] private List<Material> materials;
    [field: SerializeField] private List<GameObject> basketBalls;
    void Start()
    {
        for(int i = 0; i < SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped.Length-1; i++)
        {
            if (SkinsOwnershipDataHandler.instance.ReturnSavedValues().isEquipped[i] == true)
            {
                for(int j = 0; j<basketBalls.Count; j++)
                {
                    basketBalls[j].GetComponent<MeshRenderer>().material = materials[i];
                }
                break;
            }
        }
    }

}
