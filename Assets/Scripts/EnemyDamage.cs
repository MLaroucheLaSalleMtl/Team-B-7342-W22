using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private Image imageForHP;
    [SerializeField] private Image parentImage;
    [SerializeField] private float bleedingDuration = 0.8f;
    [SerializeField] private float knockback = 5f;
    private EnemyMoving enemyMoving;

    private Rigidbody rb;

    const float HP_MAX = 100f;
    private float hp;
    public bool IsDead { get => hp <= 0f; }

    // Start is called before the first frame update
    void Start()
    {
        Heal();
        enemyMoving = GetComponent<EnemyMoving>();
        rb = GetComponent<Rigidbody>();
    }

    public void Heal()
    {
        hp = HP_MAX;
        imageForHP.fillAmount = hp / HP_MAX;
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            imageForHP.fillAmount = hp / HP_MAX;

            if (IsDead)
            {
                rb.AddForce(-transform.forward * knockback * 1.2f, ForceMode.Impulse);
                Destroy(parentImage);
                //GetComponent<BoxCollider>().isTrigger = true;
                GetComponent<EnemyMoving>().isAlive = false;
                Invoke("DestroyEnemy", 5f);
            }
            if (enemyMoving)
            {
                rb.AddForce(-transform.forward * knockback, ForceMode.Impulse);
                enemyMoving.isAttacked = true;
                Invoke("StopBleeding", bleedingDuration);
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void StopBleeding()
    {
        if (enemyMoving)
        {
            enemyMoving.isAttacked = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
