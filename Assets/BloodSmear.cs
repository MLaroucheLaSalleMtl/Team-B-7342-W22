using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSmear : MonoBehaviour
{
    [SerializeField] private EnemyDamage enemy;
    [SerializeField] private GameObject smear;
    [SerializeField] private GameObject bloodDead;
    private float rangeSmear = 20f;
    private bool toSmear = true;
    private RaycastHit hit;

    public void Splat()
    {
        //Vector3 startHit = new Vector3(transform.position.x, transform.position.y + bloodOffsetY, transform.position.z);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, rangeSmear))
        {
            Debug.DrawLine(transform.position, -transform.forward * hit.distance, Color.red);
            Debug.Log("Hit: " + gameObject.tag);
            if (hit.collider.CompareTag("Wall") && !enemy.IsDead)
            {
                if (toSmear && !enemy.IsDead)
                {
                    toSmear = false;
                    Quaternion rot = Quaternion.LookRotation(hit.normal);
                    Instantiate(smear, hit.point, rot, hit.transform);
                    Invoke("CanSmearAgain", 0.7f);
                }
            }
        }
    }

 public void SplatDead(RaycastHit hit)
 {
     if (hit.collider.CompareTag("Wall"))
     {
         Quaternion rot = Quaternion.LookRotation(hit.normal);
         Instantiate(bloodDead, hit.point, rot, hit.transform);
     }
 }

void CanSmearAgain()
{
    toSmear = true;
}

// Update is called once per frame
void Update()
{

}
}
