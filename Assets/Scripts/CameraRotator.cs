// 2022-04-03   Mohammadreza Abolhassani    Added Arena Mode

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    private Quaternion targetRotation;

    [SerializeField] private bool arenaMode = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (arenaMode)
        {
            Vector3 lookDir = GameManager.Instance.Player.position;
            Quaternion qlook = Quaternion.LookRotation(lookDir, Vector3.up);
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, qlook.eulerAngles.y, transform.eulerAngles.z);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void RotateClockwise()
    {
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles + new Vector3(0f, 90f, 0f));
    }

  

}
