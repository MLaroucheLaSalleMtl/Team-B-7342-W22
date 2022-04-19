// 2022-04-03   Mohammadreza Abolhassani    Created the script 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class BossEnemy : MonoBehaviour
{
    [SerializeField] private float followDistance = 7f;

    private NavMeshAgent _agent;
    private Animator _anim;

    private Transform target;
    private Vector3 initialPosition;

    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackInterval = 3f;
    private bool canAttack = true;

    private BossDamage _bossDmg;

    public bool IsAttacking { get => !canAttack; }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();

        _bossDmg = GetComponent<BossDamage>();

        target = GameManager.Instance.Player;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bossDmg.IsDead)
        {
            Destroy(this);
        }

        float distance = Vector3.Distance(transform.position, target.position);
        
        if (distance < attackDistance) //if in attack range
        {
            if (canAttack) //if not cooling down
            {
                _agent.SetDestination(transform.position); //stop moving
                //transform.LookAt(target); //face target
                Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
                target.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime);
                
                _anim.SetTrigger("Attack"); //trigger Attack animation
                canAttack = false; //start of cool down period
                Invoke("EnableAttack", attackInterval); //end cool down period after specified amount of time
            }
        }
        else if (distance < followDistance) //if in following range
        {
            _agent.SetDestination(target.position); //start following target
        }
        else //if not in following range
        {
            _agent.SetDestination(initialPosition); //go back to your nest
        }

        //if moving, set the boolean Walk of the animator to true
        _anim.SetBool("Walk", (_agent.velocity != Vector3.zero));
    }

    void EnableAttack()
    {
        canAttack = true;
    }
}
