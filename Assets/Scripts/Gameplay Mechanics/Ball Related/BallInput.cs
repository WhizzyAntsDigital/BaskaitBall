using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class BallInput : MonoBehaviour
{
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private float flickTime;
    private float flickLength = 0;
    private float ballVelocity;
    private float ballSpeed = 0;
    private Vector3 worldAngle;
    private bool GetVelocity = false;
    private float minimumSwipeLength;
    private bool couldbeswipe;
    private float initialAngle = 45f;
    private bool touchWithinLimit = false;

    public bool hasGotInput = false;
    public bool CheckForUITouch = false;
    [field: SerializeField] public bool hasHitOtherObjects { get; private set; } = false;
    public bool hasHitRespawnColliders = false;

    public bool IsArcade = false;
    public bool kinematic = true;
    private float forceForArcade;

    void Start()
    {
        minimumSwipeLength = InputValues.instance.minimumSwipeLength;
        initialAngle = InputValues.instance.initialAngle;

        Time.fixedDeltaTime = 0.01f;
        GetComponent<Rigidbody>().isKinematic = kinematic;
        GetComponent<Rigidbody>().useGravity = !kinematic;
        GameManager.instance.onGameOver += () => { hasGotInput = true; };
        if (IsArcade)
        {
            forceForArcade = CalculateForce();
        }
    }

    void Update()
    {
            if (Input.touchCount > 0 && !hasGotInput)
            {
                var touch = Input.touches[0];
                Vector2 touchPosition = touch.position;
                CheckUITouch(touchPosition);
                if (touchWithinLimit)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            flickTime = 5;
                            timeIncrease();
                            couldbeswipe = true;
                            GetVelocity = true;
                            touchStart = touch.position;
                            break;

                        case TouchPhase.Moved:
                            if (touch.position.y - touchStart.y < 0)
                            {
                                couldbeswipe = false;
                            }
                            else if (touch.position.y - touchStart.y < minimumSwipeLength)
                            {
                                couldbeswipe = false;
                            }
                            else
                            {
                                couldbeswipe = true;
                            }
                            break;

                        case TouchPhase.Stationary:
                            if (Mathf.Abs(touch.position.y - touchStart.y) < minimumSwipeLength)
                            {
                                couldbeswipe = false;
                            }
                            break;

                        case TouchPhase.Ended:
                            var swipeDist = (touch.position - touchStart).magnitude;
                            if (couldbeswipe && swipeDist > minimumSwipeLength)
                            {
                                GetVelocity = false;
                                touchEnd = touch.position;
                                Vector3 directionOfShot = touchStart - touchEnd;
                            if(directionOfShot.x >= -40 && directionOfShot.x <= 40)
                            {
                                float autoAim = UnityEngine.Random.Range(0f, 1f);
                                {
                                    if (autoAim >=0f && autoAim<=0.45f)
                                    {
                                        directionOfShot = new Vector3(0f, directionOfShot.y, directionOfShot.z);
                                    }
                                }
                            }
                                GetSpeed();
                                GetAngle();
                                GetComponent<Rigidbody>().isKinematic = false;
                                GetComponent<Rigidbody>().useGravity = true;
                            //GetComponent<Rigidbody>().AddForce(new Vector3((worldAngle.x * ballSpeed), (IsArcade == true ? forceForArcade : CalculateForce()), (worldAngle.z * InputValues.instance.forwardSpeed)));
                            GetComponent<Rigidbody>().AddForce(new Vector3((directionOfShot.x), (IsArcade == true ? forceForArcade : CalculateForce()), (worldAngle.z * InputValues.instance.forwardSpeed)));
                            hasGotInput = true;
                                touchWithinLimit = false;
                                if (IsArcade)
                                {
                                    hasHitOtherObjects = false;
                                    ArcadeLevel.Instance.MoveBallToCentre();
                                }
                            }
                            break;
                    }
                    if (GetVelocity)
                    {
                        flickTime++;
                    }
                }
            }
    }

    private float CalculateForce()
    {
        var rigid = GetComponent<Rigidbody>();

        Vector3 p = GameManager.instance.ringObj.transform.position;

        float gravity = Physics.gravity.magnitude;
        float angle = initialAngle * Mathf.Deg2Rad;

        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(planarTarget, planarPostion);
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        return Mathf.Abs(finalVelocity.y) * InputValues.instance.upwardSpeedMultiplier;

    }

    private void timeIncrease()
    {
        if (GetVelocity)
        {
            flickTime++;
        }
    }

    private void GetSpeed()
    {
        flickLength = 90;
        if (flickTime > 0)
        {
            ballVelocity = flickLength / (flickLength - flickTime);
        }
        ballSpeed = ballVelocity * 30;
        ballSpeed = ballSpeed - (ballSpeed * 1.65f);
        if (ballSpeed <= -33)
        {
            ballSpeed = -33;
        }
        flickTime = 5;
    }

    private void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y, ((Camera.main.nearClipPlane - 100) * 1.8f)));
    }

    private void CheckUITouch(Vector2 position)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = position;

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(InputValues.instance.uiElementTag))
            {
                touchWithinLimit = true;
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("ArcadeParts") && !collision.gameObject.CompareTag("Ball") && !collision.gameObject.CompareTag("Net"))
        {
            hasHitOtherObjects = true;
        }
    }
}
