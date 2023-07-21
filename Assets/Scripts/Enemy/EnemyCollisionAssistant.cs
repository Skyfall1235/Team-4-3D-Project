using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionAssistant : MonoBehaviour
{
    public TestEnemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponentInParent<TestEnemy>() != null)
        {
            enemyScript = GetComponentInParent<TestEnemy>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(enemyScript != null)
        {
            enemyScript.CollisionDetected(collision);
        }
    }
}
