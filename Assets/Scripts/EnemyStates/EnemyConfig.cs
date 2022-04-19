using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/Enemy Config", order = 0)]
public class EnemyConfig : ScriptableObject
{
    [Header("Follow Script")]
    public float speedMove = 2f; //Follow speed multiplier

    [Header("Wander Script")]
    public float moveSpeed = 1.5f; //Speed of enemy
    public float rotSpeed = 100f; //Speed of which it rotates

    [Header("Dead Script")]
    public int minForce = -6; //Min force of body part for death
    public int maxForce = 6; //Max force of body part for death

    [Header("Attacking")]
    public float damage = 5f; //Enemy Damage
    public float canHitAgainTime = 0.8f; //Timer between attacks
    public float dashSpeed = 100f; // Dash Speed

    [Header("Health")]
    public float HP_MAX = 100f;

    [Header("Damage")]
    public float knockback = 1.2f;
    public float knockbackDeath = 3f;
    
}
