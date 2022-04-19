using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyState
{


    public void Exit(EnemyAi enemy)
    {
        enemy.anim.SetBool("Attack", false);
        enemy.isFollowing = false;
    }

    public EnemyStateId GetId()
    {
        return EnemyStateId.follow;
    }

    public void Start(EnemyAi enemy)
    {
        enemy.anim.SetBool("Walk", true);
        enemy.isFollowing = true;
        enemy.enemyWander.StopWandering(enemy);
    }

    public void Update(EnemyAi enemy)
    {
        if (enemy.isFollowing)
        {
            Vector3 lookDir = new Vector3(enemy.player.position.x - enemy.transform.position.x, 0, enemy.player.position.z - enemy.transform.position.z); //Get the position of the player
            Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
            enemy.transform.rotation = angle;
            enemy.transform.position += enemy.transform.forward * enemy.config.speedMove * Time.deltaTime;
        }
    }
}
