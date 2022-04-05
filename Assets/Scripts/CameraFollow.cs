using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* isometric camera follow script
 * source: https://gist.github.com/SergioCaridad/f981e2663599a77e4d4615b76968bf32
 */

public class CameraFollow : MonoBehaviour
{
    private Transform cameraTarget;
    [SerializeField] private float smoothing = 4f; //increase this if the camera movement gives you a headache!
    private Vector3 offset; //to store the initial position difference between the camera and its target

    // Start is called before the first frame update
    void Start()
    {
        cameraTarget = GameManager.Instance.Player;
        //calculate the offset
        //offset = transform.position - cameraTarget.position;
        offset = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //target camera position is where we want the camera to move to, but not too fast
        Vector3 targetCameraPosition = cameraTarget.position + offset; //the distance between the target position of the camera and the position of camera's target, is always the offset
        
        //Lerp is used for smooth transition 
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);
    }
}
