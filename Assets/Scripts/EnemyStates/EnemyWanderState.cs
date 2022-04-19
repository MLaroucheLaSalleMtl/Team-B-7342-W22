using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyState
{

    Transform enemyTransform;

    public void Exit(EnemyAi enemy)
    {
        enemy.isIdleing = false;
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.wander;
    }

    public void Start(EnemyAi enemy)
    {
        enemy.isIdleing = true;
        enemyTransform = enemy.enemyTransform;
    }

    public void Update(EnemyAi enemy)
    {
        //Keep checking if it's not attacking player
        if (enemy.isIdleing && !enemy.enemyHealth.IsDead)
        {
            //If it's not wandering
            if (!enemy.isWalking && !enemy.hitWall)
            {
                //Start the coroutine to make it wander
                enemy.enemyWander.RoutineWander(enemy);
            }
            //Rotate right
            if (enemy.enemyWander.isRotRight)
            {
                enemyTransform.Rotate(enemyTransform.up * Time.deltaTime * enemy.config.rotSpeed);
            }
            //Rotate left
            if (enemy.enemyWander.isRotLeft)
            {
                enemyTransform.Rotate(enemyTransform.up * Time.deltaTime * -enemy.config.rotSpeed);
            }
            //Make it walk
            if (enemy.enemyWander.isWalking)
            {
                enemyTransform.position += enemyTransform.forward * enemy.config.moveSpeed * Time.deltaTime;
            }
        }
    }
}
