// 2022-03-29   Mohammadreza Abolhassani    Spike trap modified to inherit from trap.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private float spikeTrapDamage = 20f;

    private BoxCollider spikeCollider; //BoxCollider for the SpikeTrap
    public bool playerOnTop = false;

    [SerializeField] private Animator spikeAnim;
 

    public new void Start()
    {
        spikeCollider = GetComponent<BoxCollider>();
        //get access to playerDamage script of the player
        playerDamage = GameManager.Instance.Player.GetComponent<PlayerDamage>();
        //start opening and closing the trap
        StartCoroutine(OpenCloseTrap());
    }

    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        if (playerOnTop)
        {
            playerDamage.TakeDamage(spikeTrapDamage);
            playerOnTop = false;
        }
        spikeAnim.SetTrigger("open");
        spikeCollider.isTrigger = false;
        //wait a few seconds;
        yield return new WaitForSeconds(openDuration);
        //play close animation;
        spikeAnim.SetTrigger("close");
        spikeCollider.isTrigger = true;
        //wait a few seconds;
        yield return new WaitForSeconds(closeDuration);
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
