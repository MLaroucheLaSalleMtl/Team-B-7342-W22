using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    [SerializeField] EnemyAi enemy;
    public bool hitWall = false;
    public bool isExploded = false;

    //Get all the body Parts
    [SerializeField] private GameObject wallCheck;
    [SerializeField] private GameObject original;
    [SerializeField] private List<GameObject> lsBodyParts = new List<GameObject>();

    private void Awake()
    {
        enemy = GetComponent<EnemyAi>();
    }

    public void EnableCheck()
    {
        wallCheck.SetActive(true);
    }

    public void DeadCheckWall()
    {
        enemy.GetComponent<Rigidbody>().AddForce(-transform.forward * enemy.config.knockbackDeath, ForceMode.Impulse);
        Invoke("Dead", 1f);
    }

    public void Dead()
    {
        if (!isExploded)
        {
            isExploded = true;
            SwapModels();
            Unparent();
            Explode();
            Invoke("DestroyEnemy", 3f);
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        foreach (GameObject gb in lsBodyParts)
        {
            Destroy(gb);
        }
    }

    void SwapModels()
    {
        original.SetActive(false);
        gameObject.layer = 8;
        foreach (GameObject gb in lsBodyParts)
        {
            gb.SetActive(true);
        }
    }

    void Unparent()
    {
        foreach (GameObject gb in lsBodyParts)
        {
            gb.transform.parent = null;
        }
    }

    void Explode()
    {
        Rigidbody rb;
        foreach (GameObject gb in lsBodyParts)
        {
            rb = gb.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * Random.Range(enemy.config.minForce, enemy.config.maxForce), ForceMode.Impulse);
            rb.AddForce(transform.forward * Random.Range(enemy.config.minForce, enemy.config.maxForce), ForceMode.Impulse);
        }
    }
}
