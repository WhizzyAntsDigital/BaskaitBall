using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInput : MonoBehaviour
{
    Vector2 touchStart;
    Vector2 touchEnd;
    float flickTime;
    float flickLength = 0;
    float ballVelocity;
    float ballSpeed = 0;
    Vector3 worldAngle;
private bool GetVelocity = false;
    GameObject[] whoosh;
    public float comfortZone;
    bool couldbeswipe;
    float startCountdownLength = 0.0f;
    bool startTheTimer = false;
static bool globalGameStart = false;
static bool shootEnable = false;
private float startGameTimer = 0.0f;

    public bool hasHitOtherObjects = false;
 
void Start()
    {
        startTheTimer = true;
        Time.timeScale = 1;
        if (Application.isEditor)
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
            globalGameStart = true;
            shootEnable = true;
            startTheTimer = false;
            startGameTimer = 0;
        }

        if (shootEnable)
        {
            if (Input.touchCount > 0)
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
                        else if (touch.position.y - touchStart.y < comfortZone)
                        {
                            couldbeswipe = false;
                        }
                        else
                        {
                            couldbeswipe = true;
                        }
                        break;  

                    case TouchPhase.Stationary:
                        if (Mathf.Abs(touch.position.y - touchStart.y) < comfortZone)
                        {
                            couldbeswipe = false;
                        }
                        break;

                    case TouchPhase.Ended:
                        var swipeDist = (touch.position - touchStart).magnitude;
                        if (couldbeswipe && swipeDist > comfortZone) {
                            GetVelocity = false;
                            touchEnd = touch.position;
                            GetSpeed();
                            GetAngle();
                            GetComponent<Rigidbody>().isKinematic = false;
                            GetComponent<Rigidbody>().useGravity = true;
                            GetComponent<Rigidbody>().AddForce(new Vector3((worldAngle.x * ballSpeed), 883.7209302f, (worldAngle.z * -8)));
                            //PlayWhoosh();

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
            //Debug.Log("shot disabled!");
        }
    }

    void timeIncrease()
    {
        if (GetVelocity)
        {
            flickTime++;
        }
    }

    void GetSpeed()
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
        //Debug.Log("flick was" + flickTime);
        flickTime = 5;
    }

    void GetAngle()
    {
        worldAngle = Camera.main.ScreenToWorldPoint(new Vector3(touchEnd.x, touchEnd.y + 800, ((Camera.main.nearClipPlane - 100) * 1.8f)));
    }

    //void PlayWhoosh()
    //{
    //    var sound = Instantiate(whoosh[Random.Range(0, whoosh.length)], transform.position, transform.rotation) as GameObject;
    //    Debug.Log("Whoosh!");
    //}
    private void OnCollisionEnter(Collision collision)
    {
        hasHitOtherObjects = true;
    }
}
