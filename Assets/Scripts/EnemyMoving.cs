using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    //Enemy Life
    public bool isAlive = true; //Check if it's alive
    EnemyDead dead;
    EnemyDamage dmg;

    //Declaring variables
    private Animator anim; //Enemy animator
    public bool isPassive = true; //Checks if it's passive
    //public bool isAttacked = false; //Checks if it's under attack
    public bool hitWall = false; //Checkf if it hit a wall

    //Moving speed of enemy variables
    [SerializeField] private float moveSpeed = 1.5f; //Speed of enemy
    [SerializeField] private float rotSpeed = 100f; //Speed of which it rotates
    
    //Wandering variables
    private bool isWandering = false; //Wether it is wandering
    private bool isRotLeft = false; //See if it rotates left
    private bool isRotRight = false; //See if it rotates right
    private bool isWalking = false; //See if it is walking

    //[SerializeField] 
    private Transform player; //Get the player

    // Start is called before the first frame update
    void Start()
    {
        dmg = GetComponent<EnemyDamage>();
        dead = GetComponent<EnemyDead>();
        anim = GetComponent<Animator>();
        player = GameManager.Instance.Player.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        //Keep checking if it's not attacking player
        if (isPassive && isAlive)
        {
            //If it's not wandering
            if (!isWandering && !hitWall)
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
        if (!isAlive)
        {
            dead.DeadCheckWall();
        }
    }

    public IEnumerator RotateAround()
    {
        hitWall = true;
        isWalking = false;
        isRotLeft = true;
        anim.SetBool("Walk", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Walk", false);
        isRotLeft = false;
        hitWall = false;
    }

    IEnumerator Wander()
    {
        //Declaring Variables
        float rotTime = Random.Range(1, 2);
        float rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 3); //1 is right, 2 is left
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
