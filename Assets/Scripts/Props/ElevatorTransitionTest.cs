using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTransitionTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.Instance != null && GameManager.Instance.currentPlayer != null && other.gameObject == GameManager.Instance.playerCharacterTransform.gameObject && FindAnyObjectByType<SceneTransitionAnimationHandler>() != null)
        {
            FindAnyObjectByType<SceneTransitionAnimationHandler>().StartTransition("LevelTwo scene");
        }
    }
}
