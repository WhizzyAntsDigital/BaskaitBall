using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTING : MonoBehaviour
{
    public GameObject hoopObj;
    public GameObject groundObj;
    public float offsetFromGround = 0.5f;
    private float minX, maxX, minY, maxY;
    public float minXWRTObj, maxXWRTObj, minYWRTObj, maxYWRTObj;
    private float objectWidth;
    private float objectHeight;
    // Start is called before the first frame update
    void Start()
    {
        GameObject apple = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Vector3 bottomLeft = new(0, 0, Vector3.Distance(Camera.main.transform.position, hoopObj.transform.position));
        Vector3 topRight = new(1, 1, Vector3.Distance(Camera.main.transform.position, hoopObj.transform.position));
        Vector3 worldBL = Camera.main.ViewportToWorldPoint(bottomLeft);
        Vector3 worldTR = Camera.main.ViewportToWorldPoint(topRight);
        minX = worldBL.x;
        maxX = -worldBL.x;
        minY = groundObj.transform.position.y + offsetFromGround;
        maxY = -worldBL.y;
        objectWidth = hoopObj.transform.GetComponent<Renderer>().bounds.extents.x;
        objectHeight = hoopObj.transform.GetComponent<Renderer>().bounds.extents.y;
        minXWRTObj = minX + objectWidth;
        maxXWRTObj = maxX - objectWidth;
        minYWRTObj = minY + objectHeight;
        maxYWRTObj = maxY - objectHeight;
        //Debug.DrawLine(worldBL, new Vector3(-worldBL.x, worldBL.y, worldBL.z), Color.red , 1000000000f); //bottom
        //Debug.DrawLine(worldBL, new Vector3(worldBL.x, -worldBL.y, worldBL.z), Color.blue, 1000000000f); //left
        //Debug.DrawLine(worldTR, new Vector3(-worldTR.x, worldTR.y, worldTR.z), Color.green, 1000000000f); //top
        //Debug.DrawLine(worldTR, new Vector3(worldTR.x, -worldTR.y, worldTR.z), Color.magenta, 1000000000f); //Right
    }

    public bool CheckIfInScreenBounds()
    {
        return (hoopObj.transform.position.x - objectWidth) > minX && (hoopObj.transform.position.x + objectWidth) < maxX && (hoopObj.transform.position.y - objectHeight) > minY && (hoopObj.transform.position.y + objectHeight) < maxY;
    }
}
