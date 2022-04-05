// 2022-03-29   Mohammadreza Abolhassani    Swing trap modified to inherit from trap.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrap : Trap
{
    [SerializeField] private float swingTrapDamage = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        if (isOpen && collision.gameObject.CompareTag("Player"))
        {
            playerDamage.TakeDamage(swingTrapDamage);
        }
    }
}