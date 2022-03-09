// 2022-02-24   Sean Hall   Created the script and set up basic variables
// 2022-02-28   Sean Hall   Tested code for animating with coroutine
// 2022-03-05   Sean Hall   Set up movement blend tree for idle/walk interaction
// 2022-03-07   Sean Hall   Set up jumping and attacking anim code to interact with PlayerControl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    PlayerControl playerC; // Used for things like check grounded state
    private Vector3 playerVelocity;

    // Animation parameters    
    [SerializeField] private float animFinishTime = 0.9f;
    private bool isAttacking_1 = false;
    float AttackSlowMod;    
    public bool inputJump = false;
           

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Cache the animator
        playerC = GetComponent<PlayerControl>();
    }

    public void Attack()
    {
        if (!isAttacking_1) // If not attacking
        {                        
            anim.SetTrigger("isAttacking_1"); // Prepare trigger to play attack animation
            StartCoroutine(StartAttack());
        }
    }

    IEnumerator StartAttack()
    {
        //Debug.Log("How many times does this play");
        yield return new WaitForSeconds(0.1f); // Pause for a moment
        isAttacking_1 = true; // Then set attacking to true to activate trigger
    }

    void CheckAttackString()
    {
        // If attacking set to true and the animation has completed a certain percentage of its runtime
        if (isAttacking_1 && anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= animFinishTime)
        {
            //Debug.Log("Does this section get checked");
            isAttacking_1 = false; // Set attacking to false (so that it can be re-activated)
        }
    }

    bool CheckIfGrounded()
    {
        return playerC.grounded; // Checks if the player controller is grounded
    }

    public void PlayJumpAnim()
    {        
        if (inputJump)
        {
            Debug.Log("Should play the jump animation");
            anim.SetTrigger("Jump");
            inputJump = false;
        }            
    }
            
    void Update()
    {
        //if (swinging)
        {
                            
        }

        
        // Animation
        anim.SetBool("Grounded", CheckIfGrounded());
        anim.SetFloat("MoveSpeed", playerC.moveVector.normalized.magnitude); // For locomotion blend tree
        anim.SetBool("Dash", playerC.dash);
        anim.SetFloat("VerticalSpeed", playerC.rBody.velocity.y);
        PlayJumpAnim();
        CheckAttackString();
    }
}
