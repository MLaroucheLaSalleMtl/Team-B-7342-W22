using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private BloodSmear smear;
    //Particle Blood
    [SerializeField] private GameObject blood; //Blood Particle
    //[SerializeField] private float bloodOffsetY = 1.1f; //Y Coordinates of blood on the enemy

    public PlayerDamage player;

    [SerializeField] private Image imageForHP;
    [SerializeField] private Image parentImage;
    [SerializeField] private float bleedingDuration = 0.8f;
    [SerializeField] private float knockback = 5f;
    public bool isAttacked = false;
    private float knockbackDead = 1.2f;
    private EnemyMoving enemyMoving;

    private Rigidbody rb;

    [SerializeField] private float HP_MAX = 100f;
    private float hp;
    public bool IsDead { get => hp <= 0f; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamage>();
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
                rb.AddForce(-transform.forward * knockback * knockbackDead, ForceMode.Impulse);
                Destroy(parentImage);
                //GetComponent<BoxCollider>().isTrigger = true;
                GetComponent<EnemyMoving>().isAlive = false;
                Invoke("DestroyEnemy", 5f);
            }
            if (enemyMoving)
            {
                isAttacked = true;
                smear.Splat();
                rb.AddForce(-transform.forward * knockback, ForceMode.Impulse);
                Invoke("StopAttack", bleedingDuration);
            }
        }
    }

    private void StopAttack()
    {
        isAttacked = false;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!player.IsDead)
        {
            if (isAttacked)
            {
                Vector3 psPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                Quaternion rot = Quaternion.LookRotation(psPos);
                GameObject newBlood = Instantiate(blood, psPos, rot);
                Destroy(newBlood, bleedingDuration);
            }
        }
        else
        {
            isAttacked = false;
        }
        
    }
}
