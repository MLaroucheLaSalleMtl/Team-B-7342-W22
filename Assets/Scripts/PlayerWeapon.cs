using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private float weaponDamage = 10f;

    private EnemyDamage enemy;
    private BossDamage boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyDamage>(out enemy))
            {
                enemy.TakeDamage(weaponDamage);
            }
            else if (other.TryGetComponent<BossDamage>(out boss))
            {
                boss.TakeDamage(weaponDamage);
            }
        }
    }
}
