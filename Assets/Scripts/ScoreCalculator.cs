using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator instance;
    
    [field: Header("Score Calculation Stuff")]
    [field: SerializeField] TextMeshProUGUI scoreText;
    [field: SerializeField] ParticleSystem basketParticle;
    [field: SerializeField] MoveBasket moveBasket;
    [field: SerializeField] float timeToMoveBasket = 1f;
    [field: SerializeField] Cloth netComponent;

    private int scoreValue = 0;
    private float timer = 0f;
    private bool startTimer = false;

    private void Awake()
    {
        instance = this;
    }

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
        basketParticle.Play();
        scoreText.text = scoreValue.ToString();
    }
}
