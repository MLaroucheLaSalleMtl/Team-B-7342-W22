using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWandering : MonoBehaviour
{
    //EnemyAI
    EnemyAi enemyAI;
    //Transform enemy;

    //Declaring variables
    //private Animator anim; //Enemy animator
    public bool hitWall = false; //Check if it hit a wall

    //Moving speed of enemy variables

    //Wandering variables
    //public bool isWandering = true; //Wether it is wandering
    public bool isRotLeft = false; //See if it rotates left
    public bool isRotRight = false; //See if it rotates right
    public bool isWalking = false; //See if it is walking

    public float walkTime;
    public int rotateLorR;
    public float rotateWait;
    public float rotTime;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<EnemyAi>();
    }

    /*private void OnCollisionEnter(Collision collision)
      {
          if (collision.transform.CompareTag("Wall") && enemyAI.isIdleing)
              RoutineWallHit(enemyAI);
      }*/

    public void StopWandering(EnemyAi enemy)
    {
        StopCoroutine(Wander(enemy));
    }

    public void RoutineWander(EnemyAi enemy)
    {
        StartCoroutine(Wander(enemy));
    }

    public void RoutineWallHit(EnemyAi enemy)
    {
        StartCoroutine(RotateAround(enemy));

    }
    public IEnumerator RotateAround(EnemyAi enemy)
    {
        hitWall = true;
        isWalking = false;
        isRotLeft = true;
        enemy.anim.SetBool("Walk", true);
        yield return new WaitForSeconds(1f);
        enemy.anim.SetBool("Walk", false);
        isRotLeft = false;
        hitWall = false;
    }

    public IEnumerator Wander(EnemyAi enemy)
    {
        //Declaring Variables
        float rotTime = Random.Range(1, 2);
        float rotateWait = Random.Range(1, 4);
        int rotateLorR = Random.Range(1, 3); //1 is right, 2 is left
        float walkTime = Random.Range(1, 3);

        //If not attacking player

        //Make it wander
        enemy.isWalking= true;

        #region Rotate
        //If turning left, make it true to turn
        //Rotate the amount of time in seconds
        //Animate using the walking animation
        //Then after turn, set to false
        if (rotateLorR == 1)
        {
            isRotRight = true;
            enemy.anim.SetBool("Walk", true);

            yield return new WaitForSeconds(rotTime);
            enemy.anim.SetBool("Walk", false);

            isRotRight = false;
        }
        //Same goes for right just like left
        if (rotateLorR == 2)
        {
            isRotLeft = true;
            enemy.anim.SetBool("Walk", true);

            yield return new WaitForSeconds(rotTime);
            enemy.anim.SetBool("Walk", false);

            isRotLeft = false;
        }
        #endregion

        #region Walking
        //Make the enemy walk
        //Animate, walk for the amount of seconds, then stop
        isWalking = true;
        enemy.anim.SetBool("Walk", true);

        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        enemy.anim.SetBool("Walk", false);

        yield return new WaitForSeconds(rotateWait);
        #endregion
        //Make it not wander
        enemy.isWalking = false;

    }
}
