using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadCheck : MonoBehaviour
{
    public EnemyAi enemy;

    private void OnTriggerEnter(Collider collider)
    {
        enemy.enemyDead.Dead();        
    }
}
