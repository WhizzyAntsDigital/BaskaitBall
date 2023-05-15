using UnityEngine;

public class ArcadeLevel : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float speedOfMovingToSpawn;
    private bool hitTrigger = false;
    private GameObject ballObj;

    private void Update()
    {
        if(hitTrigger)
        {
                float step = speedOfMovingToSpawn * Time.deltaTime;
            ballObj.transform.position = Vector3.MoveTowards(ballObj.transform.position, spawnPoint.transform.position, step);
            Debug.Log("Moving....Ball: " + ballObj.transform.position.x + " Target: " + spawnPoint.transform.position.x);
            if(ballObj.transform.position.x == spawnPoint.transform.position.x)
            {
                hitTrigger = false;
                //ballObj.GetComponent<BallInput>().hasGotInput = false;
                //ballObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
                ballObj.GetComponent<Rigidbody>().isKinematic = true;
                ballObj.GetComponent<Rigidbody>().isKinematic = false;
                ballObj = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            //other.gameObject.transform.position = Vector3.MoveTowards(other.gameObject.transform.position, spawnPoint.transform.position, 5f);
            ballObj = other.gameObject;
            hitTrigger = true;
                other.gameObject.GetComponent<BallInput>().hasGotInput = false;
        }
    }
}
