using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBall : MonoBehaviour
{
    [field: SerializeField] public GameObject ballPrefab { get; private set; }
    [field: SerializeField] public GameObject respawnPoint { get; private set; }
    [field: SerializeField] GameObject parentObject;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && parentObject.transform.childCount == 1 && !other.gameObject.GetComponent<BallInput>().hasHitRespawnColliders)
        {
            Destroy(other.gameObject);
            other.gameObject.GetComponent<BallInput>().hasHitRespawnColliders = true;
            GameObject ball = Instantiate(ballPrefab, respawnPoint.transform.position, Quaternion.identity);
            ball.transform.parent = parentObject.transform;
            ScoreCalculator.instance.AssignCollider(ball.GetComponent<SphereCollider>());
        }
    }
}
