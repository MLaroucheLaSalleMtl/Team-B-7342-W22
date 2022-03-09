using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public GameObject player;
    public float damage = 10f;

    bool canHit = true;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHit) 
        {
            player.GetComponent<PlayerDamage>().TakeDamage(damage);
            canHit = false;
            Invoke("CanHitAgain", 0.8f);
        }
    }

    void CanHitAgain()
    {
        canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
