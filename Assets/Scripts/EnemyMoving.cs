using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{


    //Declaring variables
    private Animator anim;
    public bool isPassive = true;
    //Moving speed of enemy variables
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float rotSpeed = 100f;
    //Wandering variables
    private bool isWandering = false;
    private bool isRotLeft = false;
    private bool isRotRight = false;
    private bool isWalking = false;


    //[SerializeField] 
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.instance.Player.GetComponent<Transform>();
    }

    private void Update()
    {
        //Keep checking if it's not attacking player
        if (isPassive)
        {
            //If it's not wandering
            if (!isWandering)
            {
                //Start the coroutine to make it wander
                StartCoroutine(Wander());
            }
            //Rotate right
            if (isRotRight)
            {
                transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
            }
            //Rotate left
            if (isRotLeft)
            {
                transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
            }
            //Make it walk
            if (isWalking)
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }
        }
    }

    IEnumerator Wander()
    {
        //Declaring Variables
        float rotTime = Random.Range(1, 2);
        float rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 3); //1 is left, 2 is right
        float walkTime = Random.Range(1, 3);
        
        //If not attacking player
        if (isPassive)
        {
            //Make it wander
            isWandering = true;

            #region Rotate
            //If turning left, make it true to turn
            //Rotate the amount of time in seconds
            //Animate using the walking animation
            //Then after turn, set to false
            if (rotateLorR == 1)
            {
                isRotRight = true;
                anim.SetBool("Walk", true);
                yield return new WaitForSeconds(rotTime);
                anim.SetBool("Walk", false);
                isRotRight = false;
            }
            //Same goes for right just like left
            if (rotateLorR == 2)
            {
                isRotLeft = true;
                anim.SetBool("Walk", true);
                yield return new WaitForSeconds(rotTime);
                anim.SetBool("Walk", false);
                isRotLeft = false;
            }
            #endregion

            #region Walking
            //Make the enemy walk
            //Animate, walk for the amount of seconds, then stop
            isWalking = true;
            anim.SetBool("Walk", true);
            yield return new WaitForSeconds(walkTime);
            isWalking = false;
            anim.SetBool("Walk", false);
            yield return new WaitForSeconds(rotateWait);
            #endregion
            //Make it not wander
            isWandering = false;
        }
    }
}
