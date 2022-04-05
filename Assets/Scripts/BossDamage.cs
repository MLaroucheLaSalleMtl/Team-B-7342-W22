using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private Image imageForHP;
    [SerializeField] private Image parentImage;
    

    [SerializeField] private float HP_MAX = 1000f;
    private float hp;
    public bool IsDead { get => hp <= 0f; }

    // Start is called before the first frame update
    void Start()
    {
        Heal();
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
                Destroy(parentImage);
                
                Invoke("DestroyEnemy", 5f);
            }
           
        }
    }

  
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
       

    }
}
