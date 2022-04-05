using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    wander,
    run,
    attack,
    follow,
    hurt,
    wait
}

public class EnemyFSM : MonoBehaviour
{
    private Animator anim;
    private EnemyMoving enemyMove;
    private Transform player;
    private LookAtPlayer look;
    public EnemyState currentState = global::EnemyState.idle;

    float speedMove = 1.5f;
    float distancePlayer = 3f;

    private bool follow = false;

    // Update is called once per frame
    void Update()
    {
        EnemyState();
        enemyMove = GetComponent<EnemyMoving>();
        anim = GetComponent<Animator>();
        player = GameManager.Instance.Player;
        look = GetComponentInChildren<LookAtPlayer>();
    }

    public void EnemyState()
    {
        switch (currentState)
        {
            case global::EnemyState.idle:
                Idle();
                break;
            case global::EnemyState.attack:
                follow = false;
                Attack();
                break;
            case global::EnemyState.follow:
                Follow();
                break;
            case global::EnemyState.wait:
                follow = false;
                Wait();
                break;
            case global::EnemyState.wander:
                follow = false;
                Wander();
                break;
        }
    }

    private void Idle()
    {
        follow = false;
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
        Invoke("Wander", 1f);
    }

    private void Attack()
    {
        if (currentState == global::EnemyState.attack)
        {
            follow = false;
            enemyMove.isPassive = false;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            Wait();
        }
        else
        {
            anim.SetBool("Attack", false);
            Idle();
        }
    }

    private void Follow()
    {
        follow = true;
        anim.SetBool("Walk", true);
    }

    private void Wait()
    {
        Invoke("Attack", 0.5f);
    }

    private void Wander()
    {
        enemyMove.isPassive = true;
    }

    private void FixedUpdate()
    {
        if (follow)
        {
            Vector3 lookDir = new Vector3(player.position.x - transform.position.x, 0, player.position.z - transform.position.z); //Get the position of the player
            Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
            transform.rotation = angle;
            Vector3 nextToPlayer = new Vector3(player.transform.position.x - distancePlayer, player.transform.position.y - 1.2f, player.transform.position.z - distancePlayer);
            if (transform.transform.position == nextToPlayer)
            {
                return;
            }
            else
            {
                transform.transform.position += transform.transform.forward * speedMove * Time.deltaTime;
            }
        }
    }
}
