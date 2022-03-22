using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    public Animator anim;
    public Transform enemy;
    private Transform player;

    private bool dashing = false;
    bool turn = false;
    bool attack = false;
    [SerializeField] float dashSpeed = 100f;

    Vector3 playerPos;
     
    public bool Dashing { get => dashing; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.GetComponent<EnemyMoving>().isPassive = false;
            StartCoroutine(DashAnim());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.GetComponent<EnemyMoving>().isPassive = true;
        }
    }

    //The Dash
    IEnumerator DashAnim()
    {
        dashing = true;
        //Disables the attack trigger so it doesn't follow the player
        enemy.GetComponentInChildren<LookAtPlayer>().enabled = false;
        enemy.GetComponentInChildren<AttackPlayer>().enabled = false;
        //Makes the first animation movement
        anim.SetBool("DashOne", true);
        turn = true;
        //Turn for 1 second
        yield return new WaitForSeconds(1f);
        playerPos = player.transform.position;
        turn = false;
        attack = true;
        //Move forward (2 yields because the animation looks better this way
        yield return new WaitForSeconds(0.2f);
        enemy.GetComponentInChildren<AttackPlayer>().enabled = true;
        anim.SetBool("DashTwo", true);
        yield return new WaitForSeconds(0.3f);
        //Reset the enemy to it's original state wether the player is in front or not
        attack = false;
        dashing = false;
        anim.SetBool("DashOne", false);
        anim.SetBool("DashTwo", false);
        enemy.GetComponentInChildren<LookAtPlayer>().enabled = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dashing)
        {
            //It will turn x amount of seconds (Coroutine)
            if (turn)
            {
                Vector3 lookDir = new Vector3(player.position.x - enemy.position.x, 0, player.position.z - enemy.position.z); //Get the position of the player
                Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
                enemy.rotation = angle;
            }
            //It will move forward (dash) x amount of seconds
            if (attack)
            {
                enemy.transform.position += enemy.transform.forward * dashSpeed * Time.deltaTime;
            }
        }
    }
}
