using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void RotateClockwise()
    {
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, 90f, 0f));
    }

}
