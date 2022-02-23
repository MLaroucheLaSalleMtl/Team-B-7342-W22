using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    //Declaring variables
    //Declaring variables
    private Animator anim; //Enemy animator
    public bool isPassive = true; //Checks if it's passive
    public bool isAttacked = false; //Checks if it's under attack
    public bool isExploded = false;

    public float rangeSmear = 100f;
    [SerializeField] private BloodSmear smear;

    public bool isAlive = true; //Check if it's alive
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
    [SerializeField] private GameObject blood; //Blood Particle
    [SerializeField] private float bloodOffsetY = 1.1f; //Y Coordinates of blood on the enemy
    [SerializeField] private int minForce = -6; //Min force of body part for death
    [SerializeField] private int maxForce = 6; //Max force of body part for death
  

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

    private void OnCollisionEnter(Collision collision)
    {
        if (!isAlive)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                Dead();
            }
        }
    }

    void DeadCheckWall()
    {
        isAlive = false;
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
            isAttacked = false;
        }
    }
    void GettingHit()
    {
        if (isAttacked)
        {
            smear.Splat();
            Vector3 bloodPos = new Vector3(transform.position.x, transform.position.y + bloodOffsetY, transform.position.z);
            Instantiate(blood, bloodPos, Quaternion.identity);
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
        foreach(GameObject gb in lsBodyParts)
        {
            rb = gb.GetComponent<Rigidbody>();
            rb.AddForce(transform.up * Random.Range(minForce, maxForce), ForceMode.Impulse);
            rb.AddForce(transform.forward * Random.Range(minForce, maxForce), ForceMode.Impulse);
        }
        //Instantiate(blood, transform.position, Quaternion.identity);
       
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameManager.Instance.Player.GetComponent<Transform>();

        lsBodyParts.Add(arms);
        lsBodyParts.Add(antenna);
        lsBodyParts.Add(collar);
        lsBodyParts.Add(body);
        lsBodyParts.Add(head);
        lsBodyParts.Add(legs);
        lsBodyParts.Add(wings);
        lsBodyParts.Add(sword);
    }

    private void FixedUpdate()
    {
        GettingHit();
        //Keep checking if it's not attacking player
        if (isPassive && isAlive)
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
        if (!isAlive)
        {
            DeadCheckWall();
        }
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
