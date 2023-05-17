using UnityEngine;

public class ArcadeLevel : MonoBehaviour
{
    public static ArcadeLevel Instance;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float speedOfMovingToSpawn;
    [SerializeField] private GameObject[] ballsInScene;
    private bool hitTrigger = false;
    private GameObject ballObj;
    private int ballID = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.instance.onGameOver += () => { hitTrigger = false; };
    }
    private void Update()
    {
        if(hitTrigger)
        {
                float step = speedOfMovingToSpawn * Time.deltaTime;
            ballObj.transform.position = Vector3.MoveTowards(ballObj.transform.position, spawnPoint.transform.position, step);
            if(ballObj.transform.position.x == spawnPoint.transform.position.x)
            {
                hitTrigger = false;
                ballObj.GetComponent<Rigidbody>().isKinematic = true;
                ballObj.GetComponent<BallInput>().hasGotInput = false;
                ballObj = null;
            }
        }
    }

    public void MoveBallToCentre()
    {
        ballID++;
        if(ballID >= ballsInScene.Length)
        { ballID = 0; }
        ballObj = ballsInScene[ballID];
        hitTrigger = true;
    }
}
