using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveBasket : MonoBehaviour
{
    [SerializeField] CalculateWorldBounds calculateWorldBounds;

    public void MoveBasketNow()
    {
        float yPos = Random.Range(calculateWorldBounds.minYWRTObj, calculateWorldBounds.maxYWRTObj);
        float xPos = Random.Range(calculateWorldBounds.minXWRTObj, calculateWorldBounds.maxXWRTObj);
        Vector3 newPosition = new Vector3(xPos, yPos, transform.position.z);
        transform.position = newPosition;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.D))
        {
            MoveBasketNow();
        }
#endif
    }
}
