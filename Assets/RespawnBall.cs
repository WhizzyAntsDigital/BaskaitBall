using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBall : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject respawnPoint;
    public GameObject parentObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && parentObject.transform.childCount == 1)
        {
            Destroy(other.gameObject);
            GameObject ball = Instantiate(ballPrefab, respawnPoint.transform.position, Quaternion.identity);
            ball.transform.parent = parentObject.transform;
            ScoreCalculator.instance.AssignCollider(ball.GetComponent<SphereCollider>());
        }
    }
}
