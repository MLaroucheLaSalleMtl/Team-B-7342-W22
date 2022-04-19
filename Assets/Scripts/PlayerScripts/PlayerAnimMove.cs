// 2022-02-24   Sean Hall   Created the script and set up basic variables
// 2022-02-28   Sean Hall   Tested code for animating with coroutine
// 2022-03-05   Sean Hall   Set up movement blend tree for idle/walk interaction
// 2022-03-07   Sean Hall   Set up jumping and attacking anim code to interact with PlayerControl
// 2022-03-13   Sean Hall   Moved attack animation handling to separate script


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimMove : MonoBehaviour
{
    private Animator anim;
    PlayerControl playerC; // Used for things like check grounded state
    
    public bool inputJump = false;           

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Cache the animator
        playerC = GetComponent<PlayerControl>();
    }    

    public void PlayJumpAnim()
    {        
        if (inputJump)
        {
            //Debug.Log("Should play the jump animation");
            anim.SetTrigger("Jump");
            inputJump = false;
        }            
    }
            
    void Update()
    {        
        // Animation
        anim.SetBool("Grounded", playerC.Grounded);
        anim.SetFloat("MoveSpeed", playerC.MoveVector.normalized.magnitude); // For locomotion blend tree
        anim.SetBool("Dash", playerC.Dash);
        anim.SetFloat("VerticalSpeed", playerC.GetComponent<Rigidbody>().velocity.y);
        PlayJumpAnim();        
    }
}
