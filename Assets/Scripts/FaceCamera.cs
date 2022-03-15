using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    //private Quaternion origRot;
    private Vector3 origRotEuler;

    // Start is called before the first frame update
    void Start()
    {
        //origRot = transform.rotation;
        origRotEuler = transform.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = GameManager.Instance.MainCamera.rotation * origRot;
        transform.eulerAngles = new Vector3(origRotEuler.x, GameManager.Instance.MainCamera.transform.eulerAngles.y, origRotEuler.z);

    }
}
