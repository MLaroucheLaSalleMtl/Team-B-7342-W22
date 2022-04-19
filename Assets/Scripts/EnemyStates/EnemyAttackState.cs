using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public void Exit(EnemyAi enemy)
    {
        enemy.anim.SetBool("Attack", false);
        //enemy.stateMachine.ChangeState(EnemyStateId.follow);
        //enemy.isAttacking = false;
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.attack;
    }

    public void Start(EnemyAi enemy)
    {
        enemy.isAttacking = true;/*
        enemy.isWalking = false;
        enemy.isFollowing = false;
        enemy.isIdleing = false;*/
        enemy.anim.SetBool("Walk", false);
        enemy.anim.SetBool("Attack", true);
    }

    public void Update(EnemyAi enemy)
    {
        //If player is out of attacking range
        if (!enemy.isAttacking)
            Exit(enemy);
        else
        {
            //Rotate towards player
            enemy.isFollowing = false;
            Vector3 lookDir = new Vector3(enemy.player.position.x - enemy.transform.position.x, 0, enemy.player.position.z - enemy.transform.position.z); //Get the position of the player
            Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
            enemy.transform.rotation = angle;
        }
    }
}
