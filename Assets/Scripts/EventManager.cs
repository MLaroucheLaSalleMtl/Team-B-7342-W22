// 2022-04-16   Sean Hall       Created event manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager current;

    // Event actions
    public event Action onWeaponTriggerEnter;
    public event Action onWeaponTriggerExit;

    //public delegate void AttackHit(float f);
    //public static event AttackHit onAttackHit;

    // Timer Events
    public event Action onAttackStart;
    public event Action onAttackFinish;

    // New Zone Events (i.e. Music)
    public event Action onNewZone;

    private void Awake()
    {
        // Singleton
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
            Destroy(gameObject);
        }
    }

    
    public void WeaponTriggerEnter()
    {
        // When invoking, ensure not null        
        if (onWeaponTriggerEnter != null)
        {
            onWeaponTriggerEnter();
        }
    }

    public void WeaponTriggerExit()
    {
        if (onWeaponTriggerExit != null)
        {
            onWeaponTriggerExit();
        }
    }

    public void AttackStart()
    {
        if (onAttackStart != null)
        {
            onAttackStart();
        }
    }

    public void AttackFinish()
    {
        if (onAttackFinish != null)
        {
            onAttackFinish();
        }
    }

    public void NewZone()
    {
        if (onNewZone != null)
        {
            onNewZone();
        }
    }
}
