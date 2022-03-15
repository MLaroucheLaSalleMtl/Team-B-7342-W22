// 2022-03-06   Sean Hall   Added some variables to interact with PlayerAnim script
// 2022-03-07   Sean Hall   Added jump, capsule collider height adjustment

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))] //can be replaced with rigidbody
[RequireComponent(typeof(Rigidbody))] 


public class PlayerControl : MonoBehaviour
{
    PlayerAnimMove playerAnim;
    PlayerAttack playerAttack;

    public Rigidbody rBody;
    
    //variables for player movement
    private Vector3 moveDirection; //to hold the converted movement direction
    private Vector2 inputDirection = Vector2.zero; //to hold raw input direction
    public Vector3 moveVector;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveVelocitySmoothing = 0.05f;
   
    //variables for jumping action
    public bool jump = false;
    [SerializeField] private float jumpForce = 5f;
    public bool grounded = true;
    const float CHECK_RADIOUS = 0.2f;
//[SerializeField] private float allowJumpAgainAfter = 0.6f;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask whatIsGround;
    private Vector3 refVelocity = Vector3.zero;
   
    //variables for dash action
    public bool dash = false;
    [SerializeField] private float dashForce = 0.4f;
    [SerializeField] private float allowDashAgainAfter = 0.6f;
    
    //variables for Hit action
    [SerializeField] private float hitRadius = 1f;
    [SerializeField] private float hitDamage = 10f;

    //variables for rotation of the player
    private Vector3 facingDirection = Vector3.forward;
    [SerializeField] private float rotationSmoothing = 0.05f;
    
    //variables for camera rotation
    [SerializeField] private float rotateCameraY = 30f;
    private Vector3 camRotVec = Vector3.zero;

    // Variables for player capsule scaling
    private float capsuleScale;
    const float CAP_FULL_SCALE = 1.0f;
    const float CAP_JUMP_SCALE = 0.75f;
    CapsuleCollider capsulePlayer;

    // Start is called before the first frame update
    void Start()
    {
        //cController = GetComponent<CharacterController>(); //cache character controller component 
        rBody = GetComponent<Rigidbody>(); //cache rigidbody component 
        playerAnim = GetComponent<PlayerAnimMove>();
        playerAttack = GetComponent<PlayerAttack>();
        capsulePlayer = GetComponent<CapsuleCollider>();
        capsuleScale = capsulePlayer.height; // Cache the default size
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckForGround();
        ScaleCapsule();
        ApplyMovementVelocity();
        ActivateJump();
        ApplyDashForce();
        RotatePlayer();
        RotateCamera();
    }

    private void RotateCamera()
    {
        if (camRotVec != Vector3.zero)
        {
            GameManager.Instance.MainCamera.transform.eulerAngles += (camRotVec * Time.deltaTime);
            if (!dash)
            {
                //calculate the correct movement direction Vector3 relative to the isometric camera and store it in moveDirection
                moveDirection = ConvertInputDirection(inputDirection);
            }
        }
    }

    //will ritate the player to face the input move direction vector
    private void RotatePlayer()
    {
        if (moveDirection != Vector3.zero)
        {
            facingDirection = Vector3.Lerp(facingDirection, moveDirection, rotationSmoothing);
        }
        transform.rotation = Quaternion.LookRotation(facingDirection) * Quaternion.FromToRotation(Vector3.right, Vector3.forward);
    }

    private void ApplyMovementVelocity()
    {
        //applying movement velocity smoothly
        Vector3 targetVelocity = new Vector3((moveDirection * speed).x, rBody.velocity.y, (moveDirection * speed).z);
        rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref refVelocity,moveVelocitySmoothing);
        
    }

    private void ApplyDashForce()
    {
        if (dash)
        {
            rBody.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            Invoke("AllowDashAgain", allowDashAgainAfter);
        }
    }

    private void AllowDashAgain()
    {
        dash = false;
        moveDirection = ConvertInputDirection(inputDirection);
    }

    private void ActivateJump() { 
        if (jump)
        {            
            playerAnim.inputJump = true; // Passes to the jump trigger in PlayerAnim
            rBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            ScaleCharacter(CAP_JUMP_SCALE);
            jump = false;
//Invoke("AllowJumpAgain", allowJumpAgainAfter);
        }
    }

//private void AllowJumpAgain()
//{
//    jump = false;
//}

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetMoveDirection();
            GameManager.Instance.PuaseOrPlay();
        }
    }

    public void ResetMoveDirection()
    {
        inputDirection = Vector2.zero;
        if (!dash) moveDirection = Vector3.zero;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            //Player's input is a Vector2 relative to the world
            inputDirection = context.ReadValue<Vector2>();

            if (!dash)
            {
                //calculate the correct movement direction Vector3 relative to the isometric camera and store it in moveDirection
                moveDirection = ConvertInputDirection(inputDirection);
            }
        }

        /*
         When I click right on my joystick 
        float move = context input
        playanim.Moving(move);
         */
    }

    public void OnLookRight(InputAction.CallbackContext context)
    {   
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if(context.started)
            {
                camRotVec = new Vector3(0f, rotateCameraY, 0f);
            }
        }
        if (context.canceled)
        {
            camRotVec = Vector3.zero;
        }
    }

    public void OnLookLeft(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.started)
            {
                camRotVec = new Vector3(0f, -rotateCameraY, 0f);
            }
            if (context.canceled)
            {
                camRotVec = Vector3.zero;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.performed && !jump && grounded)
            {
                jump = true;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.performed && !dash && grounded)
            {
                dash = true;
            }
        }
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.performed)
            {
                //Debug.Log("OnHit triggered");
                playerAttack.Attack();

                Collider[] colliders = Physics.OverlapSphere(transform.position, hitRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.GetComponent<EnemyDamage>())
                    {
                        if (collider.gameObject.CompareTag("Enemy"))
                        {
                            collider.gameObject.GetComponent<EnemyDamage>().TakeDamage(hitDamage);
                        }
                    }
                }
            }
        }
    }

    private void ScaleCapsule()
    {
        if (grounded)
        {
            if (capsulePlayer.height != capsuleScale) // If on the ground and not default size, make default size
                ScaleCharacter(CAP_FULL_SCALE);
        }
           
        if (rBody.velocity.y < -0.5f && capsulePlayer.height == capsuleScale) // If falling and still default size, make jump size 
        {
            ScaleCharacter(CAP_JUMP_SCALE);
        }
    }

    private void ScaleCharacter(float fraction)
    {
        capsulePlayer.height = capsuleScale * fraction; // Scales the capsule with the passed value
        //capsulePlayer.center = new Vector3(0f, capsulePlayer.height * 0.5f, 0f); // Adjusts the center of the capsule to be half its height
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
        moveVector = new Vector3(iDirection.x, 0, iDirection.y); //change the 2D movement input to 3D world direction

        float rotationAngle = GameManager.Instance.MainCamera.transform.rotation.eulerAngles.y;
        
        Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0); //rotation angels for input. camera angles should be used instead if the camera can rotate
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation); //generate the rotation matrix

        return isoMatrix.MultiplyPoint3x4(moveVector); //apply the rotation to the vector
    }
//----------------------------------------------------------------------------------------------------------------------------------
}
