using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossDamage : MonoBehaviour
{
    [SerializeField] private Image imageForHP;
    [SerializeField] private Image parentImage;
    
    //Death
    [SerializeField] private GameObject original;
    [SerializeField] private List<GameObject> lsBodyParts = new List<GameObject>();
    [SerializeField] private List<GameObject> lsOriginalBody = new List<GameObject>();

    private int minForce = -10;
    private int maxForce = 10;

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
                GameManager.Instance.UpdateGameState(GameState.Win);
                DeathOfBoss();
            }
        }
    }
  
    private void DeathOfBoss()
    {
        //Swap
        foreach (GameObject gb in lsOriginalBody)
        {
            gb.SetActive(false);
        }
        gameObject.layer = 8;
        foreach (GameObject gb in lsBodyParts)
        {
            gb.SetActive(true);
        }

        //Unparent
        foreach (GameObject gb in lsBodyParts)
        {
            gb.transform.parent = null;
        }

        //Explode
        Rigidbody rb;
        foreach (GameObject gb in lsBodyParts)
        {
            rb = gb.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
            rb.AddForce(transform.forward * Random.Range(minForce, maxForce), ForceMode.Impulse);
        }
        Invoke("DestroyEnemy", 5f);
    }

    private void DestroyEnemy()
    {
        
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneLoader.Instance.LoadNextScene();
        //Destroy(gameObject);
    }
}
