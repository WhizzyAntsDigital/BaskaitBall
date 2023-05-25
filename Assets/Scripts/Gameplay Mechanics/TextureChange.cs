using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureChange : MonoBehaviour
{
    [field: Header("Texture Changer")]
    [field: SerializeField] private List<SkinPrefabDetails> prefabs;
    [field: SerializeField] private List<GameObject> basketBalls;
    void Start()
    {
        SkinsOwnershipDataHandler skinDataHandler = SkinsOwnershipDataHandler.instance;
        SkinsOwnershipData savedValues = skinDataHandler.ReturnSavedValues();

        for (int i = 0; i < savedValues.isEquipped.Length; i++)
        {
            if (savedValues.isEquipped[i])
            {
                MeshFilter[] ballMeshFilters = new MeshFilter[basketBalls.Count];
                MeshRenderer[] ballMeshRenderers = new MeshRenderer[basketBalls.Count];

                for (int j = 0; j < basketBalls.Count; j++)
                {
                    ballMeshFilters[j] = basketBalls[j].GetComponent<MeshFilter>();
                    ballMeshRenderers[j] = basketBalls[j].GetComponent<MeshRenderer>();
                }

                Mesh skinMesh = prefabs[i].mesh;
                Material[] skinMaterials = prefabs[i].materials;

                for (int j = 0; j < basketBalls.Count; j++)
                {
                    ballMeshFilters[j].mesh = skinMesh;

                    Material[] ballMaterials = new Material[skinMaterials.Length];
                    for (int k = 0; k < skinMaterials.Length; k++)
                    {
                        ballMaterials[k] = new Material(skinMaterials[k]);
                    }

                    ballMeshRenderers[j].materials = ballMaterials;
                }

                break;
            }
        }
    }

}
