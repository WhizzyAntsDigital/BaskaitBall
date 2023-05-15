using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private float startCountdownLength = 0.0f;
    private bool startTheTimer = false;
    private static bool shootEnable = false;
    private float startGameTimer = 0.0f;
    private float initialAngle = 45f;

    public bool hasGotInput = false;
    public bool hasHitOtherObjects { get; private set; } = false;
    public bool hasHitRespawnColliders = false;
 
void Start()
    {
        minimumSwipeLength = InputValues.instance.minimumSwipeLength;
        initialAngle = InputValues.instance.initialAngle;
        
        startTheTimer = true;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.01f;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void Update()
    {
        if (startTheTimer)
        {
            startGameTimer += Time.deltaTime;
        }
        if (startGameTimer > startCountdownLength)
        {
            shootEnable = true;
            startTheTimer = false;
            startGameTimer = 0;
        }

        if (shootEnable)
        {
            if (Input.touchCount > 0 && !hasGotInput)
            {
                var touch = Input.touches[0];
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
                        if (couldbeswipe && swipeDist > minimumSwipeLength) {
                            GetVelocity = false;
                            touchEnd = touch.position;
                            GetSpeed();
                            GetAngle();
                            GetComponent<Rigidbody>().isKinematic = false;
                            GetComponent<Rigidbody>().useGravity = true;
                            GetComponent<Rigidbody>().AddForce(new Vector3((worldAngle.x * ballSpeed), CalculateForce(), (worldAngle.z * InputValues.instance.forwardSpeed)));
                            hasGotInput = true;
                        }
                        break;
                }
                if (GetVelocity)
                {
                    flickTime++;
                }
            }
        }
        if (!shootEnable)
        {
            Debug.Log("shot disabled!");
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
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y + 800, ((Camera.main.nearClipPlane - 100) * 1.8f)));
    }

    private void OnCollisionEnter(Collision collision)
    {
        hasHitOtherObjects = true;
    }
}
