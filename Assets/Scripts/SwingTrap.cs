using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTrap : MonoBehaviour
{
    [SerializeField] private float swingTrapDamage = 100f;

    public Animator swingTrapAnim; //Animator for the swing trap

    private PlayerDamage playerDamage;


    // Use this for initialization
    void Awake()
    {
        swingTrapAnim = GetComponent<Animator>();
        StartCoroutine(OpenCloseTrap());
    }

    private void Start()
    {
        playerDamage = GameManager.Instance.Player.GetComponent<PlayerDamage>();
    }

    IEnumerator OpenCloseTrap()
    {
        swingTrapAnim.SetBool("Active", true);
        //wait 4 seconds;
        yield return new WaitForSeconds(4);
        swingTrapAnim.SetBool("Active", false);
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDamage.TakeDamage(swingTrapDamage);
        }
    }
}