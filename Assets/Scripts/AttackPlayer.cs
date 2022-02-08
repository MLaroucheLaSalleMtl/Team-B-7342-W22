using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    private Animator anim;
   
    private void Start()
    {
        anim = enemy.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.GetComponent<EnemyMoving>().isPassive = false;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.GetComponent<EnemyMoving>().isPassive = true;
            anim.SetBool("Attack", false);
            anim.SetBool("Walk", true);
        }
    }
}
