// 2022-03-06   Sean Hall                   Added some variables to interact with PlayerAnim script
// 2022-03-07   Sean Hall                   Added jump, capsule collider height adjustment
// 2022-04-03   Mohammadreza Abolhassani    Camera rotation feature was removed from this script
// 2022-04-03   Mohammadreza Abolhassani    Made some fields private and used getters instead to let the serialized fields stand out in the editor
// 2022-04-10   Sean Hall                   Added movement and locked rotation of player when attacking, adjusted post-dash rotation fix to apply to both dashing and attacking
// 2022-04-10   Sean Hall                   Fixed minor bug that allowed player to dash while standing still

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))] 

public class PlayerControl : MonoBehaviour
{
    private PlayerAnimMove playerAnim;
    private PlayerAttack playerAttack;

    public Rigidbody rBody;
    
    //variables for player movement
    private Vector3 moveDirection; //to hold the converted movement direction
    private Vector2 inputDirection = Vector2.zero; //to hold raw input direction
    private Vector3 moveVector;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float moveVelocitySmoothing = 0.05f;

    //variables for jumping action
    private bool jump = false;
    [SerializeField] private float jumpForce = 5f;
    private bool grounded = true;
    const float CHECK_RADIOUS = 0.2f;
    [SerializeField] private Transform checkGroundPoint;
    [SerializeField] private LayerMask whatIsGround;
    private Vector3 refVelocity = Vector3.zero;

    //variables for dash action
    private bool dash = false;
    [SerializeField] private float dashForce = 0.4f;
    [SerializeField] private float allowDashAgainAfter = 0.6f;

    //variables for attack
    [SerializeField] private float attackForce = 350f; // How much the character will move when attacking
    

    //variables for rotation of the player
    private Vector3 facingDirection = Vector3.forward;
    [SerializeField] private float rotationSmoothing = 0.05f;
    
    // Variables for player capsule scaling
    private float capsuleScale;
    const float CAP_FULL_SCALE = 1.0f;
    const float CAP_JUMP_SCALE = 0.75f;
    private CapsuleCollider capsulePlayer;

    public bool Dash { get => dash; }
    public bool Grounded { get => grounded;  }
    public Vector3 MoveVector { get => moveVector; }
    public bool Jump { get => jump; }

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
    }

    //will rotate the player to face the input move direction vector
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
        // Player movement not based on player input while attacking
        if (!playerAttack.isAttacking)
        {
            //applying movement velocity smoothly
            Vector3 targetVelocity = new Vector3((moveDirection * speed).x, rBody.velocity.y, (moveDirection * speed).z);
            rBody.velocity = Vector3.SmoothDamp(rBody.velocity, targetVelocity, ref refVelocity, moveVelocitySmoothing);
        }   
    }

    private void ApplyDashForce()
    {
        if (Dash && !playerAttack.isAttacking)
        {
            rBody.AddForce(moveDirection * dashForce, ForceMode.Impulse);
            Invoke("RefreshMovement", allowDashAgainAfter);
        }        
    }

    // Updates movement post-action to whatever the player is currently inputting (no hold-over from before action)
    public void RefreshMovement()
    {
        dash = false;
        playerAttack.canAttack = true;
        moveDirection = ConvertInputDirection(inputDirection);
    }

    // Moves player during attack animation lunge
    public void ApplyAttackForce(float modifier)
    {
        rBody.velocity = Vector3.zero; // Halts dash momentum
        rBody.AddForce(transform.right * attackForce * modifier); // Applies movement based on attack
    }

    private void ActivateJump() { 
        if (Jump)
        {            
            playerAnim.inputJump = true; // Passes to the jump trigger in PlayerAnim
            rBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            ScaleCharacter(CAP_JUMP_SCALE);
            jump = false;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetMoveDirection();
            GameManager.Instance.PauseOrPlay();
        }
    }

    public void OnOutline(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = true;
        }
    }
    public void ResetMoveDirection()
    {
        inputDirection = Vector2.zero;
        if (!Dash) moveDirection = Vector3.zero;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            //Player's input is a Vector2 relative to the world
            inputDirection = context.ReadValue<Vector2>();

            if (!Dash && !playerAttack.isAttacking)
            {
                //calculate the correct movement direction Vector3 relative to the isometric camera and store it in moveDirection
                moveDirection = ConvertInputDirection(inputDirection);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.performed && !Jump && Grounded)
            {
                jump = true;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.state == GameState.GamePlay)
        {
            if (context.performed && !Dash && Grounded)
            {
                dash = true;
                //playerAttack.canAttack = false;
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
            }
        }
    }

    private void ScaleCapsule()
    {
        if (Grounded)
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
 * script for rotating the player's input which is relative to the world coordinates to
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

        return isoMatrix.MultiplyPoint3x4(MoveVector); //apply the rotation to the vector
    }
//----------------------------------------------------------------------------------------------------------------------------------
}
