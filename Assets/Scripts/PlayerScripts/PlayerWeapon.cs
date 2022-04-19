// 2022-04-18   Mohammadreza Abolhassani    Merged PlayerWeaponHit into PlayerWeapon
// 2022-04-18   Sean Hall                   Added bool to allow attack to go off even if trigger does not exit foe

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float weaponDamage = 20f;
    //[SerializeField] private float hitTimer = 0.8f;

    private EnemyHealth enemy;
    private BossDamage boss;
    private PlayerAttack attack;

    // Used by PlayerAttack to allow hits to trigger on follow-up attacks if the chainsaw does not OnTriggerExit
    public bool stillInside = false; 

    private void Start()
    {
        attack = GameManager.Instance.Player.GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && attack.isAttacking)
        {
            if (other.TryGetComponent<EnemyHealth>(out enemy))
            {
                enemy.TakeDamage(weaponDamage);
            }
            else if (other.TryGetComponent<BossDamage>(out boss))
            {
                boss.TakeDamage(weaponDamage);
            }
            EventManager.current.WeaponTriggerEnter();
            stillInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EventManager.current.WeaponTriggerExit();
            stillInside = false;
        }
    }

}
