using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHorn : MonoBehaviour
{
    [SerializeField] private float hornDamage = 3f;

    private BossEnemy boss;
    private PlayerDamage player;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<BossEnemy>();
        player = GameManager.Instance.Player.GetComponent<PlayerDamage>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (boss.IsAttacking)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player.TakeDamage(hornDamage);
            }
        }
    }
}
