using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator instance;
    public TextMeshProUGUI scoreText;
    public ParticleSystem explosion;
    private int scoreValue = 0;
    public Cloth netComponent;
    public MoveBasket moveBasket;
    public float timeToMoveBasket = 1f;
    private float timer = 0f;
    private bool startTimer = false;
    public GameObject ring;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        scoreValue = 0;
        scoreText.text = scoreValue.ToString();
    }
    private void Update()
    {
        if(startTimer)
        {
            timer += Time.deltaTime;
            if(timer >= timeToMoveBasket)
            {
                moveBasket.MoveBasketNow();
                startTimer = false;
                timer = 0f;
            }
        }
    }

    public void AssignCollider(SphereCollider ball)
    {
        var colliders = new ClothSphereColliderPair[1];
        colliders[0] = new ClothSphereColliderPair(ball);
        netComponent.sphereColliders = colliders;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BallInput>().hasHitOtherObjects)
        {
            scoreValue++;
            startTimer = true;
        }
        else
        {
            scoreValue += 3;
            startTimer = true;
        }
        explosion.Play();
        scoreText.text = scoreValue.ToString();
    }
}
