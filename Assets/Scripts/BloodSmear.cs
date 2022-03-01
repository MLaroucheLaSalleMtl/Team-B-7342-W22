using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSmear : MonoBehaviour
{
    [SerializeField] private GameObject smear;
    private float rangeSmear = 20f;
    private bool toSmear = true;
    [SerializeField] private float smearTime = 9f;

    public void Splat()
    {
        RaycastHit hit;
        //Vector3 startHit = new Vector3(transform.position.x, transform.position.y + bloodOffsetY, transform.position.z);
        if (Physics.Raycast(transform.position, -transform.forward, out hit, rangeSmear))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                return;
            }
            if (toSmear)
            {
                toSmear = false;
                Quaternion rot = Quaternion.LookRotation(hit.normal);
                GameObject newSmear = Instantiate(smear, hit.point, rot);
                Invoke("CanSmearAgain", 1f);
                Destroy(newSmear, smearTime);
            }
        }
    }

    void CanSmearAgain()
    {
        toSmear = true;
    }
}
