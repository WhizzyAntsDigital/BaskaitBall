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
    [field: SerializeField] private bool isTraining = false;
    [field: SerializeField] private bool hasParticleEffect = false;
    [field: SerializeField] ScreenShake screenShake;

    public int scoreValue { get; private set; } = 0;
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
        screenShake = GetComponent<ScreenShake>();  

        if (GameManager.instance.isMainGame)
        {
            var colliders = new ClothSphereColliderPair[3];
            colliders[0] = new ClothSphereColliderPair(ArcadeLevel.Instance.ballsInScene[0].GetComponent<SphereCollider>());
            colliders[1] = new ClothSphereColliderPair(ArcadeLevel.Instance.ballsInScene[1].GetComponent<SphereCollider>());
            colliders[2] = new ClothSphereColliderPair(ArcadeLevel.Instance.ballsInScene[2].GetComponent<SphereCollider>());
            netComponent.sphereColliders = colliders;
        }
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
        if (!GameManager.instance.isGameOver)
        {
            if (other.gameObject.GetComponent<BallInput>().hasHitOtherObjects)
            {
                scoreValue += 2;
                UserDataHandler.instance.ReturnSavedValues().numberOfBaskets++;
                UserDataHandler.instance.SaveUserData();
                if (isTraining)
                {
                    startTimer = true;
                }
            }
            else
            {
                screenShake.Shake();
#if !UNITY_EDITOR
                if (!SettingsDataHandler.instance.ReturnSavedValues().vibrationDisabled)
                {
                    Handheld.Vibrate();
                }
#endif
                scoreValue += 3;
                UserDataHandler.instance.ReturnSavedValues().numberOfBaskets++;
                UserDataHandler.instance.ReturnSavedValues().numberOf3Pointers++;
                UserDataHandler.instance.SaveUserData();
                if (isTraining)
                {
                    startTimer = true;
                }
            }
            if (hasParticleEffect)
            {
                basketParticle.Play();
            }
            scoreText.text = scoreValue.ToString();
        }
    }
}
