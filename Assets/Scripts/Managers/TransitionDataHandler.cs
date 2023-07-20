using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDataHandler : MonoBehaviour
{
    public static TransitionDataHandler Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void SaveDataForLoad()
    {

    }
}
