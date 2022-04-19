using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHit : MonoBehaviour
{
    public EnemyAi enemyAi;
    public PlayerDamage playerDamage;

    bool canHit = true;

    private void Start()
    {
        playerDamage = GameManager.Instance.Player.GetComponent<PlayerDamage>();
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHit)
        {
            playerDamage.TakeDamage(enemyAi.config.damage);
            canHit = false;
            Invoke("CanHitAgain", 0.8f);
        }
    }

    void CanHitAgain()
    {
        canHit = true;
    }

}
