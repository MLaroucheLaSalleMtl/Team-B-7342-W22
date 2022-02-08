using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))] //can be replaced with rigidbody
[RequireComponent(typeof(Rigidbody))] 


public class PlayerControl : MonoBehaviour
{
    private Vector3 moveDirection; //to hold the converted movement direction
    //private CharacterController cController;
    private Rigidbody rBody;
    [SerializeField] private float speed = 5f;
    private bool jump = false;
    [SerializeField] private float jumpForce = 5f;
    private bool grounded = true;
    const float CHECK_RADIOUS = 0.2f;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        //cController = GetComponent<CharacterController>(); //cache character controller component 
        rBody = GetComponent<Rigidbody>(); //cache rigidbody component 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForGround();
        //cController.Move(moveDirection * speed * Time.deltaTime);
        rBody.velocity = new Vector3((moveDirection * speed).x, rBody.velocity.y, (moveDirection * speed).z);
        if (jump)
        {
            rBody.AddForce(transform.up * jumpForce,ForceMode.Impulse);
            jump = false;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.PuaseOrPlay();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Player's input is a Vector2 relative to the world
        Vector2 inputDirection = context.ReadValue<Vector2>();

        //calculate the correct movement direction Vector3 relative to the isometric camera and store it in moveDirection
        moveDirection = ConvertInputDirection(inputDirection);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed && !jump && grounded)
        {
            jump = true;
        }
    }

    private void CheckForGround()
    {
        grounded = Physics.CheckSphere(checkGroundPoint.position, CHECK_RADIOUS, whatIsGround);
    }

//---------------------------------------------------------------------------------------------------------------------------------
    /*
 * script for rotating the player's input with is relative to the world coordinates to
 * match the direction of the isometric camera
 * source: https://michael-l-davis.medium.com/isometric-player-movement-in-unity-998d86193b8a
 */

    //gets player input as a Vector2 relative to the world and returns a Vector3 for the players movement relative to isometric camera
    //the returned Vectpr3 can be used by character controller or rigidbody
    private Vector3 ConvertInputDirection(Vector2 iDirection)
    {
        Vector3 moveVector = new Vector3(iDirection.x, 0, iDirection.y); //change the 2D movement input to 3D world direction

        Quaternion rotation = Quaternion.Euler(0, 45.0f, 0); //rotation angels for input. camera angles should be used instead if the camera can rotate
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation); //generate the rotation matrix

        return isoMatrix.MultiplyPoint3x4(moveVector); //apply the rotation to the vector
    }
//----------------------------------------------------------------------------------------------------------------------------------
}
