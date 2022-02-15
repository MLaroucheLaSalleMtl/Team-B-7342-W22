using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    private Transform player;
    [SerializeField] private float speedMove = 1.5f;

    private bool inSight = false;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
            enemy.transform.GetComponent<Animator>().SetBool("Walk", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            inSight = false;
            enemy.transform.GetComponent<Animator>().SetBool("Walk", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inSight)
        {
            Vector3 lookDir = new Vector3(player.position.x - enemy.position.x, 0, player.position.z - enemy.position.z); //Get the position of the player
            Quaternion angle = Quaternion.LookRotation(lookDir); //Get the rotation the enemy must do
            enemy.rotation = angle;
            enemy.transform.position += enemy.transform.forward * speedMove * Time.deltaTime;
        }
    }
}
