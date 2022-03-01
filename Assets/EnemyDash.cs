using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    public Animator anim;
    public Transform enemy;
    private Transform player;

    bool dashing = false;
    bool turn = false;
    bool attack = false;
    [SerializeField] float dashSpeed = 100f;

    Vector3 playerPos;

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
            StartCoroutine(TimerToWait());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.GetComponent<EnemyMoving>().isPassive = true;
        }
    }

    //Please comment this ariel for the love of god
    IEnumerator TimerToWait()
    {
        dashing = true;
        enemy.GetComponentInChildren<LookAtPlayer>().enabled = false;
        enemy.GetComponentInChildren<AttackPlayer>().enabled = false;
        anim.SetBool("DashOne", true);
        turn = true;
        yield return new WaitForSeconds(1f);
        playerPos = player.transform.position;
        turn = false;
        attack = true;
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("DashTwo", true);
        yield return new WaitForSeconds(0.3f);
        attack = false;
        dashing = false;
        anim.SetBool("DashOne", false);
        anim.SetBool("DashTwo", false);
        enemy.GetComponentInChildren<LookAtPlayer>().enabled = true;
        enemy.GetComponentInChildren<AttackPlayer>().enabled = true;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (dashing)
        {
            if (turn)
            {
                Vector3 lookDir = new Vector3(player.position.x - enemy.position.x, 0, player.position.z - enemy.position.z); //Get the position of the player
                Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
                enemy.rotation = angle;
            }
            if (attack)
            {
                enemy.transform.position += enemy.transform.forward * dashSpeed * Time.deltaTime;
            }
        }
    }
}
