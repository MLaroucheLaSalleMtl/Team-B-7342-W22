using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    PlayerControl playerC;

    bool comboPossible;
    int comboAttackNumber;

    // Animation parameters    
    [SerializeField] private float animFinishTime = 0.9f;
    private bool isAttacking_1 = false;
    float AttackSlowMod;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Cache the animator
        playerC = GetComponent<PlayerControl>();
    }

    public void Attack()
    {
        //if (!isAttacking_1) // If not attacking
        //{
        //    anim.SetTrigger("Attack1"); // Prepare trigger to play attack animation
        //    //StartCoroutine(StartAttack());
        //}

        if(comboAttackNumber == 0)
        {
            anim.Play("Attack_A-1");
            comboAttackNumber = 1;
            return;
        }
        if(comboAttackNumber != 0)
        {
            if (comboPossible)
            {  
                comboPossible = false;
                comboAttackNumber += 1;
            }
        }

    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboAttackNumber == 2)
        {
            anim.Play("Attack_A-2");
        }
        if (comboAttackNumber == 3)
        {
            anim.Play("Attack_A-3");
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboAttackNumber = 0;
    }

    //IEnumerator StartAttack()
    //{
    //    //Debug.Log("How many times does this play");
    //    yield return new WaitForSeconds(0.1f); // Pause for a moment
    //    isAttacking_1 = true; // Then set attacking to true to activate trigger
    //}

    //void CheckAttackString()
    //{
    //    // If attacking set to true and the animation has completed a certain percentage of its runtime
    //    if (isAttacking_1 && anim.GetCurrentAnimatorStateInfo(2).normalizedTime >= animFinishTime)
    //    {
    //        //Debug.Log("Does this section get checked");
    //        isAttacking_1 = false; // Set attacking to false (so that it can be re-activated)
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //CheckAttackString();
    }
}
