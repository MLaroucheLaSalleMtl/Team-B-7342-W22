// 2022-04-10   Sean Hall       Added method to allow player movement input again (called through animation event, references PlayerControl)
// 2022-04-12   Sean Hall       Added code to interact with Time Manager for SlowMo attacks (Attack duration params, UpdateAnimClipTimes)
// 2022-04-16   Sean Hall       Fixed a bug with Combo Attack 2, added event calls for attack timer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerWeapon playerW; // Hit Trigger on chainsaw
    private PlayerControl playerC;
    private TimeManager time;
    public Animator anim;
    private AnimationClip clip;    
    public float damage;

    public bool canAttack = true;
    public bool isAttacking;
    bool comboPossible;
    int comboAttackNumber;

    // Attack movement parameters
    [SerializeField] private float attack1move = 1;
    [SerializeField] private float attack2move = 1.15f;
    [SerializeField] private float attack3move = 1.1f;

    // Attack duration parameters
    [SerializeField] private float sloMoModFactor = 0.6f;
    private const float ATTACK_1_SPD_MOD = 1.5f;
    private const float ATTACK_2_SPD_MOD = 1.3f;
    private const float ATTACK_3_SPD_MOD = 1.2f;
    public float attack1time;
    public float attack2time;
    public float attack3time;

    // Animation parameters    
    //[SerializeField] private float animFinishTime = 0.9f;
    float AttackSlowMod;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); // Cache the animator
        playerC = GetComponent<PlayerControl>();
        time = GetComponent<TimeManager>();
        anim.SetFloat("attack1SpdMod", ATTACK_1_SPD_MOD);
        anim.SetFloat("attack2SpdMod", ATTACK_2_SPD_MOD);
        anim.SetFloat("attack3SpdMod", ATTACK_3_SPD_MOD);
        UpdateAnimClipTimes();
        //Debug.Log("Attack 2 has " + attack2time);
    }

    public void Attack()
    {
        if (canAttack)
        {
            if (comboAttackNumber == 0)
            {
                isAttacking = true;
                anim.Play("Attack_A-1");
                EventManager.current.AttackStart(); // Sends event to EventManager (TimeManager listening)
                playerC.ApplyAttackForce(attack1move); // Moves player
                //Debug.Log("Playing attack 1");
                //time.DoSlowMo(sloMoModFactor, attack1time / 2);
                comboAttackNumber = 1;
                return;
            }

            if (comboAttackNumber != 0)
            {
                if (comboPossible)
                {
                    comboPossible = false;
                    comboAttackNumber += 1;
                }
            }
        } //if canAttack
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo() // Called through animation events
    {
        if (comboAttackNumber == 2 && !anim.GetCurrentAnimatorStateInfo(2).IsName("Attack_A-2")) 
        {
            isAttacking = true;
            anim.Play("Attack_A-2");
            EventManager.current.AttackStart(); // Sends event to EventManager (TimeManager listening)
            playerC.ApplyAttackForce(attack2move); // Moves player            
            //time.DoSlowMo(sloMoModFactor, attack2time / 2);

            if (playerW.stillInside)
            {
                //Debug.Log("Should play second chainsaw");
                EventManager.current.WeaponTriggerEnter();
            }
        }

        if (comboAttackNumber == 3)
        {
            isAttacking = true;
            anim.Play("Attack_A-3");
            EventManager.current.AttackStart(); // Sends event to EventManager (TimeManager listening)
            playerC.ApplyAttackForce(attack3move); // Moves player
            
            //time.DoSlowMo(sloMoModFactor, attack3time / 2);

            if (playerW.stillInside)
            {
                //Debug.Log("Should play third chainsaw");
                EventManager.current.WeaponTriggerEnter();
            }
        }

    }

    public void ComboReset()
    {
        comboPossible = false;
        comboAttackNumber = 0;
        EventManager.current.AttackFinish(); // Sends event to EventManager (TimeManager listening)
        //Debug.Log("ATTACK HAS ENDED **********************");
    }

    public void AllowMoveAgain()
    {
        isAttacking = false;
        playerC.RefreshMovement();
    }

    private void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Attack_A-1":
                    attack1time = clip.length / ATTACK_1_SPD_MOD;
                    break;
                case "Attack_A-2":
                    attack2time = clip.length / ATTACK_2_SPD_MOD;
                    break;
                case "Attack_A-3":
                    attack3time = clip.length / ATTACK_3_SPD_MOD;
                    break;
            }
        }
    }
}
