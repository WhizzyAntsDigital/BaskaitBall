using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValues : MonoBehaviour
{
    public static InputValues instance;
    [field: Header("Values for Ball Input")]
    [field: SerializeField] public float minimumSwipeLength { get; private set; } = 50f; 
    [field: SerializeField] public float initialAngle { get; private set; } = 45f;
    [field: SerializeField] public float forwardSpeed { get; private set; } = -8f;
    [field: SerializeField] public float upwardSpeedMultiplier { get; private set; } = 125;

    private void Awake()
    {
        instance = this;
    }
}
