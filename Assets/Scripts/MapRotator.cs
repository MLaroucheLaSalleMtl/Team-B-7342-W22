using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRotator : MonoBehaviour
{
    public void RotateMapClockwise()
    {
        //rotate 90 degrees clockwise
        //transform.Rotate(new Vector3(0f, 90f, 0f));
        transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 90f, 0));
        //adjust the position on screen
        transform.position = new Vector3(transform.position.z, transform.position.y, -transform.position.x);
    }
}
