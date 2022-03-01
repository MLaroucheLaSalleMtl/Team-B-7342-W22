using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    private EnemyMoving enemy;

    public bool isExploded = false;

    //Get all the body Parts
    #region Body_Parts
    [SerializeField] private GameObject original;
    [SerializeField] private GameObject arms;
    [SerializeField] private GameObject antenna;
    [SerializeField] private GameObject collar;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject legs;
    [SerializeField] private GameObject wings;
    [SerializeField] private GameObject sword;
    private List<GameObject> lsBodyParts = new List<GameObject>();
    #endregion

    [SerializeField] private int minForce = -6; //Min force of body part for death
    [SerializeField] private int maxForce = 6; //Max force of body part for death

    private void Start()
    {
        enemy = GetComponent<EnemyMoving>();

        lsBodyParts.Add(arms);
        lsBodyParts.Add(antenna);
        lsBodyParts.Add(collar);
        lsBodyParts.Add(body);
        lsBodyParts.Add(head);
        lsBodyParts.Add(legs);
        lsBodyParts.Add(wings);
        lsBodyParts.Add(sword);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enemy.isAlive)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Dead();
            }
        }
    }

    public void DeadCheckWall()
    {
        enemy.isAlive = false;
        Invoke("Dead", 0.6f);
    }

    void Dead()
    {
        if (!isExploded)
        {
            isExploded = true;
            SwapModels();
            Unparent();
            Explode();
            enemy.isAttacked = false;
        }
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
            rb.AddForce(transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
            rb.AddForce(transform.forward * Random.Range(minForce, maxForce), ForceMode.Impulse);
        }
        //Instantiate(blood, transform.position, Quaternion.identity);

    }
}
