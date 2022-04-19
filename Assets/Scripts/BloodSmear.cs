using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSmear : MonoBehaviour
{
    //[SerializeField] private GameObject smear;
    [SerializeField] private float rangeSmear = 20f;
    private bool toSmear = true;
    [SerializeField] private float smearTime = 9f;

    public List<GameObject> smearList = new List<GameObject>();

    public void Splat()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, rangeSmear))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Damage") || hit.collider.CompareTag("River"))
            {
                return;
            }

            if (toSmear)
            {
                int i = Random.Range(0, smearList.Count);
                toSmear = false;
                Quaternion rot = Quaternion.LookRotation(hit.normal);
                GameObject newSmear = Instantiate(smearList[i], hit.point, rot);
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
