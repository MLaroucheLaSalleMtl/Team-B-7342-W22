using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    private EnemyAi enemyAI;

    //public bool inSight = false;

    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAi>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StopCoroutine(enemyAI.enemyWander.Wander(enemyAI));
            enemyAI.anim.SetBool("Walk", true);
            enemyAI.isAttacking = false;
            enemyAI.isWalking = false;
            enemyAI.isIdleing = false;
            enemyAI.stateMachine.ChangeState(EnemyStateId.follow);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyAI.isAttacking)
            {
                enemyAI.stateMachine.ChangeState(EnemyStateId.attack);
            }
            else
            {
                enemyAI.stateMachine.ChangeState(EnemyStateId.wander);
            }
        }
    }
}