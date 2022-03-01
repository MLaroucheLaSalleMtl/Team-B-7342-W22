// 2022-02-13   Sean Hall   Updated the Game Manager to include game state tracking

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton declaration
    private static GameManager instance = null;
    public static GameManager Instance { get => instance; }

    // Game State variables
    public GameState state;
    private GameState lastState; //used to revert to after player unpause the game
    public static event Action<GameState> OnGameStateChanged;

    //pause system variables
    private bool pause = false;
    private float oldTime = 0f;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform player;
    [SerializeField] private SelectMenu selectMenu;

    public Transform Player { get => player;}
    public Transform MainCamera { get => mainCamera; }



    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameState(GameState.GamePlay);
        selectMenu.PanelToggle(-1); //tell selectMenu to hide all panels
        Cursor.lockState = CursorLockMode.Locked; //lock the cursor in the middle of the screen
        Cursor.visible = false; //don't show cursor
    }

    // Game state referenced from "Game Manager - Controlling the flow of your game" by Tarodev
    // Found at: https://www.youtube.com/watch?v=4I0vonyqMi8
    public void UpdateGameState(GameState newState)
    {
        lastState = state;
        state = newState;

        switch (newState)
        {
            case GameState.GamePlay:
                break;
            case GameState.GamePause:
                break;
            case GameState.Death:
                break;
            case GameState.Cutscene:
                GameManager.Instance.Player.GetComponent<PlayerControl>().ResetMoveDirection();
                break;
//default:
//  error catch
        }

        OnGameStateChanged?.Invoke(newState); // Notify other scripts through event that game state has changed (condition avoids null exception error)
    }

    //to be called only from OnPause input action
    public void PuaseOrPlay()
    {
        if (state != GameState.Death)
        {
            pause = !pause;
            selectMenu.PanelToggle(pause ? 0 : -1); //show first panel/hide all panels

            //swap current and old time scales
            float temp = oldTime;
            oldTime = Time.timeScale;
            Time.timeScale = temp;

            Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = pause ? true : false;
            UpdateGameState(pause ? GameState.GamePause : lastState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
       
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public enum GameState
{
    GamePlay,
    GamePause,
    Death,
    Cutscene
}