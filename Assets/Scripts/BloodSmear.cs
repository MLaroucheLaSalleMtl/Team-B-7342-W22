using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSmear : MonoBehaviour
{
    [SerializeField] private GameObject smear;
    private float rangeSmear = 20f;
    private bool toSmear = true;

    public void Splat()
    {
        RaycastHit hit;
        //Vector3 startHit = new Vector3(transform.position.x, transform.position.y + bloodOffsetY, transform.position.z);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, rangeSmear))
        {
            Debug.DrawLine(transform.position, -transform.forward * hit.distance, Color.red);
            Debug.Log("Hit: " + gameObject.tag);
            if (hit.collider.CompareTag("Wall") && toSmear)
            {
                toSmear = false;
                Quaternion rot = Quaternion.LookRotation(hit.normal);
                Instantiate(smear, hit.point, rot, hit.transform);
                Invoke("CanSmearAgain", 0.7f);
            }
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
