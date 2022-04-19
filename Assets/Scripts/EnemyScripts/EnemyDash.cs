using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    private Transform player;

    private EnemyAi enemy;

    bool turn = false;
    public bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyAi>();
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.isIdleing = false;
            StartCoroutine(DashAnim());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.isIdleing = true;
        }
    }

    //The Dash
    IEnumerator DashAnim()
    {
        enemy.isDashing = true;
        //Disables the attack trigger so it doesn't follow the player
        enemy.GetComponentInChildren<EnemyFollow>().enabled = false;
        enemy.GetComponentInChildren<EnemyAttack>().enabled = false;
        //Makes the first animation movement
        enemy.anim.SetBool("DashOne", true);
        turn = true;
        //Turn for 1 second
        yield return new WaitForSeconds(1f);
        Vector3 playerPos = player.transform.position;
        turn = false;
        attack = true;
        //Move forward (2 yields because the animation looks better this way
        yield return new WaitForSeconds(0.2f);
        enemy.GetComponentInChildren<EnemyAttack>().enabled = true;
        enemy.anim.SetBool("DashTwo", true);
        yield return new WaitForSeconds(0.3f);
        //Reset the enemy to it's original state wether the player is in front or not
        attack = false;
        enemy.isDashing = false;
        enemy.anim.SetBool("DashOne", false);
        enemy.anim.SetBool("DashTwo", false);
        enemy.GetComponentInChildren<EnemyFollow>().enabled = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (enemy.isDashing)
        {
            //It will turn x amount of seconds (Coroutine)
            if (turn)
            {
                Vector3 lookDir = new Vector3(player.position.x - enemy.transform.position.x, 0, player.position.z - enemy.transform.position.z); //Get the position of the player
                Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
                enemy.transform.rotation = angle;
            }
            //It will move forward (dash) x amount of seconds
            if (attack)
            {
                enemy.transform.position += enemy.transform.forward * enemy.config.dashSpeed * Time.deltaTime;
            }
        }
    }
}
