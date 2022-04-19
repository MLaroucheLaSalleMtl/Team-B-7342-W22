using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDashStop : MonoBehaviour
{
    EnemyDash enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<EnemyDash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        enemy.attack = false;
    }
}
