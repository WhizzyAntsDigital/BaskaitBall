using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketTrigger : MonoBehaviour
{
    [SerializeField] TutorialController tutorialController;
    private void OnTriggerEnter(Collider other)
    {
        tutorialController.OnBallEnterBasket();
    }
}
