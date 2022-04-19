using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private BloodSmear smear;
    //Particle Blood
    [SerializeField] private GameObject blood; //Blood Particle
    GameObject newBlood;
    //[SerializeField] private float bloodOffsetY = 1.1f; //Y Coordinates of blood on the enemy

    private PlayerDamage player;

    [SerializeField] private float bleedingDuration = 0.8f;

    [SerializeField] private Image imageForHP;
    [SerializeField] private Image parentImage;

    private EnemyAi enemy;
    public float hp;

    private void Start()
    {
        player = GameManager.Instance.Player.GetComponent<PlayerDamage>();
        Heal();
    }
    public bool IsDead { get => enemy.enemyHealth.hp <= 0f; }

    // Start is called before the first frame update
    void Awake()
    {
        enemy = GetComponent<EnemyAi>();
    }

    public void Heal()
    {
        hp = enemy.config.HP_MAX;
        imageForHP.fillAmount = hp / enemy.config.HP_MAX;
    }

    public void TakeDamage(float damage)
    {
        if (!IsDead)
        {
            hp -= damage;
            imageForHP.fillAmount = hp / enemy.config.HP_MAX;

            if (IsDead)
            {
                Destroy(parentImage);
            }

            enemy.isAttacked = true;
            smear.Splat();
            enemy.GetComponent<Rigidbody>().AddForce(-transform.forward * enemy.config.knockback, ForceMode.Impulse);
            Invoke("StopAttack", bleedingDuration);
        }
    }

    private void StopAttack()
    {
        enemy.isAttacked = false;
        Destroy(newBlood);
    }

    private void Update()
    {
        if (!player.IsDead && enemy.isAttacked)
        {
            Vector3 psPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Quaternion rot = Quaternion.LookRotation(psPos);
            newBlood = Instantiate(blood, psPos, rot);
        }
    }
}
