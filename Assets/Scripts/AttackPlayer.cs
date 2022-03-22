using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    private PlayerDamage player;
    private Animator anim;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamage>();
        anim = enemy.GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.IsDead)
        {
            /* if (enemy.GetComponent<EnemyDash>().dashing)
            {
                 player.TakeDamage(25f);
            }
            else
            {*/
            enemy.GetComponent<EnemyMoving>().isPassive = false;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);
            //}
        }
        else
        {
            StartCoroutine(enemy.GetComponentInParent<EnemyMoving>().RotateAround());
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
