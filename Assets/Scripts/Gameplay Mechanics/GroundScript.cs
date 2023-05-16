using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    [field: SerializeField] float timerAmount = 1f;
    [field: SerializeField] RespawnBall respawnBall;
    [field: SerializeField] GameObject ballParentObject;
    Collision collisionObj = null;
    bool startTimer = false;
    float timer = 0f;

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
        if (collision.gameObject.CompareTag("Ball") && !collision.gameObject.GetComponent<BallInput>().hasHitRespawnColliders)
        {
            collision.gameObject.GetComponent<BallInput>().hasHitRespawnColliders = true;
            startTimer = true;
            collisionObj = collision;
        }
    }
}
