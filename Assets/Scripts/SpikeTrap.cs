using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private float spikeTrapDamage = 20f;

    [SerializeField] private Animator spikeTrapAnim; //Animator for the SpikeTrap;
    private BoxCollider spikeCollider; //BoxCollider for the SpikeTrap;
    public bool playerOnTop = false;
    private PlayerDamage playerDamage;

    // Use this for initialization
    void Awake()
    {    
        spikeCollider = GetComponent<BoxCollider>();

        StartCoroutine(OpenCloseTrap());
    }

    private void Start()
    {
        playerDamage = GameManager.Instance.Player.GetComponent<PlayerDamage>();
    }

    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        if (playerOnTop)
        {
            playerDamage.TakeDamage(spikeTrapDamage);
            playerOnTop = false;
        }
        spikeTrapAnim.SetTrigger("open");
        spikeCollider.isTrigger = false;
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //play close animation;
        spikeTrapAnim.SetTrigger("close");
        spikeCollider.isTrigger = true;
        //wait 2 seconds;
        yield return new WaitForSeconds(2);
        //Do it again;
        StartCoroutine(OpenCloseTrap());

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnTop = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnTop = false;
        }
    }
}
