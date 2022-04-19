using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("FSM")]
    public EnemyStateMachine stateMachine;
    public EnemyStateId initialState;
    public EnemyStateId current;

    [Header("Enemy Scripts")]
    public EnemyWandering enemyWander;
    public EnemyHealth enemyHealth;
    public EnemyAttack enemyAttack;
    public EnemyFollow enemyFollow;
    public EnemyDead enemyDead;
    public EnemyDash enemyDash;

    [Header("Own Components")]
    public Animator anim;
    public Transform player;
    public EnemyConfig config;
    public Transform enemyTransform;

    [Header("Action Variables")]
    public bool isAttacking = false;
    public bool isIdleing = true;
    public bool isWalking = false;
    public bool hitWall = false;
    public bool isDashing = false;
    public bool isFollowing = false;
    public bool isAttacked = false;

    // Start is called before the first frame update
    void Start()
    {       
        anim = GetComponent<Animator>();
        player = GameManager.Instance.Player;
        enemyWander = GetComponent<EnemyWandering>();
        enemyHealth = GetComponent<EnemyHealth>();
        enemyDead = GetComponent<EnemyDead>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        enemyFollow = GetComponentInChildren<EnemyFollow>();
        enemyTransform = GetComponent<Transform>();
        enemyDash = GetComponentInChildren<EnemyDash>();

        enemyHealth.Heal();

        initialState = EnemyStateId.wander;
        stateMachine = new EnemyStateMachine(this);
        stateMachine.RegisterState(new EnemyWanderState());
        stateMachine.RegisterState(new EnemyFollowState());
        stateMachine.RegisterState(new EnemyAttackState());
        stateMachine.RegisterState(new EnemyDeadState());

        stateMachine.ChangeState(initialState);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!enemyHealth.IsDead)
        {
            stateMachine.Update();
        }
        else
        {
            stateMachine.ChangeState(EnemyStateId.dead);
        }
    }
}
