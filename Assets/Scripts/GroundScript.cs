using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    public float timerAmount = 3f;
    public RespawnBall respawnBall;
    float timer = 0f;
    bool startTimer = false;
    Collision collisionObj = null;
    public GameObject ballParentObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
            if(timer>= timerAmount && collisionObj.gameObject != null)
            {
                Destroy(collisionObj.gameObject);
                GameObject ball = Instantiate(respawnBall.ballPrefab, respawnBall.respawnPoint.transform.position, Quaternion.identity);
                ball.transform.parent = ballParentObject.transform;
                ScoreCalculator.instance.AssignCollider(ball.GetComponent<SphereCollider>());
                timer = 0f;
                startTimer = false;
                collisionObj = null;
            }
            else if(collisionObj.gameObject == null)
            {
                timer = 0f;
                startTimer = false;
                collisionObj = null;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            startTimer = true;
            collisionObj = collision;
        }
    }
}
