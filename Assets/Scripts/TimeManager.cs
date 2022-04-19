// 2022-04-12   Sean Hall           Created script, with methods to slow time and lerp it back to default
// 2022-04-16   Sean Hall           Added an events timer to use with the attack combo
// 2022-04-18   Sean Hall           Added overload to DoSlowMo to allow it to subscribe to onWeaponTriggerEnter

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager instance = null;
    public static TimeManager Instance { get => instance; }

    public GameObject gameObjectGM;

    private GameManager gameManager;

    private PlayerAttack playerA;
    private float attackTime;

    // Time scale variables
    private const float DEFAULT_TIME_SCALE = 1.0f;
    private float slowMoFactor = 0.6f;
    public bool isSloMo = false;
    float currentSlowTime = 0; // Value to count
    float slowTime = 0; // Timer max value for duration of SlowMo

    // Timer variables
    public float timerTime = 0f;
    private const float DEFAULT_TIMER_TIME = 0f;
    private bool timerOn = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerA = FindObjectOfType<PlayerAttack>();
        EventManager.current.onAttackStart += StartTimer;
        EventManager.current.onAttackFinish += StopTimer;
        EventManager.current.onWeaponTriggerEnter += DoSlowMo;
    }

    private void Update()
    {
        // If game not paused and time is at a non-default scale
        //gameManager.state != GameState.GamePause &&        
        if (Time.timeScale != DEFAULT_TIME_SCALE)
        {
            if (currentSlowTime >= slowTime) // If the "Maintain SlowMo" Timer has run out
            {
                Normalize(); // Ease out of SlowMo by end of animation
                //Debug.Log("Normalizing speed, timescale at:" + Time.timeScale);
            }
            else
            {
                currentSlowTime += Time.deltaTime; // Increment "Maintain SlowMo" timer
                //Debug.Log("CURRENT SLOWTIME: " + currentSlowTime + "/" + slowTime + " and Time.timeScale: " + Time.timeScale);
            }

            if (Time.timeScale > 0.99999f) // If lerp (Normalize) has trouble getting it up
            {
                Time.timeScale = DEFAULT_TIME_SCALE; // Give it a hand and set it to default
                isSloMo = false;
            }
        }

        
        if (timerOn)
        {
            timerTime += Time.deltaTime;
        }
        //Debug.Log("Timer Time is: " + timerTime);        
    }

    public void DoSlowMo(float factor, float slowTime)
    {
        if (factor > 1 || factor < 0) { return; } // Catches errors
        Time.timeScale = factor; // Slows time
        isSloMo = true;
        this.slowTime = slowTime; // Sets the slowtime (how long slow motion should be maintained) for the Time Manager class
        currentSlowTime = 0;
        // put event in here with other stuff
    }

    public void DoSlowMo()
    {
        Time.timeScale = slowMoFactor;
        isSloMo = true;
        if (playerA.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack_A-1"))
        {
            attackTime = playerA.attack1time;
        }
        else if (playerA.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack_A-2"))
        {
            attackTime = playerA.attack2time;
        }
        else if (playerA.anim.GetCurrentAnimatorStateInfo(2).IsName("Attack_A-3"))
        {
            attackTime = playerA.attack3time;
        }
        // The amount of time in slowMo will be half of the time remaining in the animation from the time of contact
        this.slowTime = (attackTime - timerTime) / 2;
        currentSlowTime = 0;        
    }

    public void Normalize()
    {
        if (isSloMo)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, DEFAULT_TIME_SCALE, slowTime); // Transitions from current timescale to default
        }
    }

    public void StartTimer()
    {
        timerOn = true;
        timerTime = DEFAULT_TIMER_TIME;        
    }

    public void StopTimer()
    {
        timerOn = false;
        timerTime = DEFAULT_TIMER_TIME;        
    }
}
