//Ariel, 18-04-2022: Fixed player exploding when dead

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private Image imageHp;
    [SerializeField] private Image parentImage;

    private int minForce = -6;
    private int maxForce = 6;

    [SerializeField] private GameObject original;
    public List<GameObject> bodyParts = new List<GameObject>();

    public float hp;
    [SerializeField] private float maxHp = 200f;

    public bool IsDead { get => hp <= 0f; }

    // Start is called before the first frame update
    void Start()
    {
        Heal();
    }

    public void Heal()
    {
        hp = maxHp;
        imageHp.fillAmount = hp / maxHp;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        imageHp.fillAmount = hp / maxHp;
        if (IsDead)
        {
            Debug.Log("Player is dead!");
            PlayerExplode();
        }
        else
        {
            Debug.Log("Player took damage");
            
        }
    }

    private void PlayerExplode()
    {
        //Swap
        original.SetActive(false);
        gameObject.layer = 8;
        foreach (GameObject gb in bodyParts)
        {
            gb.SetActive(true);
        }

        //Unparent
        foreach (GameObject gb in bodyParts)
        {
            gb.transform.parent = null;
        }

        //Explode
        Rigidbody rb;
        foreach (GameObject gb in bodyParts)
        {
            rb = gb.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
            rb.AddForce(transform.forward * Random.Range(minForce, maxForce), ForceMode.Impulse);
        }
        GameManager.Instance.UpdateGameState(GameState.Death);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(maxHp * 0.1f * Time.deltaTime);
           
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            TakeDamage(maxHp);
        }
    }
}
