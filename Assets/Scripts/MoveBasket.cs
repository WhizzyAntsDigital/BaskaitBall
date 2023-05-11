using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveBasket : MonoBehaviour
{
    private Vector3 screenBounds;
    private float objectWidth;
    private float objectHeight;
    [SerializeField] CalculateWorldBounds calculateWorldBounds;
    [SerializeField] private float buffer = 0.1f; // adjust this value to add a buffer zone around the screen edge
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = transform.GetComponent<Renderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<Renderer>().bounds.extents.y;
    }

    private bool IsWithinScreenBounds(Vector3 newPos)
    {
        Vector3 ObjectPosWRTSize = new Vector3(Mathf.Abs(newPos.x) + objectWidth, Mathf.Abs(newPos.y) + objectHeight*2, transform.position.z);
            Vector3 objectPos = Camera.main.WorldToViewportPoint(ObjectPosWRTSize);

            return (objectPos.x >= 0 + buffer && objectPos.x <= 1 - buffer && objectPos.y >= 0 + buffer && objectPos.y <= 1 - buffer && objectPos.z > 0);
    }

    public void MoveBasketNow()
    {
        float yPos = Random.Range(calculateWorldBounds.minYWRTObj, calculateWorldBounds.maxYWRTObj);
        float xPos = Random.Range(calculateWorldBounds.minXWRTObj, calculateWorldBounds.maxXWRTObj);
        Vector3 newPosition = new Vector3(xPos, yPos, transform.position.z);
        transform.position = newPosition;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            MoveBasketNow();
        }
    }
}
