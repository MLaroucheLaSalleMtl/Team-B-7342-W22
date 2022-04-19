using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public void Exit(EnemyAi enemy)
    {
       
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.dead;
    }

    public void Start(EnemyAi enemy)
    {
        enemy.isAttacked = false;
        enemy.isAttacking = false;
        enemy.isDashing = false;
        enemy.isFollowing = false;
        enemy.isIdleing = false;
        enemy.isWalking = false;
        enemy.enemyDead.EnableCheck();
        enemy.enemyDead.DeadCheckWall();
    }

    public void Update(EnemyAi enemy)
    {

    }
}
