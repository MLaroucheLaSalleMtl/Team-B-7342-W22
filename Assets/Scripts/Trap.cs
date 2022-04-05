// 2022-03-29   Mohammadreza Abolhassani    Trap class created as a parent class for other traps to inherit from, but will also work on it's own for a simple trap like trapDoor.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Animator anim; //Animator for the trap


    [Range(0.01f, 10f)] [SerializeField] protected float openDuration = 2f;
    [Range(0.01f, 10f)] [SerializeField] protected float closeDuration = 2f;

    protected bool isOpen = false;
    protected PlayerDamage playerDamage;


    // Start is called before the first frame update
    public void Start()
    {
        //get the Animator component from the trap
        anim = GetComponent<Animator>();
        //get access to playerDamage script of the player
        playerDamage = GameManager.Instance.Player.GetComponent<PlayerDamage>();
        //start opening and closing the trap
        StartCoroutine(OpenCloseTrap());
    }

    IEnumerator OpenCloseTrap()
    {
        //play open animation;
        anim.SetTrigger("open");
        isOpen = true;
        //wait for a number of seconds;
        yield return new WaitForSeconds(openDuration);
        //play close animation;
        anim.SetTrigger("close");
        isOpen = false;
        //wait  for a number of seconds;
        yield return new WaitForSeconds(closeDuration);
        //Do it again;
        StartCoroutine(OpenCloseTrap());
    }
}
