using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyAi enemyAI;
    private PlayerDamage player;
    //[SerializeField] private string tagToAttack;

    private void Start()
    {
        enemyAI = GetComponentInParent<EnemyAi>();
        player = GameManager.Instance.Player.GetComponent<PlayerDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.IsDead)
        {
            enemyAI.isAttacking = true;
            enemyAI.stateMachine.ChangeState(EnemyStateId.attack);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyAI.isAttacking = false;
        }
    }
}
