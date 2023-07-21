using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneTransitionAnimationHandler : MonoBehaviour
{
    Animator animator;
    string sceneToTransitionTo;
    private void Start()
    {
        if(GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
    }
    public void StartTransition(string sceneName)
    {
        sceneToTransitionTo = sceneName;
        if(animator != null)
        {
            animator.SetTrigger("Change Scene");
        }
    }
    void FinishedTransitionAnimation()
    {
        if(TransitionDataHandler.Instance!= null)
        {
            TransitionDataHandler.Instance.UpdateData();
        }
        SceneManager.LoadScene(sceneToTransitionTo);
    }
}
