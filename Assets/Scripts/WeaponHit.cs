using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public GameObject player;
    public float damage = 10f;
    const float hitAgain = 0.8f;
    private float dmgMod = 1.2f;

    [SerializeField] private EnemyDash dash;

    bool canHit = true;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHit) 
        {
            float dmg = 0f;
            if (dash.Dashing)
            {
                dmg = damage * dmgMod;
            }
            else
            {
                dmg = damage;
            }
            player.GetComponent<PlayerDamage>().TakeDamage(dmg);
            canHit = false;
            Invoke("CanHitAgain", hitAgain);
        }
    }

    void CanHitAgain()
    {
        canHit = true;
    }
}
