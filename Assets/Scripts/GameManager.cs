// 2022-02-13   Sean Hall   Updated the Game Manager to include game state tracking

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton declaration
    private static GameManager _instance = null;
    public static GameManager Instance { get => _instance; }

    // Game State variables
    public GameState state;
    private GameState lastState; //used to revert to after player unpause the game
    public static event Action<GameState> OnGameStateChanged;

    //pause system variables
    private bool pause = false;
    private float oldTime = 0f;

    private Camera mainCamera;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Transform player;
    [SerializeField] private SelectMenu selectMenu;

    public Transform Player { get => player;}
    public Camera MainCamera { get => mainCamera; }
    public Camera MapCamera { get => mapCamera; }

    public int level;
    [SerializeField] private Transform[] checkPoints;

    private void Awake()
    {
       
        //singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        //cache the main camera from the scene
        mainCamera = Camera.main;
        //change the state of the game to GamePlay
        UpdateGameState(GameState.GamePlay);
        //tell selectMenu to hide all panels
        selectMenu.PanelToggle(-1);
        //lock the cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        //don't show cursor
        Cursor.visible = false;

        GotoLevel();
    }

    public void NewGame()
    {
        level = 0;
        PlayerPrefs.SetInt("level", 0);
        PlayerPrefs.Save();
        PauseOrPlay();
        ReInitScene();
    }

    private void GotoLevel()
    {
        level =  PlayerPrefs.GetInt("level", 0);
        if (level < 4)
        {
            player.position = checkPoints[level].position;
            for (int i = 0; i < level; i++)
            {
                mainCamera.GetComponent<CameraRotator>().RotateClockwise();
                mapCamera.GetComponent<MapRotator>().RotateMapClockwise();
            }
        }
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
                mapCamera.GetComponent<Camera>().enabled = true;
                break;
            case GameState.GamePause:
                break;
            case GameState.Death:
                Invoke("ReInitScene", 2f);
                break;
            case GameState.Cutscene:
                Player.GetComponent<PlayerControl>().ResetMoveDirection();
                if(mapCamera)
                    mapCamera.GetComponent<Camera>().enabled = false;
                break;
            case GameState.Win:
                break;
            case GameState.Main:
                break;
//default:
//  error catch
        }

        OnGameStateChanged?.Invoke(newState); // Notify other scripts through event that game state has changed (condition avoids null exception error)
    }

    public void ReInitScene()
    {
        SceneLoader.Instance.ReLoadScene();
    }

    //to be called only from OnPause input action
    public void PauseOrPlay()
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
    Cutscene,
    Win,
    Main
}