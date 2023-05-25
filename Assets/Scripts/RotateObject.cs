using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;  
    public float rotationSpeed = 45f;         

    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        transform.Rotate(rotationAxis, rotationAmount, Space.World);
    }
}
