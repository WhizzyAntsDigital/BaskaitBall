using UnityEngine;

public class ArcadeLevel : MonoBehaviour
{
    public static ArcadeLevel Instance;
    [field: SerializeField] private GameObject spawnPoint;
    [field: SerializeField] private float speedOfMovingToSpawn;
    [field: SerializeField] public GameObject[] ballsInScene { get; private set; }
    public int ballID { get; private set; } = 0;
    private bool hitTrigger = false;
    private GameObject ballObj;

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
                if(!GameManager.instance.isMainGame && GameManager.instance.dontActivateInput)
                {
                    ballObj.GetComponent<BallInput>().hasGotInput = true;
                }
                else if(!GameManager.instance.isMainGame && !GameManager.instance.dontActivateInput)
                {
                    ballObj.GetComponent<BallInput>().hasGotInput = false;
                }
                else
                {
                    ballObj.GetComponent<BallInput>().hasGotInput = false;
                }
                ballObj = null;
            }
        }
    }

    public void MoveBallToCentre()
    {
        ballID++;
        ballID = ballID % ballsInScene.Length;
        ballObj = ballsInScene[ballID];
        hitTrigger = true;
    }
}
